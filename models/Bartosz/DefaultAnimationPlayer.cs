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

	private AnimationPlayer _animationPlayer;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		if (_animationPlayer == null)
		{
			GD.PrintErr($"AnimationPlayer is missing.");

			return;
		}

		if (_animationPlayer.HasAnimation(DefaultAnimation) == false)
		{
			GD.PrintErr($"Animation '{DefaultAnimation}' is missing.");

			return;
		}

		PlayAnimationWithKey(DefaultAnimation);
	}
	
	public void PlayAnimationWithKey(string key)
	{
		if (_animationPlayer.CurrentAnimation == key)
			return;

		if (key == AnimationName_Sick)
		{
			Position = new Vector3(0, Sick_Y_offset, 0);
		}
		else
        {
			Position = Vector3.Zero;
        }

		float animationLength = _animationPlayer.GetAnimation(key).Length;
		float randomStartTime = GD.Randf() * animationLength;

		_animationPlayer.Play(key);
		_animationPlayer.Seek(randomStartTime, true);
    }
}