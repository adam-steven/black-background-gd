using System;
using Newtonsoft.Json; 

namespace Godot
{
    public class Enemies : Entities
    {
        public GunController gun; 
	    public RigidBody2D player;

        public Enemies() {}

        public void FacePlayer() {
            this.LookAt(player.GlobalPosition); 
        }

        public void MoveInDirection(Vector2 _thrustDirection) {
            Vector2 _thrust = _thrustDirection * movementForce;
            SetAxisVelocity(_thrust.Rotated(Rotation));
        }

        public void PushInDirection(Vector2 _thrustDirection) {
            Vector2 _thrust = _thrustDirection * movementForce;
            ApplyCentralImpulse(_thrust);
        }

        //Emit signal on death
        [Signal] public delegate void _on_death();
        public void EmitDeathSignal() {
            this.EmitSignal("_on_death");
        }
    }
}