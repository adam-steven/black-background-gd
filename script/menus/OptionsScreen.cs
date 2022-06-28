using Godot;
using System;
using static Enums;

public class OptionsScreen : Levels
{
    OptionsObj optionsData;

	public override void _Ready() {
		Godot.Control control = this.GetNode<Godot.Control>("Control");
		Godot.VBoxContainer buttonContainer = control.GetNode<Godot.VBoxContainer>("Buttons");
		Godot.Collections.Array buttons = buttonContainer.GetChildren();

		for (int i = 0; i < buttons.Count; i++)
		{
			if(!buttons[i].GetType().Equals(typeof(MenuButtons))) { continue; }
		
			Godot.Button button = (Godot.Button)buttons[i];
			button.Connect("on_pressed", this, "_OnButtonPress");
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
		}
	}

	private void Return(MenuButtons button) {
		MainGameObj restartObj = new MainGameObj(optionsData.inGame);
		EmitChangeScene("res://scenes/Main.tscn", 5f, restartObj);
		button.Disabled = true;
	}

}
