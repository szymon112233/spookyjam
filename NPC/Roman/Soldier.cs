using Godot;
using System;
using System.Diagnostics;

public partial class Soldier : NPC
{
    [Export]
    public double MinimumIdelTime;

    [Export]
    public double MaximumIdleTime;

    [Export]
    protected float ChaseTime;

    [Export]
    protected float WhackDistance;

    [Export]
    protected int NotorietyThreshold;

    [Export]
    protected float NotorietyMulti = 0.4f;

    [Export]
    protected int NotorietyThresholdSteps = 2;
    
    [Export]
    protected AudioStreamPlayer3D HitEffectAudioPlayer;

    protected double IdleTime;
    protected double CurrentIdleTime;
    protected double CurrentChaseTime;
    protected float detectorRadius;
    protected float detectorLength;
    protected float detectorZPos;
    protected bool Attacking;

    protected PlayerController FoundPlayer;
    protected CylinderShape3D PlayerCastShape;

    Random rand = new Random();

    public override void _PhysicsProcess(double delta)
    {

        if (Attacking && !AnimationPlayer.IsAnimationPlaying)
        {
            Attacking = false;
        }
        else if(Attacking)
        {
            return;
        }
        
        base._PhysicsProcess(delta);

        if(_healthStatus == HealthStatus.Healthy && !isRagdolling)
        {
            int tier = GameManager.Instance.Notoriety / NotorietyThreshold;

            PlayerCast.Enabled = GameManager.Instance.Notoriety > NotorietyThreshold;
            
            PlayerCastShape.Radius = detectorRadius + detectorRadius * tier * NotorietyMulti;
            PlayerCastShape.Height = detectorLength + detectorLength * tier * NotorietyMulti;
            var castPos = PlayerCast.Position;
            castPos.Z = detectorZPos + (detectorLength * tier * NotorietyMulti) / 2;
            PlayerCast.Position = castPos;

            if (PlayerCast.IsColliding())
            {
                var obj = PlayerCast.GetCollider(0);
                PlayerController playerNode = obj as PlayerController;

                TargetPlayer(playerNode);
                ChasePlayer(delta);
            }
            else if (FoundPlayer != null)
            {
                ChasePlayer(delta);
            }
        }
    }

    public override void _Ready()
	{
        base._Ready();
        IdleTime = rand.NextDouble() * MaximumIdleTime + MinimumIdelTime;

        PlayerCastShape = PlayerCast.Shape as CylinderShape3D;
        detectorRadius = PlayerCastShape.Radius;
        detectorLength = PlayerCastShape.Height;
        detectorZPos = PlayerCast.Position.Z;
	}

    public override void Idle(double delta)
    {
        CurrentIdleTime += delta;

        if (CurrentIdleTime >= IdleTime)
        {
            CurrentIdleTime = 0;
            IdleTime = rand.NextDouble() * MaximumIdleTime + MinimumIdelTime;
            SetNewPatrolPos();
        }
    }

    private void SetNewPatrolPos()
    {
        var poi = Map.Instance.GetRandomPOI();
        SetTarget(poi.GlobalPosition);
    }

    private void TargetPlayer(PlayerController playerController)
    {
        FoundPlayer = playerController;
        CurrentSpeed = RunSpeed;
        CurrentChaseTime = 0.0;
    }

    private void ChasePlayer(double delta)
    {
        int tier = GameManager.Instance.Notoriety / NotorietyThreshold;

        if (TestForAttack())
        {
            return;
        }
        
        CurrentChaseTime += delta;
        if (CurrentChaseTime >= (ChaseTime + ChaseTime * tier * NotorietyMulti) ) 
        {
            StopChase();
        }
        else
        {
            SetTarget(FoundPlayer.GlobalPosition);
        }
    }

    private bool TestForAttack()
    {
        var distance = GlobalPosition.DistanceTo(FoundPlayer.GlobalPosition);

        if (distance <= WhackDistance)
        {
            PlayAttack();
            FoundPlayer.Attacked(this);
            HitEffectAudioPlayer.Play();
            return true;
        }

        return false;
    }

    private void PlayAttack()
    {
        AnimationPlayer.PlayAnimationWithKey(AnimationPlayer.AnimationName_Attack);
        Attacking = true;
    }

    private void StopChase()
    {
        CurrentSpeed = MoveSpeed;
        CurrentChaseTime = 0.0;
        FoundPlayer = null;
        SetTarget(GlobalPosition);
    }
}
