using Godot;
using System;
using static Enums;

public class OptionsMenu : MenuController
{
    [Signal] internal delegate void _set_count_down(MenuButtons button);

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
