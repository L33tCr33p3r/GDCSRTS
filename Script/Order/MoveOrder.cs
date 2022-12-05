// For Orders where a unit is moving to a point in space
internal record MoveOrder : Order
{
	// Properties of a MoveOrder
	public FlowField MoveFlowField { get; init; }

	// Constructor
	public MoveOrder(HeightMap heightMap, Vector2 moveTarget, float maxSlope)
	{
		MoveFlowField = new FlowField(heightMap, (Vector2i)moveTarget, maxSlope);
	}
}
