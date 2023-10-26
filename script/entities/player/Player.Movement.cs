using Godot;
using System;
using System.Threading.Tasks;

public partial class Player
{
    private void MouseRotation()
    {
        sprite.LookAt(GetGlobalMousePosition());
    }

    //When player collides with *any rigi-body bounce
    private void OnBodyEntered(Node2D body)
    {
        Vector2 onHitVelocity = (LinearVelocity.X == 0 || LinearVelocity.Y == 0) ? this.LinearVelocity : Vector2.Zero;

        //Collided rigid-body stats  
        Godot.Sprite2D hitBodySprint = body.GetNode<Godot.Sprite2D>("Sprite2D");
        Vector2 hitCenter = hitBodySprint.GlobalPosition;
        Vector2 hitScaleHalf = (hitBodySprint.GlobalScale / 2f) * 20f; //Sprite2D scale is x20 smaller than global position (might change)    

        //Player stats
        Vector2 playerCenter = this.GlobalPosition;

        //Apply opposite directional forces
        Vector2 direction = new Vector2(
            GetCollisionForceDirection(playerCenter.X, hitCenter.X, hitScaleHalf.X),
            GetCollisionForceDirection(playerCenter.Y, hitCenter.Y, hitScaleHalf.Y)
        );
        
        PushPlayer(direction, onHitVelocity);
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
            return direction / Math.Abs(direction);
        }

        return 0;
    }

    #region Movement Forces

    private void PushPlayer(Vector2 thrustDirection, Vector2 addendVelocity = new Vector2())
    {
        //oldMoveCancel - stop the player gaining/ losing speed when changing directions
        Vector2 oldMoveCancel = Mathc.Abs(thrustDirection) * -this.LinearVelocity; 
        Vector2 thrust = thrustDirection * MovementForce;
        ApplyCentralImpulse(thrust + addendVelocity + oldMoveCancel);
    }

    private void PushPlayer(Vector2 thrustDirection, string effectDirection)
    {
        PushPlayer(thrustDirection);
        PlayEffect(effectDirection);
    }

    private void StopPlayer()
    {
        this.LinearVelocity = Vector2.Zero;

        Godot.Node2D effectNode = this.GetNode<Godot.Node2D>("Effects");
        Godot.Node2D directionNode = effectNode.GetNode<Godot.Node2D>("Stop");
        AnimationPlayer anim = directionNode.GetNode<AnimationPlayer>("AnimationPlayer");

        if (string.IsNullOrEmpty(anim.CurrentAnimation)) { PlayEffect(anim); }
    }

    #endregion

    #region  Movement effects

    private void PlayEffect(string effectNodeName)
    {
        Godot.Node2D effectNode = this.GetNode<Godot.Node2D>("Effects");
        Godot.Node2D directionNode = effectNode.GetNode<Godot.Node2D>(effectNodeName);
        AnimationPlayer anim = directionNode.GetNode<AnimationPlayer>("AnimationPlayer");

        PlayEffect(anim);
    }

    private void PlayEffect(AnimationPlayer anim)
    {
        anim.Play("Trigger");
    }

    #endregion
}