using Godot;

namespace Game.Gameplay.Battle
{
    public partial class Pokemon : Resource
    {
        [Export] public string Name { get; set; } = "Pikachu";
        [Export] public int maxHP { get; set; } = 20;
        [Export] public int currentHP { get; set; } = 20;
        [Export] public int AttackPower { get; set; } = 4;

        public bool isKilled => currentHP <= 0;

        public void TakeDamage(int damage)
        {
            currentHP -= damage;
        }
    }
}