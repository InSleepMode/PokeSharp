// using Godot;

// namespace Game.Gameplay.Battle
// {
//     public partial class UIManager : Control
//     {
//         [Export] private NodePath _battleManagerPath;
//         private BattleManager _battleManager;

//         public override void _Ready()
//         {
//             _battleManager = GetNode<BattleManager>(_battleManagerPath);

//             // Подключаем кнопку
//             GetNode<Button>("AttackButton1").Pressed += OnAttackButton1Pressed;
//         }

//         public void OnAttackButton1Pressed()
//         {
//             _battleManager.PlayerAttack();
//         }
//     }
// }
