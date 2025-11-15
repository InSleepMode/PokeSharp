using Godot;

namespace Game.UI;

public partial class MenuController : Node
{
    private PackedScene _menuScene;
    private CanvasLayer _menuInstance;
    private bool _isMenuOpen = false;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        
        _menuScene = GD.Load<PackedScene>("res://menu.tscn");
        
        if (_menuScene == null)
        {
            GD.PrintErr("Failed to load menu");
        }
        else
        {
            GD.Print("menu loaded successfully");
        }
        
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel") && !_isMenuOpen)
        {
            OpenMenu();
            GetViewport().SetInputAsHandled();
        }
    }

    private void OpenMenu()
    {
        if (_isMenuOpen)
        {
            return;
        }

        _isMenuOpen = true;
        
        GD.Print("---Opening menu---");
        
        try
        {
            if (_menuScene == null)
            {
                _isMenuOpen = false;
                return;
            }

            _menuInstance = _menuScene.Instantiate<CanvasLayer>();
            
            if (_menuInstance == null)
            {
                _isMenuOpen = false;
                return;
            }
            
            _menuInstance.Visible = true;
            _menuInstance.ProcessMode = ProcessModeEnum.Always;
            
            GetTree().Root.AddChild(_menuInstance);
            
            _menuInstance.TreeExited += OnMenuClosed;

            GetTree().Paused = true;
            
        }
        catch (System.Exception e)
        {
            _isMenuOpen = false;
        }
    }

    private void OnMenuClosed()
    {
        GetTree().Paused = false;
        _isMenuOpen = false;
    }
}