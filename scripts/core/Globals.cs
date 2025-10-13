using Godot;



namespace Game.core
{

	public partial class Globals : Node
	{
		public static Globals Instance { get; private set; }

		public const int MOVE_NUMBERS = 165;
		public const int GRID_SIZE = 16; //allows to change in Godot app

		[ExportCategory("Gameplay")]
		[Export] public ulong Seed = 1337;

		private RandomNumberGenerator RandomNumberGenerator;

		public override void _Ready()
		{
			Instance = this;

			RandomNumberGenerator = new()
			{
				Seed = Seed
			};

			Logger.Info("Loading Globals ...");
		}

		public static RandomNumberGenerator GetRandomNumberGenerator()
		{
			return Instance.RandomNumberGenerator;
		}

	}
}

