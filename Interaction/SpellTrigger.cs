using Godot;
using System;
using System.Diagnostics;

public partial class SpellTrigger : Area3D
{
    [Export]
    public SpellType RequiredSpell;

    [Export]
    public bool IsOneShot;

    [Signal]
    public delegate void CorrectSpellHitEventHandler();

    private bool IsUsed;

    public void OnBodyEntered(Node3D body)
    {
        if(body is IBaseSpell && !(IsUsed && IsOneShot))
        {
            IBaseSpell spell = body as IBaseSpell;
            if(spell.SpellType == RequiredSpell)
            {
                IsUsed = true;
                EmitSignalCorrectSpellHit();
            }
        }
    }
}
