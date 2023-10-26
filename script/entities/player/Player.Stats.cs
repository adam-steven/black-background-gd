using System;
using System.Linq;

public partial class Player
{
    public EntityStats GetStats() {
		return new EntityStats(this);
	}

    public void SetStats(EntityStats? stats, bool healthUpgrade = false) {
		if(stats is null) { return; }
		EntityStats entityStats = (EntityStats)stats;

		Health = Mathc.Limit(0, entityStats.Health, 1000);
		MovementForce = Mathc.Limit(100f, entityStats.MovementForce, 5000f);
		ShotDelay = Mathc.Limit(0.1f, entityStats.ShotDelay, 10f);
		NoOfBullets = Mathc.Limit(1, entityStats.NoOfBullets, 30);
		BulletForce = Mathc.Limit(100f, entityStats.BulletForce, 5000f);
		BulletStrength = Mathc.Limit(1, entityStats.BulletStrength, 5000);
		BulletAccuracy = Mathc.Limit(0f, entityStats.BulletAccuracy, 360f);
		BulletBurstAmount = Mathc.Limit(1, entityStats.BulletBurstAmount, 15);
		BulletTimeAlive = Mathc.Limit(0.05f, entityStats.BulletTimeAlive, 10f);
		BulletSize = Mathc.Limit(0.5f, entityStats.BulletSize, 15f);
		OnBulletDestroyScenes = new (entityStats.OnBulletDestroyScenes.Take(10));

		//Update background colour and health UI
		this.EmitSignal(SignalName.UpdateHealthUi, Health, healthUpgrade);
		Colour.UpdateBackgroundColour(Health);
	}

	public void UpdateStats(EntityStats addStats)
	{
        EntityStats stats = new EntityStats(this).Add(addStats);
        SetStats(stats, addStats.Health > 0);
	}
}