using Godot;
using System;
using static Enums;

public class PauseMenu : Control
{
	public override void _Ready() {
        this.Visible = false;

		Godot.VBoxContainer buttonContainer = this.GetNode<Godot.VBoxContainer>("Buttons");
		Godot.Collections.Array buttons = buttonContainer.GetChildren();

		for (int i = 0; i < buttons.Count; i++)
		{
			if(!buttons[i].GetType().Equals(typeof(MenuButtons))) { continue; }
		
			Godot.Button button = (Godot.Button)buttons[i];
			button.Connect("on_pressed", this, "_OnButtonPress");
		}
	}

    public override void _UnhandledInput(InputEvent @event) {
		if (@event is InputEventKey eventKey) {
			if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape) {
				TogglePause();
            }
        }
	}

    private void TogglePause() {
        bool currentPauseState = GetTree().Paused;
        GetTree().Paused = !currentPauseState;
        this.Visible = !currentPauseState;
    }

    private void TogglePause(bool forcePause) {
        GetTree().Paused = forcePause;
        this.Visible = forcePause;
    }

	private void _OnButtonPress(MenuButtons button) {
        TogglePause(false);

		switch (button.action)
		{
			case MenuButtonActions.Play:
				Play(button);
				break;
            case MenuButtonActions.MainMenu:
				MainMenu(button);
				break;
			case MenuButtonActions.Options:
				Options(button);
				break;
			case MenuButtonActions.Quit:
				Quit();
				break;	
		}
	}

	[Signal] public delegate void _restart_game();
    [Signal] public delegate void _back_to_menu();
	[Signal] public delegate void _options();

	private void Play(MenuButtons button) {
		this.EmitSignal("_restart_game");
		button.Disabled = true;
	}

    private void MainMenu(MenuButtons button) {
        this.EmitSignal("_back_to_menu");
		button.Disabled = true;
	}

	private void Options(MenuButtons button) {
		this.EmitSignal("_options");
		button.Disabled = true;
	}

	private void Quit() {
		GetTree().Quit();
	}
}
