using Godot;
using System;
using static Enums;

public class Chaser : Enemies
{
	public override void _Ready() 
    {
        InitDelayedStart();

		Godot.Sprite thisSprite = this.GetNode<Godot.Sprite>("Sprite");
		thisSprite.SelfModulate = colour;

		gun = new GunController(this); 
	}

	public override void _PhysicsProcess(float delta) 
    {
        if(!start) return;

		FacePlayer();
        MoveInDirection(Vector2.Right);

        gun.Shoot();
		gun.UpdateBurst();
	}
}
