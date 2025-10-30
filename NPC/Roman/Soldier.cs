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

    protected double IdleTime;
    protected double CurrentIdleTime;
    protected double CurrentChaseTime;

    protected PlayerController FoundPlayer;

    Random rand = new Random();

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        PlayerCast.Enabled = GameManager.Instance.Notoriety > NotorietyThreshold;

        if (PlayerCast.IsColliding())
        {
            var obj = PlayerCast.GetCollider(0);
            PlayerController playerNode = obj as PlayerController;

            TargetPlayer(playerNode);
            ChasePlayer(delta);
        }
        else if(FoundPlayer != null)
        {
            ChasePlayer(delta);
        }
    }

    public override void _Ready()
	{
        base._Ready();
        IdleTime = rand.NextDouble() * MaximumIdleTime + MinimumIdelTime;
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
        if (TestForAttack())
        {
            return;
        }
        
        CurrentChaseTime += delta;
        if (CurrentChaseTime >= ChaseTime)
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
            FoundPlayer.Attacked(this);
            return true;
        }

        return false;
    }

    private void StopChase()
    {
        CurrentSpeed = MoveSpeed;
        CurrentChaseTime = 0.0;
        FoundPlayer = null;
        SetTarget(GlobalPosition);
    }
}
