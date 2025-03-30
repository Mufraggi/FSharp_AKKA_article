module Actors.Messages

open Domain

type ETLMessage =
    | FetchData of int 
    | SaveToMongo of  Pokemon
