using Godot;
using System;
using System.Collections.Generic;

public class UpgradeMenu : Control
{
    private Random rnd = new Random();

	public Vector2 levelCenter;
	public Entities player;

    public override void _Ready()
    {
        List<string> upgrades = FileManager.GetScenes(Globals.upgradesFolder);

        Vector2[] spawnPoints = {
			new Vector2(0,-Globals.levelSize.y/2),
			new Vector2(Globals.levelSize.x/2,Globals.levelSize.y/2),
			new Vector2(-Globals.levelSize.x/2,Globals.levelSize.y/2),
		};

		for (int i = 0; i < spawnPoints.Length; i++) {
			string randomUpgrade = upgrades[rnd.Next(upgrades.Count)];
			PackedScene upgradeOptionScene = (PackedScene)GD.Load(Globals.upgradesFolder + randomUpgrade);
			Godot.Node2D upgradeOption = (Godot.Node2D)upgradeOptionScene.Instance();

			Vector2 spawnPosition = spawnPoints[i] + levelCenter;
			upgradeOption.GlobalPosition = spawnPosition;

			UpgradeButton upgradeOptionScript = (UpgradeButton)upgradeOption;
			upgradeOptionScript.player = player;

			this.AddChild(upgradeOption);

			upgradeOption.Connect("on_pressed", this, "_OnButtonPress");
		}
    }

	private void _OnButtonPress(UpgradeButton button) {
		if(button.endUpgrading) {
			FinishedUpgrading();
		}
	}


    [Signal]
	public delegate void upgrading_finished();

	//Emit signal to delete any existing upgrades
	public void FinishedUpgrading() {
		this.EmitSignal("upgrading_finished");
        this.QueueFree();
	}
}
