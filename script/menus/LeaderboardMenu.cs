using Godot;
using static Enums;

public partial class LeaderboardMenu : MenuController
{
    internal override void _OnButtonPressed(MenuBtn button)
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

    private void Return(MenuBtn button)
    {
        this.EmitSignal(MenuController.SignalName.MainMenu);
        button.Disabled = true;
    }
}
