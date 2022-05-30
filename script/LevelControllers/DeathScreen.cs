using Godot;
using System;

//DeathScreen.tscn 
public class DeathScreen : Levels
{
	public override void _Ready() {
		Godot.Control control = this.GetNode<Godot.Control>("Control");
		Godot.VBoxContainer buttonContainer = control.GetNode<Godot.VBoxContainer>("Buttons");
		Godot.Collections.Array buttons = buttonContainer.GetChildren();

		for (int i = 0; i < buttons.Count; i++)
		{
			if(!buttons[i].GetType().Equals(typeof(GameOverButtons))) { continue; }
		
			Godot.Button button = (Godot.Button)buttons[i];
			GD.Print(button.Name);
			
		}
	}

	//Handel score
	public override void LoadLevelParameters(System.Object sceneData) {
		if(sceneData == null) return;
		GD.Print(((MainGameObj)sceneData).isQuickReset);
	}
}
