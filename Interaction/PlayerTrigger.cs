using Godot;
using System;

public partial class PlayerTrigger : Area3D
{
	
	[Signal]
	public delegate void PLayerEnteredEventHandler();
	
	[Signal]
	public delegate void PLayerExitedEventHandler();

	private PlayerController playerInside;
	private bool isPLayerInside = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body.GetParentNode3D() is PlayerController player)
		{
			playerInside = player;
			isPLayerInside = true;
			EmitSignalPLayerEntered();
		}
	}
	
	private void OnBodyExited(Node3D body)
	{
		if (body.GetParentNode3D() is PlayerController player)
		{
			playerInside = null;
			isPLayerInside = false;
			EmitSignalPLayerExited();
		}
	}
	
}
