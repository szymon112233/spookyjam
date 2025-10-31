using Godot;
using System;

public partial class HealthPickup : PlayerTrigger
{
	public override void _Ready()
	{
		base._Ready();
		PlayerEntered += OnPlayerEntered;
	}

	private void OnPlayerEntered()
	{
		GameManager.Instance.ChangeLives(1);
		QueueFree();
	}
}
