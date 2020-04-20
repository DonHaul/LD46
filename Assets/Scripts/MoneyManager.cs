using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;

    public double cash;
    public double rawCPS=10;
    public Text cpsText;
    public Text cashText;
    // Start is called before the first frame update

    public float updateTime=1;

    public float globalMultiplier = 1;

    string[] sizes = { "", "Milion", "Trilion", "Quintillion", "Septillion","Nonillion","to much illion" };

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartCoroutine(UpdateCashUI());
    }


    IEnumerator UpdateCashUI()
    {
        while (true)
        {
            cash += rawCPS * updateTime;
            yield return new WaitForSeconds(updateTime);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        double len = cash;
        int order = 0;
        while (len >= 1000000 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1000000;
        }

        // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
        // show a single decimal place, and no space.
        string result = String.Format("{0:#,0.#} {1}", len, sizes[order]);
        cpsText.text = rawCPS.ToString("F2");
        cashText.text = result;
    }

    public bool Transaction(double amount)
    {
        if(cash+amount<0)
        {
            return false;
        }
        cash += amount;
        return true;
    }
}
