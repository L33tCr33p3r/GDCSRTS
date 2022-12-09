// For Orders where a unit is moving to a point in space
internal record MoveOrder : Order
{
	// Properties of a MoveOrder
	public FlowField MoveFlowField { get; init; }
	public Vector2 MoveTarget { get; init; }

	// Constructor
	public MoveOrder(HeightMap heightMap, Vector2 moveTarget, Vector2 currentPosition, float maxSlope)
	{
		MoveTarget = moveTarget;
		MoveFlowField = new FlowField(heightMap, moveTarget, currentPosition, maxSlope);
	}
}
