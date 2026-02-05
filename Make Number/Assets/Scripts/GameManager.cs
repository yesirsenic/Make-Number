using System.Collections;
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

    [SerializeField]
    GameObject ClearPopup;

    [SerializeField]
    GameObject GameOverTimer;

    [SerializeField]
    GameObject GameOverPopup;

    [SerializeField]
    Text LevelText;

    private int number;
    private int goalNumber;
    private int level;

    private float spawnDuration = 0.5f;

    public GameState state;

    private void Awake()
    {
        Instance = this;

        if (PlayerPrefs.GetInt("Level") == 0)
        {
            PlayerPrefs.SetInt("Level", 1);
        }
    }

    private void Start()
    {
        __Init__();
    }
    
    public void __Init__()
    {
        state = GameState.MainGame;
        BoardManager.Instance.Init_Cells();
        BoardManager.Instance.RandomizeCells();
        SetNumber();
        slider.__Init__();
        level = PlayerPrefs.GetInt("Level");
        LevelText.text = "Lv. " + level.ToString();
        
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

    public void GameOverStart()
    {
        StartCoroutine(GameOver());
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

        CheckClear();

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

    private void CheckClear()
    {
        if(number == goalNumber)
        {
            state = GameState.GameClear;
            level += 1;
            PlayerPrefs.SetInt("Level", level);
            ClearPopup.SetActive(true);
        }
    }

    

    IEnumerator GameOver()
    {
        state = GameState.GameOver;

        GameOverTimer.SetActive(true);

        Animator anim = GameOverTimer.GetComponent<Animator>();

        // Animator 한 프레임 기다려서 상태 갱신
        yield return null;

        // 현재 재생 중인 애니메이션 길이만큼 대기
        yield return new WaitForSeconds(
            anim.GetCurrentAnimatorStateInfo(0).length
        );

        GameOverTimer.SetActive(false);

        GameOverPopup.SetActive(true);
    }
}