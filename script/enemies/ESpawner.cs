using Godot;
using System;

public class ESpawner : Position2D
{
    Random rnd = new Random();
    [Export] private String[] enemySceneLocations = {};

    public override void _Ready() {
        AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
        anim.Connect("animation_finished", this, "SpawnEnemy");
    }

    private void SpawnEnemy(string animName) { 
        if(enemySceneLocations.Length > 0) {
            string chosenEnemyScene = enemySceneLocations[rnd.Next(enemySceneLocations.Length)];
            Godot.Node2D gameController = this.GetParent<Godot.Node2D>();

            PackedScene enemyScene = (PackedScene)GD.Load("res://scenes/enemies/" + chosenEnemyScene);
            RigidBody2D enemy = (RigidBody2D)enemyScene.Instance();
            enemy.Position = this.Position;

            gameController.AddChild(enemy);
        }
        
        this.QueueFree(); 
    }
}
