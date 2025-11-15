#if TOOLS
using Game.core;
using Godot;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Game.Gameplay;
using System.Collections.Generic;
using Game.Core;

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

public class StatChangeEntry
{
	[JsonProperty("change")]
	public int Change { get; set; }
	[JsonProperty("stat")]
	public ApiResource Stat { get; set; }
}

public class MoveApiResponce
{
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("accuracy")]
		public int? Accuracy { get; set; }
		[JsonProperty("effect_chance")]
		public int? EffectChance { get; set; }
		[JsonProperty("pp")]
		public int? Pp { get; set; }
		[JsonProperty("priority")]
		public int Priority { get; set; }
		[JsonProperty("power")]
		public int? Power { get; set; }
		[JsonProperty("target")]
		public ApiResource Target { get; set; }
		[JsonProperty("damage_class")]
		public ApiResource DamageClass { get; set; }
		[JsonProperty("generation")]
		public ApiResource Generation { get; set; }
		[JsonProperty("meta")]
		public MoveMeta Meta { get; set; }
		[JsonProperty("stat_changes")]
		public List<StatChangeEntry> StatChanges { get; set; }
		[JsonProperty("type")]
		public ApiResource Type { get; set; }
}

[Tool]
public partial class MoveImporter : EditorPlugin
{
	private const string importMenuItemText = "Import Moves";
	private const string folderPath = "res://resources/moves/";
	private const string apiPath = "https://pokeapi.co/api/v2/move/";	
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

			MoveApiResponce data = await Modules.FetchDataFromPokeApi<MoveApiResponce>($"{apiPath}{i}");

			if (data == null)
			{
				continue;
			}

			var generation = data.Generation?.Name;
			if (generation != "generation-i")
			{
				Logger.Warning($"Move {i} is not from Gen 1 ...");
				continue;
			}

			var moveName = data.Name;
			if (string.IsNullOrEmpty(moveName))
			{
				Logger.Warning($"Move {i} has no name ...");
				continue;
			}

			Logger.Info($"Creating resource for {moveName}...");

			CreateMoveResource(moveName, data); 

			if (i % gcInterval == 0)
			{
				GC.Collect();
				GC.WaitForPendingFinalizers();
				Logger.Info("Garbage collected!");
			}
			await Task.Delay(100);
		}
	}
	
	private void CreateMoveResource(string moveName, MoveApiResponce apiData)
    {
		var move = new MoveResource
		{
			Name = moveName,
			PokemonType = PokemonEnum.TypeMap.TryGetValue(apiData.Type?.Name ?? "", out var type) ? type : PokemonType.None,
			Category = MovesEnum.CategoryMap.TryGetValue(apiData.DamageClass?.Name ?? "", out var category) ? category : MoveCategory.Physical,
			Target = MovesEnum.MoveTargetMap.TryGetValue(apiData.Target?.Name ?? "", out var target) ? target : MoveTarget.SelectedPokemon,

			Accuracy = apiData.Accuracy ?? 0,
			PP = apiData.Pp ?? 0,
			Power = apiData.Power ?? 0,
			CritRate = apiData.Meta?.CritRate ?? 0,
			Drain = apiData.Meta?.Drain ?? 0,
			FlinchChance = apiData.Meta?.Drain ?? 0,
			Healing = apiData.Meta?.Healing ?? 0,
			MaxHits = apiData.Meta?.MaxHits ?? -1,
			MaxTurns = apiData.Meta?.MaxTurns ?? -1,
			MinHits = apiData.Meta?.MinHits ?? -1,
			MinTurns = apiData.Meta?.MinTurns ?? -1,

			AilmentChance = apiData.Meta?.AilmentChance ?? 0,
			Ailment = PokemonEnum.AilmentMap.TryGetValue(apiData.Meta?.Ailment?.Name ?? "", out var ailment) ? ailment : PokemonAilment.None,
			StatChanges = []
		};

		if (apiData.StatChanges != null)
		{
			foreach (var change in apiData.StatChanges)
			{
				if (PokemonEnum.StatMap.TryGetValue(change.Stat?.Name ?? "", out var stat))
				{
					move.StatChanges[stat] = change.Change;
				}
			}
		}

		var savePath = $"{folderPath}{moveName.ToLower()}.tres";
		var error = ResourceSaver.Save(move, savePath);

		if(error != Error.Ok)
        {
			Logger.Error($"There was a problem with saveng move {moveName} to {savePath}: {error}");

        }
    }
}
#endif
