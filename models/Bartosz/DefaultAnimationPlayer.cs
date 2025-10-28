using Godot;

public partial class DefaultAnimationPlayer : Node
{
	[Export]
	private string AnimationName = "default";

	private AnimationPlayer _animationPlayer;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		if (_animationPlayer == null)
		{
			GD.PrintErr($"AnimationPlayer is missing.");
			
			return;
		}
		
		if (_animationPlayer.HasAnimation(AnimationName) == false)
		{
			GD.PrintErr($"Animation '{AnimationName}' is missing.");
			
			return;
		}
		
		float animationLength = _animationPlayer.GetAnimation(AnimationName).Length;
		float randomStartTime = GD.Randf() * animationLength;
		_animationPlayer.Play(AnimationName);
		_animationPlayer.Seek(randomStartTime, true);
	}
}