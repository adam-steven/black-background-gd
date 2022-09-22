using Godot;
using System;

public class Camera : Camera2D
{
    Random rnd = new Random();

    private int shakeForce = 1;
    private int shakeDuration = 0;
    private bool isShaking;

    public override void _Process(float delta)
    {
        if (!isShaking) return;

        if ((int)OS.GetTicksMsec() >= shakeDuration)
        {
            StopShakeScreen();
            return;
        }

        this.GlobalPosition = (this.GlobalPosition == Vector2.Zero)
            ? new Vector2(rnd.Next(0, shakeForce), rnd.Next(0, shakeForce))
            : Vector2.Zero;
    }

    //shakeDuration passed in should be in seconds, (converted to ms in function) 
    public void StartShakeScreen(int shakeForce, float shakeDuration)
    {
        this.shakeForce = shakeForce;
        this.shakeDuration = (int)OS.GetTicksMsec() + (int)(shakeDuration * 1000);
        isShaking = true;
    }

    public void StopShakeScreen()
    {
        this.GlobalPosition = Vector2.Zero;
        isShaking = false;
    }
}
