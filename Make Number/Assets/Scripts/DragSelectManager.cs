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
        if (selectedCells.Contains(cell) || !cell.isSpawn || GameManager.Instance.state != GameState.MainGame) return false;
        return selectedCells.Count < maxSelectCount;
    }

    public void Select(CellSelectable cell)
    {
        if (selectedCells.Contains(cell)) return;

        // 첫 셀
        if (selectedCells.Count == 0)
        {
            selectedCells.Add(cell);
            cell.Select();
            return;
        }

        // ❗ 이미 2칸이면 더 못 고름
        if (selectedCells.Count >= 2)
            return;

        // 두 번째 셀만 검사
        CellSelectable prev = selectedCells[0];

        Vector3 a = prev.transform.position;
        Vector3 b = cell.transform.position;

        bool xChanged = Mathf.Abs(a.x - b.x) > 0.01f;
        bool yChanged = Mathf.Abs(a.y - b.y) > 0.01f;

        // 대각선 / 변화 없음 컷
        if (xChanged == yChanged)
            return;

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

    public void ClearSelection()
    {
        foreach (var cell in selectedCells)
            cell.Deselect();

        selectedCells.Clear();
    }
}