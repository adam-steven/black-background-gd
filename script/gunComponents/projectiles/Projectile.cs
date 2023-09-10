using System;
using System.Collections.Generic;
using Godot;
using static Enums;

namespace Godot
{
    public class Projectile : Area2D 
    {
        public BulletOwner bOwner;
        public int strength = 5;
        public Color colour = Color.Color8(251, 255, 255); 

        public float movementForce = 3000;
        internal Vector2 closedMotion; //The movement that the bullet has in a closed loop
        public Vector2 openMotion = Vector2.Zero; //The movement that the bullet gets from the players actions

        public float timeAlive = 0; //The range the bullet can go before destroying itself
	    public BulletVariations type = BulletVariations.Normal;

        internal Line2D trail;

        public List<string> onDestroyScenes = new Scenes();

        public override void _Ready()
        {
            this.Connect("body_entered", this, "_BodyEntered");

            float angle = this.Rotation;
		    closedMotion = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * movementForce;

            trail = this.GetNodeOrNull<Line2D>("Line2D");

            InitDeathTimer();
            _RenderColour();
            _ProjectileReady();
        }

        internal virtual void _ProjectileReady() {}
        internal virtual void _RenderColour() {}
        internal virtual void _BodyEntered(object body) {}

        internal virtual void _DestroySelf() 
        {
            this.CallDeferred("_SpawnOnDestroy");
            this.QueueFree(); 
        }

        internal virtual void _SpawnOnDestroy() 
        {
            if(onDestroyScenes is null || onDestroyScenes.Count <= 0) { return; }
            Random rnd = new Random();
            Godot.Node2D gameController = this.GetParent<Godot.Node2D>();

            for (int i = 0; i < onDestroyScenes.Count; i++)
            {
                PackedScene bulletScene = (PackedScene)GD.Load(onDestroyScenes[i]);
                Projectile projectile = (Projectile)bulletScene.Instance();
                float randomAccuracyDeviation = (float)((rnd.NextDouble() * 360) - (rnd.NextDouble() * 360));

                // Access bullet properties
                projectile.Position = this.Position;
                projectile.Rotation = this.GlobalRotation + randomAccuracyDeviation;
                projectile.Scale = this.Scale;
                projectile.colour = this.colour;

                // Access bullet script 
                projectile.bOwner = this.bOwner;
                // bulletCon.openMotion = ownerNode.LinearVelocity/2f;
                projectile.strength = this.strength;
                projectile.movementForce = this.movementForce;
                projectile.timeAlive = this.timeAlive;
                projectile.type = this.type;

                // Shoot bullet + start cooldown 
                gameController.AddChild(projectile);
            }
        }

        private void InitDeathTimer() 
        {
            Timer timer = this.GetNodeOrNull<Timer>("Timer");
            if(timeAlive <= 0 || !IsInstanceValid(timer)) { return; }

		    timer.WaitTime = timeAlive;
		    timer.Connect("timeout", this, "_DestroySelf");
		    timer.Start();
        }

        internal void ProcessTrail()
        {
            if (!IsInstanceValid(trail)) { return; }
            trail.AddPoint(this.GlobalPosition);
        }

        //Catch anim end _DestroySelf calls
        internal void _DestroySelf(string animName = "") 
        { 
            _DestroySelf(); 
        }
    }
}