using Godot;
using System;
using System.Reflection;
using static Enums;

public class BulletController : Area2D
{
	public float movementForce = 3000;
	public BulletOwner bOwner;
	private Vector2 closedMotion; //The movement that the bullet has in a closed loop
	public Vector2 openMotion; //The movement that the bullet gets from the players actions
	public int strength = 5;
	public float timeAlive = 0; //The range the bullet can go before destroying itself

	public override void _Ready()
	{
		float angle = this.Rotation;
		closedMotion = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * movementForce;

		if(timeAlive > 0) 
		{
			Timer deathTimer = this.GetNode<Timer>("Timer");
			deathTimer.WaitTime = timeAlive;
			deathTimer.Connect("timeout", this, "DestroyBullet");
			deathTimer.Start();
		}
	}

	public override void _Process(float delta)
	{
  		this.Position += (closedMotion + openMotion) * delta;
	}
	
	private void _On_Bullet_Body_Entered(object body)
	{
		Type bodyType = body.GetType();

		//if collision is made by owner return
		if(bodyType.Name == bOwner.ToString()) return;

	 	MethodInfo damageMethod = bodyType.GetMethod("TakeDamage");
		if(damageMethod != null)
			damageMethod.Invoke(body, new object[]{strength});

		DestroyBullet();
	}

	//Delete self
	private void DestroyBullet() 
	{
		this.QueueFree(); 
	}
}



