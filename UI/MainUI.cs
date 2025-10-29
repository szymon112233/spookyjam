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
		
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(NotorietyLabel, "theme_override_colors/font_color", Colors.Yellow, 0.1f);
		tween.TweenProperty(NotorietyLabel, "theme_override_font_sizes/font_size", 33.0f, 0.1f);
		tween.SetParallel();
		tween.TweenProperty(NotorietyLabel, "theme_override_colors/font_color", Colors.White, 1.5f);
		tween.TweenProperty(NotorietyLabel, "theme_override_font_sizes/font_size", 31.0f, 1.0f);
	}

	private void OnManaChanged(int newValue)
	{
		ManalLabel.Text = $"Mana: {newValue}";
		
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(ManalLabel, "theme_override_colors/font_color", Colors.Yellow, 0.1f);
		tween.TweenProperty(ManalLabel, "theme_override_font_sizes/font_size", 33.0f, 0.1f);
		tween.SetParallel();
		tween.TweenProperty(ManalLabel, "theme_override_colors/font_color", Colors.White, 1.5f);
		tween.TweenProperty(ManalLabel, "theme_override_font_sizes/font_size", 31.0f, 1.0f);
		
	}

	private void OnDivineApprovalChanged(int newValue)
	{
		DivinieApprovalLabel.Text = $"Divine Approval: {newValue}";
		
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(DivinieApprovalLabel, "theme_override_colors/font_color", Colors.Yellow, 0.1f);
		// tween.TweenProperty(DivinieApprovalLabel, "theme_override_font_sizes/font_size", 20.0f, 0.1f);
		// tween.SetParallel(true);
		tween.TweenProperty(DivinieApprovalLabel, "theme_override_colors/font_color", Colors.White, 1.5f);
		// tween.Parallel().TweenProperty(DivinieApprovalLabel, "theme_override_font_sizes/font_size", 10.0f, 1.0f);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
