using Godot;
using Game.core;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Dataflow;
using System.Xml;

namespace Game.UI;

public partial class ProfileScreen : Control
{
    [Export] private Label _playerNameLabel;
    [Export] private Label _coinsLabel;
    [Export] private Label _pokemonCountLabel;
    [Export] private GridContainer _pokemonGrid;
    [Export] private Button _backButton;

    public override void _Ready()
    {
        _backButton?.Connect("pressed", Callable.From(OnBackPressed));

        LoadProfile();
    }

    private void LoadProfile()
    {
        var gameState = GetNodeOrNull<GameState>("/root/GameState");
        if (gameState == null) return;

        if (_playerNameLabel != null)
        {
            _playerNameLabel.Text = "Player: Ash";
        }

        if (_coinsLabel != null)
        {
            _coinsLabel.Text = "Coins: 0";
        }

        if (_pokemonCountLabel != null)
        {
            var caughtPokemon = gameState.GetCaughtPokemon();
            _playerNameLabel.Text = $"You've got {caughtPokemon.Count} Pokemons!";
        }

        LoadPokemonGrid();
    }


    //TODO: createPokemonIcons for menu
    
    private void OnBackPressed()
    {
        var menuScene = GD.Load<PackedScene>("res://scenes/ui/menu.tscn");
        if(menuScene != null)
        {
            GetTree().ChangeSceneToPacked(menuScene);
        }
    }
}


