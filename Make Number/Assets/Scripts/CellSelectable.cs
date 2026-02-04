using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CellSelectable : MonoBehaviour,
    IPointerDownHandler,
    IPointerEnterHandler,
    IPointerUpHandler
{
    [Header("Visual")]
    [SerializeField] private Image background;
    [SerializeField] private Outline outline;

    [Header("Colors")]
    [SerializeField] private Color normalBg = Color.white;
    [SerializeField] private Color selectedBg = new(1f, 0.95f, 0.8f);
    [SerializeField] private Color normalOutline = Color.clear;
    [SerializeField] private Color selectedOutline = new(1f, 0.6f, 0.2f);

    private void Start()
    {
        background = GetComponent<Image>();
        outline = GetComponent<Outline>();
    }

    public bool IsSelected { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        DragSelectManager.Instance.StartDrag();
        TrySelect();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!DragSelectManager.Instance.IsDragging) return;
        TrySelect();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        DragSelectManager.Instance.EndDrag();
    }

    private void TrySelect()
    {
        if (IsSelected) return;
        DragSelectManager.Instance.Select(this);
    }

    public void Select()
    {
        IsSelected = true;
        background.color = selectedBg;
        outline.effectColor = selectedOutline;
    }

    public void Deselect()
    {
        IsSelected = false;
        background.color = normalBg;
        outline.effectColor = normalOutline;
    }
}