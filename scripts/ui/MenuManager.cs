using Godot;
using Game.core;

namespace Game.UI;

public partial class MenuManager : CanvasLayer
{
    private Button _bagButton;
    private Button _saveButton;
    private Button _exitButton;
    private Button _aboutButton;
    private Control _mainMenu;
    private Control _currentScreen;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        
        _mainMenu = GetNodeOrNull<Control>("NinePatchRect/VBoxContainer");
        
        _bagButton = FindNodeByName<Button>("Bag");
        _saveButton = FindNodeByName<Button>("Save");
        _exitButton = FindNodeByName<Button>("Exit");
        _aboutButton = FindNodeByName<Button>("About");

        if (_bagButton != null)
        {
            _bagButton.ProcessMode = ProcessModeEnum.Always;
            _bagButton.Pressed += OnBagPressed;
        }

        if (_saveButton != null)
        {
            _saveButton.ProcessMode = ProcessModeEnum.Always;
            _saveButton.Pressed += OnSavePressed;
        }

        if (_exitButton != null)
        {
            _exitButton.ProcessMode = ProcessModeEnum.Always;
            _exitButton.Pressed += OnExitPressed;
        }

        if (_aboutButton != null)
        {
            _aboutButton.ProcessMode = ProcessModeEnum.Always;
            _aboutButton.Pressed += OnAboutPressed;
        }

    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            
            if (_currentScreen != null)
            {
                _currentScreen.QueueFree();
                _currentScreen = null;
                
                if (_mainMenu != null)
                {
                    _mainMenu.Visible = true;
                }
                
                GetViewport().SetInputAsHandled();
            }
            else
            {
                // Закрываем всё меню
                GD.Print("Closing menu");
                QueueFree();
                GetViewport().SetInputAsHandled();
            }
        }
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

    private void OnBagPressed()
    {
        OpenScreen("res://scenes/levels/profile_screen.tscn");
    }

    private void OnSavePressed()
    {
        SaveGame();
    }

    private void OnExitPressed()
    {
        GetTree().Quit();
    }

    private void OnAboutPressed()
    {
        OpenScreen("res://about_screen.tscn");
    }

    private void SaveGame()
    {
        var saveManager = SaveManager.Instance;
        if (saveManager != null)
        {
            saveManager.SaveGame();
        }
        else
        {
            GD.PrintErr("SaveManager not found!");
        }
    }

    private void OpenScreen(string scenePath)
    {
        var screenScene = GD.Load<PackedScene>(scenePath);
        if (screenScene == null)
        {
            GD.PrintErr($"Failed to load {scenePath}");
            return;
        }

        _currentScreen = screenScene.Instantiate<Control>();
        AddChild(_currentScreen);

        _currentScreen.TreeExited += OnScreenClosed;

        if (_mainMenu != null)
        {
            _mainMenu.Visible = false;
        }

    }

    private void OnScreenClosed()
    {
        if (_mainMenu != null)
        {
            _mainMenu.Visible = true;
        }

        _currentScreen = null;
    }
}