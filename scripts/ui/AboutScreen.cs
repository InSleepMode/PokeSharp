using System.Runtime.CompilerServices;
using Godot;
using Microsoft.VisualBasic;

namespace Game.UI;

public partial class AboutaScreen : ControlChars{

    [Export] private Button _backButton;
    [Export] private RichTextLabel _githubLabel;

    public override void _Ready()
    {
        _backButton?.Connect("pressed", Callable.From(OnBackPressed));

        if (_githubLabel != null)
        {
            _githubLabel.BbcodeEnabled = true;
            _githubLabel.Text = "GitHub: InSleepMode";
            //TODO: add a link to github
        }
    }   
        private void OnBackPressed()
    {
        var menuScene = GD.Load<PackedScene>("res://scenes/ui/menu.tscn");
        GetTree().ChangeSceneToPacked(menuScene);
    } 
    

}
