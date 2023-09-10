using Godot;
using System;

//DeathScreen.tscn 
public class DeathScreen : Level
{
    private GameOverObj deathData;

    private Godot.Label scoreUi;
    private long scoreUiVal = 0; //Slowly gains the full score value for a tick up effect
    private int scoreNoOfTicks = 100; //How many ticks until the scoreUiVal = score
    private long scoreAbs; //the score fixed positive for limits
    private long tickAmount = 1;

    //tick up delay
    public float delay = 60;
    public float delayCounter = 0;

    public override void _Ready()
    {
        Godot.Control control = this.GetNode<Godot.Control>("Control");
        scoreUi = control.GetNode<Godot.Label>("Labels/Score");

        //Connect the menu
        control.Connect("_play_game", this, "Replay");
        control.Connect("_main_menu", this, "MainMenu");
        control.Connect("_leaderboard", this, "Leaderboard");
    }

    public override void _Process(float delta)
    {
        if (deathData is null) { return; }
        if (scoreUiVal >= scoreAbs) { return; }
        UpdateScoreUi(delta);
    }

    //Handel score
    private void UpdateScoreUi(float delta)
    {
        delayCounter += delay * delta;

        if (delayCounter >= 1)
        {
            scoreUiVal += tickAmount;
            scoreUiVal = Math.Min(scoreUiVal, scoreAbs);
            scoreUi.Text = (deathData.Score >= 0) ? scoreUiVal.ToString("D6") : (scoreUiVal * -1).ToString("D6") ;
            delayCounter = 0;
        }
    }

    public override void _LoadLevelParameters(System.Object sceneData)
    {
        deathData = (sceneData is not null) ? (GameOverObj)sceneData : new GameOverObj(0, 0);
        scoreAbs = Math.Abs(deathData.Score);
        tickAmount = Math.Max((scoreAbs / scoreNoOfTicks), 1);
        
        GD.Print("Time: " + deathData.Time);
    }

    private void Replay()
    {
        MainGameObj restartObj = new MainGameObj(true);
        EmitChangeScene("res://scenes/gameEnvironment/Main.tscn", 5f, restartObj);
    }

    private void MainMenu()
    {
        MainGameObj restartObj = new MainGameObj(false);
        EmitChangeScene("res://scenes/gameEnvironment/Main.tscn", 5f, restartObj);
    }

    private void Leaderboard()
    {
        EmitChangeScene("res://scenes/menus/LeaderboardScreen.tscn", 5f);
    }
}
