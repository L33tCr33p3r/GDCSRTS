// Base class for all Units
internal abstract partial class Unit : Node3D
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
	private Queue<Order> _orders = new();
	private Order? _goal;
	private FireStance _stance;
	private Unit? _target;
	private Vector2? _targetLastSeen;
	private float _viewRange;

	// Properties (ALL OF THESE ARE PUBLIC)
	public Guid Id { get { return _id; } }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
				if (_orders.Peek().GetType() == typeof(MoveOrder)) // Checks if currentOrder is a MoveOrder
				{
					var currentMove = (MoveOrder)_orders.Peek();

					if (true) // TODO: Check if there is something at the order's movetarget already
					{
						return currentMove; // Copies curentOrder to _goal if there is nothing at the movetarget
					}
					else if (true) // TODO: Check if the current unit is the thing at the MoveTarget
					{
						_orders.Dequeue();
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
		if (_goal != null) // If the unit has a goal, tries to execute it.
		{
			
		}
		else // If the unit has no goal, decides what else to do
		{
			
		}
	}
}
