using static Enums;

namespace Godot
{
    public class Entities : RigidBody2D
    {
        public BulletOwner entityType;
        
        [Export] public int health = 1000;
        [Export] public float movementForce = 1000f;

        public RigidBody2D player;
        public Color colour = Color.Color8(251, 255, 255);

        #region Gun

        public GunController gun;
        public Color bulletColour = Color.Color8(251, 255, 255);

        [Export] public float shotDelay = 1;
        [Export] public int noOfBullets = 1; //Number of bullets fired at once (Shotgun effect)
        [Export] public float bulletForce = 3000; //Bullet's speed
        [Export] public int bulletStrength = 100; //Amount of damage the bullet does
        [Export] public float bulletAccuracy = 0.2f; //Bullet's accuracy (0 is perfect accuracy)
        [Export] public int bulletBurstAmount = 1; //Number of bullets fired in quick succession (fixed delay interval)
        [Export] public float bulletTimeAlive = 0.25f; //Bullet Range (>0 = 0.05f)
        [Export] public float bulletSize = 1.5f; //Modifies the size of the bullet sprite

        #endregion

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

        public virtual void _TakeDamage(BulletController strikingBullet) { }

        public virtual void _UpdateHealth(int addend) { }

        internal virtual bool _IsActive() { return true; }

        #endregion
    }
}