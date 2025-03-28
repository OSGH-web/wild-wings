using Godot;
using System;
using System.Collections.Generic;
using System.Text;

public class Ref<T>
{
	public T Value;
	public Ref(T value) => Value = value;
}

public partial class DebugLabel : VBoxContainer
{
	private Dictionary<string, HSlider> _sliders = new Dictionary<string, HSlider>();
	private Dictionary<string, Label> _labels = new Dictionary<string, Label>();

	public override void _Ready()
	{
	}

	public void SetVariable(string key, float value)
	{
		if (_sliders.ContainsKey(key))
		{
			_sliders[key].Value = value;
			_labels[key].Text = $"{key}: {value:0.00}";
		}
	}
	public void AddSlider(string key, Ref<float> variable, float min = 0, float max = 100)
	{
		var hbox = new HBoxContainer();

		var label = new Label();
		label.Text = $"{key}: {variable.Value:0.00}";
		label.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;

		var slider = new HSlider();
		slider.MinValue = min;
		slider.MaxValue = max;
		slider.Value = variable.Value;
		slider.Step = 0.01f;
		slider.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
		slider.MouseFilter = Control.MouseFilterEnum.Stop;

		slider.ValueChanged += (newValue) =>
		{
			variable.Value = (float)newValue;
			label.Text = $"{key}: {variable.Value:0.00}";
		};

		hbox.AddChild(label);
		hbox.AddChild(slider);

		AddChild(hbox);
		_sliders[key] = slider;
		_labels[key] = label;
	}
}
