module PokemonClient.PokemonClient

open System.Net.Http
open PokemonApiType
open Thoth.Json.Net

let setupHttpClient (client: HttpClient) (baseUrl: string) =
    let callPokemonById (id: PokemonId) : Async<Result<Pokemon, string>> =
        async {
            let url = $"%s{baseUrl}/%d{id}"
            try
                let! response = client.GetStringAsync(url) |> Async.AwaitTask
                match Decode.Auto.fromString<Pokemon>(response) with
                | Ok pokemon -> return Ok pokemon
                | Error decodeError -> return Error (sprintf "Erreur de décodage JSON : %s" decodeError)
            with
            | :? HttpRequestException as httpEx ->
                return Error (sprintf "Erreur HTTP : %s" httpEx.Message)
            | ex ->
                return Error (sprintf "Erreur inattendue : %s" ex.Message)
        }
    callPokemonById