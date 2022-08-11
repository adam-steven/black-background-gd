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
    private float currentWaveCounter = -1; 

    public Stage(UiController ui) {
        this.ui = ui;
        ui.SetWaveSegments(stageWaveValues[stageCounter]);
    }

    public void _StageProcess(float delta) {
        if(currentWaveCounter >= stageWaveValues[stageCounter] - 1) { return; }
        
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
        double progression = (1 - (currentWaveCounter / stageWaveValues[stageCounter])) * 100;
        ui.SetWaveProgress(progression);

        if(currentWaveCounter >= stageWaveValues[stageCounter]) { 
            currentWaveCounter = 0;
            stageCounter++;

            if(stageCounter > stageWaveValues.Length - 1) {
                stageCounter = 0;
                NextLevel();
            }

            ui.SetWaveSegments(stageWaveValues[stageCounter]);
            ui.SetWaveProgress(100);
        }

        return (currentWaveCounter == 0);
    }

    private void NextLevel() {
        level++;

        //Increase dodge and fight
        stageWaveValues = new int[] {(3 + level), (4 + level), 1, 1};
    }
}