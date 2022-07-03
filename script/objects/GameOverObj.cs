//Object for DeathScreen.tscn data in 

public class GameOverObj {

    public int score { get; set; }
    public int time { get; set; }

    public GameOverObj(int score, int time ) {
        this.score = score;
        this.time = time;
    }
}