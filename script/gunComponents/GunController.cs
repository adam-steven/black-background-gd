using System;
using Godot;
using static Enums;

public class GunController : RigidBody2D
{
	Random rnd = new Random();

	private PackedScene bulletScene;
	private bool canShoot = true;
	private RigidBody2D ownerNode;
	private BulletOwner bulletOwner;
	
	private int noOfBullets;
	private int bulletStrength;
	private float bulletForce;
	private float bulletAccuracy;
	private float bulletAliveTime;

	//If currentBulletInBurst < bulletBurstAmount the fire rate is halfed
	private int currentBulletInBurst;
	private int bulletBurstAmount;
	private float betweenBurstDelay = 0.2f;

	private float shotDelay;
    private int timeLastShot = 0;

	///<summary> 
	///		Allows nodes to shoot bullets form there body.
	///		Requires: 
	///			Child Sprite Node,
	///</summary>
	public GunController(
		float shotDelay, 
		RigidBody2D ownerNode, 
		BulletOwner bulletOwner, 
		int noOfBullets, 
		int bulletStrength, 
		float bulletForce, 
		float bulletAccuracy, 
		int bulletBurstAmount, 
		float bulletAliveTime) 
	{
		this.bulletScene = (PackedScene)GD.Load("res://scenes/Bullet.tscn");
		this.ownerNode = ownerNode;
		this.bulletOwner =  bulletOwner;
		this.noOfBullets = noOfBullets;
		this.bulletStrength = bulletStrength;
		this.bulletForce = bulletForce;
		this.bulletAccuracy = bulletAccuracy;
		this.bulletBurstAmount = bulletBurstAmount;
		this.bulletAliveTime = bulletAliveTime;
		this.shotDelay = shotDelay;
	}

	public void Shoot(bool isBursting = false)
	{
		if(!CanShoot(isBursting)) return;
		
		Godot.Sprite playerSprite = ownerNode.GetNode<Godot.Sprite>("Sprite");
		Godot.Node2D gameController = ownerNode.GetParent<Godot.Node2D>();

		//Loop for shotgun effect
		for (int i = 0; i < noOfBullets; i++)
		{
			SpawnBullet(playerSprite, gameController);
		}

		currentBulletInBurst++;
		if(currentBulletInBurst > bulletBurstAmount) currentBulletInBurst = 0;
	}

	//Checks if the threshold amount of time has passed since the last shot
	//isBursting = true lowers the nextShotThreshold
	private bool CanShoot(bool isBursting = false) {
		int shotDelayMs = (int)( (!isBursting ? shotDelay : betweenBurstDelay) * 1000 );
		int nextShotThreshold = timeLastShot + shotDelayMs;
		
		int currentTime = (int)OS.GetTicksMsec();
		bool canShoot = (currentTime >= nextShotThreshold);

		if(canShoot) timeLastShot = currentTime;

    	return canShoot;
	}

	private void SpawnBullet(Godot.Sprite playerSprite, Godot.Node2D gameController) {
		Area2D bullet = (Area2D)bulletScene.Instance();
		BulletController bulletCon = (BulletController)bullet;
		float randomAccuracyDeviation = (float)((rnd.NextDouble() * bulletAccuracy) - (rnd.NextDouble() * bulletAccuracy));

		// Access bullet properties
		bullet.Position = ownerNode.Position;
		bullet.Rotation = playerSprite.Rotation + randomAccuracyDeviation;

		// Access bullet script 
		bulletCon.bOwner = bulletOwner;
		bulletCon.openMotion = ownerNode.LinearVelocity/2f;
		bulletCon.strength = bulletStrength;
		bulletCon.movementForce = bulletForce;
		bulletCon.timeAlive = bulletAliveTime;

		// Shoot bullet + start cooldown 
		gameController.AddChild(bullet);
	}

	//Call in _PhysicsProcess so that the gun can continue a burst fire without Shoot() being called
	public void UpdateBurst()
	{
		if(currentBulletInBurst != 0) {
			Shoot(true);
		}
	}
}
