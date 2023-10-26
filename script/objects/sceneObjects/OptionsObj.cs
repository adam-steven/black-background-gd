//Object for OptionsScreen.tscn data in 

public partial class OptionsObj
{
    public MainGameObj GameObj { get; set; } //true: return to game, false: return to main menu

    public OptionsObj()
    {
        this.GameObj = new MainGameObj(false);
    }

    public OptionsObj(MainGameObj gameObj)
    {
        this.GameObj = gameObj;
    }
}