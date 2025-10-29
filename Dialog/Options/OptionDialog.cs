using Godot;
using System;

public partial class OptionDialog : SimpleDialogInteraction
{
    [Export]
    public OptionData HandledOption;


    protected bool IsOptionActive;

    PlayerController BoundController;

    public new void OnPlayerEnter(Node3D player)
    {
        base.OnPlayerEnter(player);

        BoundController = player as PlayerController;
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
    }

    protected void PlayerMadeChoice(bool correct)
    {
        IsCompleted = true;
        PositiveOutput = correct;
        BoundController.PlayerDialogOptionHandler.Hide();
        Refresh();
    }
}
