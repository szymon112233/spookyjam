using System;
using Godot;

[GlobalClass]
public partial class DialogData : Resource
{
    [Export] 
    public String Speaker;

    [Export]
    public String StartingText;

    [Export]
    public String InprogressText;

    [Export]
    public String CompletedHappyText;
    
    [Export]
    public String CompletedSadText;


    public DialogData()
    {
        Speaker = "dummy";
        StartingText = "Help";
        CompletedHappyText = "ty for help";
    }
}