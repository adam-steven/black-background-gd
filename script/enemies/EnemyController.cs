//Base Enemy Controller with variants being extentions 

using Godot;
using System;
using System.Reflection;
using static Enums;

public partial class EnemyController : RigidBody2D
{
	private GunController gun; 
	private RigidBody2D player;

	//enemy specific function
	private MethodInfo variantMethod;

	public override void _Ready()
	{
		Godot.Node2D gameController = this.GetParent<Godot.Node2D>();
		player = gameController.GetNode<RigidBody2D>("Player");
		gun = new GunController(shotDelay, this, BulletOwner.EnemyController, noOfBullets, bulletStrength, bulletForce, bulletAccuracy, bulletBurstAmount, bulletTimeAlive); 

		// Calls the needed variant function based on the enemies name
		// *Variant function must equal variant name
		// *Variant function must be public
		string variantFuncName = this.Name; 
		Type thisType = this.GetType();
		variantMethod = thisType.GetMethod(variantFuncName);

		//if variant function not found delete bugged enemy 
		if(variantMethod == null) {
			GD.Print("no enemy script: " + variantFuncName);
			this.QueueFree();
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		if(!IsInstanceValid(player) || variantMethod == null || health <= 0) 
			return;

		variantMethod.Invoke(this, null);
		gun.UpdateBurst();
	}

	private void FacePlayer() 
	{
		this.LookAt(player.GlobalPosition); 
	}

	private void MoveInDirection(Vector2 _thrustDirection)
	{
		Vector2 _thrust = _thrustDirection * movementForce;
		SetAxisVelocity(_thrust.Rotated(Rotation));
	}

	private void PushInDirection(Vector2 _thrustDirection)
	{
		Vector2 _thrust = _thrustDirection * movementForce;
		ApplyCentralImpulse(_thrust);
	}
}
