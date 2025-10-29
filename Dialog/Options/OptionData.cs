using Godot;
using System;

[GlobalClass]
public partial class OptionData : Resource
{
    [Export]
    public string[] OptionTexts;

    [Export]
    public int[] OptionsCorrectness;

    [Export]
    public int[] NotorietyEffects;

    [Export]
    public int[] ApprovalEffects;
}
