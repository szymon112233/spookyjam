using Godot;
using System;

public partial class OptionDialog : SimpleDialogInteraction
{
    [Export]
    public OptionData HandledOption;
    
    [Export]
    public Sprite3D QuestionIndication;
    
    [Signal]
    public delegate void ChoiceStartedEventHandler();

    protected bool IsOptionActive;

    PlayerController BoundController;

    public new void OnPlayerEnter(Node3D player)
    {
        base.OnPlayerEnter(player);
        
        QuestionIndication.Hide();
        BoundController = player as PlayerController;
    }

    public new void OnPlayerExit(Node3D player)
    {
        base.OnPlayerExit(player);
        if (!IsCompleted)
        {
            QuestionIndication.Show();
        }
    }
    
    public override void _Ready()
    {

        if (GetParent() is NPC npc)
        {
            npc.OnRagdoll += () => { GD.Print("Yeah ragdoll"); QuestionIndication?.Hide(); };
            npc.OnCharacterControlBack += () => { QuestionIndication?.Show();};
        }
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsCompleted || IsOptionActive)
        {
            return;
        }

        if (Input.IsActionJustPressed("interact") && isPlayerInsideArea)
        {
            StartOption();
        }
    }
    
    public new void Hide()
    {
        base.Hide();
        IsOptionActive = false;
        BoundController.PlayerDialogOptionHandler.Hide();
    }

    protected void StartOption()
    {
        TextLabel.Text = HandledDialog.InprogressText;
        BoundController.PlayerDialogOptionHandler.BindOptions(HandledOption, PlayerMadeChoice);
        QuestionIndication.QueueFree();
        EmitSignalChoiceStarted();
    }

    protected void PlayerMadeChoice(bool correct)
    {
        IsCompleted = true;
        PositiveOutput = correct;
        BoundController.PlayerDialogOptionHandler.Hide();
        Refresh();
    }
}
