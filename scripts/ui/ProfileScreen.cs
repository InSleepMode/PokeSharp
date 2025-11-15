using System.Reflection.Emit;
using Godot;
using Label = Godot.Label;
namespace Game.UI;

public partial class ProfileScreen : Control
{
    private Button _backButton;
    private Label _moneyLabel;
    private Label _caughtLabel;
    private Label _battlesLabel;
    private VBoxContainer _pokemonList;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;

        _backButton = FindNodeByName<Button>("BackButton");
        _moneyLabel = FindNodeByName<Label>("MoneyLabel");
        _caughtLabel = FindNodeByName<Label>("CaughtLabel");
        _pokemonList = FindNodeByName<VBoxContainer>("PokemonList");
        _battlesLabel = FindNodeByName<Label>("BattlesLabel");

        if (_backButton != null)
        {
            _backButton.ProcessMode = ProcessModeEnum.Always;
            _backButton.Pressed += OnBackPressed;
        }

        LoadProfile();
    }

    private T FindNodeByName<T>(string name) where T : Node
    {
        return FindNodeRecursive<T>(this, name);
    }

    private T FindNodeRecursive<T>(Node parent, string name) where T : Node
    {
        if (parent.Name == name && parent is T result)
            return result;

        foreach (Node child in parent.GetChildren())
        {
            var found = FindNodeRecursive<T>(child, name);
            if (found != null)
                return found;
        }

        return null;
    }

    private void LoadProfile()
    {
        var gameState = GameState.Instance;
        
        if (gameState == null)
        {
            GD.PrintErr("GameState not found!");
            return;
        }

        if (_moneyLabel != null)
        {
            _moneyLabel.Text = $"Money: ${gameState.Money}";
        }

        if (_caughtLabel != null)
        {
            _caughtLabel.Text = $"Caught: {gameState.PlayerPokemons.Count} Pokemon";
        }

        if (_battlesLabel != null)
        {
            _battlesLabel.Text = $"BattlesWon: {gameState.BattlesWon} ";
        }

        LoadPokemonList();
    }

    private void LoadPokemonList()
    {
        if (_pokemonList == null)
            return;

        foreach (Node child in _pokemonList.GetChildren())
        {
            child.QueueFree();
        }

        var gameState = GameState.Instance;
        int index = 1;

        foreach (var pokemon in gameState.PlayerPokemons)
        {
            var label = new Label();
            label.Text = $"{index}. {pokemon.Name} - Level.{pokemon.Level} (HP: {pokemon.HP}/{pokemon.MaxHP})";
            _pokemonList.AddChild(label);
            index++;
        }
    }

    private void OnBackPressed()
    {
        GD.Print("ProfileScreen: BACK pressed");
        QueueFree();
    }
}