using Godot;
using System;

public partial class BulletController : Area2D
{
	[Export] private float  movementForce = 3000;
	public BulletOwnerList bOwner;
	private Vector2 closedMotion; //The movement that the bullet has in a closed loop
	public Vector2 openMotion; //The movement that the bullet gets from the players actions

	public override void _Ready()
	{
		float angle = this.Rotation;
		closedMotion = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * movementForce;
	}

	public override void _Process(float delta)
	{
  		this.Position += (closedMotion + openMotion) * delta;
	}
	
	private void _On_Bullet_Body_Entered(object body)
	{
		//if collision is made by owner return
		if(body.GetType().Name == bOwner.ToString()) return;
		//Delete self
		this.QueueFree(); 
	}
}




