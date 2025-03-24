namespace MongoRepository

open Domain
open MongoDB.Bson

type PokemonOp<'a> =
    | GetAll of (Pokemon list -> 'a)
    | GetById of ObjectId * (Pokemon option -> 'a)
    | Insert of Pokemon * (Pokemon -> 'a)
    | Update of Pokemon * (bool -> 'a)
    | Delete of ObjectId * (bool -> 'a)
    | GetByGeneration of int * (Pokemon list -> 'a)
    | GetAllPaginated of int * int * ((Pokemon list * int) -> 'a)
