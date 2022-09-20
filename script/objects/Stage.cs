
using System;
using static Enums;
using Newtonsoft.Json;

public class Stage
{

    [JsonProperty] public int Level { get; private set; }
    [JsonProperty] public int StageCounter { get; private set; }
    [JsonProperty] public int[] StageWaveValues { get; private set; }
    [JsonProperty] public double CurrentWaveCounter { get; private set; }

    public GameStages CurrentStage
    {
        get
        {
            switch (StageCounter)
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

    public int NoOfWaves
    {
        get
        {
            return StageWaveValues[StageCounter];
        }
    }

    public double StageProgression
    {
        get
        {
            return (1 - (CurrentWaveCounter / StageWaveValues[StageCounter])) * 100;
        }
    }

    ///<returns>timer ended</returns>
    public Nullable<bool> ProcessStageCountDown(float delta)
    {
        if (CurrentWaveCounter >= StageWaveValues[StageCounter] - 1) { return null; }

        if (Math.Round(CurrentWaveCounter * 10) % 10 != 9)
        {
            CurrentWaveCounter += 0.05f * delta;
            return false;
        }
        else
        {
            CurrentWaveCounter = Math.Floor(CurrentWaveCounter);
            return true;
        }
    }

    public double NextWave(bool gameStart = false)
    {
        if (gameStart) { return CurrentWaveCounter; }

        CurrentWaveCounter = Math.Floor(CurrentWaveCounter) + 1;

        if (CurrentWaveCounter >= StageWaveValues[StageCounter])
        {
            CurrentWaveCounter = 0;
            StageCounter++;

            if (StageCounter > StageWaveValues.Length - 1)
            {
                StageCounter = 0;
                NextLevel();
            }
        }

        return CurrentWaveCounter;
    }

    private void NextLevel()
    {
        Level++;
        UpdateStageLengths();
    }

    //Increase dodge and fight
    private void UpdateStageLengths()
    {
        StageWaveValues = new int[] { (3 + Level), (4 + Level), 1, 1 };
    }

    public Stage()
    {
        StageCounter = 0;
        CurrentWaveCounter = 0;
        UpdateStageLengths();
    }
}