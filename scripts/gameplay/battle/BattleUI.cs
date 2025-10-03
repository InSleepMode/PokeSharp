using Godot;

namespace Game.Gameplay.Battle
{
    public partial class BattleManager : Node
    {
        [Export] private NodePath _battleManagerPath;
        private BattleManager _battleManager;
        private Label _message;
        private Label _playerName;
        private Label _playerHP;
        private Label _enemyName;
        private Label _enemyHP;

        public override void _Ready()
        {
            _message = GetNode<Label>("MessageLabel");
            _playerHP = GetNode<Label>("PlayerInfo/PlayerHP");
            _enemyHP = GetNode<Label>("EnemyInfo/EnemyHP");
            _playerName = GetNode<Label>("PlayerInfo/PlayerName");
            _enemyName = GetNode<Label>("EnemyInfo/EnemyName");

            GetNode<Button>("AttackButtons/AttackButton1").Pressed += OnAttackButton1Pressed;
            GetNode<Button>("AttackButtons/AttackButton2").Pressed += OnAttackButton2Pressed;
            GetNode<Button>("AttackButtons/AttackButton3").Pressed += OnAttackButton3Pressed;
            GetNode<Button>("AttackButtons/AttackButton4").Pressed += OnAttackButton4Pressed;


        }

        private void UpdateUI()
        {
            _playerHP.Text = $"{_myPokemon.Name}: {_myPokemon.currentHP}/{_myPokemon.maxHP}";
            _enemyHP.Text = $"{_enemyPokemon.Name}: {_enemyPokemon.currentHP}/{_enemyPokemon.maxHP}";
        }


        private void ShowMessage(string msg)
        {
            GD.Print(msg);
            _message.Text = msg;
        }

        private void OnAttackButton1Pressed()
        {
            _battleManager.PlayerAttack();
            UpdateUI();
        }

        private void OnAttackButton2Pressed()
        {
            _battleManager.PlayerAttack();
            UpdateUI();
        }

        private void OnAttackButton3Pressed()
        {
            _battleManager.PlayerAttack();
            UpdateUI();
        }

        private void OnAttackButton4Pressed()
        {
            _battleManager.PlayerAttack();
            UpdateUI();
        }
    }
}