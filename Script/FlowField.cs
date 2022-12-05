// Contains pathfinding Flowfields and their related methods 
internal class FlowField
{
    // Fields (ALL OF THESE ARE PRIVATE)
    public float?[,] DistanceField { get; private set; }
	private HeightMap _terrain { get; init; }
	public Vector2i Target { get; init; }
	public float MaxSlope { get; init; }

	// Constructor
	public FlowField(HeightMap heightMap, Vector2i target, float maxSlope)
	{
		_terrain = heightMap;
		Target = target;
		MaxSlope = maxSlope;

		// instantiate and fill the array with nulls:
		DistanceField = new float?[_terrain.Ground.GetLength(0), _terrain.Ground.GetLength(1)];
		for (int i = 0; i < DistanceField.GetLength(0); i++)
		{
			for (int j = 0; j < DistanceField.GetLength(1); j++)
			{
				DistanceField[i, j] = null;
			}
		}

		GenerateDistanceField();
	}

	public Vector2i Sample(Vector2i samplePoint)
	{
		int i = samplePoint.x;
		int j = samplePoint.y;

		float? minCost = null;
		var minPoint = new Vector2i(0, 0);
		foreach (int di in new int[] {-1, 0, 1})
		{
			foreach (int dj in new int[] {-1, 0, 1})
			{
				int iprime = i + di;
				int jprime = j + dj;
				if (iprime < 0 || jprime < 0 || iprime >= DistanceField.GetLength(0) || jprime >= DistanceField.GetLength(1))
				{
					continue;
				}
				if (DistanceField[iprime, jprime] != null)
				{
					float sampled = (float)DistanceField[iprime, jprime]!;
					float distance = Math.Abs(di) != Math.Abs(dj) ? 1 : Mathf.Sqrt(2);
					float slope = (_terrain.Ground[iprime, jprime] - _terrain.Ground[i, j]) / distance;
					if ((minCost == null || sampled < minCost) && slope < MaxSlope)
					{
						minCost = sampled;
						minPoint = new Vector2i(di, dj);
					}
				}
			}
		}
		return minPoint;
	}

	private void GenerateDistanceField()
	{
		GD.Print("DF start");
		DistanceField[Target.x, Target.y] = 0;
		bool fieldChanged;
		do 
		{
			fieldChanged = false;
			// if weird behavior is happening, operate on a temp copy of DistanceField in each loop
			for (int i = 0; i < DistanceField.GetLength(0); i++)
			{
				for (int j = 0; j < DistanceField.GetLength(1); j++)
				{
					float? value = DistanceField[i, j];
					if (value == null) // if weird behavior is happening, look here
					{
						float? minCost = null;
						float? cost = null;
						foreach (int di in new int[] {-1, 0, 1})
						{
							foreach (int dj in new int[] {-1, 0, 1})
							{
								int iprime = i + di;
								int jprime = j + dj;
								if ((di == 0 && dj == 0) || iprime < 0 || jprime < 0 || iprime >= DistanceField.GetLength(0) || jprime >= DistanceField.GetLength(1))
								{
									continue;
								}
								if (DistanceField[iprime, jprime] != null)
								{
									float distance = di == 0 || dj == 0 ? 1 : Mathf.Sqrt(2); // gets the actual distance to the square rather than manhattan distance
									float slope = (_terrain.Ground[i, j] - _terrain.Ground[iprime, jprime]) / distance;
									float thisCost = (float)DistanceField[iprime, jprime]! + distance + slope;
									if ((minCost == null || thisCost < minCost) && slope < MaxSlope)
									{
										minCost = thisCost;
										cost = thisCost;
										fieldChanged = true;
									}
								}
							}
						}
						DistanceField[i, j] = cost;
					}
				}
			}
		} 
		while (fieldChanged);
		GD.Print("DF end");
	}
}
