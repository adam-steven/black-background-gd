using Godot;
using System;
using System.Threading.Tasks;

public static class ColourController
{
	public static Color playerColour { get; set; }
	public static Color enemyColour { get; set; }

	//Starts turning the background red if player health is less than 30
	public static void UpdateBackgroundColour(int playerHealth) {
		//make sure the number is never less than 0
		int red = Math.Max(0, 30 - playerHealth) * 2;
		VisualServer.SetDefaultClearColor(Color.Color8((byte)red,0,0));
	}

	//Flash a colour on the Scene of a sec + freeze frame
	public static async void FlashBackgroundColour(Color colourToFlash, SceneTree tree, int playerHealth) {
		VisualServer.SetDefaultClearColor(colourToFlash);
		tree.Paused = true;

		//Freeze frame always with colour flash to minimise seizure risk
		await Task.Delay(200);

		tree.Paused = false;
		UpdateBackgroundColour(playerHealth);
	}

	//Updates the Games colour scheme to a new random colour
	public static void UpdateGameColours(Godot.Node2D levelNode, Entities player) {
		Random rnd = new Random();
		var values = Enum.GetValues(typeof(Enums.Colour));
		String colourName = values.GetValue(rnd.Next(values.Length)).ToString();
		enemyColour = Color.ColorN(colourName);

		levelNode.Modulate = enemyColour;
		player.colour = playerColour;
	}
}