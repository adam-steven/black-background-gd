using Godot;
using System;

public partial class PlayerController
{
	// Handels detection of WASD inputs to apply character force 
	private void WASDMovement()
	{
		if (Input.IsActionJustPressed("ui_left")) {
			PushPlayer(Vector2.Left, 270);
		}
		if (Input.IsActionJustPressed("ui_right")) {
			PushPlayer(Vector2.Right, 90);
		}
		if (Input.IsActionJustPressed("ui_up")) {
			PushPlayer(Vector2.Up, 0);
		}
		if (Input.IsActionJustPressed("ui_down")) {
			PushPlayer(Vector2.Down, 180);
		}
	}

	private void MouseRotation() {
		Godot.Sprite playerSprite = this.GetNode<Godot.Sprite>("Sprite");
		playerSprite.LookAt(GetGlobalMousePosition()); 
	}

	//When player collides with *any rigibody bounce
	private void _On_Player_Body_Entered(object body)
	{
		//Collided rigidbody stats  
		Godot.Sprite hitBodySprint = ((Node)body).GetNode<Godot.Sprite>("Sprite");
		Vector2 hitCenter = hitBodySprint.GlobalPosition;
		Vector2 hitScaleHalf = (hitBodySprint.GlobalScale/2f) * 20f; //Sprite scale is x20 smaller than global position (might change)    

		//Player stats
		Vector2 playerCenter = this.GlobalPosition;

		//Apply opposite directional forces
		Vector2 direction = new Vector2 (
			GetCollisionForceDirection(playerCenter.x, hitCenter.x, hitScaleHalf.x),
			GetCollisionForceDirection(playerCenter.y, hitCenter.y, hitScaleHalf.y)
		);

		PushPlayer(direction);
	}

	//Verifies player center Axis is at the edge of a collided object same axis
	//If so it returns 1 or -1 for a force to push the player away
	//If no it returns 0
	private int GetCollisionForceDirection(float playerAxisPos, float objectAxisPos, float objectAxisScale)
	{
		float objectLowerPoint = objectAxisPos - objectAxisScale;
		float objectUpperPoint = objectAxisPos + objectAxisScale;

		if (playerAxisPos < objectLowerPoint || objectUpperPoint < playerAxisPos)
		{
			int direction = (int)(playerAxisPos - objectAxisPos);
			return (direction / Math.Abs(direction));
		}

		return 0;
	}

	//-- Movement Forces --

	private void PushPlayer(Vector2 _thrustDirection) {
		Vector2 _thrust = _thrustDirection * movementForce;
		ApplyCentralImpulse(_thrust);
    }
    private void PushPlayer(Vector2 _thrustDirection, int effectDirection) {
		Vector2 _thrust = _thrustDirection * movementForce;
		ApplyCentralImpulse(_thrust);
		DirectionEffect(effectDirection);
    }

    private void StopPlayer() {
        this.LinearVelocity = Vector2.Zero;
		StopEffect();
    }

    //-- Movement effects --
	private void DirectionEffect(float oppositeDirection) {
		Godot.Node2D directionEffect = this.GetNode<Godot.Node2D>("DirectionEffect");
		directionEffect.RotationDegrees = oppositeDirection;
		directionEffect.Visible = true;
	}

	private void StopEffect() {
		Godot.Node2D stopEffect = this.GetNode<Godot.Node2D>("StopEffect");
		stopEffect.Visible = true;
	}
}
