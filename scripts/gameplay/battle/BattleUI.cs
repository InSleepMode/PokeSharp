using Godot;
using System;



namespace Game.Gameplay.Battle;
public partial class BattleUI : Control
{
    [Export] private Button _runButton;
    
    [Export] private BattleManager _battleManager;

    public override void _Ready()
    {
        _runButton.Pressed += OnRunPressed;
    }

    private void OnRunPressed()
    {
        _battleManager.EndBattle();
    }
}
