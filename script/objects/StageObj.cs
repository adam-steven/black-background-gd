
using static Enums;

public class StageObj {

    public int level {get; private set;}
    
    protected int stageCounter = 0;
    protected int[] stageWaveValues = {3, 4, 1, 1};
    protected double currentWaveCounter = -1; 

    public GameStages currentStage { 
        get {
            switch (stageCounter)
            {
                case 0:
                    return GameStages.Dodge;
                case 1:
                    return GameStages.Fight;
                case 2:
                    return GameStages.Boss;
                case 3:
                    return GameStages.Shop; 
                default:
                    return GameStages.Event;
            }
        }
    }

    public void SetNextLevel() {
        level++;
        stageWaveValues = new int[] {(3 + level), (4 + level), 1, 1}; //Increase dodge and fight
    }

    public StageObj() {}
}