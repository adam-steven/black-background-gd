//Base Enemy Controller with variants being extentions 

using Godot;
using System;
using System.Reflection;

public partial class EnemyController : RigidBody2D
{
	private RigidBody2D player;

	public override void _Ready()
	{
	  Godot.Node2D gameController = this.GetParent<Godot.Node2D>();
	  player = gameController.GetNode<RigidBody2D>("Player");
	}

	public override void _PhysicsProcess(float delta)
	{
		if(player == null) return;

		// Calls the needed variant function based on the enemies name
		// *Variant function must equal variant name
		// *Variant function must be public
		string variantFuncName = this.Name; 
		Type thisType = this.GetType();
		MethodInfo variantMethod = thisType.GetMethod(variantFuncName);
		
		if(variantMethod != null)
			variantMethod.Invoke(this, null);
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
