//Object for OptionsScreen.tscn data in 

public class OptionsObj {

    public bool inGame { get; set; } //true: return to game, false: return to main menu

    public OptionsObj(bool inGame) {
        this.inGame = inGame;
    }
}