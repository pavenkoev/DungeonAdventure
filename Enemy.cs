using Godot;
using Godot.Collections;

namespace DungeonAdventure;

public partial class Enemy : Character
{
	[Export] private float _chaseDistance = 100.0f;

	private Player FindPlayer()
	{
		Array<Node> nodes = GetTree().GetNodesInGroup("player");
		return nodes[0] as Player;
	}

	public override void _Process(double delta)
	{
		Player player = FindPlayer();
		Vector2 direction = new Vector2(0, 0);

		if (player.Position.DistanceTo(Position) <= _chaseDistance)
			direction = (player.Position - Position).Normalized();

		Velocity = direction * _speed;

		UpdateAnimation(Velocity);

		MoveAndSlide();
		
		ProcessAttack(player);
	}

	private AttackSide SelectAttackSide(Vector2 vector)
	{
		if (vector.Y >= vector.X)
		{
			if (vector.Y >= -vector.X)
				return AttackSide.Down;
			return AttackSide.Left;
		}

		if (vector.Y >= -vector.X)
			return AttackSide.Right;
		return AttackSide.Up;
	}

	private void ProcessAttack(Player player)
	{
		Vector2 vector = player.Position - Position;

		if (vector.Length() <= _weapon.AttackRange)
		{
			AttackSide attackSide = SelectAttackSide(vector);
			SetWeaponAttackSide(attackSide);

			if (_weapon.CanAttack())
				_weapon.Attack();
		}
	}
}
