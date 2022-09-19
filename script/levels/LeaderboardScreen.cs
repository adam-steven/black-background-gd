using Godot;
using System;

public class LeaderboardScreen : Levels
{
    public override void _Ready()
    {
        Godot.Control control = this.GetNode<Godot.Control>("Control");
        control.Connect("_main_menu", this, "Return");
    }

    private void Return()
    {
        MainGameObj restartObj = new MainGameObj(false);
        EmitChangeScene("res://scenes/gameEnvironment/Main.tscn", 5f, restartObj);
    }
}
