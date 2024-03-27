using Godot;

public partial class QuickExport : MarginContainer
{
    private Button button;
    private ConfirmationDialog confirmationDialog;

    public override void _Ready()
    {
        button = this.GetChildByType<Button>();
        confirmationDialog = this.GetChildByType<ConfirmationDialog>();

        confirmationDialog.GetLabel().HorizontalAlignment = HorizontalAlignment.Center;

        button.Pressed += OnButtonPress;
        confirmationDialog.Confirmed += OnConfirmed;
    }

    private void OnButtonPress()
    {
        confirmationDialog.Show();
    }

    private void OnConfirmed()
    {
        // TODO: Implement Quick Export
    }
}
