using Godot;
using System;
using System.Collections.Generic;

//Main.tscn 
public class Main : Levels
{
	Random rnd = new Random();

	Godot.Node2D levelNode;
	Vector2 levelCenter;
	
	private int level = 0;
	private int numberOfWaves = -1;

	private int enemySpawnMin = 0;
	private int enemySpawnMax = 2;
	private int noOfEnemies = 0;

	private Entities player;

	private static string enemySpawnerFolder = "res://scenes/misc/EnemySpawner.tscn";

	private static string obstaclesFolder = "res://scenes/obstacles/";
	List<string> obstacles; //paths to obstical scenes (dodge section)

	public override void _Ready() {
		//Reset the background color
		ColourControl.UpdateBackgroundColour(10000);

		levelNode = this.GetNode<Godot.Node2D>("Level");
		levelCenter = levelNode.GlobalPosition;
		obstacles = FileManager.GetScenes(obstaclesFolder);
	}

	public override void LoadLevelParameters(System.Object sceneData) {
		MainGameObj mainData = (sceneData != null) 
			? (MainGameObj)sceneData 
			: new MainGameObj(false);

		if(mainData.isQuickReset) {
			Vector2 playerPos = new Vector2(960, 540);
			SpawnPlayer(playerPos);
			PlayGame();
		} else {
			Vector2 playerPos = new Vector2(1200, 540);
			SpawnPlayer(playerPos);
			SpawnMainMenu();
		}
	}

	#region Spawn Functions

	private void SpawnPlayer(Vector2 location) {
		PackedScene playerScene = (PackedScene)GD.Load("res://scenes/misc/Player.tscn");
		RigidBody2D playerRB = (RigidBody2D)playerScene.Instance();
		playerRB.GlobalPosition = location;
		this.AddChild(playerRB);

		player = (Entities)playerRB;
		player.Connect("end_game", this, "EndGame");
	}

	private void SpawnMainMenu() {
		PackedScene mainMenuScene = (PackedScene)GD.Load("res://scenes/menus/MainMenu.tscn");
		Godot.Control mainMenu = (Godot.Control)mainMenuScene.Instance();
		this.AddChild(mainMenu);

		mainMenu.Connect("play_game", this, "PlayGame");
	}

	//If the last enemy is dying spawn next way
	public void CheckIfEnemies() {
		noOfEnemies--;
		if(noOfEnemies > 0) return;

		//Waves >0 basic enemies
		//Wave 0 bosses
		//Waves <0 Upgrades
		numberOfWaves--;
		if(numberOfWaves < -1) {
			level++;
			numberOfWaves = (level + 2) * 2;

			//Slowly increase the number of enemies each wave
			if(level%2 == 0) { enemySpawnMax++; } 
			else { enemySpawnMin++; }
		}

		GD.Print("Level: " + level + " Wave: " + numberOfWaves);

		if (numberOfWaves > level + 2) {

			if(numberOfWaves ==  (level + 2) * 2) { DisplaySectionText("DODGE"); }
			SpawnObstacles(); 
		}
		else if (numberOfWaves > 0) { 

			if(numberOfWaves == level + 2) { DisplaySectionText("FIGHT"); }
			SpawnEnemies(); 
		}
		else if (numberOfWaves == 0) { 
			DisplaySectionText("BOSS"); 
			SpawnBoss(); 
		} 
		else { 
			LevelSpin();
			SpawnUpgrades();
		} 
	}

	private void SpawnObstacles() {
		noOfEnemies = rnd.Next(enemySpawnMin, enemySpawnMax + 2);
		for (int i = 0; i < noOfEnemies; i++) {
			string randomObstacles = obstacles[rnd.Next(obstacles.Count)];
			PackedScene obstacleScene = (PackedScene)GD.Load(obstaclesFolder + randomObstacles);
			Godot.RigidBody2D obstacle = (Godot.RigidBody2D)obstacleScene.Instance();

			int spawnPosX = rnd.Next((int)-Globals.levelSize.x, (int)Globals.levelSize.x);
			int spawnPosY = rnd.Next((int)-Globals.levelSize.y, (int)Globals.levelSize.y);
			Vector2 spawnPosition = new Vector2(spawnPosX, spawnPosY) + levelCenter;
			obstacle.GlobalPosition = spawnPosition;

			Enemies obstacleScript = (Enemies)obstacle;
			obstacleScript.player = player;

			this.AddChild(obstacle);

			obstacle.Connect("_on_death", this, "CheckIfEnemies");
		}
	}

	private void SpawnEnemies() {
		noOfEnemies = rnd.Next(enemySpawnMin, enemySpawnMax + 1);
		for (int i = 0; i < noOfEnemies; i++) {
			PackedScene spawnerScene = (PackedScene)GD.Load(enemySpawnerFolder);
			Godot.Position2D spawner = (Godot.Position2D)spawnerScene.Instance();

			int spawnPosX = rnd.Next((int)-Globals.levelSize.x, (int)Globals.levelSize.x);
			int spawnPosY = rnd.Next((int)-Globals.levelSize.y, (int)Globals.levelSize.y);
			Vector2 spawnPosition = new Vector2(spawnPosX, spawnPosY) + levelCenter;
			spawner.GlobalPosition = spawnPosition;

			this.AddChild(spawner);
		}
	}

	private void SpawnBoss() {
		GD.Print("Boss");
		SpawnEnemies();
	}

	private void SpawnUpgrades() {
		PackedScene upgradeMenuScene = (PackedScene)GD.Load("res://scenes/menus/UpgradeMenu.tscn");
		Godot.Control upgradeMenu = (Godot.Control)upgradeMenuScene.Instance();

		UpgradeMenu upgradeMenuScript = (UpgradeMenu)upgradeMenu;
		upgradeMenuScript.levelCenter = levelCenter;
		upgradeMenuScript.player = player;

		this.AddChild(upgradeMenu);

		//if the upgrading is finished call CheckIfEnemies to continue game
		upgradeMenuScript.Connect("upgrading_finished", this, "CheckIfEnemies");
	}

	#endregion 

	#region Misc Functions 

	public void PlayGame() {
		this.LevelSpin();
		this.CheckIfEnemies();
	}

	public void EndGame() {
		EmitChangeScene("res://scenes/menus/DeathScreen.tscn", 1f, null);
	}

	//Displays big faint text in the background for a short amount of time
	//Used to indicate the changes in gameplay sections 
	public void DisplaySectionText(string text) {
		Position2D sectionText = levelNode.GetNode<Position2D>("SectionText");
		Godot.Label label = sectionText.GetNode<Godot.Label>("Label");
		AnimationPlayer anim  = sectionText.GetNode<AnimationPlayer>("AnimationPlayer");

		label.Text = text;
		anim.Play("SectionTxtDisplay");
	}

	//Spins the level boarders + changes the level colour
	public void LevelSpin() {
		Position2D room = levelNode.GetNode<Position2D>("Room");
		AnimationPlayer anim = room.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("RoomSpin");
		UpdateGameColours();
	}

	#endregion 

	#region Colour Controls

	public Color playerColour;
	public Color enemyColour;

	private void UpdateGameColours() {
		var values = Enum.GetValues(typeof(Enums.Colour));
		String colourName = values.GetValue(rnd.Next(values.Length)).ToString();

		enemyColour = Color.ColorN(colourName);
		levelNode.Modulate = enemyColour;
	}

	#endregion

	#region Testing Functions

	// private void PlaceTestingDot(Vector2 tDotPos) {
	// 	PackedScene testingDot = (PackedScene)GD.Load("res://scenes/TestingDot.tscn");
	// 	Godot.Sprite tDot = (Godot.Sprite)testingDot.Instance();
	// 	tDot.GlobalPosition = tDotPos;
	// 	this.AddChild(tDot);
	// }

	// private void LogFrameRate() {
	//   	GD.Print(Performance.GetMonitor(0));
	// }

	// private int currentColour = -1;
	// private string colourName;
	// private void RunThoughColours() {
	//     currentColour++;

	//     var values = Enum.GetValues(typeof(Enums.Colour));
	//     colourName = values.GetValue(currentColour).ToString();
		
	//     enemyColour = Color.ColorN(colourName);
	//     levelNode.Modulate = enemyColour;

	// 	GD.Print(colourName);
	// }

	#endregion

}
