
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class Effect
    {
    [SerializeField]
    public enum Type { Add, Multiply};
    [SerializeField]
    public Placeable origin;
    public Type operation = Type.Add;
    public float radius;
    public GameObject prefabAffected;
    public float improvement;
    public Transform area;
    }

