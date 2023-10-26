using System.Collections.Generic;
using static Enums;

namespace Godot
{
    public partial class Entity : RigidBody2D, IStats
    {
        public BulletOwner entityType;

        public RigidBody2D player;
        public Color colour = Color.Color8(251, 255, 255);

        public GunController gun;
        public Color bulletColour = Color.Color8(251, 255, 255);

        [Export] public string defaultBulletPath = "res://scenes/misc/Bullet.tscn";

        [Export] public int Health { get; set; }
        [Export] public float MovementForce { get; set; }
        [Export] public float ShotDelay { get; set; }
        [Export] public int NoOfBullets { get; set; }
        [Export] public float BulletForce { get; set; }
        [Export] public int BulletStrength { get; set; }
        [Export] public float BulletAccuracy { get; set; }
        [Export] public int BulletBurstAmount { get; set; }
        [Export] public float BulletTimeAlive { get; set; }
        [Export] public float BulletSize { get; set; }
        [Export] public Godot.Collections.Array<string> OnBulletDestroyScenes { get; set; }
        
        #region Signals

        [Signal] public delegate void DestroyAllBulletsEventHandler(); //Event: destroy all instances of bullets on screen
        [Signal] public delegate void SectionTextEventHandler(string text, bool inverted); //Event: request text to flash in the background
        [Signal] public delegate void ShakeScreenEventHandler(int shakeForce, float shakeDuration); //Event: request the camera to shake

        [Signal] public delegate void UpdateScoreEventHandler(int points); //Event: increase/decrease the players score
        [Signal] public delegate void UpdatePlayerHeathEventHandler(int health); //Event: the player has gained/lost health
        [Signal] public delegate void BreakScoreUpdateEventHandler(); //Event: stop the update ticker add the currently displayed value
        [Signal] public delegate void OnDeathEventHandler(); //Event: this entity has died

        #endregion

        #region Public Methods 

        public virtual void _TakeDamage(Projectile strikingBullet) { }

        public virtual void _UpdateHealth(int addend) { }

        internal virtual bool _IsActive() { return true; }

        #endregion
    }
}