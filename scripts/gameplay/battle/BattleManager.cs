using Godot;
using Game.core.Data;
using Game.Gameplay;
using Game.Utilities;
using System;

namespace Game.Gameplay.Battle
{
    public partial class BattleManager : Node
    {
        
        public TextureRect _playerSprite;
        public TextureRect _enemySprite;
        private Label _enemyName;
        private Label _enemyHP;
        private PokemonData _enemyPokemon;


        public override void _Ready()
        {
            GD.Print("[BattleManager] StartBattle");
            _playerSprite = GetNode<TextureRect>("PlayerPokemon");
            _enemySprite = GetNode<TextureRect>("EnemyPokemon");
            _enemyName = GetNode<Label>("UI/EnemyName");
            _enemyHP = GetNode<Label>("UI/EnemyHP");

            StartBattle();

        }

        private void StartBattle()
        {
            var pokemons = PokemonData.GetAll();
            var random = new Random();
            _enemyPokemon = pokemons[random.Next(pokemons.Count)];

            _enemySprite.Texture = _enemyPokemon.Texture;
            _enemyName.Text = _enemyPokemon.Name;
            _enemyHP.Text = $"HP: {_enemyPokemon.HP}";

        }
        


        public void EndBattle()
        {
            GD.Print("[BattleManager] Ending battle...");
            GetTree().ChangeSceneToFile("res://scenes/levels/small_town.tscn");
        }

        public void OnRunPressed()
        {
            EndBattle();
        }
        
    }

}