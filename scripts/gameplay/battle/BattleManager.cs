using Godot;

namespace Game.Gameplay.Battle
{
    public partial class BattleManager : Node
    {
        private Pokemon _myPokemon;
        private Pokemon _enemyPokemon;




        public void StartBattle(Pokemon mine, Pokemon enemy)
        {
            _myPokemon = mine;
            _enemyPokemon = enemy;
            GD.Print($"Battle started: {mine.Name} vs {enemy.Name}");

        }

        public void PlayerAttack()
        {

            if (_enemyPokemon.isKilled) return;

            _enemyPokemon.TakeDamage(_myPokemon.AttackPower);
            GD.Print($"{_myPokemon.Name} attacks! {_enemyPokemon.Name} has {_enemyPokemon.currentHP} HP");

            if (_enemyPokemon.isKilled)
            {
                EndBattle(true);
            }
            else
            {
                EnemyTurn();
            }
        }

        private void EnemyTurn()
        {
            _myPokemon.TakeDamage(_enemyPokemon.AttackPower);
            GD.Print($"{_enemyPokemon.Name} attacks! {_myPokemon.Name} has {_myPokemon.currentHP} HP");
            if (_myPokemon.isKilled) EndBattle(false);
        }

        private void EndBattle(bool playerWon)
        {
            if (playerWon) GD.Print($"{_myPokemon.Name} won this battle!!!");
            else GD.Print($"{_enemyPokemon.Name} won this battle!");
        }

  
        
    }

}