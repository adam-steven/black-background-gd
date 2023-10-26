using Godot;
using System;

public partial class OptionsScreen : Level
{
	private OptionsObj optionsData = new OptionsObj();

	private SettingsController settings = new SettingsController();

	public override void _Ready()
	{
		OptionsMenu control = this.GetNode<OptionsMenu>("Control");

		control.Connect(MenuController.SignalName.MainMenu, new Callable(this, "Return"));
		control.Connect(OptionsMenu.SignalName.ToggleKeyPickOverlay, new Callable(this, "ToggleKeyPickOverlay"));

		control.Connect(OptionsMenu.SignalName.SetBoolSetting, new Callable(this, "SaveSettingBool"));
		control.Connect(OptionsMenu.SignalName.SetStringSetting, new Callable(this, "SaveSettingString"));

		LoadSavedOptions(control);
	}

	//Get the saved settings to load in
	private void LoadSavedOptions(OptionsMenu control)
	{
		Settings savedSettings = settings.GetAllValues();
		if (savedSettings == new Settings()) { return; }

		Godot.VBoxContainer buttonContainer = control.GetNode<Godot.VBoxContainer>("Buttons");
        Godot.Collections.Array<Node> buttons = buttonContainer.GetChildren();

		//Add the children in the tab
		for (int i = 0; i < control.uniqueContainers.Length; i++)
		{
			string containerPath = control.uniqueContainers[i];
			Godot.Container uniqueButtonContainer = control.GetNode<Godot.Container>(containerPath);
			buttons += uniqueButtonContainer.GetChildren();
		}

		for (int i = 0; i < buttons.Count; i++)
		{
			if (!buttons[i].GetType().Equals(typeof(MenuBtn))) { continue; }

			MenuBtn button = (MenuBtn)buttons[i];

			string action = button.action.ToString();
			if (!savedSettings.ContainsKey(action)) { continue; }

			object settingVal = savedSettings[action];
			switch (settingVal)
			{
				case Boolean boolean:
					button.ButtonPressed = (bool)settingVal;
					break;
				case String str:
					string decodedVal = DecodeString((string)settingVal);
					button.SetValueLabel(decodedVal);
					break;
				default:
					GD.Print($"{action} is a {settingVal.GetType()}");
					break;
			}
		}
	}

	public override void _LoadLevelParameters(object sceneData)
	{
		if (sceneData is not null)
		{ optionsData = (OptionsObj)sceneData; }
	}

	private string DecodeString(string value)
	{
		char inputType = value[0];

		//Decode key binds 
		if (inputType == 'K')
		{
			bool codeParseSuccess = Int32.TryParse(value.Remove(0, 1), out int code);
			if (codeParseSuccess)
			{
				return ((Key)code).ToString();
			}
		}

		return value;
	}

	private void Return()
	{
		EmitChangeScene("res://scenes/gameEnvironment/Main.tscn", 5f, optionsData.GameObj);
	}

	private void ToggleKeyPickOverlay(bool visiable)
	{
		Godot.Control keyPickOverlay = this.GetNode<Godot.Control>("BlockingOverlay");
		keyPickOverlay.Visible = visiable;
	}

	private void SaveSettingBool(MenuBtn button, bool value)
	{
		GD.Print($"bool: {button.action.ToString()} = {value}");
		settings.SetValue(button.action.ToString(), value);
	}

	private void SaveSettingString(MenuBtn button, string value)
	{
		GD.Print($"string: {button.action.ToString()} = {value}");
		settings.SetValue(button.action.ToString(), value);
	}
}
