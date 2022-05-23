using Godot;
using System;

public class SceneController : Node2D
{
	private Camera2D mainCamera;
	private AnimationPlayer anim;

	private Node2D newSceneInstance = null;
	private Node2D currentScene;

	public override void _Ready()
	{
		mainCamera = this.GetNode<Camera2D>("Camera2D");
		anim = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Connect("animation_finished", this, "_animation_finished");

		currentScene = this.GetNode<Node2D>("GameController");
	}

	public Camera2D GetMainCamera() {
		return mainCamera ?? this.GetNode<Camera2D>("Camera2D");
	}

	public Node2D GetCurrentScene() {
		return currentScene ?? this.GetNode<Node2D>("GameController");
	}

	private void HandelDataTransfer(Node2D oldScene, Node2D newScene, System.Object data) {
	}

	public void ChangeScene(string scenePath, float animSpeed = 1f, System.Object passThroughData = null) {
		if(String.IsNullOrEmpty(scenePath)) { return; }

		PackedScene newScene = (PackedScene)GD.Load(scenePath);
		newSceneInstance = (Node2D)newScene.Instance();
		newSceneInstance.Visible = false;
		AddChild(newSceneInstance);

		if(passThroughData != null) {
			HandelDataTransfer(currentScene, newSceneInstance, passThroughData);
		}
		
		anim.PlaybackSpeed = animSpeed;
		anim.Play("SceneTransition");
	}

	private void _animation_finished(string animName) {
		if(animName == "SceneTransition") {
			currentScene.QueueFree();
			currentScene = newSceneInstance;
			currentScene.Visible = true;
			newSceneInstance = null;
			anim.Play("SceneDefault");
		}
	}
}
