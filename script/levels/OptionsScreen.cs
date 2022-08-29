using Godot;
using System;

public class OptionsScreen : Levels
{
	private OptionsObj optionsData = new OptionsObj();

	private SettingsController settings = new SettingsController();

	public override void _Ready() {
		OptionsMenu control = this.GetNode<OptionsMenu>("Control");

		control.Connect("_main_menu", this, "Return");
		control.Connect("_toggle_key_pick_overlay", this, "ToggleKeyPickOverlay");

		control.Connect("_set_bool_setting", this, "SaveSettingBool");
		control.Connect("_set_string_setting", this, "SaveSettingString");

		LoadSavedOptions(control);
	}

	//Get the saved settings to load in
	private void LoadSavedOptions(OptionsMenu control) {
		SaveObj savedSettings = settings.GetAllValues();
		if(savedSettings == new SaveObj()) { return; }

		Godot.VBoxContainer buttonContainer = control.GetNode<Godot.VBoxContainer>("Buttons");
		Godot.Collections.Array buttons = buttonContainer.GetChildren();

		//Add the children in the tab
		for (int i = 0; i < control.uniqueContainers.Length; i++)
		{
			string containerPath = control.uniqueContainers[i];
			Godot.Container uniqueButtonContainer = control.GetNode<Godot.Container>(containerPath);
			buttons += uniqueButtonContainer.GetChildren();
		}
		
		for (int i = 0; i < buttons.Count; i++)
		{
			if(!buttons[i].GetType().Equals(typeof(MenuButtons))) { continue; }
		
			MenuButtons button = (MenuButtons)buttons[i];
			
			string action = button.action.ToString();
			if(!savedSettings.ContainsKey(action)) { continue; }

			object settingVal = savedSettings[action];
			switch (settingVal)
			{
				case Boolean boolean:
					button.Pressed = (bool)settingVal;
				break;
				case String str:
					button.SetValueLabel((string)settingVal);
				break;
				default:
					GD.Print($"{action} is a {settingVal.GetType()}");
				break;
			}
		}
	}

	public override void LoadLevelParameters(System.Object sceneData) {
		if(sceneData != null) {
			optionsData = (OptionsObj)sceneData;
		}
	}

	private void Return() {
		EmitChangeScene("res://scenes/Main.tscn", 5f, optionsData.gameObj);
	}

	private void ToggleKeyPickOverlay(bool visiable) {
		Godot.Control keyPickOverlay =  this.GetNode<Godot.Control>("BlockingOverlay");
		keyPickOverlay.Visible = visiable;
	}

	private void SaveSettingBool(MenuButtons button, bool value) {
		GD.Print($"bool {button.action.ToString()} {value}");
		settings.SetValue(button.action.ToString(), value);
	}

	private void SaveSettingString(MenuButtons button, string value) {
		GD.Print($"string {button.action.ToString()} {value}");
		settings.SetValue(button.action.ToString(), value);
	}



}
