using System;
using Godot;

public partial class GameController
{
    public Color playerColour;
    public Color enemyColour;

    private void UpdateGameColours() {
        enemyColour = Color.Color8(0,255,255);

        Position2D room = levelNode.GetNode<Position2D>("Room");
        foreach (var child in room.GetChildren())
        {
            if(!(child is RigidBody2D)) continue;
            Godot.Sprite childSprint = ((RigidBody2D)child).GetNode<Godot.Sprite>("Sprite");
            childSprint.Modulate = enemyColour;
        }
    }

    //Starts turning the background red if player health is less than 30
	public void UpdateBackgroundColour(int playerHealth) {
		//make sure the number is never less than 0
		int red = Math.Max(0, 30 - playerHealth) * 2;
		VisualServer.SetDefaultClearColor(Color.Color8((byte)red,0,0));
	}
}