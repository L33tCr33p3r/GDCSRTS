/// <summary>
/// Contains pathfinding Flowfields and their related methods 
/// </summary>
internal class FlowField
{
	// Properties (ALL OF THESE ARE PUBLIC)
	public float?[,] DistanceField { get; private set; }
	public List<Vector2i> FlowPath { get; private set; } = new();
	public List<Vector2> VectorPath { get; private set; } = new();

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="heightMap"></param>
	/// <param name="targetPosition"></param>
	/// <param name="startPosition"></param>
	/// <param name="maxSlope"></param>
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
		GenerateFlowPath(heightMap, (Vector2i) startPosition, maxSlope);
		GenerateVectorPath(heightMap, targetPosition, startPosition, maxSlope);
	}

	public Vector2i Sample(HeightMap heightMap, Vector2i samplePoint, float maxSlope)
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
					float distance = Math.Abs(di) != Math.Abs(dj) ? 1 : (float) Math.Sqrt(2);
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
	/// <param name="heightMap"></param>
	/// <param name="targetPoint"></param>
	/// <param name="MaxSlope"></param>
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
	/// <param name="heightMap"></param>
	/// <param name="startPoint"></param>
	/// <param name="maxSlope"></param>
	private void GenerateFlowPath(HeightMap heightMap, Vector2i startPoint, float maxSlope)
	{
		var currentNode = startPoint;
		FlowPath.Add(currentNode);
		while (DistanceField[currentNode.x, currentNode.y] != 0)
		{
			currentNode += Sample(heightMap, startPoint, maxSlope);
			FlowPath.Add(currentNode);
		}
	}

	/// <summary>
	/// Generates the final path to be followed by the unit from the DistanceField and NodePath
	/// </summary>
	/// <param name="heightMap"></param>
	/// <param name="targetPosition"></param>
	/// <param name="startPosition"></param>
	/// <param name="maxSlope"></param>
	private void GenerateVectorPath(HeightMap heightMap, Vector2 targetPosition, Vector2 startPosition, float maxSlope)
	{
		var currentVectorStart = startPosition;
		var currentVectorEnd = targetPosition;

		var currentFlowPathIndex = FlowPath.Count - 1;

		VectorPath.Add(currentVectorStart);

		while (currentVectorStart != currentVectorEnd)
		{
			if (IsPathAllowed(heightMap, currentVectorStart, currentVectorEnd, maxSlope))
			{
				VectorPath.Add(currentVectorEnd);

				currentVectorStart = currentVectorEnd;
				currentVectorEnd = targetPosition;
				currentFlowPathIndex = FlowPath.Count - 1;
			}
			else
			{
				currentVectorEnd = FlowPath[currentFlowPathIndex];
				currentFlowPathIndex--;
			}
		}
	}
	
	/// <summary>
	/// Checks if the path from startPos to endPos is valid
	/// </summary>
	/// <param name="heightMap"></param>
	/// <param name="startPos"></param>
	/// <param name="endPos"></param>
	/// <param name="maxSlope"></param>
	/// <returns></returns>
	private bool IsPathAllowed(HeightMap heightMap, Vector2 startPos, Vector2 endPos, float maxSlope)
	{
		var x1 = (int) startPos.x;
		var x2 = (int)endPos.x;

		var y1 = (int) startPos.y;
		var y2 = (int) endPos.y;

		var steep = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);

		if (steep)
		{
			(x1, x2) = (x2, x1);
			(y1, y2) = (y2, y1);
		}
		if (x1 > x2)
		{
			(x1, y2) = (y2, x1);
			(y1, x2) = (x2, y1);
		}

		int dx = x2 - x1;
		int dy = Math.Abs(y2 - y1);
		int error = dx / 2;
		int ystep = (y1 < y2) ? 1 : -1;
		int y = y1;

		var points = new List<Vector2i>();
		for (int x = x1; x <= x2; x++)
		{
			points.Add(new Vector2i((steep ? y : x), (steep ? x : y)));
			error -= dy;
			if (error < 0)
			{
				y += ystep;
				error += dx;
			}
		}

		for (int i = 0; i < points.Count - 1; i++)
		{

			var current = points[i];
			var next = points[i + 1];

			if (DistanceField[current.x, current.y] is null) return false;

			float distance = Math.Abs(current.x - next.x) != Math.Abs(current.y - next.y) ? 1 : (float) Math.Sqrt(2);
			float slope = (heightMap.Ground![next.x, next.y] - heightMap.Ground[current.x, current.y]) / distance;

			if (slope > maxSlope) return false;
		}

		return true;
	}

	public Vector2 VectorPathSample(Vector2 unitPosition, float unitSpeed)
	{
		
	}

	public float distancefromline(Vector2 start, Vector2 end, Vector2 point)
	{
		// get distance betweeen endpoints
		float factor = ((point - start).Dot(end - start)) / (end - start).LengthSquared();
		Vector2 closestpoint = start.Lerp(end, factor);
		return (point - closestpoint).Length();
	}
}
