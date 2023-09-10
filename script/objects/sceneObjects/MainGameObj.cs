//Object for Main.tscn data in 
public class MainGameObj
{
    public bool InGame { get; set; } //true: auto play game, false: show main menu

    public Scenes Enemies { get; set; }
    public Scenes Obstacles { get; set; }
    public Scenes Upgrades { get; set; }

    public Score Score { get; set; }
    public Stage Stage { get; set; }

    public EntityStats? PlayerStats { get; set; }

    public Scenes StoredUpgrades { get; set; }
    public Scenes StoredEnemies { get; set; }

    public int EnemySpawnDiversity { get; set; }
    public int EnemySpawnMin { get; set; }
	public int EnemySpawnMax { get; set; }

    public MainGameObj(bool inGame)
    {
        this.InGame = inGame;
        this.Score = new Score();
        this.Stage = new Stage();
        this.Enemies = new Scenes();
        this.Obstacles = new Scenes();
        this.Upgrades = new Scenes();
        this.PlayerStats = null;
        this.StoredUpgrades = null;
        this.StoredEnemies = null;
        this.EnemySpawnDiversity = 1;
        this.EnemySpawnMin = 1;
	    this.EnemySpawnMax = 2;
    }
}