using Godot;
using System;

[GlobalClass]
public partial class GameEndingsData : Resource
{
    [Export]
    public string TrueEndingTitle;
    [Export]
    public string TrueEndingDescription;
    
    [Export]
    public string BadEndingTitle;
    [Export]
    public string BadEndingDescription;
    
    [Export]
    public string JapaneseEndingTitle;
    [Export]
    public string JapaneseEndingDescription;
    
    [Export]
    public string GoldenEndingTitle;
    [Export]
    public string GoldenEndingDescription;
}
