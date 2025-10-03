using Game.core;
using Godot;
using System;


namespace Game.Gameplay
{
	public partial class SpawnPoint : Node2D
	{
		public override void _EnterTree()
		{
			AddToGroup(LevelGroup.SPAWNPOINTS.ToString());
		}

		public override void _ExitTree()
		{
			RemoveFromGroup(LevelGroup.SPAWNPOINTS.ToString());
		}

		

	}
}