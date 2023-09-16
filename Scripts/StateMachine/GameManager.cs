using Godot;
using System;
using Sitter.Scripts;

public partial class GameManager : Node
{
	[Export()] public Seat[] Seats;
	[Export()] public PlayerInput Player;
}
