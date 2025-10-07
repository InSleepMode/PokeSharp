using Godot;
using System;
using System.Collections.Generic;
using Game.Gameplay;
using Game.core;
using System.Reflection.Metadata.Ecma335;


namespace Game.core.Data
{
    public class PokemonData
    {
        public string Name;
        public Texture2D Texture;
        public int HP;
        public int Attack;


        public static List<PokemonData> GetAll()
        {
            return new List<PokemonData>
            {
                new PokemonData{
                    Name = "Haunter",
                    Texture = GD.Load<Texture2D>("res://assets/pokemons/haunter.png"),
                    HP = 100,
                    Attack = 25
                },

                new PokemonData{
                    Name = "Pikachu",
                    Texture = GD.Load<Texture2D>("res://assets/pokemons/pikachu.png"),
                    HP = 80,
                    Attack = 25
                }
            };
        }
    }
}

