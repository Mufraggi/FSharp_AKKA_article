module ConsoleApp4.Program

open System.Net.Http
open Domain
open MongoRepository
open MongoRepository.MongoInterpreter
open PokemonClient

let httpClient = new HttpClient()
let mongoConfig = { ConnectionString = "mongodb://localhost:27017"
                    DatabaseName = "pokedex"
                    CollectionName = "pokemons" }

let mongoInterpreter = MongoInterpreter(mongoConfig)

let insertPokemonMongo (pokemon: Pokemon) =   mongoInterpreter.Run(Insert (pokemon, id ))
let baseUrl = "https://tyradex.vercel.app/api/v1/pokemon"

let callPokemon = fetchPokemon httpClient baseUrl
let result = callPokemon 1 |> Async.RunSynchronously

let b =
    match result with
    | Ok pokemon ->
        let insertionResult: Pokemon = insertPokemonMongo pokemon |> Async.RunSynchronously
        printfn $"Pokémon inséré avec succès : %s{insertionResult.name.fr}"
    | Error err -> printfn "Erreur : %s" err