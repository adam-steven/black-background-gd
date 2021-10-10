using Godot;

//Player movement and firing
//Player acts as a bullet, movement done via impulse forces
public partial class PlayerController : RigidBody2D
{

	public override void _Ready()
	{
		ConfigureShootOps();
	}

	public override void _PhysicsProcess(float delta)
	{
		GetMovementInput();
	}

	//-- Movement effects --
	private void DirectionEffect(float oppositeDirection) {
		Godot.Node2D directionEffect = this.GetNode<Godot.Node2D>("DirectionEffect");
		directionEffect.RotationDegrees = oppositeDirection;
		directionEffect.Visible = true;
	}

	private void StopEffect() {
		Godot.Node2D stopEffect = this.GetNode<Godot.Node2D>("StopEffect");
	}
}


