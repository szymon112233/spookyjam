using Godot;
using System;

public partial class SimpleCameraPivot : Node3D
{
    Vector3 startPosition;
    [Export]
    Node3D playerNode3D;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        startPosition = Transform.Origin;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Position = new Vector3();
        Translate(startPosition);
        
    }
}
