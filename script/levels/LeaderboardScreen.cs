using Godot;

public partial class LeaderboardScreen : Level
{
    public override void _Ready()
    {
        MenuController control = this.GetNode<MenuController>("Control");
        control.Connect(MenuController.SignalName.MainMenu, new Callable(this, "Return"));
    }

    private void Return()
    {
        MainGameObj restartObj = new MainGameObj(false);
        EmitChangeScene("res://scenes/gameEnvironment/Main.tscn", 5f, restartObj);
    }
}
