using Godot;
using System;
using static Enums;

public class LeaderboardMenu : MenuController
{
	internal override void _OnButtonPress(MenuButtons button) 
	{
		switch (button.action)
		{
			case MenuButtonActions.Continue:
				Return(button);
				break;
			default:
				GD.Print($"Warning: unused btn action: {button.action}");
			break;
		}
	}

	private void Return(MenuButtons button) 
	{
		this.EmitSignal("_main_menu");
		button.Disabled = true;
	}
}
