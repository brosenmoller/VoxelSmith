public abstract class Tool : State<ToolUser>
{
    public virtual IToolOptions GetToolOptions() => null;
}

