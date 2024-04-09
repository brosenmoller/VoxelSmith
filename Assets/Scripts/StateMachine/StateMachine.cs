using Godot;
using System;
using System.Collections.Generic;

public class StateMachine<T>
{
    public Dictionary<Type, State<T>> stateDictionary = new();
    private List<Transition> allTransitions = new();
    private List<Transition> activeTransitions = new();
    public State<T> CurrentState {  get; private set; }
    
    public T Controller { get; private set; }

    public StateMachine(T owner, params State<T>[] states)
    {
        Controller = owner;

        foreach (State<T> state in states)
        {
            stateDictionary.Add(state.GetType(), state);
            state.Setup(this);
        }
    }

    public void ChangeState(Type newStateType)
    {
        if (!stateDictionary.ContainsKey(newStateType))
        {
            GD.Print($"{newStateType.Name} is not a state in the current statemachine ({nameof(T)})");
            return;
        }

        if (stateDictionary[newStateType] == CurrentState) { return; }

        CurrentState?.OnExit();

        CurrentState = stateDictionary[newStateType];
        activeTransitions = allTransitions.FindAll(
            currentTransition => currentTransition.fromState == CurrentState.GetType() || currentTransition.fromState == null
        );
        CurrentState.OnEnter();
    }

    public void OnUpdate(double delta)
    {
        foreach (Transition transition in activeTransitions)
        {
            if (transition.Evalutate())
            {
                ChangeState(transition.toState);
                return;
            }
        }
        CurrentState.OnUpdate(delta);
    }

    public void OnPhysicsUpdate(double delta)
    {
        CurrentState.OnPhysicsUpdate(delta);
    }

    public void AddTransition(Transition transition)
    {
        allTransitions.Add(transition);
    }
}

