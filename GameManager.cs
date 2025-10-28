using Godot;
using System;

public partial class GameManager : Node
{
	public static GameManager Instance;
	
	[Export]
	public int DivineApproval = 0;
	
	[Export]
	public Vector2I DivineApprovalRange = new Vector2I(0, 100);
	
	[Export]
	public int Notoriety = 0;
	
	[Export]
	public Vector2I NotorietyRange = new Vector2I(0, 100);
	
	[Export]
	public int Mana = 100;
	
	[Export]
	public Vector2I ManaRange = new Vector2I(0, 100);
	
	[Signal]
	public delegate void DivineApprovalChangedEventHandler(int newValue);
	[Signal]
	public delegate void NotorietyChangedEventHandler(int newValue);
	[Signal]
	public delegate void ManaChangedEventHandler(int newValue);
	
	// Called when the node enters the scene tree for the first time.
	public override void _EnterTree()
	{
		base._EnterTree();
		Instance ??= this;
		// DummyAddDA(3);
	}

	public void ChangeDivineApproval(int amount)
	{
		DivineApproval += amount;
		DivineApproval = Math.Clamp(DivineApproval, DivineApprovalRange.X, DivineApprovalRange.Y);
		EmitSignalDivineApprovalChanged(DivineApproval);
	}
	
	public void ChangeNotoriety(int amount)
	{
		Notoriety += amount;
		Notoriety = Math.Clamp(Notoriety, NotorietyRange.X, NotorietyRange.Y);
		EmitSignalNotorietyChanged(Notoriety);
	}
	
	public void ChangeMana(int amount)
	{
		Mana += amount;
		Mana = Math.Clamp(Mana, ManaRange.X, ManaRange.Y);
		EmitSignalManaChanged(Mana);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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
}
