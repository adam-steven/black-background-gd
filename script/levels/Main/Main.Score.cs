public partial class Main
{
    //tick up delay
    private float delay = 60;
    private float delayCounter = 0;

    public void _ScoreReady()
    {
        uiNode.UpdateScoreUi(mainData.Score.Value);
        uiNode.UpdateMultiplierUi(mainData.Score.ScoreMultiplier);
    }

    public void _ScoreProcess(float delta)
    {
        delayCounter += delay * delta;

        if (delayCounter >= 1)
        {
            long? score = mainData.Score.ProcessRollingScore();
            if (score is not null) { uiNode.UpdateScoreUi(score.GetValueOrDefault()); }
            delayCounter = 0;
        }
    }

    //update score for events without level info
    private void UpdateScore(int points)
    {
        UpdateScore(points, 0);
    }

    private void UpdateScore(int points, int level)
    {
        int calcPoints = mainData.Score.SetRollingScore(points, level);
        uiNode.FlashPoints(calcPoints);
    }

    private void BreakScoreUpdate()
    {
        mainData.Score.BreakRollingScore();
    }

    private void UpdateMultiplier(bool reset)
    {
        mainData.Score.UpdateMultiplier(reset);
        uiNode.UpdateMultiplierUi(mainData.Score.ScoreMultiplier);
    }
}