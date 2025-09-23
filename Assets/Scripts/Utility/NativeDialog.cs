using Godot;
using System;

public partial class NativeDialog : Control
{
    private Action<Info> OnFileDialogCallBack;
    public bool visible = false;

    private void FileDialogCallBack(Variant variantStatus, Variant variantSelectedPaths, Variant variantfilterIndex)
    {
        bool status = variantStatus.AsBool();
        visible = false;

        if (!status) { return; }

        string[] selectedPaths = variantSelectedPaths.AsStringArray();
        int filterIndex = variantfilterIndex.AsInt32();

        string path = "";
        string dir = "";
        string file = "";
        if (selectedPaths.Length > 0)
        {
            path = selectedPaths[0];

            int indexOfLastSlash = selectedPaths[0].LastIndexOf('\\') + 1;
            dir = selectedPaths[0][..indexOfLastSlash];
            file = selectedPaths[0][indexOfLastSlash..];
        }

        Info dialogInfo = new()
        {
            path = path,
            directory = dir,
            fileName = file,

            filterIndex = filterIndex,
            selectedPaths = selectedPaths,
        };

        OnFileDialogCallBack?.Invoke(dialogInfo);
    }

    public void ShowFileDialog(string title, DisplayServer.FileDialogMode mode, string[] filters, Action<Info> callBack, string directory = "", string fileName = "")
    {
        GameManager.UIController.worldController.WorldInFocus = false; // Hacky

        visible = true;
        OnFileDialogCallBack = null;
        OnFileDialogCallBack += callBack;
        Callable callable = new(this, MethodName.FileDialogCallBack);
        DisplayServer.FileDialogShow(title, directory, fileName, true, mode, filters, callable);
    }

    public struct Info
    {
        public string path;
        public string directory;
        public string fileName;
        public string[] selectedPaths;
        public int filterIndex;
    }
}