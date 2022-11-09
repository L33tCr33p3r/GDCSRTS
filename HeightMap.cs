using Godot;
using System;

public class HeightMap
{
	public float[,] Ground { get; protected set; }
	public float[,] Water { get; protected set; }
	public HeightMap(int size, int seed, float scale = 1.0f)
	{
		Ground = new float[size, size];
		Water = new float[size, size];

		FastNoiseLite noise = new();
		noise.Seed = seed;

		for (int x = 0; x < size; x++)
		{
			for (int y = 0; y < size; y++)
			{
				Ground[x, y] = noise.GetNoise2d(x * scale, y * scale);
			}
		}
	}
}
