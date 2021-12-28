using Godot;
using System;

public class GameController : Node2D
{
    Random rnd = new Random();

	private int level = 0;
	private int numberOfWaves = 0;

	private int enemySpawnMin = 0;
	private int enemySpawnMax = 2;
	private int noOfEnemies = 0;
	
	public override void _Ready() {
		CheckIfEnemies();
	}

	// public override void _Process(float delta) {}

	//If the last enemy is dying spawn next way
	public void CheckIfEnemies() {
		noOfEnemies--;
		if(noOfEnemies <= 0) {

			numberOfWaves--;
			if(numberOfWaves < 0) {
				level++;
				numberOfWaves = level + 2;

				//Slowly increase the number of enemies each wave
				if(level%2 == 0) enemySpawnMax++; 
				else enemySpawnMin++;
			}

			GD.Print("Level: " + level + 
					" Wave: " + numberOfWaves + 
					" Boss: " + (numberOfWaves == 0));
			SpawnEnemies(numberOfWaves == 0); //Bosses at level 0
		}
	}

	private void SpawnEnemies(bool isBoss = false) {
		//if(isBoss) return;

		Vector2 levelCenter = this.GetNode<Godot.Node2D>("Level").GlobalPosition;
		Vector2 levelSize = new Vector2(460, 460); //spawnable level radius (half of full dimentions)

		noOfEnemies = rnd.Next(enemySpawnMin, enemySpawnMax + 1);
		for (int i = 0; i < noOfEnemies; i++) {
			PackedScene spawnerScene = (PackedScene)GD.Load("res://scenes/enemies/Spawner.tscn");
			Godot.Position2D spawner = (Godot.Position2D)spawnerScene.Instance();

			int spawnPosX = rnd.Next((int)-levelSize.x, (int)levelSize.x);
			int spawnPosY = rnd.Next((int)-levelSize.y, (int)levelSize.y);
			Vector2 spawnPosition = new Vector2(spawnPosX, spawnPosY) + levelCenter;
			spawner.GlobalPosition = spawnPosition;

			this.AddChild(spawner);
		}
	}

	//Starts turning the background red if player health is less than 30
	public void UpdateBackgroundColor(int playerHealth) {
		//make sure the number is never less than 0
		int red = Math.Max(0, 30 - playerHealth) * 2;
		VisualServer.SetDefaultClearColor(Color.Color8((byte)red,0,0));
	}

	// --- Start Testing Function ---
	private void PlaceTestingDot(Vector2 tDotPos) {
		PackedScene testingDot = (PackedScene)GD.Load("res://scenes/TestingDot.tscn");
		Godot.Sprite tDot = (Godot.Sprite)testingDot.Instance();
		tDot.GlobalPosition = tDotPos;
		this.AddChild(tDot);
	}

	private void LogFrameRate() {
	  	GD.Print(Performance.GetMonitor(0));
	}
}
