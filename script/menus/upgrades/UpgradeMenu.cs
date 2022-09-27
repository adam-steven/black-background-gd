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
    public int numOfItems = 7;

    public override void _Ready()
    {
        SettingsController settings = new SettingsController();
        bool showNames = (bool)settings.GetValue(MenuButtonActions.UpgradeName.ToString(), false);
        bool showDesc = (bool)settings.GetValue(MenuButtonActions.UpgradeDesc.ToString(), false);

        //Connect exit listener
        UpgradeButton exitBtn = this.GetNode<UpgradeButton>("Exit");
        exitBtn.Connect("_on_pressed", this, "_OnButtonPress");
        exitBtn.Connect("_update_upgrade_ui", this, "_UpdateUpgradeDesc");
        exitBtn.showNames = showNames;
        exitBtn.showDesc = showDesc;

        SpawnUpgrades(numOfItems, showNames, showDesc);
    }

    //Spawns multiple upgrades in a circle
    private void SpawnUpgrades(int numOfItems, bool showNames, bool showDesc)
    {
        int radius = 300;
        int yAxisOffSet = 20; //Push upgrades down to counter invisible names illusions

        for (int i = 0; i < numOfItems; i++)
        {
            double x = levelCenter.x + radius * Math.Cos((2 * Math.PI * i / numOfItems) - (Math.PI/2)); 
            double y = levelCenter.y + radius * Math.Sin((2 * Math.PI * i / numOfItems) - (Math.PI/2));
            Vector2 spawnPosition = new Vector2((float)x, (float)y + yAxisOffSet);

            SpawnUpgrade(spawnPosition, showNames, showDesc);
        }
    }

    //spawn 1 upgrade at a given location
    private void SpawnUpgrade(Vector2 spawnPosition, bool showNames, bool showDesc) {
        string randomUpgrade = upgrades[rnd.Next(upgrades.Count)];
        PackedScene upgradeOptionScene = (PackedScene)GD.Load(randomUpgrade);
        UpgradeButton upgradeOption = (UpgradeButton)upgradeOptionScene.Instance();

        upgradeOption.GlobalPosition = spawnPosition;

        UpgradeButton upgradeOptionScript = upgradeOption;
        upgradeOptionScript.player = player;
        upgradeOptionScript.showNames = showNames;
        upgradeOptionScript.showDesc = showDesc;

        upgradeOption.Connect("_on_pressed", this, "_OnButtonPress");
        upgradeOption.Connect("_update_upgrade_ui", this, "_UpdateUpgradeDesc");

        this.AddChild(upgradeOption);
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


	#region Testing Functions

    // private void Test() {
    //     var items = 100;
    //     var x0 = 960;
    //     var y0 = 540;
    //     var r = 250;

    //     PlaceTestingDot(new Vector2(x0, y0));

    //     for(var i = 0; i < items; i++) {
    //         var x = x0 + r * Math.Cos((2 * Math.PI * i / items) - (Math.PI/2)); 
    //         var y = y0 + r * Math.Sin((2 * Math.PI * i / items) - (Math.PI/2));
    //         var point = new Vector2((float)x, (float)y);
            
    //         GD.Print(point);
    //         PlaceTestingDot(point);
    //     }
    // }

    // private void PlaceTestingDot(Vector2 tDotPos) {
	// 	PackedScene testingDot = (PackedScene)GD.Load("res://scenes/testing/TestingDot.tscn");
	// 	Godot.Sprite tDot = (Godot.Sprite)testingDot.Instance();
	// 	tDot.GlobalPosition = tDotPos;
	// 	this.AddChild(tDot);
	// }

    #endregion
}
