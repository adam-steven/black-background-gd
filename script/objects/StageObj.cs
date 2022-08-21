
using System;
using static Enums;

public class StageObj {

    public int level {get; private set;}
    
    protected int stageCounter = 0;
    private int[] stageWaveValues = {3, 4, 1, 1};
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

    public int noOfWaves {
        get {
            return stageWaveValues[stageCounter];
        }
    }

    public double stageProgression {
        get {
            return (1 - (currentWaveCounter / stageWaveValues[stageCounter])) * 100;
        }
    }

    public void NextWave() {
        currentWaveCounter = Math.Floor(currentWaveCounter) + 1;
        DisplayProgression();

        if(currentWaveCounter >= stageWaveValues[stageCounter]) { 
            currentWaveCounter = 0;
            stageCounter++;

            if(stageCounter > stageWaveValues.Length - 1) {
                stageCounter = 0;
                mainData.stage.NextLevel();
            }

            uiNode.SetWaveSegments(mainData.stage.noOfWaves);
            uiNode.SetWaveProgress(100);
        }

        return (currentWaveCounter == 0);
    }

    public void NextLevel() {
        level++;
        stageWaveValues = new int[] {(3 + level), (4 + level), 1, 1}; //Increase dodge and fight
    }

    public StageObj() {}
}