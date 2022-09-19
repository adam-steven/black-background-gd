//Object for DeathScreen.tscn data in 

public class GameOverObj
{
    public long score { get; set; }
    public int time { get; set; }

    public GameOverObj(long score, int time)
    {
        this.score = score;
        this.time = time;
    }
}