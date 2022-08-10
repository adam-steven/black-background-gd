using Godot;
using System;
using static Enums;

public class Stage : Godot.Object
{
    private UiController ui;

    public int level {get; private set;}

    public bool inGame {
        get {
            return level > 0;
        }
    }

    private int stageCounter = 0;
    private int[] stageWaveValues = {3, 4, 1, 1};

    private int currentWaveCounter = -1;

    public Stage(UiController ui) {
        this.ui = ui;
    }

    public GameStages GetStage() {
        switch (stageCounter)
        {
            case 0:
                return GameStages.Dodge;
            case 1:
                return GameStages.Fight;
            case 2:
                return GameStages.Boss;
            case 3:
                return GameStages.Upgrade; 
            default:
                return GameStages.Event;
        }
    }

    ///<returns>new stage</returns>
    public bool NextWave() {
        currentWaveCounter++;

        if(currentWaveCounter >= stageWaveValues[stageCounter]) { 
            currentWaveCounter = 0;
            stageCounter++;
        }

        if(stageCounter > stageWaveValues.Length - 1) {
            stageCounter = 0;
            NextLevel();
        }

        return (currentWaveCounter == 0);
    }

    private void NextLevel() {
        level++;

        //Increase dodge and fight
        stageWaveValues[0]++;
        stageWaveValues[1]++;
    }
}