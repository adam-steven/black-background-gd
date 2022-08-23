using Godot;
using System;
using static Enums;

public class OptionsMenu : MenuController
{
	[Export] public string[] tabContainers;

    [Signal] internal delegate void _set_count_down(MenuButtons button);

	public override void _Ready() {
		Godot.VBoxContainer buttonContainer = this.GetNode<Godot.VBoxContainer>("Buttons");
		Godot.Collections.Array buttons = buttonContainer.GetChildren();

		//Add the children in the tab
		for (int i = 0; i < tabContainers.Length; i++)
		{
			string tabContainer = tabContainers[i];
			Godot.VBoxContainer tabButtonContainer = buttonContainer.GetNode<Godot.VBoxContainer>(tabContainers[i]);
			buttons += tabButtonContainer.GetChildren();
		}

		for (int i = 0; i < buttons.Count; i++)
		{
			if(!buttons[i].GetType().Equals(typeof(MenuButtons))) { continue; }
		
			Godot.Button button = (Godot.Button)buttons[i];
			button.Connect("on_pressed", this, "_OnButtonPress");
		}

		_MenuReady();
	}

	internal override void _OnButtonPress(MenuButtons button) {
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
		this.EmitSignal("_main_menu");
		button.Disabled = true;
	}

	private void SaveStartCountDown(MenuButtons button) {
		this.EmitSignal("_set_count_down", button);
	}
}
