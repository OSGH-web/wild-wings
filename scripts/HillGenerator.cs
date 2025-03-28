using Godot;
using System;
using System.Collections.Generic;

public class HillGenerator
{
	public Vector2 WindowSize;

	// hill generation parameters
	public int MaxHillKeyPoints = 20;
	public int KHillSegmentWidth = 10;
	public int MaxX;

	public int PaddingTop = 30;
	public int PaddingBottom = 30;

	private int _minDX;
	private int _rangeDX;

	private int _minDY;
	private int _rangeDY;

	private Random _rnd = new Random();

	private bool _initialized = false;

	public void Init()
	{
		WindowSize = new Vector2(1200, 800);
		_minDX = (int)WindowSize.X / 6;
		MaxX = (int)(_minDX * MaxHillKeyPoints) + (int)(WindowSize.X * 0.5);
		_rangeDX = (int)WindowSize.X / 16;
		_minDY = (int)WindowSize.Y / 6;
		_rangeDY = (int)WindowSize.Y / 2;

		_initialized = true;
	}

	public List<Vector2> GenerateHills()
	{
		if (!_initialized)
		{
			Init();
		}
		List<Vector2> points = new List<Vector2>();
		// First point: offscreen to the left
		int x = -_minDX;
		int y = (int)WindowSize.Y / 2;
		points.Add(new Vector2(x, y));

		// Second point: on-screen start
		x = 0;
		y = (int)WindowSize.Y / 2;
		points.Add(new Vector2(x, y));

		int direction = 1;

		for (int i = 0; i < MaxHillKeyPoints - 2; ++i)
		{
			int dX = _minDX + _rnd.Next(1, _rangeDX + 1);
			int dY = direction * (_minDY + _rnd.Next(_rangeDY + 1));
			direction *= -1;

			x += dX;
			y += dY;
			y = Math.Max(PaddingBottom, Math.Min((int)(WindowSize.Y - PaddingTop), y));

			points.Add(new Vector2(x, y));
		}

		return points;
	}
}
