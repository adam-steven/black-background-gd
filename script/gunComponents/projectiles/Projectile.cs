using System;
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

        private void InitDeathTimer() 
        {
            Timer timer = this.GetNodeOrNull<Timer>("Timer");
            if(timeAlive <= 0 || !IsInstanceValid(timer)) { return; }

		    timer.WaitTime = timeAlive;
		    timer.Connect("timeout", this, "DestroySelf");
		    timer.Start();
        }

        internal void ProcessTrail()
        {
            if (!IsInstanceValid(trail)) { return; }
            trail.AddPoint(this.GlobalPosition);
        }

        internal void DestroySelf(string animName = "") 
        { 
            this.QueueFree(); 
        }
    }
}