using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sitter.Scripts;
using Timer = Godot.Timer;

public partial class GameManager : Node
{
	public int currentScore;

	[Export()] public float PlayerSeatDistance = 30;
	[Export()] public AnimatedSprite2D Player;
	[Export()] public Seat[] Seats;
	[Export()] public float TimeToMove = 1;
	[Export()] public float timeMultiplier = 0.90f;
	
	private bool _moveRight = false;
	private float currentTime;
	private int currentSeatPos = 1;
	private Timer Timer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Timer = GetChild<Timer>(0);

		Player = GetParent().GetChildren().OfType<Player>().First();
		Player.FlipH = _moveRight;
		
		List<Seat> seats = GetParent().GetChildren().OfType<Seat>().ToList();
		Seats = seats.ToArray();
		Player.Position = new Vector2(Seats[seats.Count / 2].Position.X, Seats[seats.Count / 2].Position.Y + PlayerSeatDistance);
		
		Timer.WaitTime = TimeToMove;
		Timer.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Sit();
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

				TimeToMove *= timeMultiplier;
				Timer.WaitTime = TimeToMove;
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

	void _on_time_passed()
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
			
		Timer.Start();
	}
}
