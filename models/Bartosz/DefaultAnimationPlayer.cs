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
			_animationPlayer.Play(AnimationName);
		}
		else
		{
			GD.PrintErr($"Animation '{AnimationName}' not found or AnimationPlayer is missing.");
		}
	}
}