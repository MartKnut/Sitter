using Godot;
using System;
using System.Linq;
using System.Collections.Generic;


namespace Sitter.Scripts.StateMachine;

public partial class StateMachine : Node
{
	[Signal] public delegate void PreStartEventHandler();
	
	[Signal] public delegate void PostStartEventHandler(); 
	
	[Signal] public delegate void PreExitEventHandler();
	
	[Signal] public delegate void PostExitEventHandler();

	public List<SimpleState> States;
	public string CurrentState;
	public string LastState;

	protected SimpleState State = null;

	public override void _Ready()
	{
		base._Ready();
		States = GetNode<Node>("States").GetChildren().OfType<SimpleState>().ToList();
		
	}

	private void SetState(SimpleState _state, Dictionary<string, object> message)
	{
		if (_state == null)
			return;
		if (_state != null)
		{
			EmitSignal(nameof(PreExit));
			_state.OnExit(_state.GetType().ToString());
			EmitSignal(nameof(PostExit));
		}

		LastState = CurrentState;
		CurrentState = _state.GetType().ToString();

		State = _state;
		EmitSignal(nameof(PreStart));
		State.OnStart(message);
		EmitSignal(nameof(PostStart));
		State.OnUpdate();
	}

	public void ChangeState(string stateName, Dictionary<string, object> message = null)
	{
		foreach (var _state in States)
		{
			if (stateName==_state.GetType().ToString())
			{
				SetState(_state, message);
				return;
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (State == null)
			return;
		State.UpdateState((float)delta);
	}
}