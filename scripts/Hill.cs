using Godot;
using System;
using System.Collections.Generic;

public partial class Hill : Node2D
{
	// generators
	private Random _rnd = new Random();
	private HillGenerator _hg = new HillGenerator();

	// label to write diagnostic info to
	private DebugLabel _labelNode;

	// data -- static -- only modified on _Ready()
	// stores the top and bottom point for each bump in the hill
	private List<Vector2> _hillKeyPoints = new List<Vector2>();

	// state -- potentially modified at runtime
	private Ref<float> _XOffset = new Ref<float>(0);
	private Ref<float> _scrollRate = new Ref<float>(300);
	private int _normalScrollRate = 300;
	private int _fastScrollRate = 600;


	// draw params
	private int LineWidth = 10;
	private Color UpColor = Colors.Green;
	private Color DownColor = Colors.Red;
	private Color NeutralColor = Colors.Blue;

	public override void _Ready()
	{
		_hillKeyPoints = _hg.GenerateHills();

		_labelNode = GetParent().GetNode<DebugLabel>("DebugLabel");
		_labelNode.AddSlider("_scrollRate", _scrollRate, 0, _fastScrollRate);
		_labelNode.AddSlider("_XOffset", _XOffset, 0, _hg.MaxX);
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent.IsActionPressed("s_key"))
		{
			_scrollRate.Value = _fastScrollRate;
		}

		if (inputEvent.IsActionReleased("s_key"))
		{
			_scrollRate.Value = _normalScrollRate;
		}
	}

	public override void _Process(double delta)
	{

		_XOffset.Value += _scrollRate.Value * (float)delta;


		// for now, force the hill to loop if _XOffset gets too big
		if (_XOffset.Value > _hg.MaxX)
		{
			_XOffset.Value = 0;
		}

		_labelNode.SetVariable("_XOffset", _XOffset.Value);
		_labelNode.SetVariable("_scrollRate", _scrollRate.Value);

		QueueRedraw();
	}

	public override void _Draw()
	{
		// draw lines for top and bottom padding
		Vector2 topPaddingLeft = new Vector2(0, _hg.PaddingTop);
		Vector2 topPaddingRight = new Vector2(_hg.WindowSize.X, _hg.PaddingTop);
		Vector2 bottomPaddingLeft = new Vector2(0, _hg.WindowSize.Y - _hg.PaddingBottom);
		Vector2 bottomPaddingRight = new Vector2(_hg.WindowSize.X, _hg.WindowSize.Y - _hg.PaddingBottom);

		DrawLine(topPaddingLeft, topPaddingRight, NeutralColor, LineWidth);
		DrawLine(bottomPaddingLeft, bottomPaddingRight, NeutralColor, LineWidth);

		for (int i = 1; i < _hillKeyPoints.Count; i++)
		{
			// draws the straight line segments connecting the key points
			// DrawLine(_hillKeyPoints[i - 1], _hillKeyPoints[i], NeutralColor, LineWidth);

			Vector2 p0 = _hillKeyPoints[i - 1];
			p0.X -= _XOffset.Value;
			Vector2 p1 = _hillKeyPoints[i];
			p1.X -= _XOffset.Value;
			int hSegments = (int)(p1.X - p0.X) / _hg.KHillSegmentWidth;
			float dX = (p1.X - p0.X) / hSegments;
			float dA = MathF.PI / hSegments;
			float yMid = (p0.Y + p1.Y) / 2;
			float ampl = (p0.Y - p1.Y) / 2;

			Vector2 pt0, pt1;
			pt0 = p0;

			for (int j = 0; j < hSegments + 1; ++j)
			{
				pt1.X = p0.X + j * dX;
				pt1.Y = yMid + ampl * MathF.Cos(dA * j);

				Color segmentColor;
				if (ampl > 0)
				{
					segmentColor = UpColor;
				}
				else
				{
					segmentColor = DownColor;
				}

				DrawLine(pt0, pt1, segmentColor, LineWidth);

				pt0 = pt1;
			}
		}
	}
}
