using Godot;
using UDA.Model;
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
    public Monster _myMonsterClass;
    
    protected virtual void SetUp()
    {
        MonsterSpritePlayer = GetNode<AnimatedSprite2D>("MonsterSprite");
        MonsterSpritePlayer.Play("default");
        StartPosition = Position;
        EndPosition = StartPosition + new Vector2(0, 3 * 16);
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
    

    //Moving this functionality into the player script
    
    public void OnBodyEntered(Player.Player theBody)
    {
        if (theBody.IsInGroup("Player"))
        {
            Random rand = RandomSingleton.GetInstance();
            int damage = rand.Next(_myMonsterClass.DamageRange.Min, _myMonsterClass.DamageRange.Max);
            
            //Calling the event bus to emit the signal for damage with the damage value passed in
            EventBus.getInstance().DealDamageToPlayer(damage);
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