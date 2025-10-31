using Godot;
using System;

public partial class GameManager : Node
{
	
	public enum Ending
	{
		TrueEnding = 0,
		BadEnding = 1,
		JapaneseEnding = 2,
		GoldenEnding = 3,
		GameBrokenEnding = 2137
	}
	
	public static GameManager Instance;
	
	[Export]
	public Timer GameTimer;

	[Export]
	public double GameDurationInSeconds = 5 * 60;
	
	[Export]
	public int DivineApproval = 0;
	
	[Export]
	public Vector2I DivineApprovalRange = new Vector2I(0, 100);
	
	[Export]
	public int DivineApprovalEndingTreshold = 80;
	
	[Export]
	public int Notoriety = 0;
	
	[Export]
	public Vector2I NotorietyRange = new Vector2I(0, 100);
	
	[Export]
	public int Mana = 100;
	
	[Export]
	public Vector2I ManaRange = new Vector2I(0, 100);

	[Export] 
	public int Lives = 3;
	
	[Export] 
	public Vector2I LivesRange = new Vector2I(0, 5);
	
	[Export]
	public GameEndingsData GameEndingsData;
	
	[Signal]
	public delegate void DivineApprovalChangedEventHandler(int newValue, bool isPositive);
	[Signal]
	public delegate void NotorietyChangedEventHandler(int newValue, bool isPositive);
	[Signal]
	public delegate void ManaChangedEventHandler(int newValue, bool isPositive);
	[Signal]
	public delegate void LivesChangedEventHandler(int newValue, bool isPositive);
	
	[Signal]
	public delegate void GameEndedEventHandler(Ending ending);
	
	private bool isCrucified =  false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _EnterTree()
	{
		base._EnterTree();
		Instance ??= this;
		// DummyAddDA(3);
	}

	public override void _Ready()
	{
		base._Ready();
		GameTimer.Start(GameDurationInSeconds);
		GameTimer.Timeout += GameTimerOnTimeout;
	}

	private void GameTimerOnTimeout()
	{
		var ending = CalculateEnding();
		EmitSignalGameEnded(ending);
	}

	public void ChangeDivineApproval(int amount)
	{
		DivineApproval += amount;
		DivineApproval = Math.Clamp(DivineApproval, DivineApprovalRange.X, DivineApprovalRange.Y);
		EmitSignalDivineApprovalChanged(DivineApproval, amount>0 ? true : false);
	}
	
	public void ChangeNotoriety(int amount)
	{
		Notoriety += amount;
		Notoriety = Math.Clamp(Notoriety, NotorietyRange.X, NotorietyRange.Y);
		EmitSignalNotorietyChanged(Notoriety, amount>0 ? true : false);
	}
	
	public void ChangeMana(int amount)
	{
		Mana += amount;
		Mana = Math.Clamp(Mana, ManaRange.X, ManaRange.Y);
		EmitSignalManaChanged(Mana, amount>0 ? true : false);
	}
	
	public void ChangeLives(int amount)
	{
		Lives += amount;
		Lives = Math.Clamp(Lives, LivesRange.X, LivesRange.Y);
		EmitSignalLivesChanged(Lives, amount>0 ? true : false);
		if (Lives == 0)
		{
			isCrucified = true;
			GameTimerOnTimeout();
		}
	}

	public async void DummyAddDA(float time)
	{
		await ToSignal(GetTree().CreateTimer(time), SceneTreeTimer.SignalName.Timeout);
		ChangeDivineApproval(5);
	}

	public void DummyAddMana()
	{
		ChangeMana(-5);
	}

	public Ending CalculateEnding()
	{
		if (isCrucified && DivineApproval >= DivineApprovalEndingTreshold)
		{
			return Ending.TrueEnding;
		}
		if (isCrucified && DivineApproval < DivineApprovalEndingTreshold)
		{
			return Ending.BadEnding;
		}
		if ( ! isCrucified && DivineApproval >= DivineApprovalEndingTreshold)
		{
			return Ending.GoldenEnding;
		}
		if ( ! isCrucified && DivineApproval < DivineApprovalEndingTreshold)
		{
			return Ending.JapaneseEnding;
		}
		
		return Ending.GameBrokenEnding;
	}
}
