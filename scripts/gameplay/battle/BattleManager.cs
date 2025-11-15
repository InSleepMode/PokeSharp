
using Godot;
using System;
using Game.core.Data;

namespace Game.Gameplay.Battle;

public partial class BattleManager : Node
{
    [Export] private ProgressBar _playerHPBar;
    [Export] private ProgressBar _enemyHPBar;
    [Export] private Label _playerHPLabel;
    [Export] private Label _enemyHPLabel;
    [Export] private Label _enemyNameLabel;

    private int playerHP = 100;
    private int playerMaxHP = 100;
    private int enemyHP = 50;
    private int enemyMaxHP = 50;
    private PokemonData wildPokemon;
    private Random random = new Random();

    public override void _Ready()
    {

        wildPokemon = GameState.Instance?.WildPokemon;

        if (wildPokemon != null)
        {
            enemyHP = wildPokemon.MaxHP;
            enemyMaxHP = wildPokemon.MaxHP;

            if (_enemyNameLabel != null)
            {
                _enemyNameLabel.Text = $"Wild {wildPokemon.Name}";
            }
        }
        else
        {
            if (_enemyNameLabel != null)
            {
                _enemyNameLabel.Text = "Wild Pokemon";
            }
        }

        UpdateHPBars();
        StartBattle();
    }

    private void StartBattle()
    {
        if (wildPokemon != null)
        {
            MessageManager.PlayText($"Wild {wildPokemon.Name} appeared!");
        }
        else
        {
            MessageManager.PlayText("A wild Pokemon appeared!");
        }
    }

    private void UpdateHPBars()
    {
        if (_playerHPBar != null)
        {
            _playerHPBar.MaxValue = playerMaxHP;
            _playerHPBar.Value = playerHP;
        }

        if (_enemyHPBar != null)
        {
            _enemyHPBar.MaxValue = enemyMaxHP;
            _enemyHPBar.Value = enemyHP;
        }

        if (_playerHPLabel != null)
        {
            _playerHPLabel.Text = $"{playerHP}/{playerMaxHP}";
        }

        if (_enemyHPLabel != null)
        {
            _enemyHPLabel.Text = $"{enemyHP}/{enemyMaxHP}";
        }
    }

    public void playerAttack()
    {
        int damage = random.Next(10, 26);
        enemyHP -= damage;

        if (enemyHP < 0)
            enemyHP = 0;

        MessageManager.PlayText($"You dealt {damage} damage!");
        UpdateHPBars();

        if (enemyHP <= 0)
        {
            MessageManager.PlayText("Enemy fainted!", "You won!");
            GameState.Instance.WinBattle();//вызовем функцию по увеличению числа побед в бою
            GetTree().CreateTimer(2.0).Timeout += EndBattle;
        }
        else
        {
            GetTree().CreateTimer(1.5).Timeout += EnemyAttack;
        }
    }

    private void EnemyAttack()
    {
        int damage = random.Next(5, 16);
        playerHP -= damage;

        if (playerHP < 0)
            playerHP = 0;

        MessageManager.PlayText($"Enemy dealt {damage} damage!");
        UpdateHPBars();

        if (playerHP <= 0)
        {
            MessageManager.PlayText("You fainted!", "Game Over!");
            GetTree().CreateTimer(2.0).Timeout += EndBattle;
        }
    }

    public void catchPokemon()
    {
        bool caught = random.Next(0, 2) == 0;

        if (caught)
        {
            if (wildPokemon != null)
            {
                MessageManager.PlayText($"Gotcha! Enemy pokemon was caught!");
                GameState.Instance.CatchPokemon(wildPokemon);
            }
            else
            {
                MessageManager.PlayText("Gotcha! Pokemon was caught!");
            }

            GetTree().CreateTimer(2.0).Timeout += EndBattle;
        }
        else
        {
            MessageManager.PlayText("Oh no! Pokemon broke free!");
            GetTree().CreateTimer(1.5).Timeout += EnemyAttack;
        }
    }

    public void runFromBattle()
    {
        MessageManager.PlayText("Got away safely!");
        GetTree().CreateTimer(1.5).Timeout += EndBattle;
    }

    private void EndBattle()
    {
        GD.Print("Battle ended");
        GetTree().Paused = false;
        SceneTransition.GoBack();
    }

}