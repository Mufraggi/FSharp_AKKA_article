module ConsoleApp4.Program

open System.Net.Http
open PokemonClient

let httpClient = new HttpClient()
let baseUrl = "https://tyradex.vercel.app/api/v1/pokemon/1"

let callPokemon = setupHttpClient httpClient baseUrl

let result = callPokemon 1 |> Async.RunSynchronously

match result with
| Ok pokemon -> printfn "Pokemon : %s" pokemon.name.fr
| Error err -> printfn "Erreur : %s" err