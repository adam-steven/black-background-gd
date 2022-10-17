using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class EntityStats
{
    [JsonProperty] public int Health { get; private set; }
    [JsonProperty] public float MovementForce { get; private set; }

    [JsonProperty] public float ShotDelay { get; private set; }
    [JsonProperty] public int NoOfBullets { get; private set; }
    [JsonProperty] public float BulletForce { get; private set; }
    [JsonProperty] public int BulletStrength { get; private set; }
    [JsonProperty] public float BulletAccuracy { get; private set; }
    [JsonProperty] public int BulletBurstAmount { get; private set; }
    [JsonProperty] public float BulletTimeAlive { get; private set; }
    [JsonProperty] public float BulletSize { get; private set; }
    [JsonProperty] public List<string> OnBulletDestroyScenes { get; private set; }

    public EntityStats() {}

    public EntityStats(int health, float movementForce, float shotDelay, int noOfBullets, float bulletForce, int bulletStrength, float bulletAccuracy, int bulletBurstAmount, float bulletTimeAlive, float bulletSize, List<string> onBulletDestroyScenes)
    {
        Health = health;
        MovementForce = movementForce;
        ShotDelay = shotDelay;
        NoOfBullets = noOfBullets;
        BulletForce = bulletForce;
        BulletStrength = bulletStrength;
        BulletAccuracy = bulletAccuracy;
        BulletBurstAmount = bulletBurstAmount;
        BulletTimeAlive = bulletTimeAlive;
        BulletSize = bulletSize;
        OnBulletDestroyScenes = onBulletDestroyScenes ?? new List<string>();  
    }
}