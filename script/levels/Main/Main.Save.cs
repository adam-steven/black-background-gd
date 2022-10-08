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

	private void ClearSpawnedUpgrades() 
	{
		mainData.StoredUpgrades = null;
	}

	private void SaveSpawnedEnemies() 
	{
        GdArray children = this.GetChildren();
		Scenes spawnedEnemies = new Scenes();

		foreach (var child in children)
        {
			if (!child.GetType().IsSubclassOf(typeof(Obstacles))) { continue; }
			Obstacles enemy = (Obstacles)child;
			if(enemy.health > 0) { spawnedEnemies.Add(enemy.Filename); }
        }

		mainData.StoredEnemies = spawnedEnemies;
	}

		private void ClearSpawnedEnemies() 
	{
		mainData.StoredEnemies = null;
	}

}