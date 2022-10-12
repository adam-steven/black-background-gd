using Godot;
using System;
using static Enums;

public class Explosion : Area2D
{
    public BulletOwner bOwner;
    public int strength = 5;
    public Color explosionColour = Color.Color8(251, 255, 255); 

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Connect("body_entered", this, "BodyEntered");

        AnimationPlayer anim = this.GetNode<AnimationPlayer>("AnimationPlayer");
        anim.Connect("animation_finished", this, "DestroyExplosion");

        Particles2D particle = this.GetNode<Particles2D>("Particles2D");
        ParticlesMaterial particleMaterial = (ParticlesMaterial)particle.ProcessMaterial;
        particleMaterial.Color = explosionColour;
    }

	private void BodyEntered(object body)
	{
		//Make sure i hit an entity
		Type bodyType = body.GetType();
		if (bodyType.IsSubclassOf(typeof(Godot.Entities)))
		{
			Entities hitEntity = (Entities)body;

			//If collision is made by owner return
			if (hitEntity.entityType == bOwner) return;

			//hitEntity._TakeDamage(this);
		}
	}

    //Delete self
	private void DestroyExplosion(string animName = "")
	{
		this.QueueFree();
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
