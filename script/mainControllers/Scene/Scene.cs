using Godot;
using System;
using Newtonsoft.Json; 


public partial class Scene : Node2D
{
	private Camera2D mainCamera;
	private AnimationPlayer anim;

	private Node2D newSceneInstance = null;
	private Node2D currentScene;

	public override void _Ready() {
		LoadKeyBinds();

		mainCamera = this.GetNode<Camera2D>("Camera2D");
		anim = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Connect("animation_finished", this, "_animation_finished");

		currentScene = this.GetNode<Node2D>("GameController");
		HandelSceneDataPass(currentScene, null);
	}

	public void ChangeScene(string scenePath, float animSpeed, string jsonData) {
		if(String.IsNullOrEmpty(scenePath)) { return; }

		LoadKeyBinds();

		PackedScene newScene = (PackedScene)GD.Load(scenePath);
		newSceneInstance = (Node2D)newScene.Instance();
		newSceneInstance.Visible = false;
		AddChild(newSceneInstance);
		
		var settings = new JsonSerializerSettings();
		settings.TypeNameHandling = TypeNameHandling.Objects;
		System.Object deserializedData = JsonConvert.DeserializeObject<System.Object>(jsonData, settings);
		HandelSceneDataPass(newSceneInstance, deserializedData);

		anim.PlaybackSpeed = animSpeed;
		anim.Play("SceneTransition");
	}

	private void HandelSceneDataPass(Node2D newScene, System.Object data = null) {
		Levels newSceneLevel = (Levels)newScene;
		newSceneLevel.mainCamera = mainCamera;
		newSceneLevel.LoadLevelParameters(data);
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
}
