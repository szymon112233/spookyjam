using Godot;
using System;

public partial class MainUI : Node
{
	[Export]
	public Label DivinieApprovalLabel;
	[Export]
	public Label ManalLabel;
	[Export]
	public Label NotorietyLabel;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var gameManager = GameManager.Instance;
		
		gameManager.DivineApprovalChanged += OnDivineApprovalChanged;
		gameManager.ManaChanged += OnManaChanged;
		gameManager.NotorietyChanged += OnNotorietyChanged;
		
		OnDivineApprovalChanged(gameManager.DivineApproval);
		OnManaChanged(gameManager.Mana);
		OnNotorietyChanged(gameManager.Notoriety);
	}

	private void OnNotorietyChanged(int newValue)
	{
		NotorietyLabel.Text = $"Notoriety: {newValue}";
	}

	private void OnManaChanged(int newValue)
	{
		ManalLabel.Text = $"Mana: {newValue}";
	}

	private void OnDivineApprovalChanged(int newValue)
	{
		DivinieApprovalLabel.Text = $"Divinie Approval: {newValue}";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
