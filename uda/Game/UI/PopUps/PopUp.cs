using Godot;
using System;

public partial class PopUp : TextureRect
{
	private string _text;
	public void OnBackPressed()
	{
		Visible = false;
	}
}
