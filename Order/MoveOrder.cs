using Godot;

// Concrete type of all Movevent orders
internal record MoveOrder : Order
{
	// Properties of a MoveOrder
	public Vector3 MoveTarg { get; init; }
	public FlowField MoveFlowField { get; init; }

	// Constructor
	public MoveOrder(Vector3 moveTarg)
	{
		MoveTarg = moveTarg;
		MoveFlowField = new(); // TODO: This constructor should take the moveTarg as well
	}
}
