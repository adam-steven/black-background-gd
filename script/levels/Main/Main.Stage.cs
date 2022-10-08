using Godot;
using static Enums;

//Wave > Stage > Level

public partial class Main
{
    public void _StageReady()
    {
        DisplayProgression();
        uiNode.SetWaveSegments(mainData.Stage.NoOfWaves);
    }

    public void _StageProcess(float delta)
    {
        bool? stageTimerEnd = mainData.Stage.ProcessStageCountDown(delta);

        if (stageTimerEnd == false) { DisplayProgression(); }
        else if (stageTimerEnd == true)
        {
            ProgressGame();
        }
    }

    //Spawn next stage;
	private void ProgressGame(bool gameStart = false)
	{
        //Set/Clear save values 
        if(!gameStart) 
        {
            SavePlayerStats();
            ClearSpawnedUpgrades(); 
            ClearSpawnedEnemies();
        }
   
		NextWave(gameStart);
		GameStages currentStage = mainData.Stage.CurrentStage;

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
				SpawnUpgrades();
				break;
		}
	}

    private void NextWave(bool gameStart = false)
    {
        double currentWaveCounter = mainData.Stage.NextWave(gameStart);
        bool newStage = (currentWaveCounter == 0);

        DisplayProgression();
        
        if (newStage) { NextStage(); }
    }

    private void NextStage() 
    {
        GD.Print($"\nNew Stage");

        GameStages currentStage = mainData.Stage.CurrentStage;
        bool newLevel = mainData.Stage.IsNewLevel();

        uiNode.SetWaveSegments(mainData.Stage.NoOfWaves);
        DisplaySectionText(currentStage.ToString().ToUpper());
        LevelSpin();

        if(newLevel) { NextLevel(); }
    }

    private void NextLevel() {
        GD.Print($"\nNew Level: {mainData.Stage.Level}");

        UpdateScenes(mainData.Stage.Level);
        IncreaseEnemySpawn();
        UpdateMultiplier(true);
    }

    //Displays how far along the stage the player is in the progress bar
    private void DisplayProgression()
    {
        uiNode.SetWaveProgress(mainData.Stage.StageProgression);
    }


}