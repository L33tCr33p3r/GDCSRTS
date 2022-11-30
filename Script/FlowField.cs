// Contains pathfinding Flowfields and their related methods 
internal class FlowField
{
	public Vector2[,]? Field { get; private set; }
	public Vector2i Target { get; init; }
	public HeightMap Terrain { get; init; }

	private int[,] _distanceField { get; set; }
	private int[,] _steepnessField { get; set; }

	public FlowField(HeightMap heightMap) 
	{
		Terrain = heightMap;
	}
	
	public void GenerateFlowField(float steepnessWeight = 1)
	{

	}

	public void GenerateDistanceField()
	{

	}
}
