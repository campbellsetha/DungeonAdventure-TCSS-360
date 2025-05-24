using Godot;

namespace UDA.Model;

// Honestly, the child classes of Monster could probably be collapsed into this class. Unfortunately, 
// C# does not allow enums to be constructed, which is how I would have handled it with Java. I need to look
// into how to do it appropriately in C#.
public partial class Monster : DungeonCharacter
{
    private double _myStunThreshold;
    private Player _thePlayer;
    private double _detectionRadius;
    [Export] private int _speed = 20;
    [Export] private double _vectorLimit = 0.5;
    [Export] private Marker2D _variableEndpoint;
    [Export] private Vector2 _startPosition;
    [Export] private Vector2 _endPosition;
    
    
    public Monster(
        string theName, 
        int theHitPoints, 
        int theAttackSpeed,
        double theHitChance, 
        (int, int) theDamageRange,
        double theHealChance,
        (int, int) theHealRange,
        double theStunThreshold
    )
        : base(theName,theHitPoints, theAttackSpeed, theHitChance,theDamageRange)
    {
        HealRange = theHealRange;
        HealChance = theHealChance;
        IsStunned = false;
        _myStunThreshold = theStunThreshold;
    }
    
    public (int Min, int Max) HealRange { get; } 
    
    public double HealChance { get; }
   
    public bool IsStunned { get; set; }

    public void Heal()
    {
        if (RandomNumberGenerator.NextDouble() > 1 - HealChance && !IsStunned)
        {
            int healAmount = RandomNumberGenerator.Next(HealRange.Min, HealRange.Max + 1);

            if (healAmount + HitPoints > MaxHitPoints)
            {
                healAmount = MaxHitPoints - HitPoints;
            }
           
            HitPoints += healAmount;
        }
    }

    public override void TakeDamage(int theDamage)
    {
        if (HitPoints / (double) theDamage >= _myStunThreshold)
        {
            IsStunned = true;
        }
       
        HitPoints -= theDamage;
        Heal();
    }

    public override string ToString()
    {
        return base.ToString() 
               + $" StunThreshold:{_myStunThreshold} HealChance:{HealChance} " +
               $"HealRange:{HealRange} ";
    }

    public override void _Ready()
    {
        _thePlayer = GetTree().GetFirstNodeInGroup("player") as Player;
        
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
        //tupple swapping the two values
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

    public override void _PhysicsProcess(double delta)
    {
        _UpdateVelocity();
        MoveAndSlide();
    }
}