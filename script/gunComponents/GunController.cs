using Godot;
using static Enums;
public class GunController : RigidBody2D
{
	private PackedScene bulletScene;
	private Timer shotTimer;
	private bool canShoot = true;
	private RigidBody2D ownerNode;
	private BulletOwner bulletOwner;

	///<summary> 
	///		Allows nodes to shoot bullets form there body.
	///		Requires: 
	///			Child Sprite Node,
	///			Child Timer Node,
	///			"ShootCooledDown()" to receive Timer call which should call "GunController.ShootCooledDown()" 
	///</summary>
	public GunController(float shotDelay, RigidBody2D ownerNode, BulletOwner bulletOwner) {
		this.bulletScene = (PackedScene)GD.Load("res://scenes/Bullet.tscn");
		this.ownerNode = ownerNode;
		this.bulletOwner =  bulletOwner;

		this.shotTimer = ownerNode.GetNode<Timer>("Timer");
		shotTimer.WaitTime = shotDelay;
		shotTimer.Connect("timeout", ownerNode, "ShootCooledDown");
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

		// Shoot bullet + start cooldown 
		gameController.AddChild(bullet);
		canShoot = false;
		shotTimer.Start();
	}

	public void ShootCooledDown() { canShoot = true; }
}
