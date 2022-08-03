using Godot;
using System;

public class OptionsScreen : Levels
{
	private OptionsObj optionsData;

	private SettingsController settings = new SettingsController();

	public override void _Ready() {
		Godot.Control control = this.GetNode<Godot.Control>("Control");
		control.Connect("_main_menu", this, "Return");
		control.Connect("_set_count_down", this, "SaveStartCountDown");

		LoadSavedOptions(control);
	}

	//Get the saved settings to load in
	private void LoadSavedOptions(Godot.Control control) {
		SaveObj savedSettings = settings.GetAllValues();
		if(savedSettings == new SaveObj()) { return; }

		Godot.VBoxContainer buttonContainer = control.GetNode<Godot.VBoxContainer>("Buttons");
		Godot.Collections.Array buttons = buttonContainer.GetChildren();

		for (int i = 0; i < buttons.Count; i++)
		{
			if(!buttons[i].GetType().Equals(typeof(MenuButtons))) { continue; }
		
			MenuButtons button = (MenuButtons)buttons[i];
			
			string action = button.action.ToString();
			if(!savedSettings.ContainsKey(action)) { return; }

			object settingVal = savedSettings[action];
			switch (Type.GetTypeCode(settingVal.GetType()))
			{
				case TypeCode.Boolean:
					button.Pressed = (bool)settingVal;
				break;
			}
		}
	}

	public override void LoadLevelParameters(System.Object sceneData) {
		optionsData = (sceneData != null) ? (OptionsObj)sceneData : new OptionsObj(false);
	}

	private void Return() {
		MainGameObj restartObj = new MainGameObj(optionsData.inGame);
		EmitChangeScene("res://scenes/Main.tscn", 5f, restartObj);
	}

	private void SaveStartCountDown(MenuButtons button) {
		settings.SetValue(button.action.ToString(), button.Pressed);
	}

}
