using System.Collections.Generic;
using static Enums;

namespace Godot
{
    public class Entity : RigidBody2D, IStats
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
        [Export] public List<string> OnBulletDestroyScenes { get; set; }
        
        #region Signals

        [Signal] internal delegate void _destroy_all_bullets();
        [Signal] internal delegate void _section_text(string text, bool inverted);
        [Signal] internal delegate void _shake_screen(int shakeForce, float shakeDuration);

        [Signal] internal delegate void _update_score(int points);
        [Signal] internal delegate void _update_player_heath(int health);
        [Signal] internal delegate void _break_score_update();
        [Signal] internal delegate void _on_death();

        #endregion

        #region Public Methods 

        public virtual void _TakeDamage(Projectile strikingBullet) { }

        public virtual void _UpdateHealth(int addend) { }

        internal virtual bool _IsActive() { return true; }

        #endregion
    }
}