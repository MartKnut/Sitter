using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Sitter.Scripts;

public partial class GameManager : Node
{
	public int currentScore;

	[Export()] public float PlayerSeatDistance = 30;
	[Export()] public Node2D Player;
	[Export()] public Seat[] Seats;

	
	
	private int currentSeatPos = 1;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		Player = GetParent().GetChildren().OfType<PlayerInput>().First();
		
		List<Seat> seats = GetParent().GetChildren().OfType<Seat>().ToList();
		Seats = seats.ToArray();
		Player.Position = new Vector2(Seats[seats.Count / 2].Position.X, Seats[seats.Count / 2].Position.Y - PlayerSeatDistance);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		Move();
		Sit();
	}

	private void Move()
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
				Player.Position = new Vector2(Seats[currentSeatPos].Position.X, Seats[currentSeatPos].Position.Y - PlayerSeatDistance);
			}
			

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
				
				currentScore += 50;
				
				var playerScene = GD.Load<PackedScene>("res://Scenes/Player.tscn");
				Player = playerScene.Instantiate<Node2D>();
				AddChild(Player);

				Player.Position = new Vector2(Seats[currentSeatPos].Position.X, Seats[currentSeatPos].Position.Y - PlayerSeatDistance);
			}
			else
			{
				Lose(currentScore);
			}
		}
	}
	
	private void Lose(int Score)
	{
		
	}
}
