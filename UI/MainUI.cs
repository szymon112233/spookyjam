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
	
	
	[Export]
	public Control EndingScreenRoot;
	[Export] 
	public Label EndingNameLabel;
	[Export]
	public Label EndingTextLabel;
	
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var gameManager = GameManager.Instance;
		
		gameManager.DivineApprovalChanged += OnDivineApprovalChanged;
		gameManager.ManaChanged += OnManaChanged;
		gameManager.NotorietyChanged += OnNotorietyChanged;
		gameManager.GameEnded += GameManagerOnGameEnded;
		
		OnDivineApprovalChanged(gameManager.DivineApproval);
		OnManaChanged(gameManager.Mana);
		OnNotorietyChanged(gameManager.Notoriety);
		
		EndingScreenRoot.Hide();
	}

	private void GameManagerOnGameEnded(GameManager.Ending ending)
	{
		ShowEnding(ending);
	}
	
	public void ShowEnding(GameManager.Ending ending)
	{
		switch (ending)
		{
			case GameManager.Ending.TrueEnding:
				EndingNameLabel.Text = GameManager.Instance.GameEndingsData.TrueEndingTitle;
				EndingTextLabel.Text = GameManager.Instance.GameEndingsData.TrueEndingDescription;
				break;
			case GameManager.Ending.GoldenEnding:
				EndingNameLabel.Text = GameManager.Instance.GameEndingsData.GoldenEndingTitle;
				EndingTextLabel.Text = GameManager.Instance.GameEndingsData.GoldenEndingDescription;
				break;
			case GameManager.Ending.BadEnding:
				EndingNameLabel.Text = GameManager.Instance.GameEndingsData.BadEndingTitle;
				EndingTextLabel.Text = GameManager.Instance.GameEndingsData.BadEndingDescription;
				break;
			case GameManager.Ending.JapaneseEnding:
				EndingNameLabel.Text = GameManager.Instance.GameEndingsData.JapaneseEndingTitle;
				EndingTextLabel.Text = GameManager.Instance.GameEndingsData.JapaneseEndingDescription;
				break;
		}
		
		
		EndingScreenRoot.Show();
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
}
