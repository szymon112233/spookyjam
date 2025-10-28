using Godot;
using System;

public partial class Map : Node3D
{
    public static Map Instance;

    [Export]
    public Node3D[] PointsOfInterest;

    [Signal]
    public delegate void TESTEventHandler();

    public override void _EnterTree()
    {
        if (Instance != null)
        {
            QueueFree();
        }
        else
        {
            Instance = this;
        }

    }

    public Node3D GetRandomPOI()
    {
        Random rand = new Random();
        return PointsOfInterest[rand.Next(PointsOfInterest.Length)];
    }
}
