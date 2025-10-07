using Godot;
using System;

public partial class BattleScene : Node2D
{
    [Export] public PackedScene PlayerPokemonScene;
    [Export] public PackedScene EnemyPokemonScene;

    [Export] public Node2D PlayerSpawn;
    [Export] public Node2D EnemySpawn;

    public override void _Ready()
    {
        SpawnPokemons();
    }

    private void SpawnPokemons()
    {
        if (PlayerPokemonScene != null && EnemyPokemonScene != null)
        {
            Node2D player = PlayerPokemonScene.Instantiate<Node2D>();
            Node2D enemy = EnemyPokemonScene.Instantiate<Node2D>();

            PlayerSpawn.AddChild(player);
            EnemySpawn.AddChild(enemy);
        }
        else
        {
            GD.PrintErr("Pokemons were not created!");
        }
    }
}
