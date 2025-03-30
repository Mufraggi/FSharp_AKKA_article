module MongoRepository.mapper

open Domain
open MongoDB.Bson

module Mapping =
    let toDomainPokemonName (mongoPokemonName: MongoPokemonName): PokemonName =
        { fr = mongoPokemonName.Fr; en = mongoPokemonName.En; jp = mongoPokemonName.Jp }
        
    let toMongoPokemonName (pokemonName: PokemonName): MongoPokemonName =
        { Fr = pokemonName.fr; En = pokemonName.en; Jp = pokemonName.jp }
        
    let toDomainGmaxSprites (mongoGmaxSprites: MongoGmaxSprites): GmaxSprites =
        { regular = mongoGmaxSprites.Regular; shiny = mongoGmaxSprites.Shiny }
        
    let toMongoGmaxSprites (gmaxSprites: GmaxSprites): MongoGmaxSprites =
        { Regular = gmaxSprites.regular; Shiny = gmaxSprites.shiny }
        
    // Mise à jour du mapper Sprites pour prendre en compte le nouveau type GmaxSprites
    let toDomainSprites (mongoSprites: MongoSprites): Sprites =
        { regular = mongoSprites.Regular; 
          shiny = mongoSprites.Shiny; 
          gmax = mongoSprites.Gmax |> Option.map toDomainGmaxSprites }
        
    let toMongoSprites (sprites: Sprites): MongoSprites =
        { Regular = sprites.regular; 
          Shiny = sprites.shiny; 
          Gmax = sprites.gmax |> Option.map toMongoGmaxSprites }
        
    let toDomainTmp (mongoTmp: MongoTmp): tmp =
        { name = mongoTmp.Name; image = mongoTmp.Image }
        
    let toMongoTmp (tmp: tmp): MongoTmp =
        { Name = tmp.name; Image = tmp.image }
        
    let toDomainTalent (mongoTalent: MongoTalent): Talent =
        { name = mongoTalent.Name; tc = mongoTalent.Tc }
        
    let toMongoTalent (talent: Talent): MongoTalent =
        { Name = talent.name; Tc = talent.tc }
        
    let toDomainRegionalForm (mongoForm: MongoRegionalForm): RegionalForm =
        { region = mongoForm.Region; name = toDomainPokemonName mongoForm.Name }
        
    let toMongoRegionalForm (form: RegionalForm): MongoRegionalForm =
        { Region = form.region; Name = toMongoPokemonName form.name }
        
    let toDomainStats (mongoStats: MongoStats): Stats =
        { hp = mongoStats.Hp; atk = mongoStats.Atk; def = mongoStats.Def; 
          spe_atk = mongoStats.SpeAtk; spe_def = mongoStats.SpeDef; vit = mongoStats.Vit }
          
    let toMongoStats (stats: Stats): MongoStats =
        { Hp = stats.hp; Atk = stats.atk; Def = stats.def; 
          SpeAtk = stats.spe_atk; SpeDef = stats.spe_def; Vit = stats.vit }
          
    let toDomainResistance (mongoResistance: MongoResistance): Resistance =
        { name = mongoResistance.Name; multiplier = mongoResistance.Multiplier }
        
    let toMongoResistance (resistance: Resistance): MongoResistance =
        { Name = resistance.name; Multiplier = resistance.multiplier }
        
    let toDomainEvolutionStep (mongoStep: MongoEvolutionStep): EvolutionStep =
        { pokedex_id = mongoStep.PokedexId; name = mongoStep.Name; condition = mongoStep.Condition }
        
    let toMongoEvolutionStep (step: EvolutionStep): MongoEvolutionStep =
        { PokedexId = step.pokedex_id; Name = step.name; Condition = step.condition }
        
    let toDomainEvolution (mongoEvolution: MongoEvolution): Evolution =
        { pre = mongoEvolution.Pre |> Option.map (List.map toDomainEvolutionStep);
          next = mongoEvolution.Next |> Option.map (List.map toDomainEvolutionStep);
          mega = mongoEvolution.Mega |> Option.map (fun _ -> box()) }
        
    let toMongoEvolution (evolution: Evolution): MongoEvolution =
        { Pre = evolution.pre |> Option.map (List.map toMongoEvolutionStep);
          Next = evolution.next |> Option.map (List.map toMongoEvolutionStep);
          Mega = if evolution.mega.IsSome then Some(BsonDocument()) else None }
        
    let toDomainGender (mongoGender: MongoGender): Gender =
        { male = mongoGender.Male; female = mongoGender.Female }
        
    let toMongoGender (gender: Gender): MongoGender =
        { Male = gender.male; Female = gender.female }
        
    let toDomainPokemon (mongoPokemon: MongoPokemon): Pokemon =
        { pokedex_id = mongoPokemon.PokedexId;
          generation = mongoPokemon.Generation;
          category = mongoPokemon.Category;
          name = toDomainPokemonName mongoPokemon.Name;
          sprites = toDomainSprites mongoPokemon.Sprites;
          types = mongoPokemon.Types |> List.map toDomainTmp;
          talents = mongoPokemon.Talents |> List.map toDomainTalent;
          stats = toDomainStats mongoPokemon.Stats;
          resistances = mongoPokemon.Resistances |> List.map toDomainResistance;
          evolution = toDomainEvolution mongoPokemon.Evolution;
          height = mongoPokemon.Height;
          weight = mongoPokemon.Weight;
          egg_groups = mongoPokemon.EggGroups;
          sexe = toDomainGender mongoPokemon.Sexe;
          catch_rate = mongoPokemon.CatchRate;
          level_100 = mongoPokemon.Level100;
          formes = mongoPokemon.Formes |> Option.map (List.map toDomainRegionalForm)
        }
        
    let toMongoPokemon (pokemon: Pokemon): MongoPokemon =
        { Id = ObjectId.GenerateNewId();
          PokedexId = pokemon.pokedex_id;
          Generation = pokemon.generation;
          Category = pokemon.category;
          Name = toMongoPokemonName pokemon.name;
          Sprites = toMongoSprites pokemon.sprites;
          Types = pokemon.types |> List.map toMongoTmp;
          Talents = pokemon.talents |> List.map toMongoTalent;
          Stats = toMongoStats pokemon.stats;
          Resistances = pokemon.resistances |> List.map toMongoResistance;
          Evolution = toMongoEvolution pokemon.evolution;
          Height = pokemon.height;
          Weight = pokemon.weight;
          EggGroups = pokemon.egg_groups;
          Sexe = toMongoGender pokemon.sexe;
          CatchRate = pokemon.catch_rate;
          Level100 = pokemon.level_100;
          Formes = pokemon.formes |> Option.map (List.map toMongoRegionalForm)
        }
