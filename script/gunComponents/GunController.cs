using System;
using Godot;
using static Enums;

public class GunController
{
	Random rnd = new Random();

	private PackedScene bulletScene;
	private Entities ownerNode;

    private int timeLastShot = 0;

	//If currentBulletInBurst < bulletBurstAmount the fire rate is halfed
	private int currentBulletInBurst;
	private float betweenBurstDelay = 0.2f;

	///<summary> 
	///		Allows nodes to shoot bullets form there body.
	///		Requires: 
	///			Child Sprite Node,
	///</summary>
	public GunController(Entities ownerNode) {
		this.bulletScene = (PackedScene)GD.Load("res://scenes/misc/Bullet.tscn");
		this.ownerNode = ownerNode;
	}

	public void Shoot(bool isBursting = false, BulletVariations bulletType = BulletVariations.Normal) {
		if(!CanShoot(isBursting)) return;

		Godot.Sprite ownerSprite = ownerNode.GetNode<Godot.Sprite>("Sprite");
		Godot.Node2D gameController = ownerNode.GetParent<Godot.Node2D>();

		//Loop for shotgun effect
		for (int i = 0; i < ownerNode.noOfBullets; i++) {
			SpawnBullet(ownerSprite, gameController, bulletType);
		}

		currentBulletInBurst++;
		if(currentBulletInBurst > ownerNode.bulletBurstAmount) currentBulletInBurst = 0;
	}

	//Checks if the threshold amount of time has passed since the last shot
	//isBursting = true lowers the nextShotThreshold
	private bool CanShoot(bool isBursting = false) {
		//Make sure the burst is always faster than normal shots
		betweenBurstDelay = (ownerNode.shotDelay < betweenBurstDelay) ? ownerNode.shotDelay/2f : 0.2f;

		int shotDelayMs = (int)( (!isBursting ? ownerNode.shotDelay : betweenBurstDelay) * 1000 );
		int nextShotThreshold = timeLastShot + shotDelayMs;
		
		int currentTime = (int)OS.GetTicksMsec();
		bool canShoot = (currentTime >= nextShotThreshold);

		if(canShoot) timeLastShot = currentTime;

    	return canShoot;
	}

	private void SpawnBullet(Godot.Sprite ownerSprite, Godot.Node2D gameController, BulletVariations bulletType) {
		BulletController bullet = (BulletController)bulletScene.Instance();
		float randomAccuracyDeviation = (float)((rnd.NextDouble() * ownerNode.bulletAccuracy) - (rnd.NextDouble() * ownerNode.bulletAccuracy));

		// Access bullet properties
		bullet.Position = ownerNode.Position;
		bullet.Rotation = ownerSprite.GlobalRotation + randomAccuracyDeviation;
		bullet.Scale = new Vector2(ownerNode.bulletSize, ownerNode.bulletSize);

		// Access bullet script 
		bullet.bOwner = ownerNode.entityType;
		// bulletCon.openMotion = ownerNode.LinearVelocity/2f;
		bullet.strength = ownerNode.bulletStrength;
		bullet.movementForce = ownerNode.bulletForce;
		bullet.timeAlive = ownerNode.bulletTimeAlive;
		bullet.type = bulletType;

		// Shoot bullet + start cooldown 
		gameController.AddChild(bullet);
	}

	//Call in _PhysicsProcess so that the gun can continue a burst fire without Shoot() being called
	public void UpdateBurst() {
		if(currentBulletInBurst != 0) {
			Shoot(true);
		}
	}
}
