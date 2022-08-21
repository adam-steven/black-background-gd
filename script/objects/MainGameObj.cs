//Object for Main.tscn data in 

public class MainGameObj {

    public bool inGame { get; set; } //true: auto play game, false: show main menu

    public ScoreObj score { get; set; }
    public StageObj stage { get; set; }

    public MainGameObj(bool inGame) {
        this.inGame = inGame;
        this.score = new ScoreObj();
        this.stage = new StageObj();
    }
}