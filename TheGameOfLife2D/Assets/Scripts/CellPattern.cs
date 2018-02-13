using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class CellPattern : ScriptableObject {

    public List<Position> Positions;
}

[Serializable]
public class Position
{
    public int x;
    public int y;
}