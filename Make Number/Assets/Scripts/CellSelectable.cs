using System.Collections;
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

    [Header("State")]
    public bool isSpawn;

    private void Start()
    {
        background = GetComponent<Image>();
        outline = GetComponent<Outline>();
        isSpawn = true;
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
        if (GameManager.Instance.state != GameState.MainGame)
            return;

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

    public void ReSpawn(float duration)
    {
        StartCoroutine(ReSpawn(gameObject, duration));
    }

    IEnumerator ReSpawn(GameObject cell, float duration)
    {
        isSpawn = false;
        Text text = cell.transform.GetChild(0).GetComponent<Text>();

        Color c = text.color;
        float t = 0f;

        BoardManager.Instance.ReRandomizeCell(cell.GetComponent<CellData>());


        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / duration);
            text.color = c;
            yield return null;
        }

        c.a = 1f;
        text.color = c;

        isSpawn = true;


    }

    public void Bug_Corutine()
    {
        StopCoroutine("ReSpawn");

        isSpawn = true;
        Text text = gameObject.transform.GetChild(0).GetComponent<Text>();
        Color c = text.color;
        c.a = 1f;
        text.color = c;

    }
}