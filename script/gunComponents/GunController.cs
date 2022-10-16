using System;
using Godot;
using static Enums;
using System.Threading.Tasks;

public class GunController
{
    Random rnd = new Random();

    private PackedScene bulletScene;
    private Entity ownerNode;
    private Node2D spawnPoint;

    private SceneTree tree;

    private int timeLastShot = 0;

    ///<summary> 
    ///		Allows nodes to shoot bullets form there body.
    ///</summary>
    public GunController(Entity ownerNode, Node2D spawnPoint, SceneTree tree)
    {
        this.bulletScene = (PackedScene)GD.Load(ownerNode.bulletPath);
        this.ownerNode = ownerNode;
        this.spawnPoint = spawnPoint;
        this.tree = tree;
    }

    public void UpdateBullet(string bulletPath) 
    {
        this.bulletScene = (PackedScene)GD.Load(bulletPath);
    }

    public void Shoot(BulletVariations bulletType)
    {
        if (CanShoot())
        {
            BurstBulletsAsync(bulletType);
        }
    }

    //Checks if the threshold amount of time has passed since the last shot
    //isBursting = true lowers the nextShotThreshold
    private bool CanShoot()
    {
        int shotDelayMs = (int)(ownerNode.shotDelay * 1000);
        int nextShotThreshold = timeLastShot + shotDelayMs;

        int currentTime = (int)OS.GetTicksMsec();
        bool canShoot = (currentTime >= nextShotThreshold);
        if (canShoot) timeLastShot = currentTime;

        return canShoot;
    }

    //Loop spawn bullets with delay for burst effect
    private async void BurstBulletsAsync(BulletVariations bulletType)
    {
        //Make sure the burst is always faster than normal shots
        float betweenBurstDelay = (ownerNode.shotDelay < 0.2f) ? ownerNode.shotDelay / 2f : 0.1f;
        betweenBurstDelay *= 1000; //convert to ms

        for (int i = 0; i < ownerNode.bulletBurstAmount; i++)
        {
            if(!Godot.Object.IsInstanceValid(ownerNode) || tree.Paused) { break; }
            SpawnBullets(bulletType);
            await Task.Delay((int)betweenBurstDelay);
        }
    }

    //Loop spawn bullets for shotgun effect
    private void SpawnBullets(BulletVariations bulletType)
    {
        Godot.Node2D gameController = ownerNode.GetParent<Godot.Node2D>();

        //Set can shoot timer here so thats the burst will never be slower than shoot rate
        timeLastShot = (int)OS.GetTicksMsec();

        //Loop for shotgun effect
        for (int i = 0; i < ownerNode.noOfBullets; i++)
        {
            SpawnBullet(gameController, bulletType);
        }
    }

    //Spawn 1 bullet
    private void SpawnBullet(Godot.Node2D gameController, BulletVariations bulletType)
    {
        Projectile projectile = (Projectile)bulletScene.Instance();
        float randomAccuracyDeviation = (float)((rnd.NextDouble() * ownerNode.bulletAccuracy) - (rnd.NextDouble() * ownerNode.bulletAccuracy));

        // Access bullet properties
        projectile.Position = ownerNode.Position;
        projectile.Rotation = spawnPoint.GlobalRotation + randomAccuracyDeviation;
        projectile.Scale = new Vector2(ownerNode.bulletSize, ownerNode.bulletSize);
        projectile.colour = ownerNode.bulletColour;

        // Access bullet script 
        projectile.bOwner = ownerNode.entityType;
        // bulletCon.openMotion = ownerNode.LinearVelocity/2f;
        projectile.strength = ownerNode.bulletStrength;
        projectile.movementForce = ownerNode.bulletForce;
        projectile.timeAlive = ownerNode.bulletTimeAlive;
        projectile.type = bulletType;

        // Shoot bullet + start cooldown 
        gameController.AddChild(projectile);
    }
}
