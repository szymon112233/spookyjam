using Godot;
using System;

public partial class Fireball : RigidBody3D, IBaseSpell
{

	[Export]
	public string Name { get; set; }

	[Export]
	public SpellType SpellType { get; set; }
	
	[Export()]
	float SPEED = 10f;
	
	[Export()]
	public float ManaCost { get; set; }
	
	[Export()]
	public NPC.HealthStatus HealthStatusEffect { get; set; }

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += _on_body_entered;
		GD.Print("Spawned");
		ContactMonitor = true;
		SetMaxContactsReported(1);
	}

	public new void SetInitialState(Transform3D trans)
	{
		Transform = trans; 
		LinearVelocity = -Basis.Z.Normalized()*SPEED;
	}

	private void OnBodyEntered(Node3D body)
	{
		GD.Print("OnBodyEntered");
	}

	private void _on_body_entered(Node node)
	{
		GD.Print(node.Name);
		if (node is PhysicalBone3D)
		{
			if (node.GetParent().GetParent().GetParent().GetParent() is NPC npc)
			{
				npc.ChangeHealthStatus(HealthStatusEffect);
				if(HealthStatusEffect == NPC.HealthStatus.Dead)
				{
					npc.AddForceAndActivateRagdoll(Transform.Basis.Z);
				}
			}
		}
		if (node is NPC npc2)
		{
			GD.Print("Hit NPC");
			npc2.ChangeHealthStatus(HealthStatusEffect);
			if(HealthStatusEffect == NPC.HealthStatus.Dead)
			{
				npc2.AddForceAndActivateRagdoll(Transform.Basis.Z);
			}
		}
		HitEffect();
	}
	
	private void HitEffect()
	{
		//do something fancy here and destroy the projectile
		QueueFree();
	}
	
	
	
	
	
}
