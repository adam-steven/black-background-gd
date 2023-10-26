using Godot;
using System;
using static Enums;

public partial class PauseMenu : MenuController
{

	#region Handel Pause

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent.IsActionPressed("Pause"))
		{
			TogglePause();
		}
	}

	private void TogglePause()
	{
		bool currentPauseState = GetTree().Paused;
		GetTree().Paused = !currentPauseState;
		this.Visible = !currentPauseState;
	}

	private void TogglePause(bool forcePause)
	{
		GetTree().Paused = forcePause;
		this.Visible = forcePause;
	}

	#endregion

	internal override void _OnButtonPressed(MenuBtn button)
	{
		TogglePause(false);

		switch (button.action)
		{
			case MenuButtonActions.Continue:
				//Do nothing
				break;
			case MenuButtonActions.Play:
                EmitPlay(button);
				break;
			case MenuButtonActions.MainMenu:
                EmitMainMenu(button);
				break;
			case MenuButtonActions.Options:
                EmitOptions(button);
				break;
			default:
				GD.Print($"Warning: unused btn action: {button.action}");
				break;
		}
	}

	private void EmitPlay(MenuBtn button)
	{
		this.EmitSignal(MenuController.SignalName.PlayGame);
		button.Disabled = true;
	}

	private void EmitMainMenu(MenuBtn button)
	{
		this.EmitSignal(MenuController.SignalName.MainMenu);
		button.Disabled = true;
	}

	private void EmitOptions(MenuBtn button)
	{
		this.EmitSignal(MenuController.SignalName.Options);
		button.Disabled = true;
	}
}
