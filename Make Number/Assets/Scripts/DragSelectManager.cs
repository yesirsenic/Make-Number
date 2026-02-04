using System.Collections.Generic;
using UnityEngine;

public class DragSelectManager : MonoBehaviour
{
    public static DragSelectManager Instance;

    [SerializeField] private int maxSelectCount = 2;

    private readonly List<CellSelectable> selectedCells = new();
    public bool IsDragging { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public bool CanSelect(CellSelectable cell)
    {
        if (selectedCells.Contains(cell) || !cell.isSpawn) return false;
        return selectedCells.Count < maxSelectCount;
    }

    public void Select(CellSelectable cell)
    {
        if (!CanSelect(cell)) return;

        selectedCells.Add(cell);
        cell.Select();
    }

    public void StartDrag()
    {
        IsDragging = true;
        ClearSelection();
    }

    public void EndDrag()
    {
        IsDragging = false;

        if (selectedCells.Count == maxSelectCount)
        {
            GameManager.Instance.OnCellsSelected(
                selectedCells[0],
                selectedCells[1]
            );
        }
    }

    private void ClearSelection()
    {
        foreach (var cell in selectedCells)
            cell.Deselect();

        selectedCells.Clear();
    }
}