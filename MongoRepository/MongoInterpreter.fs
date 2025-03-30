module MongoRepository.MongoInterpreter

open Domain
open MongoDB.Driver
open MongoRepository.mapper

type MongoConfig = {
    ConnectionString: string
    DatabaseName: string
    CollectionName: string
}
type IPokemonInterpreter =
    abstract member Run: PokemonOp<'a> -> Async<'a>

type MongoInterpreter(client: MongoClient, config: MongoConfig) =
    let db = client.GetDatabase(config.DatabaseName)
    let collection: IMongoCollection<MongoPokemon> = db.GetCollection<MongoPokemon>(config.CollectionName)

    interface IPokemonInterpreter with 
        member _.Run(op: PokemonOp<'a>) : Async<'a> =
            async {
                match op with
                | GetAll cont ->
                    let! pokemons = collection.Find(FilterDefinition.Empty).ToListAsync() |> Async.AwaitTask
                    return cont (List.ofSeq pokemons |> List.map Mapping.toDomainPokemon)

                | GetById (id, cont) ->
                     try
                          let filter = Builders.Filter.Eq("_id", id)
                          let! mongoResult = collection.Find(filter).FirstOrDefaultAsync() |> Async.AwaitTask
                            // Utiliser Option.ofObj pour convertir un objet nullable en Option
                          let result = Mapping.toDomainPokemon mongoResult  
                          return cont  (Some result)
                     with
                        | :? System.FormatException -> return cont None

                | Insert (pokemon, cont) ->
                    let pokemonMongo = Mapping.toMongoPokemon pokemon
                    do! collection.InsertOneAsync(pokemonMongo) |> Async.AwaitTask
                    return cont pokemon 

                | Update (pokemon, cont) ->
                    let pokemonMongo = Mapping.toMongoPokemon pokemon
                    let filter = Builders<MongoPokemon>.Filter.Eq("pokedex_id", pokemon.pokedex_id)
                    let! result = collection.ReplaceOneAsync(filter, pokemonMongo) |> Async.AwaitTask
                    return cont (result.ModifiedCount > 0)

                | Delete (id, cont) ->
                    let filter = Builders<MongoPokemon>.Filter.Eq("_id", id)
                    let! result = collection.DeleteOneAsync(filter) |> Async.AwaitTask
                    return cont (result.DeletedCount > 0)

            
                | GetByGeneration (gen, cont) ->
                    let filter = Builders<MongoPokemon>.Filter.Eq("generation", gen)
                    let! pokemons = collection.Find(filter).ToListAsync() |> Async.AwaitTask
                    return cont (List.ofSeq pokemons  |> List.map Mapping.toDomainPokemon)

                | GetAllPaginated (page, size, cont) ->
                    let! pokemons = collection.Find(FilterDefinition<MongoPokemon>.Empty)
                                           .Skip(page * size)
                                           .Limit(size)
                                           .ToListAsync() |> Async.AwaitTask
                    let! totalCount = collection.CountDocumentsAsync(FilterDefinition<MongoPokemon>.Empty) |> Async.AwaitTask
                    return cont (List.ofSeq pokemons
                                 |> List.map Mapping.toDomainPokemon,
                                 int totalCount)
            }
