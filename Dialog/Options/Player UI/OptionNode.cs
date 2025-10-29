using Godot;
using System;

public partial class OptionNode : Sprite3D
{
    [Export]
    private Label3D TextLabel;

    public void BindText(string text)
    {
        TextLabel.Text = text;
    }
}
