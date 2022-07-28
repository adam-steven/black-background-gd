using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class OptionsScreen : Levels
{
	OptionsObj optionsData;

	public override void _Ready() {
		Godot.Control control = this.GetNode<Godot.Control>("Control");
		control.Connect("_main_menu", this, "Return");
		control.Connect("_set_count_down", this, "SaveStartCountDown");

		LoadSavedOptions(control);
	}

	//Get the saved settings to load in
	private void LoadSavedOptions(Godot.Control control) {
		List<KeyValuePair<string, object>> savedSettings = SettingsController.GetAllValues();
		if(savedSettings == null) { return; }

		Godot.VBoxContainer buttonContainer = control.GetNode<Godot.VBoxContainer>("Buttons");
		Godot.Collections.Array buttons = buttonContainer.GetChildren();

		for (int i = 0; i < buttons.Count; i++)
		{
			if(!buttons[i].GetType().Equals(typeof(MenuButtons))) { continue; }
		
			MenuButtons button = (MenuButtons)buttons[i];
			
			string action = button.action.ToString();
			KeyValuePair<string, object> savedSettingPair = savedSettings.SingleOrDefault(kvp => kvp.Key == action);

			if(savedSettingPair.Value == null) { return; }

			switch (Type.GetTypeCode(savedSettingPair.Value.GetType()))
			{
				case TypeCode.Boolean:
					button.Pressed = (bool)savedSettingPair.Value;
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
		SettingsController.SetValue(button.action.ToString(), button.Pressed);
	}

}
