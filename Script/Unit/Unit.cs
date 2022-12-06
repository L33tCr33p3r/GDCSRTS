// Base class for all Units
internal partial class Unit : Node3D
{
	// Fields (ALL OF THESE ARE PRIVATE)
	private readonly Guid _id = Guid.NewGuid(); // To be used for syncing with clients
	private int _health;
	private readonly Queue<Order> _orders = new();
	private Order? _goal;
	private FireStance _stance;
	private float _viewRange;
	private float _firingRange;

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
		RunAI(delta);
	}

	// Called when on the right client.
	private void RunAI(double delta)
	{
		_goal = EvaluateGoal();
		PerformGoal(delta);
	}

	// Evaluates Order queue to decide what the unit's current goal is.
	private Order? EvaluateGoal()
	{
		while (true)
		{
			if (_orders.Count != 0)
			{
				if (_orders.Peek().IsDeleted) _orders.Dequeue();
				else if (_orders.Peek() is MoveOrder move) // Checks if currentOrder is a MoveOrder
				{
					if (!IsAUnitNearPoint(move.MoveTarget, 1)) // Check if there is something at the order's movetarget already
					{
						return move; // Copies curentOrder to _goal if there is nothing at the movetarget
					}
					else if (AmINearPoint(move.MoveTarget, 1)) // Check if the current unit is the thing at the MoveTarget
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
				else if (_orders.Peek() is PositionAttackOrder positionAttack)
				{
					if (AmINearPoint(positionAttack.AttackTarget, _firingRange)) // checks if the unit is within range
					{

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
	private void PerformGoal(double delta)
	{
		if (_goal is MoveOrder move)
		{

		}
		else if (_goal is PositionAttackOrder positionAttack)
		{
			var aimed = true;

			foreach (Node child in GetChildren())
			{
				if (child is TurretHardpoint turretHardpoint)
				{
					if (!turretHardpoint.Aim(positionAttack.AttackTarget, delta)) aimed = false;
				}
			}

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
