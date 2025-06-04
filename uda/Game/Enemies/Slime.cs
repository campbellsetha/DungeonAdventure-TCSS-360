using Godot;
using UDA.Model.Characters.Monster;
using Monster = UDA.Model.Characters.Monster.Monster;

namespace UDA.Game.Enemies;

public partial class Slime : CharacterBody2D
{
    private Vector2 _endPosition;
    private bool _playerDetected;

    private AnimationPlayer _slimeSpritePlayer;

    //If the speed gets raised too high it breaks the changePosition function.
    //It affects the vector math making the vector limit unreachable
    //If you want to increase the speed, increase the vector limit
    [Export] private int _speed = 50;

    private Vector2 _startPosition;
    [Export] private double _vectorLimit = 0.5;
    public Monster MyMonsterClass;

    public override void _Ready()
    {
        MyMonsterClass = MonsterFactory.CreateGremlin();
        _slimeSpritePlayer = GetNode<AnimationPlayer>("SlimeAnimations");
        _slimeSpritePlayer.Play("Idle");

        _startPosition = Position;
        _endPosition = _startPosition + new Vector2(0, 3 * 16);
    }

    private void ChangePosition()
    {
        //tuple swapping the two values
        (_endPosition, _startPosition) = (_startPosition, _endPosition);
    }

    private void _UpdateVelocity()
    {
        //This checks the final point against the current one 
        var moveDirection = _endPosition - Position;
        if (moveDirection.Length() < _vectorLimit) ChangePosition();

        Velocity = moveDirection.Normalized() * _speed;
    }

    public override void _PhysicsProcess(double theDelta)
    {
        _UpdateVelocity();
        MoveAndSlide();

        //Handy for testing fps drops 
        //GD.Print(Engine.GetFramesPerSecond());
    }


    //This should emit a signal when a "Player" character body is entered
    //This is masked to only detect the players hurtbox so this will only get called
    //When the player enters, as no other physics body is on the same mask
    public void OnBodyEntered(Player.Player theBody)
    {
        if (theBody.IsInGroup("Player")) theBody.MyClass.TakeDamage(MyMonsterClass.DamageRange.Max);
    }

    //Placed the hurtbox on a different level to avoid multiple calls with the players 2dPhysicsBody
    public void OnPlayerDetection(Player.Player theBody)
    {
        //We are checking if the hurtbox exists in the player node group, its easy to find like this
        if (theBody.IsInGroup("Player")) _endPosition = theBody.GlobalPosition;
        //GD.Print("I FOUND EM BOSS!");
    }
}