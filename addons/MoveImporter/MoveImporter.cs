#if TOOLS
using Game.core;
using Godot;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class ApiResource
{
	[JsonProperty("name")]
	public string Name { get; set; }
	[JsonProperty("url")]
	public string Url { get; set; }
}

public class EffectEntry
{
	[JsonProperty("effect")]
	public string Effect { get; set; }

	[JsonProperty("short_effect")]
	public string ShortEffect { get; set; }

	[JsonProperty("Language")]
	public ApiResource Language { get; set; }

	[JsonProperty("version_group")]
	public ApiResource VersionGroup { get; set; }

}


public class MoveMeta
{
	[JsonProperty("ailment")]
	public ApiResource Ailment { get; set; }

	[JsonProperty("category")]
	public ApiResource Category { get; set; }
	[JsonProperty("crit_rate")]
	public int? CritRate { get; set; }
	[JsonProperty("drain")]
	public int? Drain { get; set; }

	[JsonProperty("flinch_chance")]
	public int? FlinchChance { get; set; }

	[JsonProperty("healing")]
	public int? Healing { get; set; }

	[JsonProperty("max_hits")]
	public int? MaxHits { get; set; }

	[JsonProperty("max_turns")]
	public int? MaxTurns { get; set; }
	[JsonProperty("min_hits")]
	public int? MinHits { get; set; }

	[JsonProperty("min_turns")]
	public int? MinTurns { get; set; }
	[JsonProperty("stat_chance")]
	public int? StatChance { get; set; }

	[JsonProperty("ailment_chance")]
	public int? AilmentChance { get; set; }



}

[Tool]
public partial class MoveImporter : EditorPlugin
{
	private const string importMenuItemText = "Import Moves";
	private const string folderPath = "res://resources/moves/";
	private const string apiPath = "https://pokeapi.co/api/vs2/move/";	
	public override void _EnterTree()
    {
		AddToolMenuItem(importMenuItemText, Callable.From(ImportMoves)); 
    }

	public override void _ExitTree()
	{
		RemoveToolMenuItem(importMenuItemText);
	}

	public async void ImportMoves()
	{
		Logger.Info("Attempting to import moves ...");
		DirAccess.MakeDirRecursiveAbsolute(ProjectSettings.GlobalizePath(folderPath));

		const int gcInterval = 10;

		for (int i = 1; i <= Globals.MOVE_NUMBERS; i++)
		{
			Logger.Info($"Processing move with ID {i}");

			//MoveApiResponse data = await Modules.FetchDataFromPokeApi<MoveApiResponse>($"{apiPath}{i}");

			// if (data == null)
			// {
			// 	continue;
			// }

			// var generation = data.Generation?.Name;
			// if (generation != "generation-i")
			// {
			// 	Logger.Warning($"Move {i} is not from Gen 1 ...");
			// 	continue;
			// }

			// var moveName = data.Name;
			// if (string.IsNullOrEmpty(moveName))
			// {
			// 	Logger.Warning($"Move {i} has no name ...");
			// 	continue;
			// }

			// Logger.Info($"Creating resource for {moveName}...");

			// if (i % gcInterval == 0)
			// {
			// 	GC.Collect();
			// 	GC.WaitForPendingFinalizers();
			// 	Logger.Info("Garbage collected!");
			// }
			// await Task.Delay(100);
		}
	}
	
	
}
#endif
