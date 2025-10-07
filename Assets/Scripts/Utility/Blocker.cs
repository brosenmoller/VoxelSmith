using System;
using System.Collections.Generic;

public class Blocker
{
    private readonly HashSet<string> blockers = new();

    public bool IsFree => blockers.Count == 0;
    public bool IsBlocked => blockers.Count > 0;

    public event Action OnChangeStatus;

    public void AddBlocker(string id)
    {
        bool wasBlocked = IsBlocked;
        blockers.Add(id);
        CheckStatus(wasBlocked);
    }

    public void RemoveBlocker(string id)
    {
        bool wasBlocked = IsBlocked;
        blockers.Remove(id);
        CheckStatus(wasBlocked);
    }

    public void Clear()
    {
        bool wasBlocked = IsBlocked;
        blockers.Clear();
        CheckStatus(wasBlocked);
    }

    private void CheckStatus(bool wasBlocked)
    {
        if (wasBlocked == IsBlocked) { return; }

        OnChangeStatus?.Invoke();
    }
}