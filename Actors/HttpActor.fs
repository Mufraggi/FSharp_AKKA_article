module Actors.HttpActor

open Actors.Messages
open Akka.Actor
open Domain
type HttpClientActor (client:PokemonId -> Async<Result<Pokemon,string>>, mongoRef: IActorRef)  =
    inherit ReceiveActor()


    do
        base.Receive<ETLMessage>(fun msg ->
            match msg with
            | FetchData pokeId ->
                async {
                    try
                        let! response = client(pokeId)
                        match response with
                        | Ok pokemon -> mongoRef.Tell(SaveToMongo pokemon)
                        | Error errorValue -> printfn $"❌❌❌❌ error during the http call : %s{errorValue}"
                    with ex ->
                       printfn $"❌ error exit message: %s{ex.Message}"

                } |> Async.Start
        )