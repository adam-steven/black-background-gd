using Godot;
using GdArray = Godot.Collections.Array;
using System;
using static Enums;


//Main.tscn 
public partial class Main : Level
{
	MainGameObj mainData = new MainGameObj(false);

	private static Random rnd = new Random();

	private Godot.Node2D levelNode;
	private Vector2 levelCenter;

	private int noOfEnemies = 0;

	private Player player;

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
		if (sceneData is not null)
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
		ProgressGame();
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

	//Destroys all bullets on the screen
	private void DestroyBullets()
	{
		GdArray children = this.GetChildren();

		foreach (var child in children)
			if (child.GetType() == typeof(Bullet))
				((Bullet)child).QueueFree();
	}

	private void ReframePlayer()
	{
		//If camera locked, move player to center
		Vector2 cameraCenter = mainCamera.GetCameraScreenCenter();
		player.Position = cameraCenter;

		//TODO: If camera free, move camera to player
	}

	#endregion
}
