using Godot;
using System;
using System.Collections.Generic;

public partial class GameController : Node2D
{
	Random rnd = new Random();

	Godot.Node2D levelNode;
	Vector2 levelCenter;
	Vector2 levelSize = new Vector2(460, 460); //spawnable level radius (half of full dimentions)

	private int level = 0;
	private int numberOfWaves = 0;

	private int enemySpawnMin = 0;
	private int enemySpawnMax = 2;
	private int noOfEnemies = 0;

	string enemySpawnerFolder = "res://scenes/misc/EnemySpawner.tscn";

	string obstaclesFolder = "res://scenes/obstacles/";
	List<string> obstacles; //paths to obstical scenes (dodge section)

	string upgradesFolder = "res://scenes/upgrades/";
	List<string> upgrades; //paths to upgrade scenes
	
	public override void _Ready() {
		levelNode = this.GetNode<Godot.Node2D>("Level");
		levelCenter = levelNode.GlobalPosition;
		upgrades = FileManager.GetScenes(upgradesFolder);
		obstacles = FileManager.GetScenes(obstaclesFolder);
		CheckIfEnemies();
	}

	// public override void _Process(float delta) {}

	#region Spawn Functions

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
			UpdateGameColours();
			SpawnUpgrades();
		} 
	}

	private void SpawnObstacles() {
		noOfEnemies = rnd.Next(enemySpawnMin, enemySpawnMax + 2);
		for (int i = 0; i < noOfEnemies; i++) {
			string randomObstacles = obstacles[rnd.Next(obstacles.Count)];
			PackedScene obstacleScene = (PackedScene)GD.Load(obstaclesFolder + randomObstacles);
			Godot.RigidBody2D obstacle = (Godot.RigidBody2D)obstacleScene.Instance();

			int spawnPosX = rnd.Next((int)-levelSize.x, (int)levelSize.x);
			int spawnPosY = rnd.Next((int)-levelSize.y, (int)levelSize.y);
			Vector2 spawnPosition = new Vector2(spawnPosX, spawnPosY) + levelCenter;
			obstacle.GlobalPosition = spawnPosition;

			this.AddChild(obstacle);
		}
	}

	private void SpawnEnemies() {
		noOfEnemies = rnd.Next(enemySpawnMin, enemySpawnMax + 1);
		for (int i = 0; i < noOfEnemies; i++) {
			PackedScene spawnerScene = (PackedScene)GD.Load(enemySpawnerFolder);
			Godot.Position2D spawner = (Godot.Position2D)spawnerScene.Instance();

			int spawnPosX = rnd.Next((int)-levelSize.x, (int)levelSize.x);
			int spawnPosY = rnd.Next((int)-levelSize.y, (int)levelSize.y);
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
		Vector2[] spawnPoints = {
			new Vector2(0,-levelSize.y/2),
			new Vector2(levelSize.x/2,levelSize.y/2),
			new Vector2(-levelSize.x/2,levelSize.y/2),
		};

		for (int i = 0; i < spawnPoints.Length; i++) {
			string randomUpgrade = upgrades[rnd.Next(upgrades.Count)];
			PackedScene upgradeScene = (PackedScene)GD.Load(upgradesFolder + randomUpgrade);
			Godot.Node2D upgrade = (Godot.Node2D)upgradeScene.Instance();

			Vector2 spawnPosition = spawnPoints[i] + levelCenter;
			upgrade.GlobalPosition = spawnPosition;

			this.AddChild(upgrade);
		}
	}

	#endregion 

	#region Misc Functions 

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
	}

	#endregion 

	#region Signals

	[Signal]
	public delegate void upgrading_finished();

	//Emit signal to delete any existing upgrades
	public void FinishedUpgrading() {
		this.EmitSignal("upgrading_finished");
		CheckIfEnemies();
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
