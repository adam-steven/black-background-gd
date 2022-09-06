using Godot;
using System;
using System.Collections.Generic;

public class UpgradeMenu : Control
{
	[Signal] public delegate void _decrease_multiplier(int reset);
	[Signal] public delegate void _upgrading_finished();
	[Signal] public delegate void _update_upgrade_ui(string value);

    private Random rnd = new Random();
	public Vector2 levelCenter;
	public PlayerController player;

    public override void _Ready()
    {
		//Connect exit listener
		Godot.Area2D exitBtn = this.GetNode<Godot.Area2D>("Exit");
		exitBtn.Connect("_on_pressed", this, "_OnButtonPress");
		exitBtn.Connect("_update_upgrade_ui", this, "_UpdateUpgradeDesc");

		//Spawn upgrades
		Vector2[] spawnPoints = {
			new Vector2(0,-Globals.levelSize.y/2),
			new Vector2(Globals.levelSize.x/2,Globals.levelSize.y/2),
			new Vector2(-Globals.levelSize.x/2,Globals.levelSize.y/2),
		};

		SpawnUpgrades(spawnPoints);
    }

	private void SpawnUpgrades(Vector2[] spawnPoints) {
        List<string> upgrades = FileManager.GetScenes(Globals.upgradesFolder);

		for (int i = 0; i < spawnPoints.Length; i++) {
			string randomUpgrade = upgrades[rnd.Next(upgrades.Count)];
			PackedScene upgradeOptionScene = (PackedScene)GD.Load(Globals.upgradesFolder + randomUpgrade);
			UpgradeButton upgradeOption = (UpgradeButton)upgradeOptionScene.Instance();

			Vector2 spawnPosition = spawnPoints[i] + levelCenter;
			upgradeOption.GlobalPosition = spawnPosition;

			UpgradeButton upgradeOptionScript = upgradeOption;
			upgradeOptionScript.player = player;

			this.AddChild(upgradeOption);

			upgradeOption.Connect("on_pressed", this, "_OnButtonPress");
		}
	}

	private void _OnButtonPress(UpgradeButton button) {
		if(!button.endUpgrading) { DecreaseMultiplier(); }

		//If the button has endUpgrading set or only the exit button is left
		Godot.Collections.Array buttons = this.GetChildren();
		if(button.endUpgrading || (buttons.Count - 1) <= 1) {
			FinishedUpgrading();
		}
	}

	private void _UpdateUpgradeDesc(string value) {
		this.EmitSignal("_update_upgrade_ui", value);
	}

	//Emit signal to decrement score multiplier
	public void DecreaseMultiplier() {
		this.EmitSignal("_decrease_multiplier", false);
	}

	//Emit signal to delete any existing upgrades
	public void FinishedUpgrading() {
		this.EmitSignal("_upgrading_finished");
        this.QueueFree();
	}
}
