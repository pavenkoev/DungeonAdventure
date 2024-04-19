using Godot;

namespace DungeonAdventure;

public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 128.0f;
	
	public override void _Process(double delta)
	{
		Vector2 direction = new Vector2(0, 0);

		if (Input.IsActionPressed("move_up"))
		{
			direction += new Vector2(0, -1);
		}

		if (Input.IsActionPressed("move_down"))
		{
			direction += new Vector2(0, 1);
		}
		
		if (Input.IsActionPressed("move_left"))
		{
			direction += new Vector2(-1, 0);
		}
		
		if (Input.IsActionPressed("move_right"))
		{
			direction += new Vector2(1, 0);
		}

		direction = direction.Normalized();
		
		Velocity = direction * Speed;
		MoveAndSlide();
	}
}
