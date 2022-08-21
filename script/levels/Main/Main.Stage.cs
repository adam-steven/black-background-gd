using Godot;
using System;
using static Enums;

public partial class Main
{
    public void _StageReady() {
        uiNode.SetWaveSegments(stageWaveValues[stageCounter]);
    }

    public void _StageProcess(float delta) {
        if(currentWaveCounter >= stageWaveValues[stageCounter] - 1) { return; }

        if(Math.Round(currentWaveCounter * 10) % 10 != 9) {
            currentWaveCounter += 0.05f * delta;
            DisplayProgression();
        } else {
            currentWaveCounter = Math.Floor(currentWaveCounter);
            this.EmitSignal("_next_stage");
        }
    }

    ///<returns>new stage</returns>
    public bool NextWave() {
        currentWaveCounter = Math.Floor(currentWaveCounter) + 1;
        DisplayProgression();

        if(currentWaveCounter >= stageWaveValues[stageCounter]) { 
            currentWaveCounter = 0;
            stageCounter++;

            if(stageCounter > stageWaveValues.Length - 1) {
                stageCounter = 0;
                mainData.stage.SetNextLevel();
            }

            uiNode.SetWaveSegments(stageWaveValues[stageCounter]);
            uiNode.SetWaveProgress(100);
        }

        return (currentWaveCounter == 0);
    }

    private void DisplayProgression() {
        double progression = (1 - (currentWaveCounter / stageWaveValues[stageCounter])) * 100;
        uiNode.SetWaveProgress(progression);
    }
}