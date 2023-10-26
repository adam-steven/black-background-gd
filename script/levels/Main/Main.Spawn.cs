using System.Linq;
using Godot;

public partial class Main
{
    private void SpawnPlayer(Vector2 location, EntityStats? storedStats = null)
    {
        PackedScene playerScene = (PackedScene)GD.Load("res://scenes/misc/Player.tscn");
        player = playerScene.Instantiate<Player>();
        player.GlobalPosition = location;

        player.Connect(Entity.SignalName.OnDeath, new Callable(this, "EndGame"));
        player.Connect(Entity.SignalName.ShakeScreen, new Callable((Camera)mainCamera, "StartShakeScreen"));
        player.Connect(Entity.SignalName.SectionText, new Callable(this, "DisplaySectionText"));
        player.Connect(Entity.SignalName.DestroyAllBullets, new Callable(this, "DestroyBullets"));
        player.Connect(Entity.SignalName.UpdateScore, new Callable(this, "UpdateScore"));
        player.Connect(Entity.SignalName.BreakScoreUpdate, new Callable(this, "BreakScoreUpdate"));
        player.Connect(Player.SignalName.PlayerLeftCamera, new Callable(this, "ReframePlayer"));
        player.Connect(Player.SignalName.UpdateHealthUi, new Callable(uiNode, "UpdateHealthUi"));

        player.SetStats(storedStats);
        this.AddChild(player);
    }

    private void SpawnMainMenu()
    {
        PackedScene mainMenuScene = (PackedScene)GD.Load("res://scenes/menus/MainMenu.tscn");
        MenuController mainMenu = mainMenuScene.Instantiate<MenuController>();

        mainMenu.Connect(MenuController.SignalName.PlayGame, new Callable(this, "PlayGame"));
        mainMenu.Connect(MenuController.SignalName.Options, new Callable(this, "GoToOptions"));
        mainMenu.Connect(MenuController.SignalName.Leaderboard, new Callable(this, "GoToLeaderboard"));

        this.AddChild(mainMenu);
    }

    private void SpawnObstacles()
    {
        GD.Print("Response SpawnObstacles\n");

        Scenes pathsToSpawn = mainData.StoredEnemies ?? GenerateEntityPaths(mainData.Obstacles, 2);
        noOfEnemies += pathsToSpawn.Count;

        for (int i = 0; i < pathsToSpawn.Count; i++)
        {
            Entity obstacle = GetEntityData(pathsToSpawn[i]);
            this.AddChild(obstacle);
        }

        SaveSpawnedEnemies();
    }

    private void SpawnEnemies()
    {
        GD.Print("Response SpawnEnemies\n");

        Scenes pathsToSpawn = mainData.StoredEnemies ?? GenerateEntityPaths(mainData.Enemies, 1);
        noOfEnemies += pathsToSpawn.Count;

        for (int i = 0; i < pathsToSpawn.Count; i++)
        {
            Entity enemy = GetEntityData(pathsToSpawn[i]);
            enemy.UpdateScore += (points) => UpdateScore(points, mainData.Stage.Level);
            enemy.UpdatePlayerHeath += (health) => player._UpdateHealth(health);
            this.AddChild(enemy);
        }

        SaveSpawnedEnemies();
    }

    private void SpawnBoss()
    {
        GD.Print("Response SpawnBoss\n");
        SpawnEnemies();

        SaveSpawnedEnemies();
    }

    private void SpawnUpgrades()
    {
        GD.Print("Response SpawnUpgrades\n");

        PackedScene upgradeMenuScene = (PackedScene)GD.Load("res://scenes/menus/UpgradeMenu.tscn");
        UpgradeMenu upgradeMenu = upgradeMenuScene.Instantiate<UpgradeMenu>();

        upgradeMenu.levelCenter = levelCenter;
        upgradeMenu.player = player;
        upgradeMenu.upgrades = mainData.Upgrades;
        upgradeMenu.storedUpgrades = mainData.StoredUpgrades;

        upgradeMenu.Connect(UpgradeMenu.SignalName.SpawnedUpgrades, new Callable(this, "SaveSpawnedUpgrades"));
        upgradeMenu.Connect(UpgradeMenu.SignalName.UpgradingFinished, new Callable(this, "UpgradingFinished"));
        upgradeMenu.Connect(UpgradeMenu.SignalName.DecreaseMultiplier, new Callable(this, "UpdateMultiplier"));
        upgradeMenu.Connect(UpgradeMenu.SignalName.UpdateUpgradeUi, new Callable(uiNode, "UpdateUpgradeDescUi"));
   
        this.AddChild(upgradeMenu);
    }

    #region Spawn Helpers 

    //Create a List of random length, containing random paths of entities to spawn
    Scenes GenerateEntityPaths(Scenes entityList, int maxAddend) {
        Scenes pathList = new Scenes();
        int rndSpawnNum = rnd.Next(mainData.EnemySpawnMin, mainData.EnemySpawnMax + maxAddend);
        Scenes croppedEntityList = new Scenes(entityList.OrderBy(x => rnd.Next()).Take(mainData.EnemySpawnDiversity)); //Limit list to max 3 unique scenes

        for (int i = 0; i < rndSpawnNum; i++) 
        { 
            pathList.Add(croppedEntityList[rnd.Next(croppedEntityList.Count)]); 
        }

        return pathList;
    }

    //Creates an Entities Instance with the generic, needed data
    private Entity GetEntityData(string entityPath) 
    {
        PackedScene entityScene = (PackedScene)GD.Load(entityPath);
        Entity entity = entityScene.Instantiate<Entity>();

        int spawnPosX = rnd.Next((int)-Globals.levelSize.X, (int)Globals.levelSize.X);
        int spawnPosY = rnd.Next((int)-Globals.levelSize.Y, (int)Globals.levelSize.Y);
        Vector2 spawnPosition = new Vector2(spawnPosX, spawnPosY) + levelCenter;
        entity.GlobalPosition = spawnPosition;

        entity.player = player;
        entity.colour = Colour.LevelColour;
        entity.bulletColour = Colour.HarmonizingColour;

        entity.Connect(Entity.SignalName.OnDeath, new Callable(this, "CheckIfEnemies"));

        return entity;
    }

    //Slowly increase the number of enemies each wave
	private void IncreaseEnemySpawn()
	{
		if (mainData.Stage.Level % 2 == 0) { mainData.EnemySpawnMax++; }
		else { mainData.EnemySpawnMin++; }

        //every X levels increase spawn diversity
        if (mainData.Stage.Level % 3 == 0 && mainData.EnemySpawnDiversity < 3) { mainData.EnemySpawnDiversity++; }
	}

    #endregion
}