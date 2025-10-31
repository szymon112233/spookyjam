using Godot;
using System;

public partial class MusicManager : AudioStreamPlayer
{
	[Export]
	int notorietyChangeThreshold = 50;
	
	[Export] 
	private AudioStream[] notorietyTracks;
	private int currentNotorietyTrack = 0;
	
	[Export]
	private AudioStream[] endingTracks;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		if(notorietyTracks != null && notorietyTracks.Length > 0)
		{
			Stream = notorietyTracks[currentNotorietyTrack];
			Play();
		}

		GameManager.Instance.GameEnded += GameEndingMusic;

	}

	private void IncreaseNotorietyTrack()
	{
		currentNotorietyTrack++;
		if (currentNotorietyTrack < notorietyTracks.Length)
		{
			Stop();
			Stream = notorietyTracks[currentNotorietyTrack];
			Play();
		}
	}
	
	private void GameEndingMusic(GameManager.Ending ending)
	{
		Stop();
		int indexNr = (int)ending;
		if (indexNr >= endingTracks.Length)
		{
			throw new System.IndexOutOfRangeException("This endings track has not been added to the array I guess");
		}
		Stream = endingTracks[indexNr];
		Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(GameManager.Instance == null)
		{
			QueueFree();
			throw new NullReferenceException("GameManager not initialized");
		}
		if (GameManager.Instance.Notoriety > notorietyChangeThreshold)
		{
			IncreaseNotorietyTrack();
		}
	}
}
