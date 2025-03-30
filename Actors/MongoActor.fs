module Actors.MongoActor

open Actors.Messages
open Akka.Actor
open MongoRepository
open MongoRepository.MongoInterpreter

type MongoActor (mongoInterpreter: IPokemonInterpreter)  =
    inherit ReceiveActor()

    do
        base.Receive<ETLMessage>(fun msg ->
            match msg with
            | SaveToMongo pokemon ->
                async {
                    try
                         let! insertionResult = mongoInterpreter.Run(Insert (pokemon, id))
                         printfn $"Pokémon inséré avec succès : %s{insertionResult.name.fr}"
                    with ex ->
                       printfn $"❌ Erreur lors lors de linsert mongo : %s{ex.Message}"

                } |> Async.Start
        )