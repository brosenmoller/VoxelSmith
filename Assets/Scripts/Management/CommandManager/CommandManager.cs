using System.Collections.Generic;

public class CommandManager : Manager
{
    private List<ICommand> commandBuffer;
    private int commandIndex;

    public override void Setup()
    {
        commandBuffer = new List<ICommand>();
        commandIndex = 0;
    }

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();

        for (int i = commandBuffer.Count - 1; i > commandIndex; i--)
        {
            commandBuffer.RemoveAt(i);
        }

        commandBuffer.Add(command);
        commandIndex = commandBuffer.Count - 1;
    }

    public void StepForward()
    {
        commandBuffer[commandIndex].Execute();
        commandIndex++;

        if (commandIndex > commandBuffer.Count - 1)
        {
            commandIndex = commandBuffer.Count - 1;
        }
    }

    public void StepBack()
    {
        commandBuffer[commandIndex].Undo();
        commandIndex--;

        if (commandIndex < 0)
        {
            commandIndex = 0;
        }
    }
}

