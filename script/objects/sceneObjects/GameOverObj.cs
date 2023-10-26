//Object for DeathScreen.tscn data in 

public partial class GameOverObj
{
    public long Score { get; set; }
    public int Time { get; set; }

    public GameOverObj(long score, int time)
    {
        this.Score = score;
        this.Time = time;
    }
}