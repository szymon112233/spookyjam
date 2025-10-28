using Godot;
using System;

public partial class SimpleDialogInteraction : Area3D
{
    [Export] 
    public DialogData HandledDialog;

    [Export]
    public Sprite3D Billboard;

    [Export]
    public Label3D TextLabel;

    [Export]
    public bool IsCompleted = false;
    
    [Export]
    public bool IsEnabled = true;
    
    [Export]
    public bool PositiveOutput = true;
    
    [Signal]
    public delegate void CompletedEventHandler(bool isPositive);
    [Signal]
    public delegate void CompletedPositiveEventHandler();
    [Signal]
    public delegate void CompletedNegativeEventHandler();

    private int debugState = 0;
    private bool isPlayerInsideArea = false;

    public override void _Ready()
    {
        TextLabel.Text = HandledDialog.StartingText;
        IsCompleted = false;
    }

    public void Refresh()
    {
        TextLabel.Text = !IsCompleted ? HandledDialog.StartingText : PositiveOutput 
            ? HandledDialog.CompletedHappyText: HandledDialog.CompletedSadText;
    }

    public new void Show()
    {
        if (!IsEnabled)
        {
            return;
        }
        
        Refresh();
        Billboard.Visible = true;
    }

    public new void Hide()
    {
        Billboard.Visible = false;
    }
    
    public void OnPlayerEnter(Node3D player)
    {
        Show();
        isPlayerInsideArea = true;
    }

    public void OnPlayerExit(Node3D player)
    {
        Hide();
        isPlayerInsideArea = false;
    }
    
    public void Complete(bool isPositiveOutput)
    {
        IsCompleted = true;
        PositiveOutput = isPositiveOutput;
        EmitSignalCompleted(isPositiveOutput);
        if (isPositiveOutput)
        {
            EmitSignalCompletedPositive();
        }
        else
        {
            EmitSignalCompletedNegative();
        }
        Refresh();
    }
    
    public void UnComplete()
    {
        IsCompleted = false;
        PositiveOutput = true;
        Refresh();
    }
    
    
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("swtich_dialogue") && isPlayerInsideArea)
        {
            //DEBUG SHIT
            switch (debugState)
            {
                case 0:
                    Complete(true);
                    debugState++;
                    break;
                case 1:
                    Complete(false);
                    debugState++;
                    break;
                case 2:
                    UnComplete();
                    debugState = 0;
                    break;
            }
        }
    }

    public void SetEnabled(bool isEnabled)
    {
        this.IsEnabled = isEnabled;
        if (isEnabled && isPlayerInsideArea)
        {
            Show();
        }
    }

}
