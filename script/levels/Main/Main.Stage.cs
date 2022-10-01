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
            NextStage();
            SavePlayerStats();
        }
    }

    ///<returns>new stage</returns>
    public bool NextWave(bool gameStart = false)
    {
        double currentWaveCounter = mainData.Stage.NextWave(gameStart);

        DisplayProgression();
        if (currentWaveCounter == 0)
        {
            uiNode.SetWaveSegments(mainData.Stage.NoOfWaves);
        }

        return (currentWaveCounter == 0);
    }

    private void DisplayProgression()
    {
        uiNode.SetWaveProgress(mainData.Stage.StageProgression);
    }
}