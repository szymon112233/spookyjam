using Godot;

public partial class DefaultAnimationPlayer : Node
{
	[Export]
	private string AnimationName = "default";

	private AnimationPlayer _animationPlayer;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		if (_animationPlayer != null && _animationPlayer.HasAnimation(AnimationName))
		{
			float animationLength = _animationPlayer.GetAnimation(AnimationName).Length;
			float randomStartTime = GD.Randf() * animationLength;
			_animationPlayer.Play(AnimationName);
			_animationPlayer.Seek(randomStartTime, true);
		}
		else
		{
			GD.PrintErr($"Animation '{AnimationName}' not found or AnimationPlayer is missing.");
		}
	}
}