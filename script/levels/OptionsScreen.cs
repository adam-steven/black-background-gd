using Godot;
using static Enums;
using System;
using System.Linq;
using System.Collections.Generic;

public class OptionsScreen : Levels
{
	OptionsObj optionsData;

	public override void _Ready() {
		Godot.Control control = this.GetNode<Godot.Control>("Control");
		Godot.VBoxContainer buttonContainer = control.GetNode<Godot.VBoxContainer>("Buttons");
		Godot.Collections.Array buttons = buttonContainer.GetChildren();

		//Get the saved settings to load in
		List<KeyValuePair<string, object>> savedSettings = SettingsController.GetAllValues();

		for (int i = 0; i < buttons.Count; i++)
		{
			if(!buttons[i].GetType().Equals(typeof(MenuButtons))) { continue; }
		
			Godot.Button button = (Godot.Button)buttons[i];
			button.Connect("on_pressed", this, "_OnButtonPress");

			LoadSavedValue((MenuButtons)button);
		}

		void LoadSavedValue(MenuButtons button) {
			if(savedSettings == null) { return; }

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

	private void _OnButtonPress(MenuButtons button) {
		switch (button.action)
		{
			case MenuButtonActions.Continue:
				Return(button);
				break;
			case MenuButtonActions.StartCountDown:
				SaveStartCountDown(button);
				break;
		}
	}

	private void Return(MenuButtons button) {
		MainGameObj restartObj = new MainGameObj(optionsData.inGame);
		EmitChangeScene("res://scenes/Main.tscn", 5f, restartObj);
		button.Disabled = true;
	}

	private void SaveStartCountDown(MenuButtons button) {
		SettingsController.SetValue(button.action.ToString(), button.Pressed);
	}

}
