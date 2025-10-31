using System;
using System.Data;
using Godot;

public partial class DefaultAnimationPlayer : Node3D
{
	[Export]
	public string AnimationName_Idle = "default";

	[Export]
	public string AnimationName_Walk;

	[Export]
	public string AnimationName_Dance;

	[Export]
	public string AnimationName_Sick;

	[Export]
	public string AnimationName_Attack;

	[Export]
	public string DefaultAnimation;

	[Export]
	public float Sick_Y_offset;

	public AnimationPlayer AnimationPlayer;

	[Export]
	public bool ShouldBeAWoman;

	[Export]
	public Node3D WomanObject;

	public bool IsAnimationPlaying => AnimationPlayer.IsPlaying();

	public override void _Ready()
	{
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		if (AnimationPlayer == null)
		{
			GD.PrintErr($"AnimationPlayer is missing.");

			return;
		}

		if (AnimationPlayer.HasAnimation(DefaultAnimation) == false)
		{
			GD.PrintErr($"Animation '{DefaultAnimation}' is missing.");

			return;
		}

		PlayAnimationWithKey(DefaultAnimation);

		if (ShouldBeAWoman == false)
		{
			WomanObject.Visible = false;
		}
	}
	
	public void PlayAnimationWithKey(string key)
	{
		if (AnimationPlayer.CurrentAnimation == key)
			return;

		if (key == AnimationName_Sick)
		{
			Position = new Vector3(0, Sick_Y_offset, 0);
		}
		else
		{
			Position = Vector3.Zero;
		}

		if (key == AnimationName_Attack)
		{
			AnimationPlayer.SpeedScale = 2;
		}
		else
		{
			AnimationPlayer.SpeedScale = 1;	
        }

		float animationLength = AnimationPlayer.GetAnimation(key).Length;
		float randomStartTime = GD.Randf() * animationLength;

		AnimationPlayer.Play(key);
		AnimationPlayer.Seek(randomStartTime, true);
    }
}