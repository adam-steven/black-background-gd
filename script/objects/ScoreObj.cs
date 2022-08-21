using System;

public class ScoreObj {

    public long score { get; private set; }
    private long tempScore { get; set; } //Temporarily holds the full score value for a tick up effect
	public int scoreMultiplier {  get; private set; }

    public int SetRollingScore(int points, int level){
        float levelMultiplier = 1 + (level * 0.1f);
        int calcPoints = (int)Math.Round(points * scoreMultiplier * levelMultiplier);
        tempScore += calcPoints;
        tempScore = Mathc.Limit(-9999999999999, tempScore, 99999999999999);

        return calcPoints;
    }

    public void BreakRollingScore() {
        tempScore = score;
    }


    public Nullable<long> ProcessRollingScore() {
        if(score >= tempScore) { return null; }
        score++;
        return score;
    }

    public void UpdateMultiplier(bool reset) {
        scoreMultiplier = (reset) ? 4 : scoreMultiplier - 1;
    }

    public ScoreObj() {}
}