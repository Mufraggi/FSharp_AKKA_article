module PokemonClient

open System.Net.Http
open Thoth.Json.Net
open Domain


let fetchPokemon (client: HttpClient) (baseUrl: string) (id: PokemonId) : Async<Result<Pokemon, string>> =
         async {
            let url = $"%s{baseUrl}/%d{id}"

            try
                let! response = client.GetStringAsync(url) |> Async.AwaitTask

                match Decode.Auto.fromString<Pokemon> (response) with
                | Ok pokemon -> return Ok pokemon
                | Error decodeError -> return Error(sprintf "Erreur de décodage JSON : %s" decodeError)
            with
            | :? HttpRequestException as httpEx -> return Error(sprintf "Erreur HTTP : %s" httpEx.Message)
            | ex -> return Error(sprintf "Erreur inattendue : %s" ex.Message)
        }


