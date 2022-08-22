using Godot;
using System;
using static Enums;

public partial class Main
{
    public void _StageReady() {
        uiNode.SetWaveSegments(mainData.stage.noOfWaves);
    }

    public void _StageProcess(float delta) {
        bool? stageTimerEnd = mainData.stage.ProcessStageCountDown(delta);  
        
        if(stageTimerEnd == false) { DisplayProgression(); }
        else if(stageTimerEnd == true) { NextStage(); }
    }

    ///<returns>new stage</returns>
    public bool NextWave() {
        double currentWaveCounter = mainData.stage.NextWave();

        DisplayProgression();
        if(currentWaveCounter == 0) { 
            uiNode.SetWaveSegments(mainData.stage.noOfWaves); 
        }

        return (currentWaveCounter == 0);
    }

    private void DisplayProgression() {
        uiNode.SetWaveProgress(mainData.stage.stageProgression);
    }
}