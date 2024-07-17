public class SelectionBrushTool : BrushTool
{
    protected override void BreakAction()
    {
        GameManager.CommandManager.ExecuteCommand(new ClearSelectionListCommand(ctx.VoxelPosition));
    }

    protected override void PlaceAction()
    {
        GameManager.CommandManager.ExecuteCommand(new AddSelectionListCommand(ctx.VoxelPosition));
    }
}
