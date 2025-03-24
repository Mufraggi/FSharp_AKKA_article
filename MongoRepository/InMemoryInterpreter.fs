module MongoRepository.InMemoryInterpreter

open System.Collections.Generic
open MongoRepository.MongoInterpreter
open MongoRepository.mapper

type InMemoryInterpreter() =
    let db = new List<MongoPokemon>()
   
    interface IPokemonInterpreter with
        member _.Run(op: PokemonOp<'a>) : Async<'a> =
            async {
            match op with
            | GetAll cont ->
                return cont (List.ofSeq db |> List.map Mapping.toDomainPokemon)

            | GetById (id, cont) ->
                return cont ((db |> Seq.toList |>
                             List.tryFind (fun p -> p.Id = id)
                             )|> Option.map Mapping.toDomainPokemon) 

            | Insert (pokemon, cont) ->
                let pokemonMongo = Mapping.toMongoPokemon pokemon
                db.Add(pokemonMongo)
                return cont pokemon

            | Update (pokemon, cont) ->
                let pokemonMongo = Mapping.toMongoPokemon pokemon
                let index = db |> Seq.toList |> List.findIndex (fun p -> p.PokedexId = pokemonMongo.PokedexId)
                if index >= 0 then
                    db.[index] <- pokemonMongo
                    return cont true
                else
                    return cont false

            | Delete (id, cont) ->
                let removed = db.RemoveAll(fun p -> p.Id = id) > 0
                return cont removed

            | GetByGeneration (gen, cont) ->
                let results = db |> Seq.toList |> List.filter (fun p -> p.Generation = gen)
                return cont (results|> List.map Mapping.toDomainPokemon)

            | GetAllPaginated (page, size, cont) ->
                let results = db |> Seq.toList |> List.skip (page * size) |> List.take size
                return cont ((results|> List.map Mapping.toDomainPokemon), db.Count)
        }
