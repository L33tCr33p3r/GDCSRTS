// For Orders where a unit is moving to a point in space
internal record MoveOrder : Order
{
	// Properties of a MoveOrder
	public Vector3 MoveTarg { get; init; }
	public FlowField MoveFlowField { get; init; }

	// Constructor
	public MoveOrder(HeightMap heightMap, Vector3 moveTarg, float maxSlope)
	{
		MoveTarg = moveTarg;
		// MoveFlowField = new(heightMap, moveTarg, maxSlope); // TODO: This constructor needs moveTarg as a Vector2i...
	}
}
