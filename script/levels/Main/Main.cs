using Godot;
using System;
using static Enums;

//Main.tscn 
public partial class Main : Levels
{
	MainGameObj mainData = new MainGameObj(false);

	private static Random rnd = new Random();

	private Godot.Node2D levelNode;
	private Vector2 levelCenter;

	private int enemySpawnMin = 1;
	private int enemySpawnMax = 2;
	private int noOfEnemies = 0;

	private PlayerController player;

	private bool isStartingCountDown;

	private UiController uiNode;

	public override void _Ready()
	{
		uiNode = this.GetNode<UiController>("UI");

		levelNode = this.GetNode<Godot.Node2D>("Level");
		levelCenter = levelNode.GlobalPosition;

		this.SetProcess(false);
	}

	public override void _LoadLevelParameters(System.Object sceneData)
	{
		if (sceneData != null)
		{
			mainData = (MainGameObj)sceneData;
		}

		if (mainData.InGame)
		{
			Vector2 playerPos = new Vector2(960, 540);
			SpawnPlayer(playerPos, mainData.PlayerStats);
			PlayGame();
		}
		else
		{
			Vector2 playerPos = new Vector2(1200, 540);
			SpawnPlayer(playerPos);
			SpawnMainMenu();
		}
	}

	public override void _Process(float delta)
	{
		_ScoreProcess(delta);
		_StageProcess(delta);
	}

	#region Misc Functions

	//If the last enemy is dying spawn next way
	public void CheckIfEnemies()
	{
		noOfEnemies--;
		if (noOfEnemies > 0) return;

		noOfEnemies = 0;
		NextStage();
		SavePlayerStats();
	}

	//Spawn next stage;
	private void NextStage(bool gameStart = false)
	{
		bool newStage = NextWave(gameStart);
		GameStages currentStage = mainData.Stage.CurrentStage;

		GD.Print("\nnewStage " + newStage);
		GD.Print("currentStage " + currentStage);

		if (newStage)
		{
			DisplaySectionText(currentStage.ToString().ToUpper());
			LevelSpin();
		}

		switch (currentStage)
		{
			case GameStages.Dodge:
				GD.Print("Call SpawnObstacles");
				SpawnObstacles();
				break;
			case GameStages.Fight:
				GD.Print("Call SpawnEnemies");
				SpawnEnemies();
				break;
			case GameStages.Boss:
				GD.Print("Call SpawnBoss");
				SpawnBoss();
				break;
			case GameStages.Event:
				//Event
				break;
			default:
				GD.Print("Call SpawnUpgrades");

				UpdateScenes(mainData.Stage.Level + 1);
				IncreaseEnemySpawn();
				UpdateMultiplier(true);

				SpawnUpgrades();
				break;
		}
	}

	//Displays big faint text in the background for a short amount of time
	//Used to indicate the changes in gameplay sections 
	private void DisplaySectionText(string text, bool inverted = false)
	{
		Position2D sectionText = this.GetNode<Position2D>("SectionText");
		Godot.Label label = sectionText.GetNode<Godot.Label>("Label");
		AnimationPlayer anim = sectionText.GetNode<AnimationPlayer>("AnimationPlayer");

		string textColor = inverted ? "black" : "white";
		sectionText.Modulate = Color.ColorN(textColor);

		label.Text = text;
		anim.Play("SectionTxtDisplay");
	}

	private void DisplaySectionTextCountDown(string callFunction)
	{
		AnimationPlayer anim = this.GetNode<AnimationPlayer>("SectionText/AnimationPlayer");

		anim.Connect("animation_finished", this, callFunction, null, (uint)ConnectFlags.Oneshot);
		anim.Play("SectionTextCountDown");
	}

	//Spins the level boarders + changes the level colour
	private void LevelSpin()
	{
		AnimationPlayer anim = levelNode.GetNode<AnimationPlayer>("Room/AnimationPlayer");
		anim.Play("RoomSpin");
		Colour.UpdateGameColours(levelNode, player);
	}

	//Slowly increase the number of enemies each wave
	private void IncreaseEnemySpawn()
	{
		if (mainData.Stage.Level % 2 == 0) { enemySpawnMax++; }
		else { enemySpawnMin++; }
	}

	//Destroys all bullets on the screen
	private void DestroyBullets()
	{
		var children = this.GetChildren();

		foreach (var child in children)
			if (child.GetType() == typeof(BulletController))
				((BulletController)child).QueueFree();
	}

	private void ReframePlayer()
	{
		//If camera locked, move player to center
		Vector2 cameraCenter = mainCamera.GetCameraScreenCenter();
		player.Position = cameraCenter;

		//TODO: If camera free, move camera to player
	}

	private void SavePlayerStats() 
	{
		mainData.PlayerStats = player.GetStats();
	}

	#endregion

	#region Testing Functions

	// private void PlaceTestingDot(Vector2 tDotPos) {
	// 	PackedScene testingDot = (PackedScene)GD.Load("res://scenes/TestingDot.tscn");
	// 	Godot.Sprite tDot = (Godot.Sprite)testingDot.Instance();
	// 	tDot.GlobalPosition = tDotPos;
	// 	this.AddChild(tDot);
	// }

	// private int currentColour = -1;
	// private string colourName;
	// private void RunThoughColours() {
	//     currentColour++;

	//     var values = Enum.GetValues(typeof(Enums.Colour));
	//     colourName = values.GetValue(currentColour).ToString();

	//     enemyColour = Color.ColorN(colourName);
	//     levelNode.Modulate = enemyColour;

	// 	GD.Print(colourName);
	// }

	#endregion

}
