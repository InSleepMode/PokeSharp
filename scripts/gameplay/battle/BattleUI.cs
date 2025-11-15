using Godot;
using System;

namespace Game.Gameplay.Battle;

public partial class BattleUI : CanvasLayer
{
    [Export] private Button _fightButton;
    [Export] private Button _runButton;
    [Export] private Button _pokeballButton;
    [Export] private Button _bagButton;
    [Export] private BattleManager _battleManager;

    public override void _Ready()
    {
        _fightButton.Pressed += OnFightPressed;
        _pokeballButton.Pressed += OnPokeballPressed;
        _runButton.Pressed += OnRunPressed;
        
        if (_bagButton != null)
        {
            _bagButton.Visible = false;
        }
        
    }

    private void OnFightPressed()
    {
        _battleManager?.playerAttack();
    }

    private void OnPokeballPressed()
    {
        _battleManager?.catchPokemon();
    }

    private void OnRunPressed()
    {
        _battleManager?.runFromBattle();
    }
}