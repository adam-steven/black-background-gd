using Godot;
using System;
using System.Reflection;
using static Enums;

public class BulletController : Area2D
{
	public float movementForce = 3000;
	public BulletOwner bOwner;
	private Vector2 closedMotion; //The movement that the bullet has in a closed loop
	public Vector2 openMotion = Vector2.Zero; //The movement that the bullet gets from the players actions
	public int strength = 5;
	public float timeAlive = 0; //The range the bullet can go before destroying itself

	public bool special = false;
	[Export] Godot.Color specialColor;

	public override void _Ready() {
		float angle = this.Rotation;
		closedMotion = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * movementForce;

		if(timeAlive <= 0) { timeAlive = 0.05f; }

		if(special) { Glow(); }
		
		Timer deathTimer = this.GetNode<Timer>("Timer");
		deathTimer.WaitTime = timeAlive;
		deathTimer.Connect("timeout", this, "DestroyBullet");
		deathTimer.Start();
	}

	public override void _Process(float delta) {
  		this.Position += (closedMotion + openMotion) * delta;
	}
	
	private void _On_Bullet_Body_Entered(object body) {
		//Make sure i hit an entity
		Type bodyType = body.GetType();
		if(bodyType.IsSubclassOf(typeof(Godot.Entities))) {
			Entities hitEntity = (Entities)body;

			//If collision is made by owner return
			if(hitEntity.entityType == bOwner) return;

			hitEntity.TakeDamage(this);
		} 

		DestroyBullet();
	}

	//Delete self
	private void DestroyBullet() {
		this.QueueFree(); 
	}

	private void Glow() {
		this.Modulate = specialColor;
		strength = strength * 2;
	}
}




