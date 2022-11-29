using Godot;

// Concrete type of all Movevent orders
internal record MoveOrder : Order
{
	// Properties of a MoveOrder
	Vector3 MoveTarg { get; init; }
	FlowField MoveFlowField { get; init; }

	// Constructor
	MoveOrder(Vector3 moveTarg)
	{
		MoveTarg = moveTarg;
		MoveFlowField = new(); // TODO: This constructor should take the moveTarg as well
	}
}
