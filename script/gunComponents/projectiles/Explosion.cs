using Godot;
using System;
using static Enums;

public class Explosion : Projectile
{
    internal override void _ProjectileReady() 
    {
        AnimationPlayer anim = this.GetNode<AnimationPlayer>("AnimationPlayer");
        anim.Connect("animation_finished", this, "DestroySelf");
    }

    internal override void _RenderColour() 
    {
        Particles2D particle = this.GetNode<Particles2D>("Particles2D");
        ParticlesMaterial particleMaterial = (ParticlesMaterial)particle.ProcessMaterial;
        particleMaterial.Color = colour;
    }

	internal override void _BodyEntered(object body)
	{
		//Make sure i hit an entity
		Type bodyType = body.GetType();
		if (bodyType.IsSubclassOf(typeof(Godot.Entity)))
		{
			Entity hitEntity = (Entity)body;

			//If collision is made by owner return
			if (hitEntity.entityType == bOwner) return;

			//hitEntity._TakeDamage(this);
		}
	}
}
