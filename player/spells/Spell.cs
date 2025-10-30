using Godot;
using System;

public partial class Spell : Resource
{
    [Export]
    public string Name;
    
    [Export]
    public SpellType SpellType;
    
    [Export]
    public int ManaCost;

}


public enum SpellType
{
    FIREBALL,
    HEAL
}
