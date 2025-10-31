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
	public delegate void OnRagdollEventHandler();
	[Signal]
	public delegate void OnCharacterControlBackEventHandler();
	
	
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
	protected HealthStatus _healthStatus = HealthStatus.Healthy;
	
	[Export]
	protected PhysicalBoneSimulator3D Ragdoll;
    
	[Export]
	protected CollisionShape3D mainCollider;

	private bool _ragdolledThisFrame = false;
	private bool _ragdolledDisabledThisFrame = false;
	private Vector3 ragdollDirection;

	protected float CurrentSpeed;
	
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

		if (Ragdoll != null)
		{
			if (_ragdolledThisFrame)
			{
				ActivateRagdoll(Vector3.Zero, ragdollDirection);
				_ragdolledThisFrame = false;
			}

			if (_ragdolledDisabledThisFrame)
			{
				DeactivateRagdoll();
				_ragdolledDisabledThisFrame = false;
			}

		}

		if (_healthStatus == HealthStatus.Dead)
		{
			return;
		}
		
		if(_healthStatus == HealthStatus.Sick)
		{
			AnimationPlayer.PlayAnimationWithKey(AnimationPlayer.AnimationName_Sick);
			return;
        }
		
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

			if (GlobalPosition != pos)
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
				// Rotation = new Vector3(90, 0, 0);
				if (previousStatus == HealthStatus.Healthy || previousStatus == HealthStatus.Sick)
				{
					HandleDeath();	
				}
				break;
		}
	}

	public void FusRohDah(Vector3 direction)
	{
		_ragdolledThisFrame = true;
		ragdollDirection = direction;
		// Velocity = direction * 100;
		// MoveAndSlide();
	}

	public void AddForceAndActivateRagdoll(Vector3 direction)
	{
		ragdollDirection = direction;
		_ragdolledThisFrame = true;
		// ActivateRagdoll(Vector3.Zero, direction);
	}
	
	
	
	public void HandleResurrection()
	{
		EmitSignalOnResurrect();
		GameManager.Instance.ChangeDivineApproval(ResurectResult.DivineApprovalChange);
		GameManager.Instance.ChangeNotoriety(ResurectResult.NotorietyChange);
		_ragdolledDisabledThisFrame = true;
	}
	
	public void HandleHeal()
	{
		EmitSignalOnHeal();
		GameManager.Instance.ChangeDivineApproval(HealResult.DivineApprovalChange);
		GameManager.Instance.ChangeNotoriety(HealResult.NotorietyChange);
		_ragdolledDisabledThisFrame = true;
	}

	public void HandleDeath()
	{
		EmitSignalOnDeath();
		GameManager.Instance.ChangeDivineApproval(DeathResult.DivineApprovalChange);
		GameManager.Instance.ChangeNotoriety(DeathResult.NotorietyChange);
		_ragdolledThisFrame = true;
	}
	
	private void ActivateRagdoll(Vector3 position, Vector3 direction)
	{
		EmitSignalOnRagdoll();
		mainCollider.Disabled = true;

		
		Ragdoll.Active = true;
		Ragdoll.PhysicalBonesStartSimulation();
		for (int i = 0; i < Ragdoll.GetChildCount(); i++)
		{
			if (Ragdoll.GetChild(i) is PhysicalBone3D bone)
			{
				bone.ApplyImpulse(-direction * 50);
			}
		}
	}


	private void DeactivateRagdoll()
	{
        EmitSignalOnCharacterControlBack();
		GlobalPosition = ((Node3D)Ragdoll.GetChild(0)).GlobalPosition; 
		Ragdoll.PhysicalBonesStopSimulation();
		Ragdoll.Active = false;
		mainCollider.Disabled = false;
	}
}
