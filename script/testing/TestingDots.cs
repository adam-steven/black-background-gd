using Godot;

public static class TestingDots
{
    public static void PlaceCenterDot(SceneTree tree) {
        Vector2 levelCenter = new Vector2(960, 540);
        PlaceTestingDot(tree, levelCenter);
    }

    public static void PlaceTestingDot(SceneTree tree, Vector2 tDotPos) {
        GD.Print(tDotPos);

        Node rootNode = tree.Root.GetChild(0);
        PackedScene testingDot = (PackedScene)GD.Load("res://scenes/testing/TestingDot.tscn");
        Godot.Sprite2D tDot = testingDot.Instantiate<Godot.Sprite2D>();
        tDot.GlobalPosition = tDotPos;
        rootNode.AddChild(tDot);
    }
}