namespace MongoRepository

open MongoDB.Bson
open MongoDB.Bson.Serialization.Attributes

[<BsonIgnoreExtraElements>]
type MongoPokemonName = {
    [<BsonElement("fr")>] Fr: string
    [<BsonElement("en")>] En: string
    [<BsonElement("jp")>] Jp: string
}

[<BsonIgnoreExtraElements>]
type MongoGmaxSprites = {
    [<BsonElement("regular")>] Regular: string
    [<BsonElement("shiny")>] Shiny: string
}

[<BsonIgnoreExtraElements>]
type MongoSprites = {
    [<BsonElement("regular")>] Regular: string
    [<BsonElement("shiny")>] Shiny: string
    [<BsonElement("gmax")>] Gmax: MongoGmaxSprites option  // Changé de string option à MongoGmaxSprites option
}


[<BsonIgnoreExtraElements>]
type MongoTmp = {
    [<BsonElement("name")>] Name: string
    [<BsonElement("image")>] Image: string
}

[<BsonIgnoreExtraElements>]
type MongoTalent = {
    [<BsonElement("name")>] Name: string
    [<BsonElement("tc")>] Tc: bool
}

[<BsonIgnoreExtraElements>]
type MongoRegionalForm = {
    [<BsonElement("region")>] Region: string
    [<BsonElement("name")>] Name: MongoPokemonName
}

[<BsonIgnoreExtraElements>]
type MongoStats = {
    [<BsonElement("hp")>] Hp: int
    [<BsonElement("atk")>] Atk: int
    [<BsonElement("def")>] Def: int
    [<BsonElement("spe_atk")>] SpeAtk: int
    [<BsonElement("spe_def")>] SpeDef: int
    [<BsonElement("vit")>] Vit: int
}

[<BsonIgnoreExtraElements>]
type MongoResistance = {
    [<BsonElement("name")>] Name: string
    [<BsonElement("multiplier")>] Multiplier: float
}

[<BsonIgnoreExtraElements>]
type MongoEvolutionStep = {
    [<BsonElement("pokedex_id")>] PokedexId: int
    [<BsonElement("name")>] Name: string
    [<BsonElement("condition")>] Condition: string
}

[<BsonIgnoreExtraElements>]
type MongoEvolution = {
    [<BsonElement("pre")>] Pre: MongoEvolutionStep list option
    [<BsonElement("next")>] Next: MongoEvolutionStep list option
    [<BsonElement("mega")>] Mega: BsonDocument option  // Utilisation de BsonDocument pour un objet dynamique
}

[<BsonIgnoreExtraElements>]
type MongoGender = {
    [<BsonElement("male")>] Male: float
    [<BsonElement("female")>] Female: float
}

[<BsonIgnoreExtraElements>]
type MongoPokemon = {
    [<BsonId>]
    [<BsonRepresentation(BsonType.ObjectId)>]
    [<BsonElement("_id")>] Id: ObjectId
    [<BsonElement("pokedex_id")>] PokedexId: int
    [<BsonElement("generation")>] Generation: int
    [<BsonElement("category")>] Category: string
    [<BsonElement("name")>] Name: MongoPokemonName
    [<BsonElement("sprites")>] Sprites: MongoSprites
    [<BsonElement("types")>] Types: MongoTmp list
    [<BsonElement("talents")>] Talents: MongoTalent list
    [<BsonElement("stats")>] Stats: MongoStats
    [<BsonElement("resistances")>] Resistances: MongoResistance list
    [<BsonElement("evolution")>] Evolution: MongoEvolution
    [<BsonElement("height")>] Height: string
    [<BsonElement("weight")>] Weight: string
    [<BsonElement("egg_groups")>] EggGroups: string list
    [<BsonElement("sexe")>] Sexe: MongoGender
    [<BsonElement("catch_rate")>] CatchRate: int
    [<BsonElement("level_100")>] Level100: int
    [<BsonElement("formes")>] Formes: MongoRegionalForm list option
}