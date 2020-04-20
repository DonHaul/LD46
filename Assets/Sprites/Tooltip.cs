using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public GameObject bonusFab;
    public Text prods;
    public Text sells;
    public Text fx;
    public Text name;

    public Text bonuses;

    public List<GameObject> createdBonuses;

    // Start is called before the first frame update
    public void SetTooltip(Placeable p)
    {
        



        foreach (var item in createdBonuses)
        {
            if(item!=null)
            Destroy(item);
        }
        createdBonuses.Clear();

        transform.position = Camera.main.WorldToScreenPoint(p.transform.position + Vector3.up * 1);

        name.text = p.stats.name;
        prods.text = " Producing: " + p.cps.ToString("F2") + " each second";
        sells.text = " Sells for: " + p.sellprice.ToString("F2") + "";
        if(p.outgoingEffects.Length>0)
        {
            string oper = "+";
            if(p.outgoingEffects[0].operation == Effect.Type.Multiply)
            {
                oper = "x";
            }

            fx.text = " Effect: " + oper + p.outgoingEffects[0].improvement.ToString() +" ";
        }
        else
        {
            fx.text = "";
        }
        if(p.incomingEffects.Count > 0 && p.globEffects.Count > 0)
        {
            bonuses.gameObject.SetActive(true);
        }else
        {
            bonuses.gameObject.SetActive(false);
        }
        foreach( var e in p.incomingEffects)
        {

            GameObject go = Instantiate(bonusFab, transform.GetChild(0));


            string oper = "+";
            if (e.operation == Effect.Type.Multiply)
            {
                oper = "x";
            }

            go.GetComponent<Text>().text =" " +  oper + e.improvement.ToString() + " 💰";
            createdBonuses.Add(go);

           
        }


        foreach (var e in p.globEffects)
        {

            GameObject go = Instantiate(bonusFab, transform.GetChild(0));

            
            go.GetComponent<Text>().text = " " + "+" + (e.improvement*100).ToString() + "% from the \"" + e.name + "\" Upgrade";
            createdBonuses.Add(go);


        }


        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());
    }

}
