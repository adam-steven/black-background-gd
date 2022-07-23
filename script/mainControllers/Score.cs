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

    private void UpdateScore(int points) {
        int calcPoints = points * scoreMultiplier;

        ui.FlashPoints(calcPoints);

        tempScore += calcPoints;
        tempScore = Math.Min(tempScore, 99999999999999);
    }

    private void BreakScoreUpdate() {
        tempScore = score;
    }

    public void DecrementMultiplier () { scoreMultiplier--; }
    public void ResetMultiplier() { scoreMultiplier = 4; }
}
