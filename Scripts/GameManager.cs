using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Sitter.Scripts;

public partial class GameManager : Node
{
	public int currentScore;

	[Export()] public float PlayerSeatDistance = 30;
	[Export()] public AnimatedSprite2D Player;
	[Export()] public Seat[] Seats;
	[Export()] public float TimeToMove;

	private bool _moveRight = false;
	private float currentTime;
	private int currentSeatPos = 1;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Player = GetParent().GetChildren().OfType<PlayerInput>().First();
		Player.FlipH = _moveRight;
		
		List<Seat> seats = GetParent().GetChildren().OfType<Seat>().ToList();
		Seats = seats.ToArray();
		Player.Position = new Vector2(Seats[seats.Count / 2].Position.X, Seats[seats.Count / 2].Position.Y + PlayerSeatDistance);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		Move(delta);
		Sit();
	}

	private void Move(double delta)
	{
		float direction = Input.GetAxis("Left", "Right");
		
		if (Input.IsActionJustPressed("Left") || Input.IsActionJustPressed("Right"))
		{
			currentSeatPos += 1 * (int)direction;
			
			if (currentSeatPos >= Seats.Length) 
				currentSeatPos = Seats.Length - 1;
			
			if (currentSeatPos <= 0) 
				currentSeatPos = 0;

			if (Player != null)
			{
				Player.Position = new Vector2(Seats[currentSeatPos].Position.X, Seats[currentSeatPos].Position.Y + PlayerSeatDistance);
			}
			
			
		}

		currentTime += (float)delta;
		
		if (currentTime >= TimeToMove)
		{
			if (currentSeatPos <= 0 || currentSeatPos >= Seats.Length - 1)
			{
				_moveRight = !_moveRight;
				Player.FlipH = _moveRight;
			}
				

			if (_moveRight)
			{
				currentSeatPos++;
				Player.Position = new Vector2(Seats[currentSeatPos].Position.X, Seats[currentSeatPos].Position.Y + PlayerSeatDistance);
			}
			else
			{
				currentSeatPos--;
				Player.Position = new Vector2(Seats[currentSeatPos].Position.X, Seats[currentSeatPos].Position.Y + PlayerSeatDistance);
			}
			
			currentTime = 0;
		}


	}
	private void Sit()
	{
		if (Input.IsActionJustPressed("Sit"))
		{
			if (!Seats[currentSeatPos].Occupied)
			{
				Seats[currentSeatPos].Occupied = true;
				Player.Position = new Vector2(Seats[currentSeatPos].Position.X, Seats[currentSeatPos].Position.Y);
				Player.ZIndex -= 1;

				Player.Animation = new StringName("GirlSit");
				
				currentScore += 50;
				
				var playerScene = GD.Load<PackedScene>("res://Scenes/Player.tscn");
				Player = playerScene.Instantiate<AnimatedSprite2D>();
				AddChild(Player);

				Player.Position = new Vector2(Seats[currentSeatPos].Position.X, Seats[currentSeatPos].Position.Y + PlayerSeatDistance);
				
				TimeToMove *= 0.90f;
			}
			else
			{
				Lose(currentScore);
			}
		}
	}
	
	private void Lose(int Score)
	{
		GetTree().Quit();
	}
}
