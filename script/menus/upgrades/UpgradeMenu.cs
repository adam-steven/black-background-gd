using Godot;
using Newtonsoft.Json;
using System;
using static Enums;

public partial class UpgradeMenu : Control
{
	[Signal] public delegate void SpawnedUpgradesEventHandler(string spawnedUpgrades); //Event: spawn upgrade to choose
	[Signal] public delegate void DecreaseMultiplierEventHandler(int reset); //Event: decrease the players round multiplier
	[Signal] public delegate void UpgradingFinishedEventHandler(); //Event: the upgrading stage has finished
	[Signal] public delegate void UpdateUpgradeUiEventHandler(string value); //Event: bubble up UpgradeBtn event

    private Random rnd = new Random();
	public Vector2 levelCenter;
	public Player player;

	public Scenes storedUpgrades = null;
	public Scenes upgrades;

	public int numOfItems = 3;

	public override void _Ready()
	{
		SettingsController settings = new SettingsController();
		bool showNames = (bool)settings.GetValue(MenuButtonActions.UpgradeName.ToString(), false);
		bool showDesc = (bool)settings.GetValue(MenuButtonActions.UpgradeDesc.ToString(), false);

		//Connect exit listener
		UpgradeBtn exitBtn = this.GetNode<UpgradeBtn>("Exit");
        exitBtn.Connect(UpgradeBtn.SignalName.OnPressed, new Callable(this, "_OnButtonPressed"));
		exitBtn.Connect(UpgradeBtn.SignalName.UpdateUpgradeUi, new Callable(this, "_UpdateUpgradeDesc"));
		exitBtn.showNames = showNames;
		exitBtn.showDesc = showDesc;

		numOfItems = (storedUpgrades is null) ? numOfItems : storedUpgrades.Count;
		SpawnUpgrades(numOfItems, showNames, showDesc); 
	}

	//Spawns multiple upgrades in a circle
	private void SpawnUpgrades(int numOfItems, bool showNames, bool showDesc)
	{
		Scenes spawnedUpgrades = new Scenes(); 
		int radius = 300;
		int yAxisOffSet = 20; //Push upgrades down to counter invisible names illusions

		for (int i = 0; i < numOfItems; i++)
		{
			double x = levelCenter.X + radius * Math.Cos((2 * Math.PI * i / numOfItems) - (Math.PI/2)); 
			double y = levelCenter.Y + radius * Math.Sin((2 * Math.PI * i / numOfItems) - (Math.PI/2));
			Vector2 spawnPosition = new Vector2((float)x, (float)y + yAxisOffSet);

			string upgradePath = SpawnUpgrade(i, spawnPosition, showNames, showDesc);
			spawnedUpgrades.Add(upgradePath);
		}

		SaveSpawnedUpgrades(spawnedUpgrades);
	}

	//spawn 1 upgrade at a given location
	private string SpawnUpgrade(int index, Vector2 spawnPosition, bool showNames, bool showDesc) {
		string upgradePath = (storedUpgrades is not null) ? storedUpgrades[index] : upgrades[rnd.Next(upgrades.Count)];
		PackedScene upgradeOptionScene = (PackedScene)GD.Load(upgradePath);
		UpgradeBtn upgradeOption = upgradeOptionScene.Instantiate<UpgradeBtn>();
		 
		upgradeOption.GlobalPosition = spawnPosition;

		UpgradeBtn upgradeOptionScript = upgradeOption;
		upgradeOptionScript.player = player;
		upgradeOptionScript.showNames = showNames;
		upgradeOptionScript.showDesc = showDesc;

		upgradeOption.Connect(UpgradeBtn.SignalName.OnPressed, new Callable(this, "_OnButtonPressed"));
		upgradeOption.Connect(UpgradeBtn.SignalName.UpdateUpgradeUi, new Callable(this, "EmitUpdateUpgradeDesc"));

		this.AddChild(upgradeOption);
		return upgradePath;
	}

	private void _OnButtonPressed(UpgradeBtn button)
	{
		if (!button.endUpgrading) { EmitDecreaseMultiplier(); }

		//If the button has endUpgrading set or only the exit button is left
		Godot.Collections.Array<Node> buttons = this.GetChildren();

		if (button.endUpgrading || (buttons.Count - 1) <= 1)
		{ EmitFinishedUpgrading(); }
	}

	private void SaveSpawnedUpgrades(Scenes spawnedUpgrades) {
		 var settings = new JsonSerializerSettings();
		settings.TypeNameHandling = TypeNameHandling.Objects;
		string jsonData = JsonConvert.SerializeObject(spawnedUpgrades, settings);
		this.EmitSignal(SignalName.SpawnedUpgrades, jsonData);
	}

	private void EmitUpdateUpgradeDesc(string value)
	{
		this.EmitSignal(SignalName.UpdateUpgradeUi, value);
	}

	//Emit signal to decrement score multiplier
	public void EmitDecreaseMultiplier()
	{
		this.EmitSignal(SignalName.DecreaseMultiplier, false);
	}

	//Emit signal to delete any existing upgrades
	public void EmitFinishedUpgrading()
	{
		this.EmitSignal(SignalName.UpgradingFinished);
		this.QueueFree();
	}

	#region Testing Functions

	// private void Test() {
	//     var items = 100;
	//     var x0 = 960;
	//     var y0 = 540;
	//     var r = 300;

	//     TestingDots.PlaceCenterDot(GetTree());

	//     for(var i = 0; i < items; i++) {
	//         var x = x0 + r * Math.Cos((2 * Math.PI * i / items) - (Math.PI/2)); 
	//         var y = y0 + r * Math.Sin((2 * Math.PI * i / items) - (Math.PI/2));
	//         var point = new Vector2((float)x, (float)y);
			
	//         TestingDots.PlaceTestingDot(GetTree(), point);
	//     }
	// }

	#endregion
}
