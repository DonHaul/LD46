using System;
using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Objects", menuName = "AAAAAAAAAA/Objects", order = 1)]
public class ObjectSO : ScriptableObject
{



    //[HideInInspector]
    public ObjectM plac;

}
[Serializable]
public struct ObjectM
{
    
    public GameObject prefab;
    public string name;
    public string description;
    public double virgincps;
    public int count;
    public int id;
    public float clickMoney;
    public float basecost;
    public float curcost;
    public float priceMultiplier;
    public float sellingPercent;

    
}

