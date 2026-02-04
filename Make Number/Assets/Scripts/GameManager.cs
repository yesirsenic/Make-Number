using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    Text numberText;

    [SerializeField]
    Text goalText;

    private int number;
    private int goalNumber;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        __Init__();
    }
    
    void __Init__()
    {
        BoardManager.Instance.RandomizeCells();
        SetNumber();
    }

    // ⭐ 핵심 함수
    public void OnCellsSelected(CellSelectable first, CellSelectable second)
    {
        CellData a = first.GetComponent<CellData>();
        CellData b = second.GetComponent<CellData>();

        if (a == null || b == null)
        {
            Debug.LogError("CellData missing");
            return;
        }

        CellData[] c_arr = { a, b };


        CalulateNum(c_arr);

    }

    private void CalulateNum(CellData[] arr)
    {
        CellData op_Cell = null; ;
        CellData num_Cell = null;

        foreach (CellData k in arr)
        {
            if (k.cellType == CellType.Number)
            {
                num_Cell = k;
            }

            else
            {
                op_Cell = k;
            }
        }

        if(num_Cell == null || op_Cell == null)
        {
            Debug.LogError("현재 셀에 제대로된 값이 들어오지 못했습니다.");
        }

        int num = num_Cell.num;
        
        switch(op_Cell.operatorType)
        {
            case OperatorType.Add:
                number += num;
                break;

            case OperatorType.Sub:
                number -= num;
                break;

            case OperatorType.Mul:
                number *= num;
                break;

            case OperatorType.Div:
                number /= num;
                break;
        }

        numberText.text = number.ToString();

    }

    private void SetNumber()
    {
        number = Random.Range(1, 100);
        numberText.text = number.ToString();

        do
        {
            goalNumber = Random.Range(1, 100);
        }
        while (goalNumber == number);

        goalText.text = goalNumber.ToString();


    }
}