using System.Collections.Generic;
using Game.core;
using Godot;
using Godot.Collections;

namespace Game.Gameplay
{

	public partial class Level : Node2D
	{
		[ExportCategory("Level Basics")]
		[Export]

		public LevelName LevelName;

		[Export(PropertyHint.Range, "0,100")] public int EncounterRate;

		[ExportCategory("Camera Limits")]
		[Export]

		public int Top;
		[Export]

		public int Bottom;
		[Export]

		public int Left;
		[Export]

		public int Right;


		private readonly HashSet<Vector2> reservedTiles = [];

		public override void _Ready()
		{
			Logger.Info($"Loading level {LevelName} ...");

		}

		public bool ReserveTile(Vector2 position)
		{
			if (reservedTiles.Contains(position))
			{
				return false;
			}

			reservedTiles.Add(position);
			return true;
		}

		public bool IsTileFree(Vector2 position)
		{
			return !reservedTiles.Contains(position);
		}

		public void ReleaseTile(Vector2 position)
		{
			reservedTiles.Remove(position);
		}

	}
}