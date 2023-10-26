using System.Collections.Generic;

public interface IStats
{
    /// <summary>
    /// Entity health, destroy self when x <= 0
    /// </summary>
    int Health { get; set; }

    /// <summary>
    /// How fast the entity moves
    /// Used for both walking and pushing
    /// </summary>
    /// <value></value>
    float MovementForce { get; set; }
    
    /// <summary>
    /// Time between gun fires
    /// </summary>
    /// <value></value>
    float ShotDelay { get; set; }

    /// <summary>
    /// Number of bullets fired at once (Shotgun effect)
    /// </summary>
    int NoOfBullets { get; set; }

    /// <summary>
    /// Bullet's speed
    /// </summary>
    float BulletForce { get; set; }

    /// <summary>
    /// Amount of damage the bullet does
    /// </summary>
    int BulletStrength { get; set; }

    /// <summary>
    /// Bullet's accuracy (0 is perfect accuracy)
    /// </summary>
    float BulletAccuracy { get; set; }

    /// <summary>
    /// Number of bullets fired in quick succession (fixed delay interval)
    /// </summary>
    int BulletBurstAmount { get; set; }

    /// <summary>
    /// Bullet Range (>0 = 0.05f)
    /// </summary>
    float BulletTimeAlive { get; set; } 

    /// <summary>
    /// Modifies the size of the bullet sprite
    /// </summary>
    float BulletSize { get; set; }

    /// <summary>
    /// Scenes to spawn in a random direction after the bullet is destroyed 
    /// </summary>
    Godot.Collections.Array<string> OnBulletDestroyScenes { get; set; }
}
