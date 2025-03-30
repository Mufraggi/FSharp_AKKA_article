module ConsoleApp4.Program

open System.Net.Http
open Actors.HttpActor
open Actors.Messages
open Actors.MongoActor
open Akka.Actor
open Akka.Routing
open Domain
open MongoDB.Driver
open MongoRepository.MongoInterpreter
open PokemonClient

//et FetchAndInsertPokemon (httpClient: HttpClient) (baseUrl: string) (interpreter: IPokemonInterpreter) pokeId =
//   async {
//       let! result = fetchPokemon httpClient baseUrl pokeId

//       match result with
//       | Ok pokemon ->
//           let! insertionResult = interpreter.Run(Insert (pokemon, id))
//           printfn $"Pokémon inséré avec succès : %s{insertionResult.name.fr}"
//           return Some insertionResult
//       | Error err ->
//           printfn "Erreur : %s" err
//           return None
//   }

let httpClient = new HttpClient()

let mongoConfig =
    { ConnectionString = "mongodb://localhost:27017"
      DatabaseName = "pokedex"
      CollectionName = "pokemons" }

let client = MongoClient("mongodb://localhost:27017")
let mongoInterpreter: MongoInterpreter = MongoInterpreter(client, mongoConfig)

let baseUrl = "https://tyradex.vercel.app/api/v1/pokemon"

let callPokemon: PokemonId -> Async<Result<Pokemon, string>> =
    fetchPokemon httpClient baseUrl

let system = ActorSystem.Create("ETLSystem")

let mongoActor =
    system.ActorOf(Props.Create(fun () -> MongoActor(mongoInterpreter)), "mongo-actor")

let httpActor =
    system.ActorOf(
        Props
            .Create(fun () -> HttpClientActor(callPokemon, mongoActor))
            .WithRouter(SmallestMailboxPool(2)),
        "http-actor"
    )

for i in 0..200 do
    httpActor.Tell(FetchData i)

printfn "🔄 Système ETL démarré. Appuyez sur une touche pour quitter..."
System.Console.ReadLine() |> ignore
