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
	
	[Signal]
	public delegate void OnDeathEventHandler();
	[Signal]
	public delegate void OnHealEventHandler();
	[Signal]
	public delegate void OnResurrectEventHandler();
	
	
	[Signal]
	public delegate void OnStatusChangedEventHandler(HealthStatus status);

	[Export]
	protected NavigationAgent3D NavAgent3D;

	[Export]
	protected ShapeCast3D PlayerCast;

	[Export]
	protected Vector3 TargetPosition_Debug;

	[Export]
	protected Node3D DebugPosPoint;

	[Export]
	protected DefaultAnimationPlayer AnimationPlayer;

	[Export]
	protected float MoveSpeed;

	[Export]
	protected float RunSpeed;

	[Export]
	protected bool DancingMan;

	[Export]
	private HealthStatus _healthStatus = HealthStatus.Healthy;

	private float CurrentSpeed;
	
	[Export] 
	public InteractionResultData DeathResult;
	[Export] 
	public InteractionResultData HealResult;
	[Export] 
	public InteractionResultData ResurectResult;
	
	public override void _Ready()
	{
		CurrentSpeed = MoveSpeed;
		PlayerCast.Enabled = false;
		SetTarget(GlobalPosition);
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
			if (DancingMan)
			{
				AnimationPlayer.PlayAnimationWithKey(AnimationPlayer.AnimationName_Dance);
			}
			else
			{
				AnimationPlayer.PlayAnimationWithKey(AnimationPlayer.AnimationName_Idle);
			}
			
			Idle(delta);
		}
		else
		{
			AnimationPlayer.PlayAnimationWithKey(AnimationPlayer.AnimationName_Walk);

			var pos = NavAgent3D.GetNextPathPosition();
			Velocity = GetVelocity(pos);
			
			MoveAndSlide();

			if(GlobalPosition != pos)
				LookAt(pos, useModelFront: true);
		}

	}
	
	public virtual Vector3 GetVelocity(Vector3 targetPos)
	{
		var dir = targetPos - GlobalPosition;
		dir = dir.Normalized();

		return dir * CurrentSpeed;
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
		var previousStatus = _healthStatus;
		_healthStatus = status;
		EmitSignalOnStatusChanged(status);
		switch (status)
		{
			case HealthStatus.Healthy:
				Rotation = new Vector3(0, 0, 0);
				if (previousStatus == HealthStatus.Dead)
				{
					HandleResurrection();
				}

				if (previousStatus == HealthStatus.Sick)
				{
					HandleHeal();
				}
				break;
			case HealthStatus.Sick:
				Rotation = new Vector3(45, 0, 0);
				break;
			case HealthStatus.Dead:
				Rotation = new Vector3(90, 0, 0);
				if (previousStatus == HealthStatus.Healthy || previousStatus == HealthStatus.Sick)
				{
					HandleDeath();	
				}
				break;
		}
	}

	public void FusRohDah(Vector3 direction)
	{
		Velocity = direction * 100;
		MoveAndSlide();
	}
	
	
	
	public void HandleResurrection()
	{
		EmitSignalOnResurrect();
		GameManager.Instance.ChangeDivineApproval(ResurectResult.DivineApprovalChange);
		GameManager.Instance.ChangeNotoriety(ResurectResult.NotorietyChange);
	}
	
	public void HandleHeal()
	{
		EmitSignalOnHeal();
		GameManager.Instance.ChangeDivineApproval(HealResult.DivineApprovalChange);
		GameManager.Instance.ChangeNotoriety(HealResult.NotorietyChange);
	}

	public void HandleDeath()
	{
		EmitSignalOnDeath();
		GameManager.Instance.ChangeDivineApproval(DeathResult.DivineApprovalChange);
		GameManager.Instance.ChangeNotoriety(DeathResult.NotorietyChange);
	}
}
