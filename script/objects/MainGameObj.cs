//Object for Main.tscn data in 

public class MainGameObj {

    public bool inGame { get; set; } //true: auto play game, false: show main menu

    public MainGameObj(bool inGame) {
        this.inGame = inGame;
    }
}