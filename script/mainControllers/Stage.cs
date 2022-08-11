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

    private bool newWaveFirstCall = false; 

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
        bool newStage = false; 
        stageWaveValues[stageCounter]--;

        //for the first call to return true 
        if(!newWaveFirstCall) {
            newStage = true;
            stageWaveValues[stageCounter]++;
            newWaveFirstCall = true;
        }

        if(stageWaveValues[stageCounter] <= 0) { 
            stageCounter++;
            newStage = true;

            if(stageCounter > stageWaveValues.Length - 1) {
                stageCounter = 0;
                NextLevel();
            }
        }

        return newStage;
    }

    private void NextLevel() {
        level++;

        //Increase dodge and fight
        stageWaveValues = new int[] {(3 + level), (4 + level), 1, 1};
    }
}