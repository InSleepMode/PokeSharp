using Godot;
using Game.core;

namespace Game.Overworld
{
    public partial class SceneTrigger : Area2D
    {
        [Export] public string TargetScene = "small_town";
        [Export] public int SpawnTriggerIndex = 0;
        
        private bool _triggered = false;

        public override void _Ready()
        {
            BodyEntered += OnBodyEntered;
            BodyExited += OnBodyExited;
            
        }

        private void OnBodyEntered(Node2D body)
        {
            if (_triggered) return;
            
            if (body.Name != "Player") return;
            
            _triggered = true;
            
            GameState.Instance.SaveSpawnTrigger(SpawnTriggerIndex);
            
            SceneTransition.GoTo(TargetScene);
        }

        private void OnBodyExited(Node2D body)
        {
            if (body.Name == "Player")
            {
                _triggered = false;
            }
        }
    }
}