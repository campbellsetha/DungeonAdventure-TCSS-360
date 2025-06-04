using Godot;
namespace UDA.Game.Enemies;

public partial class MonsterControls : CharacterBody2D
{
    private Game.Player.Player _thePlayer;
    private double _detectionRadius;
    [Export] private int _speed = 20;
    [Export] private double _vectorLimit = 0.5;
    [Export] private Marker2D _variableEndpoint;
    [Export] private Vector2 _startPosition;
    [Export] private Vector2 _endPosition;

    public override void _Ready()
    {
        _thePlayer = GetTree().GetFirstNodeInGroup("player") as Game.Player.Player;
        
        //Play default animation
        //Should write a separate script for handling enemy animations
        
        //Set the start position equal to its current placement
        _startPosition = Position;
        
        //This can be used to manually alter the position of a monster as needed. 
        //Make the move distance a vector of y distance 3 tiles
        //_moveDistance = new Vector2(0, 3 * 16);
        //Make the final position a vector that distance 3 (tiles) away
        //_endPosition = _startPosition + _moveDistance;
        
        //Can instead have the slimes move towards a specified global position
        _endPosition = _variableEndpoint.GlobalPosition;
    }
    
    private void _ChangePosition()
    {
        //tuple swapping the two values
        (_endPosition, _startPosition) = (_startPosition, _endPosition);
    }

    private void _UpdateVelocity()
    {
        Vector2 moveDirection;

        if (GlobalPosition.DistanceTo(_thePlayer.GlobalPosition) > _detectionRadius)
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
}