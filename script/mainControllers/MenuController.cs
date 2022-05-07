using System;
using Godot;

//Handles spawning in menus 
public partial class GameController
{
	public void ShowGameOverScreen() {
		string gameOverSceneLocation = "res://scenes/menus/DeathScreen.tscn";
		PackedScene gameOverScene = (PackedScene)GD.Load(gameOverSceneLocation);
		Godot.Control gameOver = (Godot.Control)gameOverScene.Instance();
		this.AddChild(gameOver);
	}
}