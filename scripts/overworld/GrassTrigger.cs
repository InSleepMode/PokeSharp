using Godot;
using System;

namespace Game.Overworld
{
    public partial class GrassTrigger : Area2D
    {
        public override void _Ready()
        {
            BodyEntered += OnBodyEntered;

        }

        private void OnBodyEntered(Node2D body)
        {
            if (body.Name == "Player")
            {
                Random rand = new Random();
                if (rand.Next(5) == 0)
                {
                    GD.Print("Battle found!");
                    GetTree().ChangeSceneToFile("res://scenes/battle/battle_scene.tscn");
                }
            }
        }
    }
}