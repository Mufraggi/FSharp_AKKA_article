namespace Domain

type PokemonId = int

type PokemonName = {
    fr: string
    en: string
    jp: string
}

type Sprites = {
    regular: string
    shiny: string
    gmax: string option
}

type tmp = {
    name: string
    image: string
}

type Talent = {
    name: string
    tc: bool
}
type RegionalForm = {
    region: string
    name: PokemonName
}

type Stats = {
    hp: int
    atk: int
    def: int
    spe_atk: int
    spe_def: int
    vit: int
}

type Resistance = {
    name: string
    multiplier: float
}

type EvolutionStep = {
    pokedex_id: int
    name: string
    condition: string
}

type Evolution = {
    pre: EvolutionStep option
    next: EvolutionStep list
    mega: obj option // Utilisé 'obj' car le type exact n'est pas spécifié dans l'exemple
}

type Gender = {
    male: float
    female: float
}

type Pokemon = {
    pokedex_id: PokemonId
    generation: int
    category: string
    name: PokemonName
    sprites: Sprites
    types: tmp list
    talents: Talent list
    stats: Stats
    resistances: Resistance list
    evolution: Evolution
    height: string
    weight: string
    egg_groups: string list
    sexe: Gender
    catch_rate: int
    level_100: int
    formes: RegionalForm list  option
}