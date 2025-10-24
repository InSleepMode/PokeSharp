using Godot;
using Game.core;
using Microsoft.VisualBasic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Dataflow;

namespace Game.UI;

public partial class MenuManager : Control
{
    [Export] private Button _profileButton;
    [Export] private Button _bagButton;
    [Export] private Button _saveButton;
    [Export] private Button _exitButton;
    [Export] private Button _aboutButton;

    [Export] private Control _profileScreen;
    [Export] private Control _bagScreen;
    [Export] private Control _aboutScreen;

    public override void _Ready()
    {

        _profileButton?.Connect("pressed", Callable.From(OnProfilePressed));
        _bagButton?.Connect("pressed", Callable.From(OnBagPressed));
        _saveButton?.Connect("pressed", Callable.From(OnSavePressed));
        _exitButton?.Connect("pressed", Callable.From(OnExitPressed));
        _aboutButton?.Connect("pressed", Callable.From(OnAboutPressed));

        HideAllScreens();
    }


    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            CloseMenu();
        }
    }

    private void OnProfilePressed()
    {
        if (_profileScreen != null)
        {
            ShowScreen(_profileScreen);
        }
        else
        {
            var profileScene = GD.Load<PackedScene>("res://scenes/ui/profile_screen.tscn");
            if (profileScene != null)
            {
                GetTree().ChangeSceneToPacked(profileScene);
            }
        }
    }

    private void OnBagPressed()
    {

        if (_bagScreen != null)
        {
            ShowScreen(_bagScreen);
        }
        else
        {
            GD.Print("Bag is unable");
        }
    }

    private void OnSavePressed()
    {
        SaveGame();

        ShowNotification("Game saved!");
    }


    private void OnExitPressed()
    {
        GetTree().Quit();
    }

    private void OnAboutPressed()
    {
        if (_aboutScreen != null)
        {
            ShowScreen(_aboutScreen);
        }
        else
        {
            GD.Print("=====About=====");
            GD.Print("PokeGodot version");
            GD.Print("Just for study use");
            GD.Print("Made by InSleepMode dev");

        }
    }

    private void SaveGame()
    {
        var gameState = GetNodeOrNull<GameState>("/root/GameState");
        if (gameState != null)
        {
            GD.Print("game saved");
            //TODO: save to file in json
        }
    }

    private void ShowScreen(ControlChars screen)
    {
        HideAllScreens();
        screen.Visible = true;
    }
    
    private void HideAllScreens()
    {
        if (_profileScreen != null) _profileScreen.Visible = false;
        if (_bagScreen != null) _bagScreen.Visible = false;
        if (_aboutScreen != null) _aboutScreen.Visible = false;

    }

    private void ShowNotification(string message)
    {
        GD.Print($"{message}");

    }

    private void CloseMenu()
    {
        GD.Print("Closing menu");
        Visible = false;
    }
}   
