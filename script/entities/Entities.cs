using System;
using static Enums;
using Newtonsoft.Json; 

namespace Godot
{
    public class Entities : RigidBody2D
    {
        [Export] public BulletOwner entityType;

        [Export] public int health = 100;
        [Export] public float movementForce = 1000f;

        public Color colour;
        
        #region Gun
                
            public GunController gun; 

            [Export] public float shotDelay = 1;
            [Export] public int noOfBullets = 1; //Number of bullets fired at once (Shotgun effect)
            [Export] public float bulletForce = 3000; //Bullet's speed
            [Export] public int bulletStrength = 10; //Amount of damage the bullet does
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
            [Signal] internal delegate void _break_score_update();

        #endregion       

        #region Public Methods 


            public virtual void TakeDamage(BulletController strikingBullet) {}

            public void UpdateStats() {

            }

            public void UpdateHealth(int addend) {
                health += addend;

                //Cap health
                health = Math.Min(health, 100);
            }

        #endregion
    }
}