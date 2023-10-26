using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

public struct EntityStats : IStats
{
    [JsonProperty] public int Health { get; set; }
    [JsonProperty] public float MovementForce { get; set; }
    [JsonProperty] public float ShotDelay { get; set; }
    [JsonProperty] public int NoOfBullets { get; set; }
    [JsonProperty] public float BulletForce { get; set; }
    [JsonProperty] public int BulletStrength { get; set; }
    [JsonProperty] public float BulletAccuracy { get; set; }
    [JsonProperty] public int BulletBurstAmount { get; set; }
    [JsonProperty] public float BulletTimeAlive { get; set; }
    [JsonProperty] public float BulletSize { get; set; }
    [JsonProperty] public Godot.Collections.Array<string> OnBulletDestroyScenes { get; set; }

    public EntityStats(IStats stats)
    {
        Health = stats.Health;
        MovementForce = stats.MovementForce;
        ShotDelay = stats.ShotDelay;
        NoOfBullets = stats.NoOfBullets;
        BulletForce = stats.BulletForce;
        BulletStrength = stats.BulletStrength;
        BulletAccuracy = stats.BulletAccuracy;
        BulletBurstAmount = stats.BulletBurstAmount;
        BulletTimeAlive = stats.BulletTimeAlive;
        BulletSize = stats.BulletSize;
        OnBulletDestroyScenes = stats.OnBulletDestroyScenes ?? new ();  
    }

    public EntityStats(int health, float movementForce, float shotDelay, int noOfBullets, float bulletForce, int bulletStrength, float bulletAccuracy, int bulletBurstAmount, float bulletTimeAlive, float bulletSize, Godot.Collections.Array<string> onBulletDestroyScenes)
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
        OnBulletDestroyScenes = onBulletDestroyScenes ?? new ();  
    }

    public EntityStats Add(EntityStats item)
    {
        return new EntityStats(
            Health + item.Health,
            MovementForce + item.MovementForce,
            ShotDelay + item.ShotDelay,
            NoOfBullets + item.NoOfBullets,
            BulletForce + item.BulletForce,
            BulletStrength + item.BulletStrength,
            BulletAccuracy + item.BulletAccuracy,
            BulletBurstAmount + item.BulletBurstAmount,
            BulletTimeAlive + item.BulletTimeAlive,
            BulletSize + item.BulletSize,
			new (OnBulletDestroyScenes.Concat(item.OnBulletDestroyScenes))
		);
    }
}