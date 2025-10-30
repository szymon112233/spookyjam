using Godot;
using System;

public interface IBaseSpell
{
	public string Name{ get; set; }
	public float ManaCost { get; set; }
	public NPC.HealthStatus HealthStatusEffect { get; set; }
	public SpellType SpellType { get; set; }
	public void SetInitialState(Transform3D trans);

}
