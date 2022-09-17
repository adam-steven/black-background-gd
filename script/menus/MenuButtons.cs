using Godot;
using static Enums;

public class MenuButtons : Button
{
	[Export] public MenuButtonActions action;

	[Export] public string valueLabelPath = null;
	[Export] public int labelTruncate = 10;

	public override void _Ready()
	{
		this.Connect("mouse_entered", this, "_OnMouseEntered");
		this.Connect("mouse_exited", this, "_OnMouseExit");
	}

	//Play expand anim
	private void _OnMouseEntered() 
	{
		AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("MenuBtnSelected");
	}

	//Play collapse anim
	private void _OnMouseExit() 
	{
		AnimationPlayer anim  = this.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("MenuBtnDeselected");
	}

	public void SetValueLabel(string value) 
	{
		if(string.IsNullOrEmpty(valueLabelPath)) { return; }
		Godot.Label valueLabel = this.GetNode<Godot.Label>(valueLabelPath);

		string truncatedVal = (value.Length < labelTruncate) ? value : value.Substring(0, labelTruncate); 
		valueLabel.Text = truncatedVal;
	}
}
