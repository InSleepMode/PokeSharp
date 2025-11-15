using Godot;
using System.Collections.Generic;
using System.Text.Json;
using Game.core.Data;

namespace Game.core;

public class SaveData
{
    public string PlayerName { get; set; } = "Ash";
    public int Money { get; set; } = 1000;
    public float PlayTime { get; set; } = 0f;
    public string CurrentScene { get; set; } = "small_town";
    public float PlayerX { get; set; } = 400f;
    public float PlayerY { get; set; } = 300f;
    public int LastSpawnTrigger { get; set; } = 0;
    public List<SavedPokemon> PlayerPokemons { get; set; } = new List<SavedPokemon>();

    public int BattlesWon { get; set; } = 0;
    public int PokemonCaught { get; set; } = 0;

}

public class SavedPokemon
{
    public string Name { get; set; }
    public int Level { get; set; }
    public int HP { get; set; }
    public int MaxHP { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
}

public partial class SaveManager : Node
{
    public static SaveManager Instance { get; private set; }
    
    private const string SAVE_PATH = "user://savegame.json";

    public override void _Ready()
    {
        Instance = this;
    }

    public void SaveGame()
    {

        var gameState = GameState.Instance;
        if (gameState == null)
        {
            return;
        }

        var currentScene = GetTree().CurrentScene;

        var player = currentScene?.GetNodeOrNull<CharacterBody2D>("Player");

        if (player != null)
        {
            gameState.PlayerPosition = player.Position;
        }

        var saveData = new SaveData
        {
            PlayerName = "Ash",
            Money = gameState.Money,
            PlayTime = gameState.PlayTime,
            CurrentScene = gameState.CurrentScene,
            PlayerX = gameState.PlayerPosition.X,
            PlayerY = gameState.PlayerPosition.Y,
            LastSpawnTrigger = gameState.LastSpawnTrigger,
            PlayerPokemons = new List<SavedPokemon>(),

            BattlesWon = gameState.BattlesWon,
            PokemonCaught = gameState.PokemonCaught
        };

        foreach (var pokemon in gameState.PlayerPokemons)
        {
            saveData.PlayerPokemons.Add(new SavedPokemon
            {
                Name = pokemon.Name,
                Level = pokemon.Level,
                HP = pokemon.HP,
                MaxHP = pokemon.MaxHP,
                Attack = pokemon.Attack,
                Defense = pokemon.Defense
            });
        }

        string jsonString = JsonSerializer.Serialize(saveData, new JsonSerializerOptions
        {
            WriteIndented = true
        });


        using var file = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Write);
        if (file == null)
        {
            return;
        }

        file.StoreString(jsonString);
        file.Close();
    }
    
    public bool LoadGame()
{
    if (!FileAccess.FileExists(SAVE_PATH))
    {
        return false;
    }
    
    using var file = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Read);
    if (file == null)
    {
        return false;
    }
    
    string jsonString = file.GetAsText();
    file.Close();
    
    SaveData saveData;
    try
    {
        saveData = JsonSerializer.Deserialize<SaveData>(jsonString);
    }
    catch (System.Exception e)
    {
        return false;
    }
    
    if (saveData == null)
    {
        return false;
    }
    
    var gameState = GameState.Instance;
    if (gameState == null)
    {
        return false;
    }
    
    gameState.Money = saveData.Money;
    gameState.PlayTime = saveData.PlayTime;
    gameState.CurrentScene = saveData.CurrentScene;
    gameState.PlayerPosition = new Vector2(saveData.PlayerX, saveData.PlayerY);
    gameState.LastSpawnTrigger = saveData.LastSpawnTrigger;
    gameState.BattlesWon = saveData.BattlesWon;
    gameState.PokemonCaught = saveData.PokemonCaught;

    gameState.PlayerPokemons.Clear();
    foreach (var savedPokemon in saveData.PlayerPokemons)
    {
        gameState.PlayerPokemons.Add(new PokemonData
        {
            Name = savedPokemon.Name,
            Level = savedPokemon.Level,
            HP = savedPokemon.HP,
            MaxHP = savedPokemon.MaxHP,
            Attack = savedPokemon.Attack,
            Defense = savedPokemon.Defense
        });
    }
    
    return true;
}

    public bool HasSaveFile()
    {
        return FileAccess.FileExists(SAVE_PATH);
    }
    
    public void DeleteSave()
    {
        if (FileAccess.FileExists(SAVE_PATH))
        {
            DirAccess.RemoveAbsolute(SAVE_PATH);
        }
    }
}