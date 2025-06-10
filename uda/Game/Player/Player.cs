using Godot;
using UDA.Game.Enemies;
using UDA.Game.Resources;
using UDA.Model.Characters;

using UDA.Model.Characters.Monster;

using UDA.inventory;


namespace UDA.Game.Player;

public partial class Player : CharacterBody2D
{
    [Export] private Area2D MonsterArea;
	[Export] private int _speed = 200;
	private Vector2 _currentVelocity;
	private AnimatedSprite2D _animatedSprite2D;
    private AnimationPlayer _animationPlayer;
    private Node2D _myWeapon;
    private Area2D _myWeaponHitBox;
    private string _myLastDirection = "Down";
    public PlayerClassInfo MyClassInfo;
	private string _myName;
    public Hero MyClass;
	public Inventory Inventory { get; private set; }
    private TextureProgressBar _healthBar;

	
	//Fun C# fact, these are called expression bodies
	public string MyName 
	{
		get => _myName;
		set => _myName = value;
	}
    
	public override void _Ready()
    {
		MyClassInfo = ResourceLoader.Load<PlayerClassInfo>("res://Game/Resources/PlayerClass.tres");
		MyClass = HeroFactory.CreateHero(MyClassInfo.MyPlayerClass, MyClassInfo.MyPlayerName);
        
        //Connecting to the event bus, we connect to the specific signal not the method in the bus
        EventBus.getInstance().Connect(nameof(EventBus.DealDamage), new Callable(this, nameof(OnHurtBoxEntered)));
        
		_animatedSprite2D = GetNode<AnimatedSprite2D>("PlayerAnimation");
		_animatedSprite2D.Play("default");
        _animationPlayer = GetNode<AnimationPlayer>("WeaponAnimation");
        _myWeapon = GetNode<Node2D>("Weapon");
        _myWeaponHitBox = GetNode<Area2D>("Weapon/Sword");
        _myWeapon.Visible = false;
        _healthBar = GetNode<TextureProgressBar>("Hp Bar");
        //Setting the maximum value of the healthBar to the current classes max hit points
        //This makes updating it only take the current hp of the players class
       _healthBar.MaxValue = MyClass.MaxHitPoints;
       _healthBar.Value = MyClass.HitPoints;

       //_myWeaponHitBox.SetCollisionMask(0);
       //_myWeaponHitBox.set
    }

    public override void _Process(double theDelta)
    {
        base._Process(theDelta);
        ChangeAnimation(_currentVelocity);
    }

    public override void _PhysicsProcess(double theDelta)
    {
        base._PhysicsProcess(theDelta);
        //Check player input
        HandleInput();
        //Update velocity
        Velocity = _currentVelocity;
        //Trigger physics process and move the player
        MoveAndSlide();
        ChangeAnimation(_currentVelocity);
        //HandleCollision();
    }

    private void HandleCollision()
    {
        //Currently prints out each execution of a collision.
        for (var i = 0; i < GetSlideCollisionCount(); i++)
        {
            //KinematicCollision2D collision = GetSlideCollision(i);
            //GD.Print("Collided with: ", (collision.GetCollider() as Node).Name);
            //var collider = GetSlide
        }
    }

    private void HandleInput()
    {
        _currentVelocity = Input.GetVector(
            "moveLeft", "moveRight",
            "moveUp", "moveDown");
        _currentVelocity *= _speed;
        
        //Weapon animations
        //This probably needs to change, but it works for now and we are going to go with that
        if (Input.IsActionJustPressed("Attack")) Attack();
    }
    
    private async Task Attack()
    {
        _myWeapon.Visible = true;
        _myWeaponHitBox.SetCollisionMask(3);
        _animationPlayer.Play("Attack" + _myLastDirection);
        
        /*
         * While this does work, it comes with some graphical issues. If an attack animation is triggered again,
         * the current animation will end and make the weapon visible again.
         * Ideally, this should be either connected to the animation finished signal, or have a check in place
         * to prevent the player from re-triggering the attack animation during the current one.
         */
        await ToSignal(GetTree().CreateTimer(0.3, false), SceneTreeTimer.SignalName.Timeout);
        _myWeapon.Visible = false;
    }
    

    /// <summary>
    ///     Should run at every processing step.
    ///     Checks inputVector and updates current animation to reflect that.
    ///     E.g. Input vector is -200.X is moving left so we play the left animation.
    ///     This is called from _Process because running things in the physics process method can cause slowdown and
    ///     runtime issues.
    /// </summary>
    private void ChangeAnimation(Vector2 theCurrentVector2)
    {
        //If the vector value is greater than zero we are moving
        if (!(theCurrentVector2.Length() > 0))
        {
            _animatedSprite2D.Play("default");
            return;
        }

        //Horizontal movement versus vertical movement
        if (Math.Abs(theCurrentVector2.X) > Math.Abs(theCurrentVector2.Y))
            //Horizontal
            if (theCurrentVector2.X < 0)
            {
                _myLastDirection = "Left";
                _animatedSprite2D.Play("walk" + _myLastDirection);
            }
            else
            {
                _myLastDirection = "Right";
                _animatedSprite2D.Play("walk" + _myLastDirection);
            }
            //Vertical
            else if (theCurrentVector2.Y < 0)
            {
                _myLastDirection = "Up";
                _animatedSprite2D.Play("walk" + _myLastDirection);
            }
            else
            {
                _myLastDirection = "Down";
                _animatedSprite2D.Play("walk" + _myLastDirection);
            }
            
    }
    
    
    //Can add a check to see if what entered was the global class monster
    private void OnHurtBoxEntered(int theDamageAmount)
    {
        //Take damage
        MyClass.TakeDamage(theDamageAmount);
        
        if (_healthBar != null)
        {
            _healthBar.Value = MyClass.HitPoints;
        }

        if (MyClass.HitPoints == 0)
        {
            //Simulates death, still need to create a game over screen
            QueueFree();
            GD.Print("Im dead, maybe add queue free to delete me from the scene");
        }

        //Testing to see that the appropriate damage is being delivered
        GD.Print("Ouch" + theDamageAmount);
    }

    private Godot.Collections.Dictionary<string, Variant> Save()
    {
        return new Godot.Collections.Dictionary<string, Variant>()
        {
            {"FileName", SceneFilePath },
            {"Parent", GetParent().GetPath() },
            {"PosX", Position.X },
            {"PosY", Position.Y},
            {"AnimatedSprite", _animatedSprite2D},
            {"CurrentVelocity", _currentVelocity},
            {"PlayerName", MyClassInfo.MyPlayerName},
            //{"PlayerHealth", MyClass.HitPoints}
        };
    }
}