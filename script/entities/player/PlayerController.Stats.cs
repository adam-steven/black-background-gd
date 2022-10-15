using Godot;
using System;
using static Enums;

public partial class PlayerController
{
    public EntityStats GetStats() {
		return new EntityStats(health, movementForce, shotDelay, noOfBullets, bulletForce, bulletStrength, bulletAccuracy, bulletBurstAmount, bulletTimeAlive, bulletSize);
	}

    public void SetStats(EntityStats stats, bool healthUpgrade = false) {
		health = Mathc.Limit(0, stats.Health, 1000);
		movementForce = Mathc.Limit(100f, stats.MovementForce, 5000f);
		shotDelay = Mathc.Limit(0.1f, stats.ShotDelay, 10f);
		noOfBullets = Mathc.Limit(1, stats.NoOfBullets, 30);
		bulletForce = Mathc.Limit(100f, stats.BulletForce, 5000f);
		bulletStrength = Mathc.Limit(1, stats.BulletStrength, 5000);
		bulletAccuracy = Mathc.Limit(0f, stats.BulletAccuracy, 360f);
		bulletBurstAmount = Mathc.Limit(1, stats.BulletBurstAmount, 15);
		bulletTimeAlive = Mathc.Limit(0.05f, stats.BulletTimeAlive, 10f);
		bulletSize = Mathc.Limit(0.5f, stats.BulletSize, 15f);

		//Update background colour and health UI
		this.EmitSignal("_update_health_ui", health, healthUpgrade);
		Colour.UpdateBackgroundColour(health);
	}

	public void UpdateStats(EntityStats addStats)
	{
        EntityStats stats = new EntityStats(
            health + addStats.Health,
            movementForce + addStats.MovementForce,
            shotDelay + addStats.ShotDelay,
            noOfBullets + addStats.NoOfBullets,
            bulletForce + addStats.BulletForce,
            bulletStrength + addStats.BulletStrength,
            bulletAccuracy + addStats.BulletAccuracy,
            bulletBurstAmount + addStats.BulletBurstAmount,
            bulletTimeAlive + addStats.BulletTimeAlive,
            bulletSize + addStats.BulletSize
        );
		
        SetStats(stats, (addStats.Health > 0));
	}


}