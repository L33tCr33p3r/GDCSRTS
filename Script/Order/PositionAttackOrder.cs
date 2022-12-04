// For orders where a unit targets a position on the ground, rather than a unit
internal class PositionAttackOrder
{
	// Properties of a PositionAttackOrder
	public Vector3 AttackTarg { get; init; }

	// Constructor
	public PositionAttackOrder(Vector3 attackTarg)
	{
		AttackTarg = attackTarg;
	}
}
