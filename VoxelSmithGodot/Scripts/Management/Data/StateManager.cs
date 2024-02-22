public class StateManager
{
    public State CurrentState { get; private set; }

    public void CreateNew(string name)
    {
        if (CurrentState != null)
        {
            // TODO: Warn User about unsaved data
        }

        CurrentState = new State(name);
    }

    public void SaveCurrent()
    {

    }
}

