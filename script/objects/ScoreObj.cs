using System;

public class ScoreObj {

    public long score { get; private set; }
    private long tempScore { get; set; } //Temporarily holds the full score value for a tick up effect
	public int scoreMultiplier {  get; private set; }

    public void SetRollingScore(long val){
        tempScore += val;
        tempScore = Mathc.Limit(-9999999999999, tempScore, 99999999999999);
    }

    public ScoreObj() {}
}