using Godot;
using static Enums;

public partial class DeathMenu : MenuController
{
    internal override void _OnButtonPressed(MenuBtn button)
    {
        switch (button.action)
        {
            case MenuButtonActions.Play:
                EmitReplay(button);
                break;
            case MenuButtonActions.MainMenu:
                EmitMainMenu(button);
                break;
            case MenuButtonActions.Leaderboard:
                EmitLeaderboard(button);
                break;
            default:
                GD.Print($"Warning: unused btn action: {button.action}");
                break;
        }
    }

    private void EmitReplay(MenuBtn button)
    {
        this.EmitSignal(MenuController.SignalName.PlayGame);
        button.Disabled = true;
    }

    private void EmitMainMenu(MenuBtn button)
    {
        this.EmitSignal(MenuController.SignalName.MainMenu);
        button.Disabled = true;
    }

    private void EmitLeaderboard(MenuBtn button)
    {
        this.EmitSignal(MenuController.SignalName.Leaderboard);
        button.Disabled = true;
    }
}
