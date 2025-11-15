using Godot;
using System.Collections.Generic;

namespace Game.core.Data
{
    public class PokemonData
    {
        public string Name { get; set; }
        public Texture2D Texture { get; set; }
        public int Level { get; set; }
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }

        public static List<PokemonData> GetAll()
        {
            return new List<PokemonData>
            {
                new PokemonData {
                    Name = "Haunter",
                    Texture = GD.Load<Texture2D>("res://assets/pokemons/haunter.png"),
                    Level = 5,
                    HP = 100,
                    MaxHP = 100,
                    Attack = 25,
                    Defense = 15
                },

                new PokemonData {
                    Name = "Pikachu",
                    Texture = GD.Load<Texture2D>("res://assets/pokemons/pikachu.png"),
                    Level = 5,
                    HP = 80,
                    MaxHP = 80,
                    Attack = 25,
                    Defense = 10
                },

                new PokemonData {
                    Name = "Rattata",
                    Texture = GD.Load<Texture2D>("res://assets/pokemons/rattata.png"),
                    Level = 3,
                    HP = 70,
                    MaxHP = 70,
                    Attack = 15,
                    Defense = 8
                },

                new PokemonData {
                    Name = "Pidgey",
                    Texture = GD.Load<Texture2D>("res://assets/pokemons/pidgey.png"),
                    Level = 3,
                    HP = 40,
                    MaxHP = 40,
                    Attack = 10,
                    Defense = 5
                },

                new PokemonData {
                    Name = "Caterpie",
                    Texture = GD.Load<Texture2D>("res://assets/pokemons/caterpie.png"),
                    Level = 2,
                    HP = 60,
                    MaxHP = 60,
                    Attack = 20,
                    Defense = 7
                },

                new PokemonData {
                    Name = "Spearow",
                    Texture = GD.Load<Texture2D>("res://assets/pokemons/spearow.png"),
                    Level = 4,
                    HP = 50,
                    MaxHP = 50,
                    Attack = 20,
                    Defense = 6
                }
            };
        }

        public static PokemonData GetRandomWildPokemon()
        {
            var allPokemon = GetAll();
            var random = new System.Random();
            int index = random.Next(0, allPokemon.Count);

            var template = allPokemon[index];

            return new PokemonData
            {
                Name = template.Name,
                Texture = template.Texture,
                Level = template.Level,
                HP = template.HP,
                MaxHP = template.MaxHP,
                Attack = template.Attack,
                Defense = template.Defense
            };
        }
    }
}