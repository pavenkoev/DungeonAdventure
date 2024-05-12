using Godot;
using System;

public class ValueLabel : Label
{
	public override void _Ready()
	{
		// I'm assuming that the parent node has a property named `Value`.
		// You must ensure that the parent node has a public float or int property named 'Value'
		if (GetParent() is IHasValue parent)
		{
			this.Text = parent.Value.ToString() + "%";
		}
	}

	// This function should be connected to a signal from a HScrollBar in the Godot Editor or programmatically in _Ready().
	public void OnHScrollBarValueChanged(float value)
	{
		this.Text = value.ToString() + "%";
	}
}

public interface IHasValue
{
	float Value { get; }
}
