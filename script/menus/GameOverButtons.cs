using Godot;
using static Enums;

public class GameOverButtons : Button
{
	[Export] public MenuButtons action;

	public override void _Ready()
	{
		this.Connect("pressed", this, "_OnButtonPress");
		this.Connect("mouse_entered", this, "_OnMouseEntered");
		this.Connect("mouse_exited", this, "_OnMouseExit");
	}

	[Signal]
    public delegate void on_pressed(GameOverButtons button);
	private void _OnButtonPress() {
		this.EmitSignal("on_pressed", this);
	}

	//Play expand anim
	private void _OnMouseEntered() {
		AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("MenuBtnSelected");
	}

	//Play collapse anim
	private void _OnMouseExit() {
		AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("MenuBtnDeselected");
	}
}
