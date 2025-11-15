using Godot;
using Game.core;

namespace Game.UI;

public partial class MainMenu : Control
{
    private Button _startButton;
    private Button _continueButton;
    private Button _exitButton;
    private Tween _blinkTween;
    private bool _isTransitioning = false;

    public override void _Ready()
    {
        _startButton = FindButton("StartButton");
        _continueButton = FindButton("ContinueButton");
        _exitButton = FindButton("ExitButton");

        if (_startButton != null)
        {
            _startButton.Pressed += OnStartPressed;
            StartBlinkAnimation(_startButton);
        }
        

        if (_continueButton != null)
        {
            bool hasSave = SaveManager.Instance?.HasSaveFile() ?? false;
            _continueButton.Disabled = !hasSave;
            _continueButton.Pressed += OnContinuePressed;

            if (!hasSave)
            {
                _continueButton.Modulate = new Color(1, 1, 1, 0.3f);
            }
            
        }

        if (_exitButton != null)
        {
            _exitButton.Pressed += OnExitPressed;
        }

        FadeInMenu();
    }

    private Button FindButton(string name)
    {
        return FindNodeByName<Button>(this, name);
    }

    private T FindNodeByName<T>(Node parent, string name) where T : Node
    {
        if (parent.Name == name && parent is T result)
            return result;

        foreach (Node child in parent.GetChildren())
        {
            var found = FindNodeByName<T>(child, name);
            if (found != null)
                return found;
        }

        return null;
    }

    private void FadeInMenu()
    {
        Modulate = new Color(1, 1, 1, 0);
        var tween = CreateTween();
        tween.TweenProperty(this, "modulate:a", 1.0f, 1.0f);
    }

    private void StartBlinkAnimation(Button button)
    {
        _blinkTween = CreateTween();
        _blinkTween.SetLoops();
        _blinkTween.TweenProperty(button, "modulate:a", 0.4f, 0.8f);
        _blinkTween.TweenProperty(button, "modulate:a", 1.0f, 0.8f);
    }

private void OnStartPressed()
{
    if (_isTransitioning)
        return;

    _isTransitioning = true;
    StopBlinking();

    if (GameState.Instance.ReturnFromBattle)
    {
        SceneTransition.GoTo(GameState.Instance.CurrentScene);
        return;
    }

    bool hasSave = SaveManager.Instance?.HasSaveFile() ?? false;

    if (hasSave)
    {
        
        SaveManager.Instance.LoadGame();
        GameState.Instance.GameLoadedFromSave = true;
        
        SceneTransition.GoTo(GameState.Instance.CurrentScene);
    }
    else
    {
        GD.Print("--- NEW GAME ---");
        
        GameState.Instance.IsNewGame = true;
        GameState.Instance.GameLoadedFromSave = false;
        GameState.Instance.Money = 1000;
        GameState.Instance.PlayerPokemons.Clear();
        GameState.Instance.PlayTime = 0f;
        GameState.Instance.CurrentScene = "small_town";
        GameState.Instance.LastSpawnTrigger = 0;
        GameState.Instance.PlayerPosition = Vector2.Zero;
        
        SceneTransition.GoTo("small_town");
    }
}
    private void OnContinuePressed()
    {
        GD.Print("--- CONTINUE GAME ---");

        if (_isTransitioning)
            return;

        _isTransitioning = true;
        StopBlinking();

        if (SaveManager.Instance != null)
        {
            bool loaded = SaveManager.Instance.LoadGame();

            if (loaded)
            {
                GameState.Instance.GameLoadedFromSave = true;
                SceneTransition.GoTo(GameState.Instance.CurrentScene);
            }
            else
            {
                _isTransitioning = false;
            }
        }
    }

    private void OnExitPressed()
    {
        GetTree().Quit();
    }

    private void StopBlinking()
    {
        if (_blinkTween != null)
        {
            _blinkTween.Kill();
        }

        if (_startButton != null)
        {
            _startButton.Disabled = true;
            _startButton.Modulate = new Color(1, 1, 1, 1);
        }
    }

    public override void _ExitTree()
    {
        if (_blinkTween != null)
        {
            _blinkTween.Kill();
        }
    }
}