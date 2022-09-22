using Godot;
using static Enums;

public partial class Main
{
    //play game for events without anim name info
    private void PlayGame()
    {
        mainData.InGame = true;
        ResetScenes();
        _ScoreReady();
        _StageReady();

        PlayGame(string.Empty);
    }

    private void PlayGame(string animName = "")
    {
        //count down
        SettingsController settings = new SettingsController();
        isStartingCountDown = (bool)settings.GetValue(MenuButtonActions.StartCountDown.ToString(), false);
        if (isStartingCountDown && animName != "SectionTextCountDown")
        {
            DisplaySectionTextCountDown("PlayGame");
            return;
        }

        this.SetProcess(true);
        this.LevelSpin();
        this.NextStage(true);

        PackedScene pauseMenuScene = (PackedScene)GD.Load("res://scenes/menus/PauseMenu.tscn");
        Godot.Control pauseMenu = (Godot.Control)pauseMenuScene.Instance();
        pauseMenu.Visible = false;
        this.AddChild(pauseMenu);

        pauseMenu.Connect("_play_game", this, "RestartGame");
        pauseMenu.Connect("_main_menu", this, "ReturnToMenu");
        pauseMenu.Connect("_options", this, "GoToOptions");
    }

    private void RestartGame()
    {
        MainGameObj restartObj = new MainGameObj(true);
        EmitChangeScene("res://scenes/gameEnvironment/Main.tscn", 5f, restartObj);
    }

    private void EndGame()
    {
        GameOverObj deathObj = new GameOverObj(mainData.Score.Value, 0);
        EmitChangeScene("res://scenes/menus/DeathScreen.tscn", 1f, deathObj);
    }

    private void ReturnToMenu()
    {
        MainGameObj restartObj = new MainGameObj(false);
        EmitChangeScene("res://scenes/gameEnvironment/Main.tscn", 5f, restartObj);
    }

    private void GoToOptions()
    {
        OptionsObj optionsObj = new OptionsObj(mainData);
        EmitChangeScene("res://scenes/menus/OptionsScreen.tscn", 5f, optionsObj);
    }

    private void GoToLeaderboard()
    {
        EmitChangeScene("res://scenes/menus/LeaderboardScreen.tscn", 5f);
    }

    //upgrading finished for events without anim name info
    private void UpgradingFinished()
    {
        UpgradingFinished(string.Empty);
    }

    private void UpgradingFinished(string animName = "")
    {
        //count down
        if (isStartingCountDown && animName != "SectionTextCountDown")
        {
            DisplaySectionTextCountDown("UpgradingFinished");
        }
        else
        {
            this.CheckIfEnemies();
        }
    }
}