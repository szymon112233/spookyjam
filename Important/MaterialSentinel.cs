using Godot;
using System;
using Godot.Collections;

[GlobalClass][Tool]
public partial class MaterialSentinel : Resource
{
    [Export]
    public ColorPalette palette;
    
    [Export]
    public Array<BaseMaterial3D> materials;

    public MaterialSentinel()
    {
    }

    // public override void _ValidateProperty(Dictionary property)
    // {
    //     base._ValidateProperty(property);
    //     LoadColors();
    //     GD.Print("its time!");
    // }
    
    [ExportToolButton("Laod colors")]
    public Callable ClickMeButton => Callable.From(LoadColors);
    public void LoadColors()
    {
        GD.Print("loading colors");
        for (int i = 0; i < palette.Colors.Length; i++)
        {
            materials[i].AlbedoColor = palette.Colors[i];
            ResourceSaver.Save(materials[i], materials[i].ResourcePath);
        }
        GD.Print("Colors loaded");
        
    }
}
