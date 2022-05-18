using Godot;
using System;

public class SceneController : Node2D
{
	private Camera2D mainCamera;
	private Node2D currentScene;

	public override void _Ready()
	{
		mainCamera = this.GetNode<Camera2D>("Camera2D");
		currentScene = this.GetChild<Node2D>(1);
	}

	public Camera2D GetMainCamera() {
		return mainCamera ?? this.GetNode<Camera2D>("Camera2D");
	}

	public Node2D GetCurrentScene() {
		return currentScene ?? this.GetChild<Node2D>(1);
	}

	public void ChangeScene(string scenePath) {
		if(String.IsNullOrEmpty(scenePath)) { return; }

		PackedScene newScene = (PackedScene)GD.Load(scenePath);
		Node2D newSceneInstance = (Node2D)newScene.Instance();
		AddChild(newSceneInstance);
		currentScene.QueueFree();
		currentScene = newSceneInstance;
	}
}
