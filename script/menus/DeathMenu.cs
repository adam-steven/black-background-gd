using Godot;
using System;
using static Enums;

public class DeathMenu : MenuController
{
    internal override void _OnButtonPress(MenuBtn button)
    {
        switch (button.action)
        {
            case MenuButtonActions.Play:
                Replay(button);
                break;
            case MenuButtonActions.MainMenu:
                MainMenu(button);
                break;
            case MenuButtonActions.Leaderboard:
                Leaderboard(button);
                break;
            default:
                GD.Print($"Warning: unused btn action: {button.action}");
                break;
        }
    }

    private void Replay(MenuBtn button)
    {
        this.EmitSignal("_play_game");
        button.Disabled = true;
    }

    private void MainMenu(MenuBtn button)
    {
        this.EmitSignal("_main_menu");
        button.Disabled = true;
    }

    private void Leaderboard(MenuBtn button)
    {
        this.EmitSignal("_leaderboard");
        button.Disabled = true;
    }
}
