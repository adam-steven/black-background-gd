using Godot;
using System;
using static Enums;

//MainMenu.tscn 
public class MainMenu : MenuController
{
    internal override void _OnButtonPress(MenuButtons button)
    {
        switch (button.action)
        {
            case MenuButtonActions.Play:
                Play(button);
                break;
            case MenuButtonActions.Options:
                Options(button);
                break;
            case MenuButtonActions.Leaderboard:
                Leaderboard(button);
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

    private void Play(MenuButtons button)
    {
        this.EmitSignal("_play_game");
        button.Disabled = true;
    }

    private void Options(MenuButtons button)
    {
        this.EmitSignal("_options");
        button.Disabled = true;
    }

    private void Leaderboard(MenuButtons button)
    {
        this.EmitSignal("_leaderboard");
        button.Disabled = true;
    }

    private void Quit()
    {
        GetTree().Quit();
    }
}
