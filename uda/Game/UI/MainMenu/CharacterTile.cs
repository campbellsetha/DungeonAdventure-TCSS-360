using Godot;
using UDA.Game.Resources;

namespace UDA.Game.UI;

[GlobalClass]
<<<<<<< Updated upstream
public partial class CharacterTile : PanelContainer
{
	private Label _label;
	private AnimatedSprite2D _sprite;
	private Tween _flashingBorder;
	private StyleBoxFlat _borderStyle;
	private Color _color1 = new Color(1, 1, 1, 0.4f);
	private Color _color2 = new Color(1, 1, 1, 1.0f);
	private PlayerClassInfo _classInfo;

	public bool IsSelected { get; private set; } = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("VBoxContainer/AnimatedSprite");
		if (_sprite.SpriteFrames != null && _sprite.SpriteFrames.HasAnimation("Idle"))
			_sprite.Play("Idle");
		
		_label = GetNode<Label>("VBoxContainer/Label");
		
		_flashingBorder = GetTree().CreateTween();

		_borderStyle = new StyleBoxFlat();
		_borderStyle.SetBorderWidthAll(4);
		_borderStyle.BorderColor = Colors.Transparent;
		AddThemeStyleboxOverride("panel", _borderStyle);

		MouseFilter = MouseFilterEnum.Stop;
	}

	public Label GetLabel()
	{
		return _label;
	}

	public void SetLabel(string theString)
	{
		_label.Text = theString;
	}

	public AnimatedSprite2D GetSprite()
	{
		return _sprite;
	}

	public void SetSpriteFrames(SpriteFrames theSpriteFrames)
	{
		_sprite.SpriteFrames = theSpriteFrames;
		_sprite.Play("Idle");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void _InputHandler(InputEvent @theEvent)
	{
		if (@theEvent is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			Select();
		}
	}

	public void Select()
	{
		if (IsSelected)
			return;
		
		IsSelected = true;
		_borderStyle.BgColor = new Color(1, 0.5f, 2.5f);
		FlashBorder();
	}

	public void Deselect()
	{
		if (!IsSelected)
			return;
		
		IsSelected = false;
		_borderStyle.BgColor = new Color(0, 0, 0, 0);
		_flashingBorder.Kill();
		_borderStyle.BorderColor = Colors.Transparent;
	}

	private void FlashBorder()
	{
		_flashingBorder.Kill();
		AnimateBorderFlash();
	}

	private void AnimateBorderFlash()
	{
		_flashingBorder.TweenProperty(_borderStyle, "border_color", _color2, 0.5f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);
		_flashingBorder.TweenProperty(_borderStyle, "border_color", _color1, 0.5f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);

		_flashingBorder.TweenCallback(Callable.From(() =>
		{
			if (IsSelected)
				AnimateBorderFlash();
		}));
	}

	private void SetPlayerInfo()
	{
		_classInfo.MyPlayerClass = _label.Text;
	}
=======
public partial class CharacterTile : Control
{
	[Signal]
	public delegate void CharacterTileSelectedEventHandler(CharacterTile sender);

	private bool _isSelected;
	
	private Button _button;
	
	[Export]
	private PlayerClassInfo _classInfo { get; set; }

	private StyleBoxFlat Borderstyle;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Borderstyle = new StyleBoxFlat();
		_isSelected = false;
		_button = GetNode<Button>("Button");
		_button.Pressed += OnPressed;
		UpdateVisuals();
	}

	private void OnPressed()
	{
		EmitSignal(SignalName.CharacterTileSelected, this);
	}

	public void SetSelected(bool theSelection)
	{
		_isSelected = theSelection;
		UpdateVisuals();
	}

	public bool IsSelected()
	{
		return _isSelected;
	}

	private void UpdateVisuals()
	{

		Borderstyle.BorderColor = IsSelected() ? new Color("FCDB07") : Colors.Transparent;
	}

	public void SetClassInfo(PlayerClassInfo theClassInfo)
	{
		_classInfo = theClassInfo;
		
	}

>>>>>>> Stashed changes

	public PlayerClassInfo GetClassInfo()
	{
		return _classInfo;
	}
}
