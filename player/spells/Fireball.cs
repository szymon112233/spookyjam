using Godot;
using System;

public partial class Fireball : RigidBody3D
{
	[Export()]
	float SPEED = 10f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void SetTransform(Transform3D trans)
	{
		Transform = trans; 
		LinearVelocity = -Basis.Z.Normalized()*SPEED;
	}
}
