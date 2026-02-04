using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    MainGame, GameOver, GameClear
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    Text numberText;

    [SerializeField]
    Text goalText;

    [SerializeField]
    TimerSlider slider;

    private int number;
    private int goalNumber;

    private float spawnDuration = 0.5f;

    public GameState state;

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
        state = GameState.MainGame;
        BoardManager.Instance.Init_Cells();
        BoardManager.Instance.RandomizeCells();
        SetNumber();
        slider.__Init__();
        
    }

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

        VanishNum(c_arr);



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

    private void VanishNum(CellData[] arr)
    {
        foreach(CellData k in arr)
        {
            GameObject textObject = k.transform.GetChild(0).gameObject;
            
            k.gameObject.GetComponent<CellSelectable>().Deselect();
            Color c = textObject.GetComponent<Text>().color;
            c.a = 0;
            textObject.GetComponent<Text>().color = c;
            k.GetComponent<CellSelectable>().ReSpawn(spawnDuration);
        }




    }
}