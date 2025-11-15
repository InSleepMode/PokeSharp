using Godot;
using Vector2 = Godot.Vector2;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

public partial class SceneTransition : CanvasLayer
{
    public static SceneTransition Instance { get; private set; }

    private ColorRect _fadeRect;
    private bool _isTransitioning = false;

    private Vector2 _battleReturnPosition = Vector2.Zero;
    private string _battleReturnScene = "small_town";

    public override void _Ready()
    {
        Instance = this;

        _fadeRect = new ColorRect();
        _fadeRect.Color = new Color(0, 0, 0, 0);
        _fadeRect.MouseFilter = Control.MouseFilterEnum.Ignore;
        _fadeRect.SetAnchorsPreset(Control.LayoutPreset.FullRect);
        AddChild(_fadeRect);

    }

    public static void GoTo(string sceneName, Vector2? spawnPosition = null)
    {

        if (Instance == null)
        {
            GD.PrintErr("ERROR: Instance is NULL!");
            return;
        }

        if (Instance._isTransitioning)
        {
            return;
        }

        if (sceneName.ToLower() == "battle" || sceneName.ToLower() == "battlescene")
        {
            var currentScene = Instance.GetTree().CurrentScene;
            var player = currentScene?.GetNodeOrNull<CharacterBody2D>("Player");

            if (player != null)
            {
                GoToBattle(player.GlobalPosition);
                return;
            }
            else
            {
                Vector2 pos = GameState.Instance.PlayerPosition;
                GoToBattle(pos);
                return;
            }
        }

        Instance.StartTransition(sceneName);
    }

    public static void GoToBattle(Vector2 playerPosition)
    {
        if (Instance == null || Instance._isTransitioning) return;

        Instance._battleReturnPosition = playerPosition;
        Instance._battleReturnScene = GameState.Instance.CurrentScene;

        if (GameState.Instance != null)
        {
            GameState.Instance.ReturnFromBattle = true;
            GameState.Instance.BattleReturnPosition = playerPosition;
            GameState.Instance.LastSpawnTrigger = 0;

        }

        string[] battleScenes =
        {
            "battle_scene",
            "battle_scene_rattata",
            "battle_scene_squirtle"
        };

        var random = new System.Random();
        string selectedBattle = battleScenes[random.Next(battleScenes.Length)];

        Instance.StartTransition(selectedBattle);

    }

    public static void GoBack()
    {
        if (Instance == null) return;

        if (GameState.Instance != null && GameState.Instance.ReturnFromBattle)
        {
            GoTo(Instance._battleReturnScene);
        }
        else if (GameState.Instance != null)
        {
            var lastScene = GameState.Instance.LastScene;
            GoTo(lastScene);
        }
    }

    private async void StartTransition(string sceneName)
    {
        _isTransitioning = true;

        try
        {
            await FadeOut();

            GameState.Instance.LastScene = GameState.Instance.CurrentScene;
            GameState.Instance.CurrentScene = sceneName;

            var scenePath = GetScenePath(sceneName);

            if (!ResourceLoader.Exists(scenePath))
            {
                GD.PrintErr($"Scene file NOT FOUND: {scenePath}");
                _isTransitioning = false;
                return;
            }

            GD.Print("Changing scene...");
            var error = GetTree().ChangeSceneToFile(scenePath);
            GD.Print($"ChangeSceneToFile returned: {error}");

            if (error != Error.Ok)
            {
                GD.PrintErr($"Failed to change scene: {error}");
                _isTransitioning = false;
                return;
            }

            GD.Print("Waiting for scene load...");
            await ToSignal(GetTree(), SceneTree.SignalName.NodeAdded);
            await ToSignal(GetTree().CreateTimer(0.1), Timer.SignalName.Timeout);

            await FadeIn();

            _isTransitioning = false;
            GD.Print("Transition complete!");
        }
        catch (System.Exception e)
        {
            GD.PrintErr($"Exception in StartTransition: {e}");
            _isTransitioning = false;
        }
    }

    private async Task FadeOut()
    {
        var tween = CreateTween();
        tween.TweenProperty(_fadeRect, "color:a", 1.0f, 0.3f);
        await ToSignal(tween, Tween.SignalName.Finished);
    }

    private async Task FadeIn()
    {
        var tween = CreateTween();
        tween.TweenProperty(_fadeRect, "color:a", 0.0f, 0.3f);
        await ToSignal(tween, Tween.SignalName.Finished);
    }

    private string GetScenePath(string sceneName)
    {
        string normalized = sceneName.Replace("_", "").ToLower();

        return normalized switch
        {
            "mainmenu" => "res://main_menu.tscn",
            "menu" => "res://main_menu.tscn",
            "smalltown" => "res://scenes/levels/small_town.tscn",
            "battlescene" => "res://scenes/levels/battle_scene.tscn",
            "battle" => "res://scenes/levels/battle_scene.tscn",
            "battlescenerattata" => "res://scenes/levels/battle_scene_rattata.tscn",
            "battlescenesquirtle" => "res://scenes/levels/battle_scene_squirtle.tscn",
        

            "smalltowngreenhouse" => "res://scenes/levels/small_town_green_house.tscn",
            "smalltowngreenshouse" => "res://scenes/levels/small_town_green_house.tscn",
            "smalltownpurpleshouse" => "res://scenes/levels/small_town_purples_house.tscn",
            "smalltownpokemonscenter" => "res://scenes/levels/small_town_pokemons_center.tscn",
            "smalltowncave" => "res://scenes/levels/small_town_cave.tscn",

            "overworld" => "res://scenes/levels/small_town.tscn",
            "house1" => "res://scenes/levels/small_town_green_house.tscn",
            "house2" => "res://scenes/levels/small_town_purples_house.tscn",
            "pokemoncenter" => "res://scenes/levels/small_town_pokemons_center.tscn",
            "cave" => "res://scenes/levels/small_town_cave.tscn",
            "shop" => "res://scenes/levels/shop.tscn",
            "shop2" => "res://scenes/levels/shop2.tscn",
            "room1" => "res://scenes/levels/room1.tscn",
            "room2" => "res://scenes/levels/room2.tscn",
            "room3" => "res://scenes/levels/room3.tscn",
            "room4" => "res://scenes/levels/room4.tscn",
            "cozyhouse" => "res://scenes/levels/cozy_house.tscn",
            "gamblinghouse" => "res://scenes/levels/gambling_house.tscn",
            "school" => "res://scenes/levels/school.tscn",
            "winterhouse1" => "res://scenes/levels/winter_house1.tscn",
            "winterhouse2" => "res://scenes/levels/winter_house2.tscn",
            "winterhouse3" => "res://scenes/levels/winter_house3.tscn",
            "winterhouse4" => "res://scenes/levels/winter_house4.tscn",
            "gym" => "res://scenes/levels/gym.tscn",

            _ => "res://scenes/levels/small_town.tscn"
        };
    }
}