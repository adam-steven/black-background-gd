using Godot;
using System;

public partial class Main
{
    //tick up delay
    private float delay = 60;
    private float delayCounter = 0;

    public void _ScoreReady() {
        ResetMultiplier();
    }

    public void _ScoreProcess(float delta) {
        if(score >= tempScore) { return; }
        delayCounter += delay * delta;

        if(delayCounter >= 1) {
            score++;
            uiNode.UpdateScoreUi(score);
            delayCounter = 0;
        }
    }

    //update score for events without level info
    private void UpdateScore(int points) {
        UpdateScore(points, 0);
    }

    private void UpdateScore(int points, int level) {
        float levelMultiplier = 1 + (level * 0.1f);
        int calcPoints = (int)Math.Round(points * mainData.score.scoreMultiplier * levelMultiplier);

        uiNode.FlashPoints(calcPoints);

        mainData.score.SetRollingScore(calcPoints);
    }

    private void BreakScoreUpdate() {
        mainData.score.SetRollingScore(mainData.score.score);
    }

    public void DecrementMultiplier () { 
        scoreMultiplier--;
        uiNode.UpdateMultiplierUi(scoreMultiplier);
    }

    public void ResetMultiplier() { 
        scoreMultiplier = 4; 
        uiNode.UpdateMultiplierUi(scoreMultiplier);
    }
}