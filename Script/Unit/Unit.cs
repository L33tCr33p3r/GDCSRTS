// Base class for all Units
internal partial class Unit : Node3D
{
	public enum FireStance
	{
		/// <summary> Run away from enemies </summary>
		Flee,
		/// <summary> Do not attack enemies </summary>
		Hold,
		/// <summary> Attack enemies that attacked first </summary>
		Return,
		/// <summary> Attack any spotted enemies </summary>
		AtWill
	}

	// Fields (ALL OF THESE ARE PRIVATE)
	private readonly Guid _id = Guid.NewGuid(); // To be used for syncing with clients
	private int _health;
	private readonly Queue<Order> _orders = new();
	private Order? _goal;
	private FireStance _stance;
	private float _viewRange;

	// Properties (ALL OF THESE ARE PUBLIC)
	public Guid Id { get { return _id; } }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup("Units");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		RunAI();
	}

	// Called when on the right client.
	private void RunAI()
	{
		_goal = EvaluateGoal();
		PerformGoal();
	}

	// Evaluates Order queue to decide what the unit's current goal is.
	private Order? EvaluateGoal()
	{
		while (true)
		{
			if (_orders.Count != 0)
			{
				if (_orders.Peek().IsDeleted) _orders.Dequeue();
				else if (_orders.Peek().GetType() == typeof(MoveOrder)) // Checks if currentOrder is a MoveOrder
				{
					var currentMove = (MoveOrder)_orders.Peek();

					if (!IsAUnitNearPoint(currentMove.MoveTarget, 1)) // Check if there is something at the order's movetarget already
					{
						return currentMove; // Copies curentOrder to _goal if there is nothing at the movetarget
					}
					else if (AmINearPoint(currentMove.MoveTarget, 1)) // Check if the current unit is the thing at the MoveTarget
					{
						_orders.Dequeue(); // Remove the order if the current unit is the thing at the point
					}
					else // TODO: Find where else to pathfind to
					{
						// Find the nearest unoccupied space the unit could move to. If
						// the check to see if the next place is unoccupied says the unit
						// is already there, it means you should remove the move order, as
						// the unit has already gotten as close as it can. If it instead
						// finds an unoccupied space first, check if _goal has a MoveOrder
						// with a matching (or closely matching) MoveTarg. If it does,
						// return _goal. If it doesn't, generate a full new MoveOrder and
						// return that.
					}
				}
			}
			else
			{
				return null;
			}
		}

	}

	// Tries to complete whatever the unit's current goal is. 
	private void PerformGoal()
	{
		if (_goal is MoveOrder order)
		{

		}
	}

	// Check if any unit is within a certain radius of a given point
	private bool IsAUnitNearPoint(Vector3 checkPoint, double tolerance)
	{
		var isAUnitNearPoint = false;
		foreach (Node node in GetTree().GetNodesInGroup("Units"))
		{
			if (typeof(Unit).IsAssignableFrom(node.GetType()))
			{
				var unit = (Unit)node;
				if (unit.AmINearPoint(checkPoint, tolerance)) isAUnitNearPoint = true;
			}
		}
		return isAUnitNearPoint;
	}

	// Check if the unit is within a certain radius of a given point
	private bool AmINearPoint(Vector3 checkPoint, double tolerance)
	{
		return (GlobalPosition - checkPoint).Length() <= tolerance;
	}
}
