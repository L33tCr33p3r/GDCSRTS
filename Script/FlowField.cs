// Contains pathfinding Flowfields and their related methods 
internal class FlowField
{
	// Properties (ALL OF THESE ARE PUBLIC)
	public float?[,] DistanceField { get; private set; }
	public List<Vector2i> NodePath { get; private set; } = new();
	public List<Vector2> VectorPath { get; private set; } = new();

	// Constructor
	public FlowField(HeightMap heightMap, Vector2 targetPosition, Vector2 startPosition, float maxSlope)
	{
		// instantiate and fill the array with nulls:
		DistanceField = new float?[heightMap.Ground!.GetLength(0), heightMap.Ground.GetLength(1)];
		for (int i = 0; i < DistanceField.GetLength(0); i++)
		{
			for (int j = 0; j < DistanceField.GetLength(1); j++)
			{
				DistanceField[i, j] = null;
			}
		}

		GenerateDistanceField(heightMap, (Vector2i) targetPosition, maxSlope);
		GenerateNodePath(heightMap, (Vector2i) startPosition, maxSlope);
		GenerateVectorPath(heightMap, targetPosition, startPosition, maxSlope);
	}

	public Vector2i Sample(HeightMap heightMap, Vector2i samplePoint, float maxSlope) // TODO: fix random spots of null
	{
		// TODO: make this interpolate the input and output vectors
		int i = samplePoint.x;
		int j = samplePoint.y;

		float? minCost = null;
		var minPoint = new Vector2i(0, 0);
		foreach (int di in new int[] {-1, 0, 1})
		{
			foreach (int dj in new int[] { -1, 0, 1 })
			{
				int iprime = i + di;
				int jprime = j + dj;
				if ((di == 0 && dj == 0) || iprime < 0 || jprime < 0 || iprime >= DistanceField.GetLength(0) || jprime >= DistanceField.GetLength(1))
				{
					continue;
				}
				if (DistanceField[iprime, jprime] != null)
				{
					float sampled = (float)DistanceField[iprime, jprime]!;
					float distance = Math.Abs(di) != Math.Abs(dj) ? 1 : Mathf.Sqrt(2);
					float slope = (heightMap.Ground![iprime, jprime] - heightMap.Ground[i, j]) / distance;
					if ((minCost == null || sampled <= minCost) && slope < maxSlope)
					{
						minCost = sampled;
						minPoint = new Vector2i(di, dj);
					}
				}
			}
		}
		return minPoint;
	}



	/// <summary>
	/// Generates the distance field that constistutes the actual flowfield
	/// </summary>
	private void GenerateDistanceField(HeightMap heightMap, Vector2i targetPoint, float MaxSlope)
	{
		DistanceField[targetPoint.x, targetPoint.y] = 0;
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
									float slope = (heightMap.Ground![i, j] - heightMap.Ground[iprime, jprime]) / distance;
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
	}

	/// <summary>
	/// Generates the node path from the unit's initial position to the path's end
	/// </summary>
	private void GenerateNodePath(HeightMap heightMap, Vector2i startPoint, float maxSlope)
	{
		var currentNode = startPoint;
		NodePath.Add(currentNode);
		while (DistanceField[currentNode.x, currentNode.y] != 0)
		{
			currentNode += Sample(heightMap, startPoint, maxSlope);
			NodePath.Add(currentNode);
		}
	}

	/// <summary>
	/// Generates the final path to be followed by the unit from the DistanceField and NodePath
	/// </summary>
	private void GenerateVectorPath(HeightMap heightMap, Vector2 targetPosition, Vector2 startPosition, float maxSlope)
	{
		var currentVectorStart = startPosition;
		var currentVectorEnd = targetPosition;
		

	}
}
