using Godot;
using UDA.Game.Enemies;
using UDA.Game.Resources;
using UDA.Model.Characters;
using UDA.Model.Characters.Monster;

namespace UDA.Game.Player;

public partial class Player : CharacterBody2D
{
    [Export] private Area2D MonsterArea;
	[Export] private int _speed = 200;
	private Vector2 _currentVelocity;
	private AnimatedSprite2D _animatedSprite2D;
    public PlayerClassInfo MyClassInfo;
	public Hero MyClass;
    
	public override void _Ready()
    {
		MyClassInfo = ResourceLoader.Load<PlayerClassInfo>("res://Game/Resources/PlayerClass.tres");
		MyClass = HeroFactory.CreateHero(MyClassInfo.MyPlayerClass, MyClassInfo.MyPlayerName);
        
        //Connecting to the event bus, we connect to the specific signal not the method in the bus
        EventBus.getInstance().Connect(nameof(EventBus.DealDamage), new Callable(this, nameof(OnHurtBoxEntered)));
        
		_animatedSprite2D = GetNode<AnimatedSprite2D>("PlayerAnimation");
		_animatedSprite2D.Play("default");
		//AddToGroup("player");
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
        HandleCollision();
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
            //Horizontal,
            _animatedSprite2D.Play(theCurrentVector2.X < 0 ? "walkLeft" : "walkRight");
        else
            //Vertical
            _animatedSprite2D.Play(theCurrentVector2.Y < 0 ? "walkUp" : "walkDown");
    }
    
    //Can add a check to see if what entered was the global class monster
    private void OnHurtBoxEntered(int theDamageAmount)
    {
        //Testing to see that the appropiate damge is being delivered
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