using Godot;
using System;
using System.Reflection;

public partial class PlayerController : CharacterBody3D
{
	Vector3 start_position;
	float gravity;
	Camera3D camera;
	
	[Export()]
	float MAX_SPEED = 3.5f;
	[Export()]
	float JUMP_SPEED = 6.5f;
	[Export()]
	float ACCELERATION = 4;
	[Export()]
	float DECELERATION = 4;
	[Export()]
	float SPRINT_MAX_SPEED_MULTIPLIER = 1.5f;
	
	//var camera;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//camera = $Target/Camera3D;
		camera = GetViewport().GetCamera3D();
		
		start_position = Position;
		gravity = -(float)ProjectSettings.GetSetting("physics/3d/default_gravity");
	}
	

	
	public override void _PhysicsProcess(double delta)
	{
		if(Input.IsActionJustPressed("fire1"))
		{
			Shoot();
		}
		
		if (Input.IsActionJustPressed("exit"))
		{
			GetTree().Quit();
		}
		if (Input.IsActionJustPressed("reset_position") || GlobalPosition.Y < - 6)
		{
			//Pressed the reset key or fell off the ground.
			Position = start_position;
			Velocity = new Vector3();
		}
		
		bool isSprinting= Input.IsActionPressed("sprint");
		
		var dir = new Vector3();
		dir.X = Input.GetAxis("move_left", "move_right");
		dir.Z = Input.GetAxis("move_forward", "move_back");
		
		//# Limit the input to a length of 1. length_squared is faster to check.
		if (dir.LengthSquared() > 1)
		{
			dir /= dir.Length();
		}

		//# Get the camera's transform basis, but remove the X rotation such
		//# that the Y axis is up and Z is horizontal.
		var cam_basis = camera.GlobalTransform.Basis;
		cam_basis = cam_basis.Rotated(cam_basis.X, -cam_basis.GetEuler().X);
		dir = cam_basis * dir;

		
		//# Using only the horizontal velocity, interpolate towards the input.
		var hvel = Velocity;
		hvel.Y = 0;

		var target = dir * MAX_SPEED;

		if (isSprinting)
		{
			target *= SPRINT_MAX_SPEED_MULTIPLIER;
		}
			
		float acceleration = 0;
		if (dir.Dot(hvel) > 0)
			acceleration = ACCELERATION;
		else
			acceleration = DECELERATION;

		hvel = hvel.Lerp(target, acceleration * (float)delta);
		
		if(hvel.Length() > 0)
		{
			Transform = Transform.LookingAt(hvel+Transform.Origin, new Vector3(0,1,0));
		}

		//# Assign hvel's values back to velocity, and then move.
		Velocity = new Vector3(hvel.X, Velocity.Y, hvel.Z);

		Velocity += new Vector3(0, (float)delta * gravity, 0);
		MoveAndSlide();
		
		if (IsOnFloor() && Input.IsActionPressed("jump"))
			Velocity += new Vector3(0, JUMP_SPEED, 0);
	}
	
	public  void _on_tcube_body_entered(Node3D body){
		if(body == this)
		{
			var canvasItem = (CanvasItem)GetNode("WinText");
			canvasItem.Show();
		}
	}
	
	private void Shoot(){
		var scene = ResourceLoader.Load<PackedScene>("res://player/spells/Fireball.tscn").Instantiate();
		GD.Print("Fired1");
		//var b = Bullet.instantiate()ö
		Owner.AddChild(scene);
		
		//b.transform = $Muzzle.global_transform
		//GD.Print(((Node3D)scene).Transform);
		Transform3D trans = ((Node3D)this).GlobalTransform;
		trans.Origin += -trans.Basis.Z;
		// RigidBody3D sceneRb = scene.GetNode<RigidBody3D>(""
		//sceneRb.LinearVelocity += -trans.Basis.Z;
		// RigidBody3D sceneRb = (RigidBody3D)scene;
		//((Node3D)scene).Transform = ((Node3D)(this)).GlobalTransform;
		((Node3D)scene).Transform = trans;
		
	}
}
