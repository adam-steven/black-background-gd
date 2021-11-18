using Godot;
using static Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

public class GunController : RigidBody2D
{
	private PackedScene bulletScene;
	private bool canShoot = true;
	private RigidBody2D ownerNode;
	private BulletOwner bulletOwner;
	private int bulletStrength;
	private float bulletForce;
	private float bulletAliveTime;
	private float shotDelay;

	///<summary> 
	///		Allows nodes to shoot bullets form there body.
	///		Requires: 
	///			Child Sprite Node,
	///			Child Timer Node,
	///			"ShootCooledDown()" to receive Timer call which should call "GunController.ShootCooledDown()" 
	///</summary>
	public GunController(float shotDelay, RigidBody2D ownerNode, BulletOwner bulletOwner, int bulletStrength, float bulletForce, float bulletAliveTime) {
		this.bulletScene = (PackedScene)GD.Load("res://scenes/Bullet.tscn");
		this.ownerNode = ownerNode;
		this.bulletOwner =  bulletOwner;
		this.bulletStrength = bulletStrength;
		this.bulletForce = bulletForce;
		this.bulletAliveTime = bulletAliveTime;
		this.shotDelay = shotDelay;
	}

	public void Shoot()
	{
		if(!canShoot) return;
		
		Godot.Sprite playerSprite = ownerNode.GetNode<Godot.Sprite>("Sprite");
		Godot.Node2D gameController = ownerNode.GetParent<Godot.Node2D>();
		Area2D bullet = (Area2D)bulletScene.Instance();
		BulletController bulletCon = (BulletController)bullet;

		// Access bullet properties
		bullet.Position = ownerNode.Position;
		bullet.Rotation = playerSprite.Rotation;

		// Access bullet script 
		bulletCon.bOwner = bulletOwner;
		bulletCon.openMotion = ownerNode.LinearVelocity/2f;
		bulletCon.strength = bulletStrength;
		bulletCon.movementForce = bulletForce;
		bulletCon.timeAlive = bulletAliveTime;

		// Shoot bullet + start cooldown 
		gameController.AddChild(bullet);
		ShootCooledDown();
	}

	private async void ShootCooledDown() {
		TimeSpan span = TimeSpan.FromSeconds((double)(new decimal(shotDelay)));
		canShoot = false;
		await Task.Delay(span);
        canShoot = true;
	}
}
