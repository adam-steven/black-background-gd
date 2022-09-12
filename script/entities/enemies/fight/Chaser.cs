using Godot;
using System;
using static Enums;

public class Chaser : Enemies
{
	public override void _Ready() 
	{
		FacePlayer();
		InitDelayedStart("LoadingSpinner");

		Godot.Sprite thisSprite = this.GetNode<Godot.Sprite>("Sprite");
		thisSprite.SelfModulate = colour;

		gun = new GunController(this); 
	}

	public override void _PhysicsProcess(float delta) 
	{
		TurnToPlayer(delta);
		MoveInDirection(Vector2.Right);

		gun.Shoot(BulletVariations.Normal);
	}
}
