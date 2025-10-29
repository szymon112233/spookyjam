using Godot;
using System;

public partial class PlayerDialogOption : Node3D
{
    [Export]
    protected OptionNode[] Options;

    private bool IsActive = false;
    private OptionData BoundData;
    private Action<bool> Callback;

    public override void _Process(double delta)
    {
        if (!IsActive)
        {
            return;
        }
        
        if (Input.IsActionJustPressed("PlayerOption1"))
        {
            OptionChosen(0);
        }

        if (Input.IsActionJustPressed("PlayerOption2"))
        {
            OptionChosen(1);
        }
        
        if (Input.IsActionJustPressed("PlayerOption3"))
        {
            OptionChosen(2);
        }
    }


    public void BindOptions(OptionData optionData, Action<bool> callback)
    {
        BoundData = optionData;
        Callback = callback;

        for (int i = 0; i < 3; i++)
        {
            Options[i].BindText(optionData.OptionTexts[i]);
        }
        
        Show();
    }

    public void Show()
    {
        //TODO::Add some sort of animation
        Visible = true;
        IsActive = true;
    }

    public void Hide()
    {
        Visible = false;
        IsActive = false;
    }

    public void OptionChosen(int id)
    {
        GameManager.Instance.ChangeNotoriety(BoundData.NotorietyEffects[id]);
        GameManager.Instance.ChangeDivineApproval(BoundData.ApprovalEffects[id]);
        Callback?.Invoke(BoundData.OptionsCorrectness[id] > 0);

        Hide();
    }
    

}
