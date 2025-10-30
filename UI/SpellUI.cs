using Godot;
using System;

public partial class SpellUI : TextureRect
{
    [Export]
    private Texture2D[] spellTextures;
    
    public override void _Ready()
    {
        PlayerController.ChangedSpell += ChangedSpell;
    }

    private void ChangedSpell(int index)
    {
        if (index >= spellTextures.Length)
        {
            throw new System.IndexOutOfRangeException("Textures for the spell haven't been added or is out of index");
        }
        Texture = spellTextures[index];
    }

}
