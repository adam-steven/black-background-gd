using Godot;
using System;

public partial class UiController : Control
{
	public override void _Ready()
	{
		GetScoreUi();
		GetWaveIndicatorUi();
	}
}
