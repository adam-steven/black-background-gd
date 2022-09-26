using Godot;
using System;
using System.Collections.Generic;
using static Enums;

public class UpgradeMenu : Control
{
    [Signal] public delegate void _decrease_multiplier(int reset);
    [Signal] public delegate void _upgrading_finished();
    [Signal] public delegate void _update_upgrade_ui(string value);

    private Random rnd = new Random();
    public Vector2 levelCenter;
    public PlayerController player;
    public Scenes upgrades;

    public override void _Ready()
    {
        // SettingsController settings = new SettingsController();
        // bool showNames = (bool)settings.GetValue(MenuButtonActions.UpgradeName.ToString(), false);
        // bool showDesc = (bool)settings.GetValue(MenuButtonActions.UpgradeDesc.ToString(), false);

        // //Connect exit listener
        // UpgradeButton exitBtn = this.GetNode<UpgradeButton>("Exit");
        // exitBtn.Connect("_on_pressed", this, "_OnButtonPress");
        // exitBtn.Connect("_update_upgrade_ui", this, "_UpdateUpgradeDesc");
        // exitBtn.showNames = showNames;
        // exitBtn.showDesc = showDesc;

        // //Spawn upgrades
        // Vector2[] spawnPoints = {
        //     new Vector2(0,-Globals.levelSize.y/2),
        //     new Vector2(Globals.levelSize.x/2,Globals.levelSize.y/2),
        //     new Vector2(-Globals.levelSize.x/2,Globals.levelSize.y/2),
        // };

        // SpawnUpgrades(spawnPoints, showNames, showDesc);

        Test();
    }

    private void SpawnUpgrades(Vector2[] spawnPoints, bool showNames, bool showDesc)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            string randomUpgrade = upgrades[rnd.Next(upgrades.Count)];
            PackedScene upgradeOptionScene = (PackedScene)GD.Load(randomUpgrade);
            UpgradeButton upgradeOption = (UpgradeButton)upgradeOptionScene.Instance();

            Vector2 spawnPosition = spawnPoints[i] + levelCenter;
            upgradeOption.GlobalPosition = spawnPosition;

            UpgradeButton upgradeOptionScript = upgradeOption;
            upgradeOptionScript.player = player;
            upgradeOptionScript.showNames = showNames;
            upgradeOptionScript.showDesc = showDesc;

            upgradeOption.Connect("_on_pressed", this, "_OnButtonPress");
            upgradeOption.Connect("_update_upgrade_ui", this, "_UpdateUpgradeDesc");

            this.AddChild(upgradeOption);
        }
    }

    private void _OnButtonPress(UpgradeButton button)
    {
        if (!button.endUpgrading) { DecreaseMultiplier(); }

        //If the button has endUpgrading set or only the exit button is left
        Godot.Collections.Array buttons = this.GetChildren();
        if (button.endUpgrading || (buttons.Count - 1) <= 1)
        {
            FinishedUpgrading();
        }
    }

    private void _UpdateUpgradeDesc(string value)
    {
        this.EmitSignal("_update_upgrade_ui", value);
    }

    //Emit signal to decrement score multiplier
    public void DecreaseMultiplier()
    {
        this.EmitSignal("_decrease_multiplier", false);
    }

    //Emit signal to delete any existing upgrades
    public void FinishedUpgrading()
    {
        this.EmitSignal("_upgrading_finished");
        this.QueueFree();
    }


    private void Test() {
        var items = 5;
        var x0 = 960;
        var y0 = 540;
        var r = 100;

        PlaceTestingDot(new Vector2(x0, y0));

        for(var i = 0; i < items; i++) {
            var x = x0 + r * Math.Cos((2 * Math.PI * i / items) - (Math.PI/2)); 
            var y = y0 + r * Math.Sin((2 * Math.PI * i / items) - (Math.PI/2));
            var point = new Vector2((float)x, (float)y);
            
            GD.Print(point);
            PlaceTestingDot(point);
        }
    }

    private void PlaceTestingDot(Vector2 tDotPos) {
		PackedScene testingDot = (PackedScene)GD.Load("res://scenes/testing/TestingDot.tscn");
		Godot.Sprite tDot = (Godot.Sprite)testingDot.Instance();
		tDot.GlobalPosition = tDotPos;
		this.AddChild(tDot);
	}
}
