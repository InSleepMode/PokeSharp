using Godot;

namespace Game.core
{
	public partial class GameManager : Node
	{
		public static GameManager Instance { get; private set; }

		public override void _Ready()
		{
			Instance = this;
		}

		public static Player GetPlayer()
		{
			if (Instance == null) return null;
			
			var currentScene = Instance.GetTree().CurrentScene;
			var player = FindPlayerInNode(currentScene);
			
			return player;
		}
		
		private static Player FindPlayerInNode(Node node)
		{
			if (node is Player player)
				return player;
			
			foreach (Node child in node.GetChildren())
			{
				var foundPlayer = FindPlayerInNode(child);
				if (foundPlayer != null)
					return foundPlayer;
			}
			
			return null;
		}
	}
}