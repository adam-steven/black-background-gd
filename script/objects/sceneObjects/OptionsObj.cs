//Object for OptionsScreen.tscn data in 

public class OptionsObj
{
    public MainGameObj gameObj { get; set; } //true: return to game, false: return to main menu

    public OptionsObj() 
    {
        this.gameObj = new MainGameObj(false);
    }

    public OptionsObj(MainGameObj gameObj) {
        this.gameObj = gameObj;
    }
}