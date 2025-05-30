using System;
using Godot;
using UDA.Model;
using UDA.Model.Characters;
using Hero = UDA.Model.Characters.Hero;

public class Player
{
}

namespace UDA.Player
{
	public partial class Player : CharacterBody2D
	{
		[Export] private int _speed = 200;
		private Vector2 _currentVelocity;
		private AnimatedSprite2D _animatedSprite2D;
		private string _myName;
	
		//Fun C# fact, these are called expression bodies
		public string MyName 
		{
			get => _myName;
			set => _myName = value;
		}
		public Hero MyClass;
	
		public override void _Ready()
		{
			var thisHurtbox = GetNode<Area2D>("Hurtbox");
			thisHurtbox.Connect(Area2D.SignalName.AreaEntered, new Callable(this, nameof(OnHurtBoxEntered)));
			_animatedSprite2D = GetNode<AnimatedSprite2D>("PlayerAnimation");
			_animatedSprite2D.Play("default");
			AddToGroup("player");

			// Connect the Hero's HealthChanged signal to the UpdateHealthBar method
			(MyClass as Hero)?.Connect("HealthChanged", new Callable(this, nameof(UpdateHealthBar)));
		}
	
		private void UpdateHealthBar(int theCurrentHealth, int theMaxHealth)
		{
			// Get the UI canvas layer from the scene tree
			var uiNode = GetTree().Root.GetNode<CanvasLayer>("UI");
			if (uiNode != null)
			{
				var healthBar = uiNode.GetNode<TextureProgressBar>("HealthBar");
				if (healthBar != null)
				{
					healthBar.Value = (float)theCurrentHealth / theMaxHealth * 100; // Update percentage
					GD.Print($"Health updated: {theCurrentHealth}/{theMaxHealth}");
				}
				else
				{
					GD.PrintErr("HealthBar node not found in UI");
				}
			}
			else
			{
				GD.PrintErr("UI CanvasLayer not found in scene tree");
			}
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
			for (int i = 0; i < GetSlideCollisionCount(); i++)
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
		/// Should run at every processing step.
		/// Checks inputVector and updates current animation to reflect that.
		/// E.g. Input vector is -200.X is moving left so we play the left animation.
		/// This is called from _Process because running things in the physics process method can cause slowdown and
		/// runtime issues.
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
			{
				//Horizontal,
				_animatedSprite2D.Play(theCurrentVector2.X < 0 ? "walkLeft" : "walkRight");
			}
			else
			{
				//Vertical
				_animatedSprite2D.Play(theCurrentVector2.Y < 0 ? "walkUp" : "walkDown");
			}
		}
	
		public void SetClass(string theClassType)
		{
			if (MyName == null)
			{
				throw new ArgumentNullException("theClassType")
				{
					HelpLink = null,
					HResult = 0,
					Source = null
				};
			}
			if (theClassType == null)
			{
				throw new ArgumentNullException("theClassType")
				{
					HelpLink = null,
					HResult = 0,
					Source = null
				};
			}
			MyClass = HeroFactory.CreateHero(theClassType, MyName);
		}

		public virtual void OnHurtBoxEntered(Area2D theAreaThatEntered)
		{
			GD.Print("Ouch");
		}
	
		public Godot.Collections.Dictionary<string, Variant> Save()
		{
			return new Godot.Collections.Dictionary<string, Variant>()
			{
				{"FileName", SceneFilePath },
				{"Parent", GetParent().GetPath() },
				{"PosX", Position.X },
				{"PosY", Position.Y},
				{"AnimatedSprite", _animatedSprite2D},
				{"CurrentVelocity", _currentVelocity},
				{"PlayerName", MyName},
				{"PlayerClass", MyClass?.GetType().Name}
			};
		}
	
	}
}
