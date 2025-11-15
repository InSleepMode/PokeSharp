using Godot;
using Game.core;
using Game.core.Data;

namespace Game.Overworld
{
    public partial class GrassTrigger : Area2D
    {
        [Export] public int EncounterChance = 30;
        
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

    var random = new System.Random();
    int roll = random.Next(0, 100);
    
    if (roll > EncounterChance) return;
    
    _triggered = true;
    
    GameState.Instance.WildPokemon = PokemonData.GetRandomWildPokemon();
    
    Vector2 playerPosition = body.GlobalPosition;
    
    SceneTransition.GoToBattle(playerPosition);
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