using Godot;
using System;
using System.Threading.Tasks;

public static class Colour
{
    //https://lospec.com/palette-list/r-place-2022-day2
    private static Color[] gameColors = {
        new Godot.Color("#be0039"),
        new Godot.Color("#ff4500"),
        new Godot.Color("#ffa800"),
        new Godot.Color("#ffd635"),
        new Godot.Color("#00a368"),
        new Godot.Color("#00cc78"),
        new Godot.Color("#7eed56"),
        new Godot.Color("#009eaa"),
        new Godot.Color("#3690ea"),
        new Godot.Color("#51e9f4"),
        new Godot.Color("#6a5cff"),
        new Godot.Color("#811e9f"),
        new Godot.Color("#b44ac0"),
        new Godot.Color("#ff3881"),
        new Godot.Color("#ff99aa"),
    };

    public static Color LevelColour { get; set; }
    public static Color HarmonizingColour { get; set; }

    //Starts turning the background red if player health is less than 30
    public static void UpdateBackgroundColour(int playerHealth)
    {
        //make sure the number is never less than 0
        float healthPercent = Math.Max(0.0f, 0.5f - (playerHealth / 1000.0f));
        int red = (int)Math.Floor(50 * healthPercent);
        VisualServer.SetDefaultClearColor(Color.Color8((byte)red, 0, 0));
    }

    //Flash a colour on the Scene of a sec + freeze frame
    public static async void FlashBackgroundColourAsync(Color colourToFlash, SceneTree tree, int playerHealth)
    {
        bool isFlashBlack = colourToFlash.IsEqualApprox(Color.ColorN("black"));
        Node2D rootScene = (!isFlashBlack) ? (Node2D)tree.CurrentScene : new Node2D();

        rootScene.Modulate = Color.ColorN("black");
        VisualServer.SetDefaultClearColor(colourToFlash);

        //Freeze frame always with colour flash to minimise seizure risk
        await FreezeFrameAsync(tree);

        rootScene.Modulate = Color.ColorN("white");
        UpdateBackgroundColour(playerHealth);
    }

    private static async Task FreezeFrameAsync(SceneTree tree)
    {
        tree.Paused = true;
        await Task.Delay(300);
        tree.Paused = false;
    }

    //Updates the Games colour scheme to a new random colour
    public static void UpdateGameColours(Godot.Node2D levelNode, Entity player)
    {
        Random rnd = new Random();
        int chosenColour = rnd.Next(gameColors.Length);
        Color newColour = gameColors[chosenColour];

        if (newColour == LevelColour)
        {
            chosenColour = incrementColour();
            newColour = gameColors[chosenColour];
        }

        LevelColour = newColour;
        HarmonizingColour = gameColors[incrementColour()];
        levelNode.Modulate = newColour;

        int incrementColour()
        {
            return (chosenColour + 1 >= gameColors.Length) ? 0 : chosenColour + 1;
        }
    }
}