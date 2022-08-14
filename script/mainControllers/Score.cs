using Godot;
using System;

public class Score : Godot.Object
{
    private UiController ui;
    public long score { private set; get; }
    private long tempScore; //Temporarily holds the full score value for a tick up effect
	public int scoreMultiplier { private set; get; }

    //tick up delay
    public float delay = 60;
    public float delayCounter = 0;

    public Score(UiController ui) {
        this.ui = ui;
        ResetMultiplier();
    }

    public void _ScoreProcess(float delta) {
        if(score >= tempScore) { return; }
        delayCounter += delay * delta;

        if(delayCounter >= 1) {
            score++;
            ui.UpdateScoreUi(score);
            delayCounter = 0;
        }
    }

    //update score for events without level info
    private void UpdateScore(int points) {
        UpdateScore(points, 0);
    }

    private void UpdateScore(int points, int level) {
        float levelMultiplier = 1 + (level * 0.1f);
        int calcPoints = (int)Math.Round(points * scoreMultiplier * levelMultiplier);

        ui.FlashPoints(calcPoints);

        tempScore += calcPoints;
        tempScore = Math.Min(tempScore, 99999999999999);
        tempScore = Math.Max(tempScore, -9999999999999);
    }

    private void BreakScoreUpdate() {
        tempScore = score;
    }

    public void DecrementMultiplier () { 
        scoreMultiplier--;
        ui.UpdateMultiplierUi(scoreMultiplier);
    }

    public void ResetMultiplier() { 
        scoreMultiplier = 4; 
        ui.UpdateMultiplierUi(scoreMultiplier);
    }
}
