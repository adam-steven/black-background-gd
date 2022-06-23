using System;
using Godot;
using static Enums;

public class GunController : RigidBody2D
{
	Random rnd = new Random();

	private PackedScene bulletScene;
	private RigidBody2D ownerNode;
	private BulletOwner bulletOwner;
	private Entities stats;

    private int timeLastShot = 0;

	//If currentBulletInBurst < bulletBurstAmount the fire rate is halfed
	private int currentBulletInBurst;
	private float betweenBurstDelay = 0.2f;

	///<summary> 
	///		Allows nodes to shoot bullets form there body.
	///		Requires: 
	///			Child Sprite Node,
	///</summary>
	public GunController(
		RigidBody2D ownerNode, 
		BulletOwner bulletOwner
	){
		this.bulletScene = (PackedScene)GD.Load("res://scenes/misc/Bullet.tscn");
		this.ownerNode = ownerNode;
		this.bulletOwner =  bulletOwner;
		this.stats = (Entities)ownerNode;
	}

	public void Shoot(bool isBursting = false) {
		if(!CanShoot(isBursting)) return;
		
		Godot.Sprite ownerSprite = ownerNode.GetNode<Godot.Sprite>("Sprite");
		Godot.Node2D gameController = ownerNode.GetParent<Godot.Node2D>();

		//Loop for shotgun effect
		for (int i = 0; i < stats.noOfBullets; i++) {
			SpawnBullet(ownerSprite, gameController);
		}

		currentBulletInBurst++;
		if(currentBulletInBurst > stats.bulletBurstAmount) currentBulletInBurst = 0;
	}

	//Checks if the threshold amount of time has passed since the last shot
	//isBursting = true lowers the nextShotThreshold
	private bool CanShoot(bool isBursting = false) {
		//Make sure the burst is always faster than normal shots
		betweenBurstDelay = (stats.shotDelay < betweenBurstDelay) ? stats.shotDelay/2f : 0.2f;

		int shotDelayMs = (int)( (!isBursting ? stats.shotDelay : betweenBurstDelay) * 1000 );
		int nextShotThreshold = timeLastShot + shotDelayMs;
		
		int currentTime = (int)OS.GetTicksMsec();
		bool canShoot = (currentTime >= nextShotThreshold);

		if(canShoot) timeLastShot = currentTime;

    	return canShoot;
	}

	private void SpawnBullet(Godot.Sprite ownerSprite, Godot.Node2D gameController) {
		Area2D bullet = (Area2D)bulletScene.Instance();
		BulletController bulletCon = (BulletController)bullet;
		float randomAccuracyDeviation = (float)((rnd.NextDouble() * stats.bulletAccuracy) - (rnd.NextDouble() * stats.bulletAccuracy));

		// Access bullet properties
		bullet.Position = ownerNode.Position;
		bullet.Rotation = ownerSprite.GlobalRotation + randomAccuracyDeviation;

		// Access bullet script 
		bulletCon.bOwner = bulletOwner;
		// bulletCon.openMotion = ownerNode.LinearVelocity/2f;
		bulletCon.strength = stats.bulletStrength;
		bulletCon.movementForce = stats.bulletForce;
		bulletCon.timeAlive = stats.bulletTimeAlive;

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
