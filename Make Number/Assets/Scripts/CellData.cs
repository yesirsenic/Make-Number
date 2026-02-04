using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    Number,
    Operator
}

public enum OperatorType
{
    Add,    // +
    Sub,    // -
    Mul,    // ¡¿
    Div     // ¡À
}

public class CellData : MonoBehaviour
{
    public CellType cellType;

    public int num;
    public OperatorType operatorType;

}
