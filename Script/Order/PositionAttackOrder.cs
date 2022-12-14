// For orders where a unit targets a position on the ground, rather than a unit
internal record PositionAttackOrder : Order
{
	// Properties of a PositionAttackOrder
	public Vector3 AttackTarget { get; init; }

	// Constructor
	public PositionAttackOrder(Vector3 attackTarg)
	{
		AttackTarget = attackTarg;
	}
}
