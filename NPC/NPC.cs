using Godot;
using System;

public partial class NPC : CharacterBody3D
{

	[Export]
	protected NavigationAgent3D NavAgent3D;

	[Export]
	protected ShapeCast3D PlayerCast;

	[Export]
	protected Vector3 TargetPosition_Debug;

	[Export]
	protected Node3D DebugPosPoint;

	[Export]
	protected float MoveSpeed;

	[Export]
	protected float RunSpeed;

	// private double time = 0.0f; 

	public override void _Ready()
	{
		PlayerCast.Enabled = false;
		NavAgent3D.TargetPosition = Position;
	}

	public virtual void SetTarget(Vector3 position)
	{
		NavAgent3D.TargetPosition = position;
		TargetPosition_Debug = position;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (NavAgent3D.IsNavigationFinished())
		{
			Idle(delta);
		}
		else
		{
			var pos = NavAgent3D.GetNextPathPosition();
			Velocity = GetVelocity(pos);

			MoveAndSlide();
		}

	}
	
	public virtual Vector3 GetVelocity(Vector3 targetPos)
	{
		var dir = targetPos - Position;
		dir = dir.Normalized();

		return dir * MoveSpeed;
	} 

	public virtual void Idle(double delta)
	{
		// time += delta;

		// if(time >= 5)
		// {
		// 	SetDebugPos();
		// 	time = -9999999999999999f;
		// }
	}

	public void SetDebugPos()
	{
		SetTarget(DebugPosPoint.Position);
	}
}
