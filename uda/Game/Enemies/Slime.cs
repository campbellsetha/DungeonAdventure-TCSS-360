using Godot;
using UDA.Model.Characters.Monster;
using Monster = UDA.Model.Characters.Monster.Monster;

namespace UDA.Game.Enemies;

public partial class Slime : CharacterBody2D
{
    [Export] private int _speed = 20;
    [Export] private double _vectorLimit = 0.5;
    //You can place markers on these "Slimes" in the node tree
    //these allow us to update end positions which allows for unique movements
    //Markers have to be assigned in the dev engine under the 'Slime' properties
    //TODO: See if this can be assigned directly to a monster class
    //Right now this needs to be configured at the scene level that contains the monster
    [Export] private Marker2D _variableEndpoint;
    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private Vector2 _moveDistance;
    private AnimationPlayer _slimeSpritePlayer;
    private global::UDA.Game.Player.Player _thePlayer;
    private int _detectionRadius;
    public Monster MyMonsterClass;
    public override void _Ready()
    {
        MyMonsterClass = MonsterFactory.CreateGremlin();
        _slimeSpritePlayer = GetNode<AnimationPlayer>("SlimeAnimations");
        _slimeSpritePlayer.Play("Idle");
       
        
        //Play default animation
        //Should write a separate script for handling enemy animations
        
        //Set the start position equal to its current placement
        _startPosition = Position;
        
        //This can be used to manually alter the position of a monster as needed. 
        //Make the move distance a vector of y distance 3 tiles
        //_moveDistance = new Vector2(0, 3 * 16);
        //Make the final position a vector that distance 3 (tiles) away
        //_endPosition = _startPosition + _moveDistance;
        if (_variableEndpoint != null)
        {
            _endPosition = _variableEndpoint.GlobalPosition;
        }
        
        
        //Can instead have the slimes move towards a specified global position
        //_endPosition = _variableEndpoint.GlobalPosition;
    }

    private void _ChangePosition()
    {
        //tuple swapping the two values
        (_endPosition, _startPosition) = (_startPosition, _endPosition);
    }

    private void _UpdateVelocity()
    {
        _thePlayer = GetTree().GetFirstNodeInGroup("player") as global::UDA.Game.Player.Player;
        Vector2 moveDirection;
        if (_thePlayer != null &&
            GlobalPosition.DistanceTo(_thePlayer.GlobalPosition) > _detectionRadius)
        {
            //End position for moving down is greater y val than current
            moveDirection = _endPosition - Position;
            /*
        Checks if the current move vector is too small
        If it is we have reached close to the endpoint of our specified distance
        If it is not we simply set the velocity of the object to a normalized vector scaled by its speed value
        This prevents an infinite approach/bouncing issue
        */
            if (moveDirection.Length() < _vectorLimit) 
                _ChangePosition();
        }
        else
        {
            // TODO: add reference to this field
            moveDirection = _thePlayer.GlobalPosition - GlobalPosition;
        }
        
        if (moveDirection.LengthSquared() > 0.001f) 
            Velocity = moveDirection.Normalized() * _speed;
        else 
            Velocity = Vector2.Zero;
    }

    public override void _PhysicsProcess(double theDelta)
    {
        _UpdateVelocity();
        MoveAndSlide();
    }

    public override void _Process(double theDelta)
    {
        //_slimeSprite.Play("default");
    }

    //This should emit a signal when a "Player" character body is entered
    public void OnBodyEntered(global::UDA.Game.Player.Player theBody)
    {
        theBody.MyClass.TakeDamage(MyMonsterClass.DamageRange.Max);
    }
}