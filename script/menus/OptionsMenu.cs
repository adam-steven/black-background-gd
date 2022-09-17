using Godot;
using static Enums;
using System.Threading.Tasks;

public class OptionsMenu : MenuController
{
	[Signal] internal delegate void _toggle_key_pick_overlay(bool visiable);
    [Signal] internal delegate void _set_bool_setting(MenuButtons button, bool value);
	[Signal] internal delegate void _set_string_setting(MenuButtons button, string value);

	private MenuButtons waitingButton = null;

	internal override void _OnButtonPress(MenuButtons button) 
	{
		switch (button.action)
		{
			case MenuButtonActions.Continue:
				Return(button);
				break;
			case MenuButtonActions.StartCountDown:
				SaveToggle(button);
				break;
			case MenuButtonActions.UpgradeName: 
				SaveToggle(button);
				break;
			case MenuButtonActions.UpgradeDesc: 
				SaveToggle(button);
				break;
			case MenuButtonActions.Up:
				HandelKeyChange(button);
				break;
			case MenuButtonActions.Down:
				HandelKeyChange(button);
				break;
			case MenuButtonActions.Left:
				HandelKeyChange(button);
				break;
			case MenuButtonActions.Right:
				HandelKeyChange(button);
				break;
			case MenuButtonActions.Shoot:
				HandelKeyChange(button);
				break;
			case MenuButtonActions.Block:
				HandelKeyChange(button);
				break;
			case MenuButtonActions.Pause:
				HandelKeyChange(button);
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

	private void SaveToggle(MenuButtons button) 
	{
		this.EmitSignal("_set_bool_setting", button, button.Pressed);
	}

	//Displays a blocking overlay for allowing the user to select a key
	private void HandelKeyChange(MenuButtons button) 
	{
		this.EmitSignal("_toggle_key_pick_overlay", true);
		waitingButton = button;
	}

	//Hides the blocking overlay (with a delay to prevent accidental instant reopening )
	private async void EndKeyChangeAsync() 
	{
		await Task.Delay(200);
		this.EmitSignal("_toggle_key_pick_overlay", false);
		waitingButton = null;
	}

	public override void _Input(InputEvent inputEvent) 
	{
		if(waitingButton == null) { return; }
		if (!inputEvent.IsPressed()) { return; }

		if (inputEvent is InputEventKey keyEvent) {
			var keyCode = keyEvent.PhysicalScancode;
			waitingButton.SetValueLabel(((KeyList)keyCode).ToString());
			this.EmitSignal("_set_string_setting", waitingButton, $"K{(int)keyCode}");
		}

		if (inputEvent is InputEventMouseButton mouseEvent) {
			var mouseButtonCode = mouseEvent.ButtonIndex;
			waitingButton.SetValueLabel($"M{(int)mouseButtonCode}");
			this.EmitSignal("_set_string_setting", waitingButton, $"M{(int)mouseButtonCode}");
		}

		EndKeyChangeAsync();
	}


}
