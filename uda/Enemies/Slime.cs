using Godot;
using System;
using System.Net;

public partial class Slime : CharacterBody2D
{
    [Export] private int _speed = 20;
    [Export] private double _vectorLimit = 0.5;
    //You can place markers on these "Slimes" in the node tree
    //these allow us to update end positions which allows for unique movements
    //Markers have to be assigned in the dev engine under the 'Slime' properties
    //TODO: See if this can be assigned directly to a monster class
    //Right now this needs to be configured at the scene level that contains the monster
    //[Export] private Marker2D _variableEndpoint;
    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private Vector2 _moveDistance;
    private AnimationPlayer _slimeSpritePlayer;
    private PlayerMove _thePlayer;
    [Export]
    private int _detectionRadius;
    public override void _Ready()
    {
        _slimeSpritePlayer = GetNode<AnimationPlayer>("SlimeAnimations");
        _slimeSpritePlayer.Play("Idle");
        _thePlayer = GetTree().GetFirstNodeInGroup("player") as PlayerMove;
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
        _endPosition = GlobalPosition;
        _detectionRadius = 50;
    }

    private void _ChangePosition()
    {
        //tupple swapping the two values
        (_endPosition, _startPosition) = (_startPosition, _endPosition);
    }

    private void _UpdateVelocity()
    {
        Vector2 moveDirection;
        //we need to check that the player exists
        if (_thePlayer == null)
        {
            moveDirection = _endPosition - Position;
            if (moveDirection.Length() < _vectorLimit) 
                _ChangePosition();
        }

        else if (GlobalPosition.DistanceTo(_thePlayer.GlobalPosition) > _detectionRadius)
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

    public override void _PhysicsProcess(double delta)
    {
        _UpdateVelocity();
        MoveAndSlide();
    }

    public override void _Process(double delta)
    {
        //_slimeSprite.Play("default");
    }
}
