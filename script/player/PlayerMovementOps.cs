using Godot;
using System;

public partial class PlayerController
{
    private void GetMovementInput() {
        WASDMovement();
        MouseRotation();
    }

    // Handels detection of a right click to apply a complete character force  stop
	public override void _IntegrateForces(Physics2DDirectBodyState state)
	{
		if (Input.IsActionJustPressed("ui_fire2"))
			state.LinearVelocity = Vector2.Zero;
	}

	// Handels detection of WASD inputs to apply character force 
	private void WASDMovement()
	{
		Vector2 _thrust = Vector2.Zero;
		float effectDirection = 0;

		if (Input.IsActionJustPressed("ui_left")) {
			_thrust += (Vector2.Left * movementForce);
			effectDirection = 270;
		}
		if (Input.IsActionJustPressed("ui_right")) {
			_thrust += (Vector2.Right * movementForce);
			effectDirection = 90;
		}
		if (Input.IsActionJustPressed("ui_up")) {
			_thrust += (Vector2.Up * movementForce);
			effectDirection = 0;
		}
		if (Input.IsActionJustPressed("ui_down")) {
			_thrust += (Vector2.Down * movementForce);
			effectDirection = 180;
		}

		if(_thrust != Vector2.Zero) {
			ApplyCentralImpulse(_thrust);
			DirectionEffect(effectDirection);
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
		Vector2 _thrust = this.LinearVelocity + new Vector2 (
			GetCollisionForceDirection(playerCenter.x, hitCenter.x, hitScaleHalf.x),
			GetCollisionForceDirection(playerCenter.y, hitCenter.y, hitScaleHalf.y)
		) * movementForce;
		ApplyCentralImpulse(_thrust);
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
}
