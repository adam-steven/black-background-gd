using Godot;
using System;
using static Enums;

public partial class Bullet : Projectile
{
	//internal override void _ProjectileReady() {}

	public override void _Process(double delta)
	{
		this.Position += (closedMotion + openMotion) * (float)delta;
		ProcessTrail();
	}

	internal override void _RenderColour() 
	{
		//Damage
		switch (type)
		{
			case BulletVariations.NormalStrong:
				this.Modulate = colour + new Godot.Color(0f, 0f, 0f, 0.25f);
				strength *= 2;
				break;
			case BulletVariations.Spectral:
				this.Modulate = colour.Lerp(new Color(Colors.White), 0.65f) + new Godot.Color(0f, 0f, 0f, 0.15f);
				strength *= 2;
				break;
			default:
				this.Modulate = colour;
				break;
		}
	  
	  	if(IsInstanceValid(trail))
		{
			trail.TopLevel = true;
			trail.DefaultColor = this.Modulate;
			trail.Width *= this.Scale.X;
		}
	}

	internal override void _BodyEntered(Node2D body)
	{
		//Make sure i hit an entity
		Type bodyType = body.GetType();
		if (bodyType.IsSubclassOf(typeof(Godot.Entity)))
		{
			Entity hitEntity = (Entity)body;

			//If collision is made by owner return
			if (hitEntity.entityType == bOwner) { return; }

			hitEntity._TakeDamage(this);
		}

		_DestroySelf();
	}
}