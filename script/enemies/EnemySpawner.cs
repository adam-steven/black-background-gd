using Godot;
using System;
using System.Collections.Generic;

public class EnemySpawner : Position2D
{
	Random rnd = new Random();
	//private String[] enemySceneLocations = {};

	string enemyFolder = "res://scenes/enemies/";
	List<string> enemies; //paths to enemy scenes

	public override void _Ready() {
		enemies = FileManager.GetScenes(enemyFolder);
		AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Connect("animation_finished", this, "SpawnEnemy");
	}

	private void SpawnEnemy(string animName) { 
		if(enemies.Count > 0) {
			string chosenEnemyScene = enemies[rnd.Next(enemies.Count)];
			Godot.Node2D gameController = GetNode<SceneController>(Globals.scenePath).GetCurrentScene();

			PackedScene enemyScene = (PackedScene)GD.Load(enemyFolder + chosenEnemyScene);
			RigidBody2D enemy = (RigidBody2D)enemyScene.Instance();
			enemy.Position = this.Position;

			gameController.AddChild(enemy);
		}
		
		this.QueueFree(); 
	}
}
