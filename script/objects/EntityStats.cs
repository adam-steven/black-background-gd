using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class EntityStats : IStats
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

    [JsonProperty] public int health { get; set; }
	[JsonProperty] public float movementForce { get; set; }
	[JsonProperty]public float shotDelay { get; set; }
	[JsonProperty] public int noOfBullets { get; set; }
	[JsonProperty] public float bulletForce { get; set; }
	[JsonProperty] public int bulletStrength { get; set; }
	[JsonProperty] public float bulletAccuracy { get; set; }
	[JsonProperty] public int bulletBurstAmount { get; set; }
	[JsonProperty] public float bulletTimeAlive { get; set; }
	[JsonProperty] public float bulletSize { get; set; }
	[JsonProperty] public List<string> onBulletDestroyScenes { get; set; }

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