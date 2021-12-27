using Godot;
using System;

public class GameController : Node2D
{
    Random rnd = new Random();

	public override void _Ready() {
		SpawnEnemies();
	}

	// public override void _Process(float delta) {}

	private void SpawnEnemies(bool isBoss = false) {
		if(isBoss) return;

		Vector2 levelCenter = this.GetNode<Godot.Node2D>("Level").GlobalPosition;
		Vector2 levelSize = new Vector2(460, 460); //spawnable level radius (half of full dimentions)

		int noOfEnemies = 1;
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
