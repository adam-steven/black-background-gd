using Godot;
using static BulletOwnerList;
public partial class PlayerController
{
    private PackedScene bulletScene;
	private bool canShoot = true;

    public override void _Process(float delta)
	{
		if (Input.IsActionPressed("ui_fire1"))
			Shoot();
	}
	
    private void ConfigureShootOps() {
	    bulletScene = (PackedScene)GD.Load("res://scenes/Bullet.tscn");

		Timer shotTimer = this.GetNode<Timer>("Timer");
		shotTimer.WaitTime = shotDelay;
		shotTimer.Connect("timeout", this, "ShootCoolDown");
    }

    private void Shoot()
	{
		if(!canShoot) return;
		
        Godot.Sprite playerSprite = this.GetNode<Godot.Sprite>("Sprite");
		Godot.Node2D gameController = this.GetParent<Godot.Node2D>();
		Area2D bullet = (Area2D)bulletScene.Instance();
		BulletController bulletCon = (BulletController)bullet;

		// Access bullet properties
		bullet.Position = this.Position;
		bullet.Rotation = playerSprite.Rotation;

		// Access bullet script 
		bulletCon.bOwner = BulletOwner.PlayerController;
		bulletCon.openMotion = this.LinearVelocity/2f;

		// Shoot bullet + start cooldown 
		gameController.AddChild(bullet);
		canShoot = false;
        Timer shotTimer = this.GetNode<Timer>("Timer");
		shotTimer.Start();
	}

	private void ShootCoolDown() { canShoot = true; }
}
