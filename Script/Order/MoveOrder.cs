using Godot;

// For Orders where a unit is moving to a point in space
internal record MoveOrder : Order
{
	// Properties of a MoveOrder
	public Vector2 MoveTarg { get; init; }
	public FlowField MoveFlowField { get; init; }

	// Constructor
	public MoveOrder(Vector2 moveTarg)
	{
		MoveTarg = moveTarg;
		MoveFlowField = new(); // TODO: This constructor should take the moveTarg as well
	}
}
