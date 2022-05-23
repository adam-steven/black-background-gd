//Object for Main.tscn data in 

public class MainGameObj {

    public bool isQuickReset { get; set; } //true, auto play game, false show main menu

    public MainGameObj(bool isQuickReset) {
        this.isQuickReset = isQuickReset;
    }
}