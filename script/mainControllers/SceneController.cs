using Godot;
using System;
using Newtonsoft.Json; 

public class SceneController : Node2D
{
	private Camera2D mainCamera;
	private AnimationPlayer anim;

	private Node2D newSceneInstance = null;
	private Node2D currentScene;

	//private System.Object sceneData = null;

	public override void _Ready() {
		mainCamera = this.GetNode<Camera2D>("Camera2D");
		anim = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Connect("animation_finished", this, "_animation_finished");

		currentScene = this.GetNode<Node2D>("GameController");
		currentScene.Connect("change_scene", this, "ChangeScene");
	}

	public void ChangeScene(string scenePath, float animSpeed, string jsonData) {
		if(String.IsNullOrEmpty(scenePath)) { return; }

		PackedScene newScene = (PackedScene)GD.Load(scenePath);
		newSceneInstance = (Node2D)newScene.Instance();
		newSceneInstance.Visible = false;
		AddChild(newSceneInstance);
		
		System.Object deserializedData = JsonConvert.DeserializeObject<System.Object>(jsonData);
		HandelSceneDataPass(newSceneInstance, deserializedData);

		anim.PlaybackSpeed = animSpeed;
		anim.Play("SceneTransition");
	}

	//Remember to remove
	public void ChangeScene(string scenePath, float animSpeed = 1f, System.Object passThroughData = null) {
		if(String.IsNullOrEmpty(scenePath)) { return; }

		PackedScene newScene = (PackedScene)GD.Load(scenePath);
		newSceneInstance = (Node2D)newScene.Instance();
		newSceneInstance.Visible = false;
		AddChild(newSceneInstance);
		
		HandelSceneDataPass(newSceneInstance, passThroughData);

		anim.PlaybackSpeed = animSpeed;
		anim.Play("SceneTransition");
	}

	private void HandelSceneDataPass(Node2D newScene, System.Object data = null) {
		
		Levels newSceneLevel = (Levels)newScene;

		if(data != null) {
			newSceneLevel.LoadLevelParameters(data);
		}

		newSceneLevel.Connect("change_scene", this, "ChangeScene");
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
