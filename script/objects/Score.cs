using System;
using Newtonsoft.Json;

public class Score
{
    [JsonProperty] public long Value { get; private set; }
    [JsonProperty] public long TempValue { get; private set; } //Temporarily holds the full score value for a tick up effect
    [JsonProperty] public int ScoreMultiplier { get; private set; }

    public int SetRollingScore(int points, int level)
    {
        float levelMultiplier = 1 + (level * 0.1f);
        int calcPoints = (int)Math.Round(points * ScoreMultiplier * levelMultiplier);

        TempValue += calcPoints;
        TempValue = Mathc.Limit(-9999999999999, TempValue, 99999999999999);

        return calcPoints;
    }

    public void BreakRollingScore()
    {
        TempValue = Value;
    }

    public Nullable<long> ProcessRollingScore()
    {
        long difference = (TempValue - Value);

        if (difference == 0) { return null; }
        Value += difference / Math.Abs(difference);
        return Value;
    }

    public void UpdateMultiplier(bool reset)
    {
        ScoreMultiplier = (reset) ? 4 : ScoreMultiplier - 1;
    }

    public Score()
    {
        ScoreMultiplier = 4;
    }
}