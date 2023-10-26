using Godot;
using System;
using static Enums;

//MainMenu.tscn 
public partial class MainMenu : MenuController
{
	internal override void _OnButtonPressed(MenuBtn button)
	{
		switch (button.action)
		{
			case MenuButtonActions.Play:
                EmitPlay(button);
				break;
			case MenuButtonActions.Options:
                EmitOptions(button);
				break;
			case MenuButtonActions.Leaderboard:
                EmitLeaderboard(button);
				break;
			case MenuButtonActions.Quit:
				Quit();
				break;
			default:
				GD.Print($"Warning: unused btn action: {button.action}");
				break;
		}

		this.QueueFree();
	}

	private void EmitPlay(MenuBtn button)
	{
		this.EmitSignal(MenuController.SignalName.PlayGame);
		button.Disabled = true;
	}

	private void EmitOptions(MenuBtn button)
	{
		this.EmitSignal(MenuController.SignalName.Options);
		button.Disabled = true;
	}

	private void EmitLeaderboard(MenuBtn button)
	{
		this.EmitSignal(MenuController.SignalName.Leaderboard);
		button.Disabled = true;
	}

	private void Quit()
	{
		GetTree().Quit();
	}
}
