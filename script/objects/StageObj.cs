
using System;
using static Enums;
using Newtonsoft.Json;

public class StageObj {

    [JsonProperty] public int level {get; private set;}
    [JsonProperty] public int stageCounter { get; private set; }
    [JsonProperty] public int[] stageWaveValues { get; private set; }
    [JsonProperty] public double currentWaveCounter { get; private set; }

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

    ///<returns>timer ended</returns>
    public Nullable<bool> ProcessStageCountDown(float delta) {
        if(currentWaveCounter >= stageWaveValues[stageCounter] - 1) { return null; }

        if(Math.Round(currentWaveCounter * 10) % 10 != 9) {
            currentWaveCounter += 0.05f * delta;
            return false;
        } else {
            currentWaveCounter = Math.Floor(currentWaveCounter);
            return true;
        }
    }

    public double NextWave() {
        currentWaveCounter = Math.Floor(currentWaveCounter) + 1;

        if(currentWaveCounter >= stageWaveValues[stageCounter]) { 
            currentWaveCounter = 0;
            stageCounter++;

            if(stageCounter > stageWaveValues.Length - 1) {
                stageCounter = 0;
                NextLevel();
            }
        }

        return currentWaveCounter;
    }

    private void NextLevel() {
        level++;
        UpdateStageLengths();
    }

    //Increase dodge and fight
    private void UpdateStageLengths() {
        stageWaveValues = new int[] {(3 + level), (4 + level), 1, 1}; 
    }

    public StageObj() {
        stageCounter = 0;
        currentWaveCounter = -1;
        UpdateStageLengths();
    }
}