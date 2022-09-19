//Object for Main.tscn data in 

public class MainGameObj
{
    public bool inGame { get; set; } //true: auto play game, false: show main menu

    public Scenes enemies { get; set; }
    public Scenes obstacles { get; set; }
    public Scenes upgrades { get; set; }

    public Score score { get; set; }
    public Stage stage { get; set; }

    public MainGameObj(bool inGame)
    {
        this.inGame = inGame;
        this.score = new Score();
        this.stage = new Stage();
        this.enemies = new Scenes();
        this.obstacles = new Scenes();
        this.upgrades = new Scenes();
    }
}