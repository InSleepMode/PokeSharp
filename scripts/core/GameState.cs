using Godot;
using System.Collections.Generic;
using Game.core.Data;

public partial class GameState : Node
{
    public static GameState Instance { get; private set; }
    
    public bool GameLoadedFromSave = false;
    public bool IsNewGame = false;
    
    public int Money = 1000;
    public Vector2 PlayerPosition = Vector2.Zero;
    public string CurrentScene = "small_town";
    public string LastScene = "small_town";
    public int LastSpawnTrigger = 0;
    public List<PokemonData> PlayerPokemons = new List<PokemonData>();
    public bool ReturnFromBattle = false;
    public Vector2 BattleReturnPosition = Vector2.Zero;
    public float PlayTime = 0f;
    public PokemonData WildPokemon;

    public int BattlesWon = 0;
    public int PokemonCaught = 0;

    public override void _Ready()
    {
        Instance = this;
    }
    
    public override void _Process(double delta)
    {
        PlayTime += (float)delta;
    }
    
    public void SaveSpawnTrigger(int triggerId)
    {
        LastSpawnTrigger = triggerId;
    }
    
    public int GetSpawnTrigger()
    {
        return LastSpawnTrigger;
    }

    public void CatchPokemon(PokemonData pokemon)
    {
        PlayerPokemons.Add(pokemon);
        ++PokemonCaught;
    }
    
    public void WinBattle()
    {
        ++BattlesWon;
    }
}