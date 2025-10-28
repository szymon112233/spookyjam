using Godot;
using System;

public partial class Soldier : NPC
{
    [Export]
    public double MinimumIdelTime;

    [Export]
    public double MaximumIdleTime;

    protected double IdleTime;
    protected double CurrentTime;

    // public override void _Ready()
    // {
    //     NavAgent3D.TargetReached += GoToIdle;
    // }

    public override void Idle(double delta)
    {
        CurrentTime += delta;

        if (CurrentTime >= IdleTime)
        {
            var rand = new Random();

            CurrentTime = 0;
            IdleTime = rand.NextDouble() * MaximumIdleTime + MinimumIdelTime;
            SetNewPatrolPos();
        }
    }

    private void SetNewPatrolPos()
    {
        var poi = Map.Instance.GetRandomPOI();
        SetTarget(poi.Position);

    }

    // private void GoToIdle()
    // {
        
    // }
}
