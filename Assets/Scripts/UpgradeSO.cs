using System;
using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Objects", menuName = "Amaz/Objects", order = 1)]
public class UpgradeSO : ScriptableObject
{
    public Sprite sprite;
    public string name;
    public string description;
    public GlobEffect effect;
    public int id;

}


