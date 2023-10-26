namespace Godot;

public partial class MenuController : Control
{
    //Button locations other that the default button location
    [Export] public string[] uniqueContainers = new string[0];

    [Signal] public delegate void PlayGameEventHandler(); //Event: starts or restarts the game
    [Signal] public delegate void OptionsEventHandler(); //Event: open the options scene
    [Signal] public delegate void LeaderboardEventHandler(); //Event: open the leader-board scene
    [Signal] public delegate void MainMenuEventHandler(); //Event: open the main menu scene

    public override void _Ready()
    {
        VBoxContainer buttonContainer = this.GetNode<VBoxContainer>("Buttons");
        Godot.Collections.Array<Node> buttons = buttonContainer.GetChildren();

        //Get buttons in the unique containers
        for (int i = 0; i < uniqueContainers.Length; i++)
        {
            string containerPath = uniqueContainers[i];
            Godot.Container uniqueButtonContainer = this.GetNode<Container>(containerPath);
            buttons += uniqueButtonContainer.GetChildren();
        }

        for (int i = 0; i < buttons.Count; i++)
        {
            if (!buttons[i].GetType().Equals(typeof(MenuBtn))) { continue; }

            MenuBtn button = (MenuBtn)buttons[i];
            button.ButtonDown += () => _OnButtonDown(button);
            button.ButtonUp += () => _OnButtonUp(button);
            button.Pressed += () => _OnButtonPressed(button);
        }

        _MenuReady();
    }

    internal virtual void _MenuReady() { }
    internal virtual void _OnButtonDown(MenuBtn button) { }
    internal virtual void _OnButtonUp(MenuBtn button) { }
    internal virtual void _OnButtonPressed(MenuBtn button) { }
}