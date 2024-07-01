using System.Collections.Generic;

public class CommandManager : Manager
{
    private List<ICommand> commandBuffer;
    private int commandIndex;

    public override void Setup()
    {
        commandBuffer = new List<ICommand>();
        commandIndex = 0;

        DataManager.OnProjectLoad += ClearHistory;
    }

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();

        for (int i = commandBuffer.Count - 1; i >= commandIndex; i--)
        {
            commandBuffer.RemoveAt(i);
        }

        commandBuffer.Add(command);
        commandIndex++;
    }

    public void StepBack()
    {
        if (commandIndex > 0)
        {
            commandIndex--;
            commandBuffer[commandIndex].Undo();
        }
    }

    public void StepForward()
    {
        if (commandIndex < commandBuffer.Count)
        {
            commandBuffer[commandIndex].Execute();
            commandIndex++;
        }
    }

    public void ClearHistory()
    {
        commandBuffer.Clear();
        commandIndex = 0;
    }
}
