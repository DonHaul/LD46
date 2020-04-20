using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Objects", menuName = "Amaz/Dialog", order = 1)]
public class Dialog : ScriptableObject
{
    public string name;
    public Sprite sprite;
    [TextArea(2,6)]
    public string[] sentences;

    public Dialog next;
}
