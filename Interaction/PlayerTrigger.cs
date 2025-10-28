using Godot;
using System;

public partial class PlayerTrigger : Area3D
{
	[Signal]
	public delegate void PlayerEnteredEventHandler();

	[Signal]
	public delegate void PlayerExitedEventHandler();

	[Signal]
	public delegate void InteractionPressedEventHandler();

	private PlayerController playerInside;
	private bool isPlayerInside = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

    public override void _Process(double delta)
    {
		if(Input.IsActionJustPressed("interact") && isPlayerInside)
		{
			EmitSignalInteractionPressed();
        }
    }

	private void OnBodyEntered(Node3D body)
	{
		if (body is PlayerController player)
		{
			playerInside = player;
			isPlayerInside = true;
			EmitSignalPlayerEntered();
		}
	}
	
	private void OnBodyExited(Node3D body)
	{
		if (body is PlayerController player)
		{
			playerInside = null;
			isPlayerInside = false;
			EmitSignalPlayerExited();
		}
	}
	
}
