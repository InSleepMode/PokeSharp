using Godot;
using System;
using Game.UI;
using Game.Utilities;
using Game.core;

namespace Game.Gameplay;

public partial class PlayerMessageState : State
{
    
    public override void _Ready()
	{
		Signals.Instance.MessageBoxOpen += (value) =>
		{
			if (!value)
			{
				StateMachine.ChangeState("Roam");
			}
		};
	}
    public override void _Process(double delta)
    {
        if (!MessageManager.Scrolling() && Input.IsActionJustReleased("use"))
        {
            MessageManager.ScrollText();

        }
    }
}
