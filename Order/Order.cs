// Abstract Order class, all concrete Orders inherit from this
internal abstract record Order
{
	// Properties of all Orders
	public Guid OrderID { get; set; } = new();
}
