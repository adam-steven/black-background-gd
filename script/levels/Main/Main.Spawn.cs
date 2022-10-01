using Godot;

public partial class Main
{
    private void SpawnPlayer(Vector2 location, EntityStats storedStats = null)
    {
        PackedScene playerScene = (PackedScene)GD.Load("res://scenes/misc/Player.tscn");
        player = (PlayerController)playerScene.Instance();
        player.GlobalPosition = location;

        player.Connect("_on_death", this, "EndGame");
        player.Connect("_shake_screen", (Camera)mainCamera, "StartShakeScreen");
        player.Connect("_section_text", this, "DisplaySectionText");
        player.Connect("_destroy_all_bullets", this, "DestroyBullets");
        player.Connect("_update_score", this, "UpdateScore");
        player.Connect("_break_score_update", this, "BreakScoreUpdate");
        player.Connect("_player_left_camera", this, "ReframePlayer");
        player.Connect("_update_health_ui", uiNode, "UpdateHealthUi");

        if (storedStats != null)
        {
            GD.Print(Newtonsoft.Json.JsonConvert.SerializeObject(storedStats));
            uiNode.UpdateHealthUi(storedStats.Health, false);
        }

        this.AddChild(player);
    }

    private void SpawnMainMenu()
    {
        PackedScene mainMenuScene = (PackedScene)GD.Load("res://scenes/menus/MainMenu.tscn");
        Godot.Control mainMenu = (Godot.Control)mainMenuScene.Instance();

        mainMenu.Connect("_play_game", this, "PlayGame");
        mainMenu.Connect("_options", this, "GoToOptions");
        mainMenu.Connect("_leaderboard", this, "GoToLeaderboard");

        this.AddChild(mainMenu);
    }

    private void SpawnObstacles()
    {
        GD.Print("Response SpawnObstacles\n");

        int noToSpawn = rnd.Next(enemySpawnMin, enemySpawnMax + 2);
        noOfEnemies += noToSpawn;
        for (int i = 0; i < noToSpawn; i++)
        {
            Entities obstacle = PickSpawnEntity(mainData.Obstacles);
            this.AddChild(obstacle);
        }
    }

    private void SpawnEnemies()
    {
        GD.Print("Response SpawnEnemies\n");

        int noToSpawn = rnd.Next(enemySpawnMin, enemySpawnMax + 1);
        noOfEnemies += noToSpawn;
        for (int i = 0; i < noToSpawn; i++)
        {
            Entities enemy = PickSpawnEntity(mainData.Enemies);
            enemy.Connect("_update_score", this, "UpdateScore", new Godot.Collections.Array(mainData.Stage.Level));
            enemy.Connect("_update_player_heath", player, "_UpdateHealth");
            this.AddChild(enemy);
        }
    }

    private void SpawnBoss()
    {
        GD.Print("Response SpawnBoss\n");
        SpawnEnemies();
    }

    private void SpawnUpgrades()
    {
        GD.Print("Response SpawnUpgrades\n");

        PackedScene upgradeMenuScene = (PackedScene)GD.Load("res://scenes/menus/UpgradeMenu.tscn");
        UpgradeMenu upgradeMenu = (UpgradeMenu)upgradeMenuScene.Instance();

        upgradeMenu.levelCenter = levelCenter;
        upgradeMenu.player = player;
        upgradeMenu.upgrades = mainData.Upgrades;

        upgradeMenu.Connect("_upgrading_finished", this, "UpgradingFinished");
        upgradeMenu.Connect("_decrease_multiplier", this, "UpdateMultiplier");
        upgradeMenu.Connect("_update_upgrade_ui", uiNode, "UpdateUpgradeDescUi");

        this.AddChild(upgradeMenu);
    }

    #region Spawn Helpers 

    private Entities PickSpawnEntity(Scenes entityList)
    {
        string chosenEntityScene = entityList[rnd.Next(entityList.Count)];
        PackedScene entityScene = (PackedScene)GD.Load(chosenEntityScene);
        Entities entity = (Entities)entityScene.Instance();

        int spawnPosX = rnd.Next((int)-Globals.levelSize.x, (int)Globals.levelSize.x);
        int spawnPosY = rnd.Next((int)-Globals.levelSize.y, (int)Globals.levelSize.y);
        Vector2 spawnPosition = new Vector2(spawnPosX, spawnPosY) + levelCenter;
        entity.GlobalPosition = spawnPosition;

        entity.player = player;
        entity.colour = Colour.LevelColour;
        entity.bulletColour = Colour.HarmonizingColour;

        entity.Connect("_on_death", this, "CheckIfEnemies");

        return entity;
    }

    #endregion
}