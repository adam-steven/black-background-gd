using System;
using Godot;
using static Enums;
using System.Threading.Tasks;

public class GunController
{
    Random rnd = new Random();

    private PackedScene defaultBulletScene;
    private Entity ownerNode;
    private Node2D spawnPoint;

    private SceneTree tree;

    private int timeLastShot = 0;

    ///<summary> 
    ///		Allows nodes to shoot bullets form there body.
    ///</summary>
    public GunController(Entity ownerNode, Node2D spawnPoint, SceneTree tree)
    {
        this.defaultBulletScene = (PackedScene)GD.Load(ownerNode.defaultBulletPath);
        this.ownerNode = ownerNode;
        this.spawnPoint = spawnPoint;
        this.tree = tree;
    }

    public void Shoot()
    {
        Shoot(this.defaultBulletScene, BulletVariations.Normal);
    }

    public void Shoot(string bulletPath)
    {
        PackedScene bulletScene = (PackedScene)GD.Load(bulletPath);
        Shoot(bulletScene, BulletVariations.Normal);
    }

    public void Shoot(BulletVariations bulletType)
    {
        Shoot(this.defaultBulletScene, bulletType);
    }

    public void Shoot(PackedScene bulletScene, BulletVariations bulletType)
    {
        if (CanShoot())
        {
            Godot.Node2D gameController = ownerNode.GetParent<Godot.Node2D>();
            BurstBulletsAsync(gameController, bulletScene, bulletType);
        }
    }

    //Checks if the threshold amount of time has passed since the last shot
    //isBursting = true lowers the nextShotThreshold
    private bool CanShoot()
    {
        int shotDelayMs = (int)(ownerNode.ShotDelay * 1000);
        int nextShotThreshold = timeLastShot + shotDelayMs;

        int currentTime = (int)OS.GetTicksMsec();
        bool canShoot = (currentTime >= nextShotThreshold);
        if (canShoot) timeLastShot = currentTime;

        return canShoot;
    }

    //Loop spawn bullets with delay for burst effect
    private async void BurstBulletsAsync(Godot.Node2D gameController, PackedScene bulletScene, BulletVariations bulletType)
    {
        //Make sure the burst is always faster than normal shots
        float betweenBurstDelay = (ownerNode.ShotDelay < 0.2f) ? ownerNode.ShotDelay / 2f : 0.1f;
        betweenBurstDelay *= 1000; //convert to ms

        for (int i = 0; i < ownerNode.BulletBurstAmount; i++)
        {
            if(!Godot.Object.IsInstanceValid(ownerNode) || tree.Paused) { break; }
            SpawnBullets(gameController, bulletScene, bulletType);
            await Task.Delay((int)betweenBurstDelay);
        }
    }

    //Loop spawn bullets for shotgun effect
    private void SpawnBullets(Godot.Node2D gameController, PackedScene bulletScene, BulletVariations bulletType)
    {
        

        //Set can shoot timer here so thats the burst will never be slower than shoot rate
        timeLastShot = (int)OS.GetTicksMsec();

        //Loop for shotgun effect
        for (int i = 0; i < ownerNode.NoOfBullets; i++)
        {
            SpawnBullet(gameController, bulletScene, bulletType);
        }
    }

    //Spawn 1 bullet
    private void SpawnBullet(Godot.Node2D gameController, PackedScene bulletScene, BulletVariations bulletType)
    {
        Projectile projectile = (Projectile)bulletScene.Instance();
        float randomAccuracyDeviation = (float)((rnd.NextDouble() * ownerNode.BulletAccuracy) - (rnd.NextDouble() * ownerNode.BulletAccuracy));

        // Access bullet properties
        projectile.Position = ownerNode.Position;
        projectile.Rotation = spawnPoint.GlobalRotation + randomAccuracyDeviation;
        projectile.Scale = new Vector2(ownerNode.BulletSize, ownerNode.BulletSize);
        projectile.colour = ownerNode.bulletColour;

        // Access bullet script 
        projectile.bOwner = ownerNode.entityType;
        // bulletCon.openMotion = ownerNode.LinearVelocity/2f;
        projectile.strength = ownerNode.BulletStrength;
        projectile.movementForce = ownerNode.BulletForce;
        projectile.timeAlive = ownerNode.BulletTimeAlive;
        projectile.type = bulletType;
        projectile.onDestroyScenes = ownerNode.OnBulletDestroyScenes;

        // Shoot bullet + start cooldown 
        gameController.AddChild(projectile);
    }
}
