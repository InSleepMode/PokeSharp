using System.Collections.Generic;
using Game.core;
using Godot;

namespace Game.Gameplay
{
    public partial class Level : Node2D
    {
        [ExportCategory("Level Basics")]
        [Export]
        public LevelName LevelName;

        [Export(PropertyHint.Range, "0,100")] 
        public int EncounterRate;

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
        private CharacterBody2D player;
    public override void _Ready()
    {
        Logger.Info($"Loading level {GetTree().CurrentScene.Name} ...");
        
        if (GameState.Instance.ReturnFromBattle)
        {
            GD.Print($"   Position: {GameState.Instance.BattleReturnPosition}");
            
            player = GetNodeOrNull<CharacterBody2D>("Player");
            if (player != null)
            {
                player.Position = GameState.Instance.BattleReturnPosition;
            }
            
            GameState.Instance.ReturnFromBattle = false;
            return;
        }
        
        bool shouldLoadFromSave = false;
        
        if (GameState.Instance.IsNewGame)
        {
            GD.Print("NEW GAME - ignoring save");
            GameState.Instance.IsNewGame = false;
            shouldLoadFromSave = false;
        }
        else if (GameState.Instance.GameLoadedFromSave)
        {
            GD.Print("CONTINUE - using loaded save");
            shouldLoadFromSave = true;
            GameState.Instance.GameLoadedFromSave = false;
        }
        else if (GameState.Instance.LastScene != GameState.Instance.CurrentScene)
        {
            shouldLoadFromSave = false;
        }
        else if (SaveManager.Instance?.HasSaveFile() == true)
        {
            GD.Print("Auto-loading save...");
            shouldLoadFromSave = SaveManager.Instance.LoadGame();
            
            if (shouldLoadFromSave)
            {
                GD.Print("Save auto-loaded!");
            }
        }
        else
        {
            GD.Print("No save - starting fresh");
        }
        
        if (shouldLoadFromSave)
        {
            SpawnPlayerFromSave();
        }
        else
        {
            InitializePlayer();
        }
    }
		private void SpawnPlayerFromSave()
		{
			player = GetNodeOrNull<CharacterBody2D>("Player");

			if (player == null)
			{
				return;
			}
			player.Position = GameState.Instance.PlayerPosition;

		}

        private void InitializePlayer()
        {
            player = GetNodeOrNull<CharacterBody2D>("Player");

            if (player == null)
            {
                Logger.Info("Player not found in this level");
                return;
            }

            var gameState = GameState.Instance;

            if (gameState == null)
            {
                GD.PrintErr("GameState not found");
                return;
            }

            if (gameState.ReturnFromBattle)
            {
                GD.Print("Using battle return position");
                player.Position = gameState.BattleReturnPosition;
                gameState.ReturnFromBattle = false;
            }
            else
            {
                GD.Print("Using spawn trigger");
                int spawnTriggerId = gameState.GetSpawnTrigger();
                SpawnPlayerAtTrigger(spawnTriggerId);
            }
        }

        private void SpawnPlayerAtTrigger(int triggerIndex)
        {
            var spawnPoints = GetNodeOrNull("SpawnPoints");
            
            if (spawnPoints == null)
            {
                Logger.Info("SpawnPoints node not found, player stays at default position");
                return;
            }
            
            var spawnPoint = spawnPoints.GetNodeOrNull<Node2D>($"SpawnPoint{triggerIndex}");
            
            if (spawnPoint != null)
            {
                player.Position = spawnPoint.Position;
                Logger.Info($"Player spawned at trigger {triggerIndex}: {player.Position}");
            }
            else
            {
                var defaultSpawn = spawnPoints.GetNodeOrNull<Node2D>("SpawnPoint0");
                if (defaultSpawn != null)
                {
                    player.Position = defaultSpawn.Position;
                    Logger.Info($"SpawnPoint{triggerIndex} not found, using default: {player.Position}");
                }
            }
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