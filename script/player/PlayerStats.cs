using Godot;

public partial class PlayerController
{
    [Export] private int health = 100;
	[Export] private float movementForce = 1000f;
    [Export] private float shotDelay = 0.2f;
}
