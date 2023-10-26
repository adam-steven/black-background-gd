using Godot;
using static Enums;
using System.Threading.Tasks;

public partial class OptionsMenu : MenuController
{
    [Signal] public delegate void ToggleKeyPickOverlayEventHandler(bool visiable); //Event: show/hide blocking overlay for the user to select a key
    [Signal] public delegate void SetBoolSettingEventHandler(MenuBtn button, bool value); //Event: update a boolean setting
    [Signal] public delegate void SetStringSettingEventHandler(MenuBtn button, string value); //Event: update a string setting

    private MenuBtn waitingButton = null;

    internal override void _OnButtonPressed(MenuBtn button)
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

    private void Return(MenuBtn button)
    {
        this.EmitSignal(MenuController.SignalName.MainMenu);
        button.Disabled = true;
    }

    private void SaveToggle(MenuBtn button)
    {
        this.EmitSignal(SignalName.SetBoolSetting, button, button.ButtonPressed);
    }

    //Displays a blocking overlay for allowing the user to select a key
    private void HandelKeyChange(MenuBtn button)
    {
        this.EmitSignal(SignalName.ToggleKeyPickOverlay, true);
        waitingButton = button;
    }

    //Hides the blocking overlay (with a delay to prevent accidental instant reopening )
    private async void EndKeyChangeAsync()
    {
        await Task.Delay(200);
        this.EmitSignal(SignalName.ToggleKeyPickOverlay, false);
        waitingButton = null;
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (waitingButton is null) { return; }
        if (!inputEvent.IsPressed()) { return; }

        if (inputEvent is InputEventKey keyEvent)
        {
            var keyCode = keyEvent.PhysicalKeycode;
            waitingButton.SetValueLabel(((Key)keyCode).ToString());
            this.EmitSignal(SignalName.SetStringSetting, waitingButton, $"K{(int)keyCode}");
        }

        if (inputEvent is InputEventMouseButton mouseEvent)
        {
            var mouseButtonCode = mouseEvent.ButtonIndex;
            waitingButton.SetValueLabel($"M{(int)mouseButtonCode}");
            this.EmitSignal(SignalName.SetStringSetting, waitingButton, $"M{(int)mouseButtonCode}");
        }

        EndKeyChangeAsync();
    }


}
