using Godot;
using System;

public class Score : Godot.Object
{
    private UiController ui;
    public long score { private set; get; }
    private long tempScore; //Temporarily holds the full score value for a tick up effect
	public int scoreMultiplier { private set; get; }

    public Score(UiController ui) {
        this.ui = ui;
        ResetMultiplier();
    }

    public void _ScoreProcess(float delta) {
        if(score < tempScore) {
            score = score + 1;
            ui.UpdateScoreUi(score);
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
