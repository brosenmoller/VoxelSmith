using Godot;

public partial class GameManager : Node
{
    public static DataManager DataManager { get; private set; }
    public static CommandManager CommandManager { get; private set; }
    public static ExportManager ExportManager { get; private set; }
    public static ImportManager ImportManager { get; private set; }
    public static TimerManager TimerManager { get; private set; }
    public static SelectionManager SelectionManager { get; private set; }

    public static SurfaceMesh SurfaceMesh { get; private set; }
    public static PrefabMesh PrefabMesh { get; private set; }
    public static PlayerMovement Player { get; private set; }
    public static ToolUser ToolUser { get; private set; }
    public static UIController UIController { get; private set; }
    public static PaletteUI PaletteUI { get; private set; }
    public static TopBarUI TopBarUI { get; private set; }
    public static ToolOptionsUI ToolOptionsUI { get; private set; }
    public static NativeDialog NativeDialog { get; private set; }
    public static WorldController WorldController { get; private set; }

    public static bool IsInitialized = false;

    private Manager[] activeManagers;

    public override void _Ready()
    {
        IsInitialized = false;
        Player = this.GetNodeByType<PlayerMovement>();
        ToolUser = this.GetNodeByType<ToolUser>();
        UIController = this.GetNodeByType<UIController>();
        PaletteUI = this.GetNodeByType<PaletteUI>();
        TopBarUI = this.GetNodeByType<TopBarUI>();
        ToolOptionsUI = this.GetNodeByType<ToolOptionsUI>();
        NativeDialog = this.GetNodeByType<NativeDialog>();
        WorldController = this.GetNodeByType<WorldController>();
        
        SurfaceMesh = this.GetNodeByType<SurfaceMesh>();
        PrefabMesh = this.GetNodeByType<PrefabMesh>();

        SurfaceMesh.Setup();
        PrefabMesh.Setup();

        SetupManagers();
    }

    private void SetupManagers()
    {
        DataManager.OnProjectLoad += HandleProjectLoad;

        TimerManager = new TimerManager();
        SelectionManager = new SelectionManager();
        CommandManager = new CommandManager();
        ExportManager = new ExportManager();
        ImportManager = new ImportManager();
        DataManager = new DataManager();

        activeManagers = [
            TimerManager,
            SelectionManager,
            CommandManager,
            ExportManager,
            ImportManager,
            DataManager,
        ];


        foreach (Manager manager in activeManagers)
        {
            manager.Setup();
        }
    }

    private void HandleProjectLoad()
    {
        if (IsInitialized) { return; }
        IsInitialized = true;

        SetupInputContext();
        UIController.ClickBlockerLayer.Visible = false;
    }

    private static void SetupInputContext()
    {
        EditMenu.OnUndoPressed += CommandManager.StepBack;
        EditMenu.OnRedoPressed += CommandManager.StepForward;

        ProjectMenu.OnSavePressed += DataManager.SaveProject;
        ProjectMenu.OnSaveAsPressed += UIController.ShowSaveProjectAsDialog;
        ProjectMenu.OnNewPressed += UIController.newProjectDialog.Show;
        ProjectMenu.OnOpenPressed += UIController.ShowLoadProjectDialog;
        ProjectMenu.OnExportPressed += UIController.exportWindow.Show;
        ProjectMenu.OnImportSchematicPressed += UIController.ShowImportSchematicDialog;

        PaletteMenu.OnOpenPressed += UIController.ShowLoadPaletteDialog;
        PaletteMenu.OnSaveAsPressed += UIController.ShowSavePaletteAsDialog;
        PaletteMenu.OnNewPressed += UIController.ShowCreateNewPaletteDialog;
        PaletteMenu.OnSavePressed += DataManager.SavePalette;
        PaletteMenu.OnImportFromProjectPressed += UIController.ShowImportPaletteFromProjectDialog;

        SelectMenu.OnSelectAllPressed += SelectionManager.SelectAll;
        SelectMenu.OnDeselectPressed += SelectionManager.DeselectAll;
        SelectMenu.OnInvertSelectionPressed += SelectionManager.InvertSelection;
        SelectMenu.OnCopySelectionPressed += SelectionManager.CopySelection;
        SelectMenu.OnCutSelectionPressed += SelectionManager.CutSelection;
        SelectMenu.OnPastePressed += SelectionManager.PasteClipboardItem;
        SelectMenu.OnDeleteSelectionPressed += SelectionManager.DeleteSelection;
        SelectMenu.OnRotateClockWisePressed += SelectionManager.RotateClipboardClockWise;
        SelectMenu.OnRotateAntiClockWisePressed += SelectionManager.RotateClipboardAntiClockwise;
        SelectMenu.OnFlipPressed += SelectionManager.FlipClipboard;
    }

    public override void _Process(double delta)
    {
        foreach (Manager manager in activeManagers)
        {
            manager.OnUpdate(delta);
        }
    }
}