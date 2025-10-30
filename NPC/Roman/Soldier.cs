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

    protected double IdleTime;
    protected double CurrentTime;
    protected PlayerController FoundPlayer;

    Random rand = new Random();

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        PlayerCast.Enabled = true;

        if(PlayerCast.IsColliding())
        {
            var obj = PlayerCast.GetCollider(0);
            Node3D playerNode = obj as Node3D;
           
            SetTarget(playerNode.GlobalPosition);
        }
    }

    public override void _Ready()
	{
        base._Ready();
        IdleTime = rand.NextDouble() * MaximumIdleTime + MinimumIdelTime;
	}

    public override void Idle(double delta)
    {
        CurrentTime += delta;

        if (CurrentTime >= IdleTime)
        {
            CurrentTime = 0;
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
        
    }

    // private void GoToIdle()
    // {
        
    // }
}
