// Contains pathfinding Flowfields and their related methods 
internal class FlowField
{
	// Properties (ALL OF THESE ARE PUBLIC)
	public float?[,] DistanceField { get; private set; }
	public List<Vector2i> NodePath { get; private set; } = new();
	public List<Vector2> VectorPath { get; private set; } = new();

	// Constructor
	public FlowField(HeightMap heightMap, Vector2 target, Vector2 startPosition, float maxSlope)
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

		GenerateDistanceField(heightMap, (Vector2i) target, maxSlope);
		GenerateNodePath(heightMap, (Vector2i) startPosition, maxSlope);
		GenerateVectorPath();
	}

	public Vector2i Sample(HeightMap heightMap, Vector2i samplePoint, float maxSlope) // TODO: fix random spots of null
	{
		// TODO: make this interpolate the input and output vectors
		int i = samplePoint.x;
		int j = samplePoint.y;

		float? minCost = null;
		var minPoint = new Vector2(0, 0);
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
					float slope = (Terrain.Ground[iprime, jprime] - Terrain.Ground[i, j]) / distance;
					if ((minCost == null || sampled <= minCost) && slope < MaxSlope)
					{
						minCost = sampled;
						minPoint = new Vector2(di, dj);
					}
				}
			}
		}
		return minPoint;
	}

	// public Vector2 Sample(Vector2i samplePoint)
	// {
	// 	// TODO: make this interpolate the input and output vectors
	// 	int i = samplePoint.x;
	// 	int j = samplePoint.y;

	// 	Vector2 result = new Vector2(0, 0);
	// 	foreach (int di in new int[] {-1, 0, 1})
	// 	{
	// 		foreach (int dj in new int[] {-1, 0, 1})
	// 		{
	// 			int iprime = i + di;
	// 			int jprime = j + dj;
	// 			if (iprime < 0 || jprime < 0 || iprime >= DistanceField.GetLength(0) || jprime >= DistanceField.GetLength(1))
	// 			{
	// 				continue;
	// 			}
	// 			if (DistanceField[i, j] != null && DistanceField[iprime, jprime] != null)
	// 			{
	// 				float distance = Math.Abs(di) != Math.Abs(dj) ? 1 : Mathf.Sqrt(2);
	// 				float slope = (Terrain.Ground[iprime, jprime] - Terrain.Ground[i, j]) / distance;
	// 				float gradient = ((float)DistanceField[i, j] - (float)DistanceField[iprime, jprime]) / distance;
	// 				if (slope < MaxSlope)
	// 				{
	// 					result += new Vector2(di, dj) * gradient;
	// 				}
	// 			}
	// 		}
	// 	}
	// 	return result.Normalized();
	// }

	/// <summary>
	/// Generates the distance field that constistutes the actual flowfield
	/// </summary>
	private void GenerateDistanceField(HeightMap Terrain, Vector2i Target, float MaxSlope)
	{
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
									float slope = (Terrain.Ground![i, j] - Terrain.Ground[iprime, jprime]) / distance;
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
	private void GenerateNodePath(HeightMap heightMap, Vector2i startPos, float maxSlope)
	{
		var currentNode = startPos;
		NodePath.Add(currentNode);
		while (DistanceField[currentNode.x, currentNode.y] != 0)
		{
			currentNode += Sample(heightMap, startPos, maxSlope);
			NodePath.Add(currentNode);
		}
	}

	/// <summary>
	/// Generates the final path to be followed by the unit from the DistanceField and NodePath
	/// </summary>
	private void GenerateVectorPath()
	{

	}
}
