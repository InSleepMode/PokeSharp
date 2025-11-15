using Game.core;
using Godot;
using System;
using Game.core.Data;
namespace Game.Gameplay;

public partial class TallGrass : Area2D
{
    [Export] public AnimatedSprite2D AnimatedSprite2D;
    [Export] public int EncounterRate = 30;

    private bool _canTrigger = true;

    public override void _Ready()
    {
        AnimatedSprite2D ??= GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    public void OnBodyEntered(Node2D node2D)
    {
        if (node2D is not Player)
        {
            return;
        }

        AnimatedSprite2D?.Play("down");

        if (_canTrigger)
        {
            CalculateEncounterChance();
        }
    }

    public void OnBodyExited(Node2D node2D)
    {
        if (node2D is not Player)
        {
            return;
        }

        AnimatedSprite2D?.Play("up");
        _canTrigger = true;
    }

    public void CalculateEncounterChance()
    {
        int rate = EncounterRate;
        
        int chance = Globals.GetRandomNumberGenerator().RandiRange(0, 100);

        if (chance <= rate)
        {
            _canTrigger = false;
            
            if (GameState.Instance == null || SceneTransition.Instance == null)
            {
                return;
            }
            
            var rng = Globals.GetRandomNumberGenerator();
            
            GameState.Instance.WildPokemon = new PokemonData
            {
                Name = GetRandomPokemon(),
                HP = 25,
                Attack = 10,
            };
            
            SceneTransition.GoTo("battle");
        }
    }
    
    private string GetRandomPokemon()
    {
        string[] pokemons = { "Pidgey", "Rattata", "Caterpie", "Weedle", "Pikachu" };
        
        var rng = Globals.GetRandomNumberGenerator();
        return pokemons[rng.RandiRange(0, pokemons.Length - 1)];
    }
}