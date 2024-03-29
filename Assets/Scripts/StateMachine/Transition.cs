using System;

public class Transition
{
    public Type fromState;
    public Type toState;
    public Func<bool> condition;

    public Transition(Type fromState, Type toState, Func<bool> condition)
    {
        this.fromState = fromState;
        this.toState = toState;
        this.condition = condition;
    }

    public bool Evalutate()
    {
        return condition();
    }
}