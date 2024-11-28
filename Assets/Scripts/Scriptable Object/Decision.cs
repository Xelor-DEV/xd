using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Decision", menuName = "Game/Decision",order = 1)]
public class Decision : ScriptableObject
{
    public string decisionName;
    public int cost;
    public float satisfactionImpact;
    public int gain;
}
