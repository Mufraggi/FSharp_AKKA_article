module ConsoleApp4.Program

open System.Net.Http
open Domain
open MongoDB.Driver
open MongoRepository
open MongoRepository.InMemoryInterpreter
open MongoRepository.MongoInterpreter
open PokemonClient

let FetchAndInsertPokemon (httpClient: HttpClient) (baseUrl: string) (interpreter: IPokemonInterpreter) pokeId =
    async {
        let! result = fetchPokemon httpClient baseUrl pokeId 

        match result with
        | Ok pokemon ->
            let! insertionResult = interpreter.Run(Insert (pokemon, id))
            printfn $"Pokémon inséré avec succès : %s{insertionResult.name.fr}"
            return Some insertionResult
        | Error err ->
            printfn "Erreur : %s" err
            return None
    }

let httpClient = new HttpClient()
let mongoConfig = { ConnectionString = "mongodb://localhost:27017"
                    DatabaseName = "pokedex"
                    CollectionName = "pokemons" }
let client = MongoClient("mongodb://localhost:27017")
let mongoInterpreter = MongoInterpreter(client, mongoConfig)

let baseUrl = "https://tyradex.vercel.app/api/v1/pokemon"

let callPokemon = fetchPokemon httpClient baseUrl





let runSynchronously = FetchAndInsertPokemon httpClient baseUrl mongoInterpreter 1 |> Async.RunSynchronously
let inMemoryInterpreter = InMemoryInterpreter()  // Fake MongoDB
let pokemonOption = FetchAndInsertPokemon httpClient baseUrl inMemoryInterpreter 1 |> Async.RunSynchronously
