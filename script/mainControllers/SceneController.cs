using Godot;
using System;

public class SceneController : Node2D
{
	private Node2D currentScene;
	public override void _Ready()
	{
		currentScene = this.GetChild<Node2D>(0);
	}

	public Node2D GetCurrentScene() {
		return currentScene ?? this.GetChild<Node2D>(0);
	}

	public void ChangeScene(string scenePath) {
		if(String.IsNullOrEmpty(scenePath)) { return; }

		PackedScene newScene = (PackedScene)GD.Load(scenePath);
		Node2D newSceneInstance = (Node2D)newScene.Instance();
		AddChild(newSceneInstance);
		currentScene.QueueFree();
		currentScene = newSceneInstance;
	}

	public void ShowGameOverScreen() {
		string gameOverSceneLocation = "res://scenes/menus/DeathScreen.tscn";
		PackedScene gameOverScene = (PackedScene)GD.Load(gameOverSceneLocation);
		Godot.Node2D gameOver = (Godot.Node2D)gameOverScene.Instance();
		this.AddChild(gameOver);
	}
}