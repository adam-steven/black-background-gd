using Godot;
using Newtonsoft.Json;

public partial class Scene : Node2D
{
	private Camera2D mainCamera;
	private AnimationPlayer anim;

	private Node2D newSceneInstance = null;
	private Node2D currentScene;

	private SectionedScenes enemiesSections; //Paths to enemy scenes
	private SectionedScenes obstaclesSections; //Paths to obstacle scenes
	private SectionedScenes upgradeSections; //Paths to upgrade scenes

	public override void _Ready()
	{
		LoadKeyBinds();

		mainCamera = this.GetNode<Camera2D>("Camera2D");
		anim = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Connect(AnimationPlayer.SignalName.AnimationFinished, new Callable(this, "AnimationFinished"));

		obstaclesSections = FileManager.GetScenesViaFolders(Globals.obstaclesFolder);
		enemiesSections = FileManager.GetScenesViaFolders(Globals.enemyFolder);
		upgradeSections = FileManager.GetScenesViaFolders(Globals.upgradesFolder);

		currentScene = this.GetNode<Node2D>("GameController");
		HandelSceneDataPass(currentScene, null);
	}

	public void ChangeSceneToFile(string scenePath, float animSpeed, string jsonData)
	{
		if (string.IsNullOrEmpty(scenePath)) { return; }

		LoadKeyBinds();

		PackedScene newScene = (PackedScene)GD.Load(scenePath);
		newSceneInstance = newScene.Instantiate<Node2D>();
		newSceneInstance.Visible = false;
		AddChild(newSceneInstance);

		var settings = new JsonSerializerSettings();
		settings.TypeNameHandling = TypeNameHandling.Objects;
		object deserializedData = JsonConvert.DeserializeObject<object>(jsonData, settings);
		HandelSceneDataPass(newSceneInstance, deserializedData);

		anim.SpeedScale = animSpeed;
		anim.Play("SceneTransition");
	}

	private void HandelSceneDataPass(Node2D newScene, object data = null)
	{
		Level newSceneLevel = (Level)newScene;

		newSceneLevel.mainCamera = mainCamera;
		newSceneLevel.obstaclesSections = obstaclesSections;
		newSceneLevel.enemiesSections = enemiesSections;
		newSceneLevel.upgradeSections = upgradeSections;

		newSceneLevel._LoadLevelParameters(data);
		newSceneLevel.Connect(Level.SignalName.ChangeScene, new Callable(this, "ChangeSceneToFile"));
	}

	private void AnimationFinished(string animName)
	{
		if (animName == "SceneTransition")
		{
			currentScene.QueueFree();
			currentScene = newSceneInstance;
			currentScene.Visible = true;
			newSceneInstance = null;
			anim.Play("SceneDefault");
		}
	}
}
