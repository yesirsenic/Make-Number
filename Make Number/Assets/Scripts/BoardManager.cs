using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;

    [SerializeField]
    CellData[] cells;

    private void Awake()
    {
        Instance = this;
    }

    public void RandomizeCells()
    {
        foreach (var cell in cells)
        {
            switch (cell.cellType)
            {
                case CellType.Number:
                    cell.num = Random.Range(1, 10); // 1~9
                    cell.transform.GetChild(0).GetComponent<Text>().text = cell.num.ToString();
                    break;

                case CellType.Operator:
                    cell.operatorType =
                        (OperatorType)Random.Range(0, System.Enum.GetValues(typeof(OperatorType)).Length);
                    cell.transform.GetChild(0).GetComponent<Text>().text = SetOperatorText(cell.operatorType);
                    break;
            }
        }
    }

    public void Init_Cells()
    {
        foreach (var cell in cells)
        {
            cell.GetComponent<CellSelectable>().Bug_Corutine();
        }
    }

    public void ReRandomizeCell(CellData cell)
    {
        if(cell.cellType == CellType.Number)
        {
            cell.num = Random.Range(1, 10); // 1~9
            cell.transform.GetChild(0).GetComponent<Text>().text = cell.num.ToString();
        }

        else
        {
            cell.operatorType =
                        (OperatorType)Random.Range(0, System.Enum.GetValues(typeof(OperatorType)).Length);
            cell.transform.GetChild(0).GetComponent<Text>().text = SetOperatorText(cell.operatorType);
        }
    }

    private string SetOperatorText(OperatorType type)
    {
        switch(type)
        {
            case OperatorType.Add:
                return "+";

            case OperatorType.Sub:
                return "-";

            case OperatorType.Mul:
                return "x";

            case OperatorType.Div:
                return "¡À";
        }

        return "?";
    }
}
