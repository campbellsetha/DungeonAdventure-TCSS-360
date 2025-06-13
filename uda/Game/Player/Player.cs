using Godot;
using UDA.Game.Enemies;
using UDA.Game.GameManager;
using UDA.Game.Resources;
using UDA.Model.Characters;
using UDA.inventory;
using UDA.Model;
using UDA.Model.Characters.Monster;

namespace UDA.Game.Player;
public partial class Player : CharacterBody2D
{
    //[Export] private Area2D MonsterArea;
	[Export] private int _speed = 200;
	private Vector2 _currentVelocity;
	private AnimatedSprite2D _animatedSprite2D;
    private AnimationPlayer _animationPlayer;
    private Node2D _myWeapon;
    private Area2D _myWeaponHitBox;
    private CollisionShape2D _myWeaponHitBoxShape;
    private PointLight2D _myLight;
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
		//MyClass = HeroFactory.CreateHero(MyClassInfo.MyPlayerClass, MyClassInfo.MyPlayerName);

        MyClass = MyClassInfo.MyPlayerClass switch
        {
            nameof(Priest) => HeroFactory.CreatePriest(MyClassInfo.MyPlayerName),
            nameof(Thief) => HeroFactory.CreateThief(MyClassInfo.MyPlayerName),
            nameof(Warrior) => HeroFactory.CreateWarrior(MyClassInfo.MyPlayerName),
            _ => throw new Exception($"Unknown player class : {MyClassInfo.MyPlayerName}")
        };

        //Okay, so we need to load the inventory resource within the player scene.
        //This can be saved manually in the game manager resource saver method
        //This should probably be placed in the "user://" path instead, as it will contain instanced player data 
        Inventory = GD.Load<Inventory>("res://Game/Player/player_inventory.tres");
        
        //Connecting to the event bus, we connect to the specific signal not the method in the bus
        EventBus.getInstance().Connect(nameof(GameManager.EventBus.DealDamage), new Callable(this, nameof(OnDamageTaken)));
        EventBus.getInstance().Connect(nameof(GameManager.EventBus.AddItem), new Callable(this, nameof(ItemAdded)));
        EventBus.getInstance().Connect(nameof(EventBus.UseHealthPotion), new Callable(this, nameof(UseHealthPotion)));
        EventBus.getInstance().Connect(nameof(EventBus.UseVisionPotion), new Callable(this, nameof(UseVisionPotion)));
        EventBus.getInstance().Connect(nameof(EventBus.SetPlayerPosition), new Callable(this, nameof(SetStartingPosition)));
        
        
		_animatedSprite2D = GetNode<AnimatedSprite2D>("PlayerAnimation");
		_animatedSprite2D.Play("default");
        _animationPlayer = GetNode<AnimationPlayer>("WeaponAnimation");
        _myWeapon = GetNode<Node2D>("Weapon");
        _myWeaponHitBox = GetNode<Area2D>("Weapon/Sword");
        _myWeaponHitBox.Connect(Area2D.SignalName.AreaEntered, new Callable(this, nameof(EnemyHit)));
        _myWeaponHitBoxShape = _myWeaponHitBox.GetNode<CollisionShape2D>("CollisionShape2D");
        _myWeaponHitBoxShape.Disabled = true;
        _myWeapon.Visible = false;
        _myLight = GetNode<PointLight2D>("MyLight");
        _healthBar = GetNode<TextureProgressBar>("CanvasLayer/BarLayout/Hp Bar");
        //Setting the maximum value of the healthBar to the current classes max hit points
        //This makes updating it only take the current hp of the players class
        _healthBar.MaxValue = MyClass.MyMaxHitPoints;
        _healthBar.Value = MyClass.MyHitPoints;
        _healthBar.Visible = true;
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

    private async void HandleInput()
    {
        _currentVelocity = Input.GetVector(
            "moveLeft", "moveRight",
            "moveUp", "moveDown");
        _currentVelocity *= _speed;
        
        //Weapon animations
        //This probably needs to change, but it works for now and we are going to go with that
        if (Input.IsActionJustPressed("Attack")) await Attack();

        if (Input.IsActionJustPressed("CheatMode"))
        {
            CollisionShape2D myBody = GetNode<CollisionShape2D>("CollisionShape2D");
            myBody.Disabled = !myBody.Disabled;
        }
    }
    
    private async Task Attack()
    {
        _myWeapon.Visible = true;
        _myWeaponHitBoxShape.Disabled = false;
        _animationPlayer.Play("Attack" + _myLastDirection);
        /*
         * While this does work, it comes with some graphical issues. If an attack animation is triggered again,
         * the current animation will end and make the weapon visible again.
         * Ideally, this should be either connected to the animation finished signal, or have a check in place
         * to prevent the player from re-triggering the attack animation during the current one.
         */
        await ToSignal(GetTree().CreateTimer(0.3, false), SceneTreeTimer.SignalName.Timeout);
        _myWeapon.Visible = false;
        _myWeaponHitBoxShape.Disabled = true;
    }
    
    private void EnemyHit(Area2D theMonster)
    {
        if (theMonster.IsInGroup("Monster"))
        {
            //Random rand = RandomSingleton.GetInstance();
            //int damage = rand.Next(MyClass.DamageRange.Min, MyClass.DamageRange.Max);
            MonsterBase theMonsterInstance = theMonster.GetParent<MonsterBase>();
            Monster monsterClass = theMonsterInstance._myMonsterClass;
            int attackDamage = MyClass.Attack(monsterClass);
            theMonsterInstance.TakeDamage(attackDamage);
            //EventBus.getInstance().DealDamageToEnemy(damage);
        }
    }

    private void ItemAdded(InventoryItem theItem)
    {
        Inventory.AddToInventory(theItem);
        EventBus.getInstance().ItemAddedToInventory(theItem);
        if (Inventory.GetKeyItems().Count == 4)
        {
            EventBus.getInstance().HoldingAllPillers();
            //Maybe add a little popup that the dungeon can be left now?
        }
    }

    private void UseHealthPotion(InventoryItem theHealthPotion)
    {
        //We want to modify the inventory before we attempt to modify the player
        Inventory.UsePotion(theHealthPotion);
        MyClass.Heal(50);
        _healthBar.Value = MyClass.MyHitPoints;
    }

    private void UseVisionPotion(InventoryItem theVisionPotion)
    {
        Inventory.UsePotion(theVisionPotion);
        _myLight.TextureScale = 10;
        //Temporary timer to manage the duration of the vision potion
        var timer = GetTree().CreateTimer(30);
        timer.Timeout += ResetLight;
    }

    private void ResetLight()
    {
        _myLight.TextureScale = (float)4.6;
        GD.Print("The light was successfully reset");
    }

    private void SetStartingPosition(Vector2 thePosition)
    {
        GlobalPosition = thePosition;
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
    
    private void OnDamageTaken(int theDamageAmount)
    {
        //Take damage
        MyClass.TakeDamage(theDamageAmount);
        
        if (_healthBar != null)
        {
            _healthBar.Value = MyClass.MyHitPoints;
        }

        if (MyClass.MyHitPoints == 0)
        {
            //Simulates death, still need to create a game over screen
            //QueueFree();
            //TODO: ADD THE LOSS SCREEN
            GetTree().ChangeSceneToFile("res://Game/Resources/GameOverScreen.tscn");
            
        }
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