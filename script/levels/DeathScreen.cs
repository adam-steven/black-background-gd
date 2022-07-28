using Godot;
using System;

//DeathScreen.tscn 
public class DeathScreen : Levels
{
	private GameOverObj deathData;
	
	private Godot.Label scoreUi;
	private long scoreUiVal = 0; //Slowly gains the full score value for a tick up effect
	private int scoreNoOfTicks = 100; //How many ticks until the scoreUiVal = score
	private long tickAmount = 1;

	//tick up delay
    public float delay = 60;
    public float delayCounter = 0;

	public override void _Ready() {
		Godot.Control control = this.GetNode<Godot.Control>("Control");

		//Labels 
		Godot.VBoxContainer labelContainer = control.GetNode<Godot.VBoxContainer>("Labels");
		scoreUi = labelContainer.GetNode<Godot.Label>("Score");

		//Connect the menu
		control.Connect("_play_game", this, "Replay");
		control.Connect("_main_menu", this, "MainMenu");
		control.Connect("_leaderboard", this, "Leaderboard");
	}

	public override void _Process(float delta) {
		if(deathData == null) { return; }
		if(scoreUiVal >= deathData.score) { return; }
		UpdateScoreUi(delta);
	}

	//Handel score
	private void UpdateScoreUi(float delta) {
		delayCounter += delay * delta;

        if(delayCounter >= 1) {
			scoreUiVal += tickAmount;
			scoreUiVal = Math.Min(scoreUiVal, deathData.score);
			scoreUi.Text = scoreUiVal.ToString("D6");
			delayCounter = 0;
        }
	}

	public override void LoadLevelParameters(System.Object sceneData) {
		deathData = (sceneData != null) ? (GameOverObj)sceneData : new GameOverObj(0, 0);
		tickAmount = Math.Max((deathData.score / scoreNoOfTicks), 1);
		GD.Print("Time: " + deathData.time);
	}

	private void Replay() {
		MainGameObj restartObj = new MainGameObj(true);
		EmitChangeScene("res://scenes/Main.tscn", 5f, restartObj);
	}

	private void MainMenu() {
		MainGameObj restartObj = new MainGameObj(false);
		EmitChangeScene("res://scenes/Main.tscn", 5f, restartObj);
	}

	private void Leaderboard() {
		EmitChangeScene("res://scenes/menus/LeaderboardScreen.tscn", 5f);
	}

}
