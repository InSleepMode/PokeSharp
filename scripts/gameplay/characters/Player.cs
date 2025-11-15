using Game.Utilities;
using Godot;
using System;
using Game.core;
public partial class Player : CharacterBody2D
{
    [Export] public StateMachine StateMachine;

    private bool _isInBattle = false;
    public override void _Ready()
    {
        StateMachine.ChangeState(StateMachine.GetNode<State>("Roam"));//Change roam state

        Signals.Instance.BattleStarted += OnBattleStarted;
        Signals.Instance.BattleEnded += OnBattleEnded;

        //отладка
        if (Signals.Instance == null)
        {
            GD.PrintErr("Signals.Instance is NULL!");
            return;
        }
    }

    public override void _ExitTree()
    {
        if (Signals.Instance != null)
        {
            Signals.Instance.BattleStarted -= OnBattleStarted;
            Signals.Instance.BattleEnded -= OnBattleEnded;
        }
    }

    private void OnBattleStarted()
    {
        _isInBattle = true;
        GD.Print("[Player] Battle started - disabling input");
        GD.Print("[Player] OnBattleStarted CALLED!");//отладка
        SetProcessInput(false);
        SetProcess(false);
        SetPhysicsProcess(false);
        
        if (StateMachine != null)
        {
            StateMachine.SetProcess(false);
            StateMachine.SetPhysicsProcess(false);
        }
    }

    private void OnBattleEnded()
    {
        _isInBattle = false;
        GD.Print("[Player] Battle ended - enabling input");
        GD.Print("[Player] OnBattleEnded CALLED!");//отладка
        SetProcessInput(true);
        SetProcess(true);
        SetPhysicsProcess(true);
        
        if (StateMachine != null)
        {
            StateMachine.SetProcess(true);
            StateMachine.SetPhysicsProcess(true);
        }
    }

    public bool IsInBattle() => _isInBattle;

    
}
