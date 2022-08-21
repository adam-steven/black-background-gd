public class ScoreObj {

    protected long score { set; get; }
    protected long tempScore { set; get; } //Temporarily holds the full score value for a tick up effect
	protected int scoreMultiplier { set; get; }

    public ScoreObj() {}
}