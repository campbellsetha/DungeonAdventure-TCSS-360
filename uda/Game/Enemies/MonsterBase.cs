using Godot;
using UDA.Game.Resources;
using UDA.Model;
using UDA.Model.Characters;
using UDA.Model.Characters.Monster;
using Monster = UDA.Model.Characters.Monster.Monster;

namespace UDA.Game.Enemies;

public partial class MonsterBase : CharacterBody2D
{
    //If the speed gets raised too high it breaks the changePosition function.
    //It affects the vector math making the vector limit unreachable
    //If you want to increase the speed, increase the vector limit
    [Export] protected int Speed = 50;
    [Export] protected double VectorLimit = 0.5;

    private Signal DamageToPlayer;
    protected Vector2 StartPosition;
    protected Vector2 EndPosition { get; set;}
    protected AnimatedSprite2D MonsterSpritePlayer;
    private bool _playerDetected;
    private Area2D myHitBox;
    public Monster _myMonsterClass;
    
    protected virtual void SetUp()
    {
        Random rand = RandomSingleton.GetInstance();
        MonsterSpritePlayer = GetNode<AnimatedSprite2D>("MonsterSprite");
        MonsterSpritePlayer.Play("default");
        myHitBox = GetNode<Area2D>("Hitbox");
        StartPosition = Position;
        EndPosition = StartPosition + new Vector2(rand.Next(-49,49), rand.Next(-49,49));
    }

    private void ChangePosition()
    {
        //tuple swapping the two values
        (EndPosition, StartPosition) = (StartPosition, EndPosition);
    }

    private void _UpdateVelocity()
    {
        //This checks the final point against the current one 
        var moveDirection = (EndPosition - Position);
        if (moveDirection.Length() < VectorLimit)
        {
            ChangePosition();
        }

        Velocity = moveDirection.Normalized() * Speed;
    }

    public override void _PhysicsProcess(double theDelta)
    {
        _UpdateVelocity();
        MoveAndSlide();
        
        //Handy for testing fps drops 
        //GD.Print(Engine.GetFramesPerSecond());
    }
    
    
    //It is really not a great idea to pass an instance of the player to the enemy.
    //This only works because enemies only search for the player, otherwise this would need to be more general
    public void OnBodyEntered(CharacterBody2D theBody)
    {
        if (theBody.IsInGroup("Player"))
        {
            Random rand = RandomSingleton.GetInstance();
            int damage = rand.Next(_myMonsterClass.MyDamageRange.Min, _myMonsterClass.MyDamageRange.Max);
            
            //Calling the event bus to emit the signal for damage with the damage value passed in
            GameManager.EventBus.getInstance().DealDamageToPlayer(damage);
        }
        
    }

    public void TakeDamage(int theDamage)
    {
        _myMonsterClass.TakeDamage(theDamage);
        if (_myMonsterClass.MyHitPoints <= 0)
        {
            //GD.Print("Im hella dead nooooo");
            QueueFree();
        }
    }

    

    //Placed the hurtbox on a different level to avoid multiple calls with the players 2dPhysicsBody
    public void OnPlayerDetection(Player.Player theBody)
    {
        //We are checking if the hurtbox exists in the player node group, its easy to find like this
        if (theBody.IsInGroup("Player"))
        {
            EndPosition = theBody.GlobalPosition;
            //GD.Print("I FOUND EM BOSS!");
        }
    }
}