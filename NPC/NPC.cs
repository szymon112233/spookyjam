using Godot;
using System;

public partial class NPC : CharacterBody3D
{
	public enum HealthStatus
	{
		Healthy = 0,
		Sick = 1,
		Dead = 2
	}

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

	[Export] 
	private HealthStatus _healthStatus = HealthStatus.Healthy;

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

	}

	public void SetDebugPos()
	{
		SetTarget(DebugPosPoint.Position);
	}

	public void ChangeHealthStatus(HealthStatus status)
	{
		switch (status)
		{
			case HealthStatus.Healthy:
				Rotation = new Vector3(0, 0, 0);
				break;
			case HealthStatus.Sick:
				Rotation = new Vector3(45, 0, 0);
				break;
			case HealthStatus.Dead:
				Rotation = new Vector3(90, 0, 0);
				break;
		}
	}
}
