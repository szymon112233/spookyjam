using Godot;
using System;

public interface IBaseSpell
{
	public float ManaCost { get; set; }
	public NPC.HealthStatus HealthStatusEffect { get; set; }
}

// public 
