using Godot;

// Concrete type of all Movevent orders
internal record MoveOrder : Order
{
	Vector3 MoveTarg;
	FlowField MoveFlowField;
}
