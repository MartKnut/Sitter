using Godot;
using System.Collections.Generic;

public partial class SimpleState : Node
{
	private bool _hasBeenInitialized = false;
	private bool _onUpdateHasFired = false;

	[Signal] public delegate void StateStartEventHandler();
	[Signal] public delegate void StateUpdatedEventHandler();
	[Signal] public delegate void StateExitedEventHandler();

	public virtual void OnStart(Dictionary<string, object> message)
	{
		EmitSignal(nameof(StateStart));
		_hasBeenInitialized = true;
	}

	public virtual void OnUpdate()
	{
		if (!_hasBeenInitialized) 
			return;
		EmitSignal(nameof(StateUpdated));
		_onUpdateHasFired = true;
	}
	public virtual void UpdateState(float delta)
	{
		if (!_onUpdateHasFired) 
			return;
		
	}
	public virtual void OnExit(string nextState)
	{
		if (!_hasBeenInitialized) 
			return;
		EmitSignal(nameof(StateExited));
		_hasBeenInitialized = false;
		_onUpdateHasFired = false;
	}
	public virtual void NextSeat(int seatNr)
	{
		return;
		
	}
}
