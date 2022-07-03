using Godot;
using System;
using static Enums;

public class Chaser : Enemies
{
	private Random rnd = new Random();

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

		int shootSpecialChance = rnd.Next(20);
        gun.Shoot(false, (shootSpecialChance > 15));
		gun.UpdateBurst();
	}
}
