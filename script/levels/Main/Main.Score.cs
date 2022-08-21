using Godot;
using System;

public partial class Main
{
    //tick up delay
    private float delay = 60;
    private float delayCounter = 0;

    public void _ScoreReady() {
        uiNode.UpdateMultiplierUi(mainData.score.scoreMultiplier);
    }

    public void _ScoreProcess(float delta) {
        delayCounter += delay * delta;

        if(delayCounter >= 1) {
            long? score = mainData.score.ProcessRollingScore();  
            if(score != null) { uiNode.UpdateScoreUi(score.GetValueOrDefault()); } 
            delayCounter = 0;
        }
    }

    //update score for events without level info
    private void UpdateScore(int points) {
        UpdateScore(points, 0);
    }

    private void UpdateScore(int points, int level) {
        int calcPoints = mainData.score.SetRollingScore(points, level);
        uiNode.FlashPoints(calcPoints);
    }

    private void BreakScoreUpdate() {
        mainData.score.BreakRollingScore();
    }

    private void UpdateMultiplier(bool reset) {
        mainData.score.UpdateMultiplier(reset);
        uiNode.UpdateMultiplierUi(mainData.score.scoreMultiplier);
    }
}