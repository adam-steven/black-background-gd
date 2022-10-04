using Godot;
using Newtonsoft.Json;
using GdArray = Godot.Collections.Array;

public partial class Main
{
	private void SavePlayerStats() 
	{
		mainData.PlayerStats = player.GetStats();
	}

	private void SaveSpawnedUpgrades(string spawnedUpgrades) 
	{
		JsonSerializerSettings settings = new JsonSerializerSettings();
		settings.TypeNameHandling = TypeNameHandling.Objects;
		Scenes deserializedData = JsonConvert.DeserializeObject<Scenes>(spawnedUpgrades, settings);
		mainData.StoredUpgrades = deserializedData;
	}

	private void SaveSpawnedEnemies() 
	{
        return;
        GD.Print("SaveSpawnedEnemies");
        GdArray children = this.GetChildren();

		foreach (var child in children)
        {
            GD.Print($"{child.GetType()} {child.GetType() == typeof(Obstacles)}");
			// if (child.GetType() == typeof(Obstacles))
			// 	GD.Print(child);
        }
	}

}