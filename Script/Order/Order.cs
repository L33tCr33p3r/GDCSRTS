// Abstract Order class, all concrete Orders inherit from this
internal abstract record Order
{
	// Properties of all Orders
	public Guid OrderId { get; init; } = new(); // Used for syncing between clients
	public bool IsDeleted { get; init; } = false; // Order will be removed without any further checks if this becomes true
}
