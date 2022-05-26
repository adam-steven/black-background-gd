using Godot;
using System;

public class SceneController : Node2D
{
	private Camera2D mainCamera;
	private AnimationPlayer anim;

	private Node2D newSceneInstance = null;
	private Node2D currentScene;

	private System.Object sceneData = null;

	public override void _Ready()
	{
		mainCamera = this.GetNode<Camera2D>("Camera2D");
		anim = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Connect("animation_finished", this, "_animation_finished");

		currentScene = this.GetNode<Node2D>("GameController");
	}

	public void ChangeScene(string scenePath, float animSpeed = 1f, System.Object passThroughData = null) {
		if(String.IsNullOrEmpty(scenePath)) { return; }

		sceneData = passThroughData;

		PackedScene newScene = (PackedScene)GD.Load(scenePath);
		newSceneInstance = (Node2D)newScene.Instance();
		newSceneInstance.Visible = false;
		AddChild(newSceneInstance);

		if(passThroughData != null) {
			HandelSceneDataPass(newSceneInstance, passThroughData);
		}

		anim.PlaybackSpeed = animSpeed;
		anim.Play("SceneTransition");
	}

	private void HandelSceneDataPass(Node2D newScene, System.Object data) {
		Levels newSceneLevel = (Levels)newScene;

		newSceneLevel.LoadLevelParameters(data);

		newSceneLevel.Connect("change_scene", this, "ChangeScene");
		//Add listener to level
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

	#region Getters

	public Camera2D GetMainCamera() {
		return mainCamera ?? this.GetNode<Camera2D>("Camera2D");
	}

	public Node2D GetCurrentScene() {
		return currentScene ?? this.GetNode<Node2D>("GameController");
	}

	#endregion
}
