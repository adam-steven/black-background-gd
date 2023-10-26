using Godot;
using System;
using System.Threading.Tasks;

public partial class Camera : Camera2D
{
    Random rnd = new Random();

    private int shakeForce = 1;
    private int shakeDuration = 0;
    private bool isShaking;

    //shakeDuration passed in should be in seconds, (converted to ms in function) 
    public void StartShakeScreen(int shakeForce, float shakeDuration)
    {
        this.shakeForce = shakeForce;
        this.shakeDuration = (int)Time.GetTicksMsec() + (int)(shakeDuration * 1000);
        
        if(!isShaking) { ShakeScreenAsync(); }
    }

    private async void ShakeScreenAsync()
    {
        isShaking = true;

        while ((int)Time.GetTicksMsec() < shakeDuration)
        {
            this.GlobalPosition = (this.GlobalPosition == Vector2.Zero)
                ? new Vector2(rnd.Next(0, shakeForce), rnd.Next(0, shakeForce))
                : Vector2.Zero;
            
            await Task.Delay(10);
        }

        StopShakeScreen();
    }

    public void StopShakeScreen()
    {
        this.GlobalPosition = Vector2.Zero;
        isShaking = false;
    }
}
