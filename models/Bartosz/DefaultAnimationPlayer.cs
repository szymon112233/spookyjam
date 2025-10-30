using System.Data;
using Godot;

public partial class DefaultAnimationPlayer : Node
{
	[Export]
	public string AnimationName_Idle = "default";

	[Export]
	public string AnimationName_Walk;

	[Export]
	public string AnimationName_Dance;

	[Export]
	public string DefaultAnimation;

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


		float animationLength = _animationPlayer.GetAnimation(key).Length;
		float randomStartTime = GD.Randf() * animationLength;

		_animationPlayer.Play(key);
		_animationPlayer.Seek(randomStartTime, true);
    }
}