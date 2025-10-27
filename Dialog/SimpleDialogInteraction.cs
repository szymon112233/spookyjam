using Godot;
using System;

public partial class SimpleDialogInteraction : Area3D
{
    [Export]
    public string StartingText;

    [Export]
    public string CompletedText;

    [Export]
    public Sprite3D Billboard;

    [Export]
    public Label3D TextLabel;

    [Export]
    public bool IsCompleted = false;

    public override void _Ready()
    {
        TextLabel.Text = StartingText;
        IsCompleted = false;
    }

    public void Refresh()
    {
        TextLabel.Text = !IsCompleted ? StartingText : CompletedText;
    }

    public new void Show()
    {
        Billboard.Visible = true;
    }

    public new void Hide()
    {
        Billboard.Visible = false;
    }
    
    public void OnPlayerEnter(Node3D player)
    {
        Show();
    }

    public void OnPlayerExit(Node3D player)
    {
        Hide();
    }

    public void Complete()
    {
        IsCompleted = true;
        Refresh();
    }

}
