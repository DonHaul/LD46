using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorUnlockManager : MonoBehaviour
{

    public static FloorUnlockManager instance;

    public float curprice;
    public float priceMultiplier;

    public GameObject purchaseTooltip;

    private void Awake()
    {

        instance = this;
    }


    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void ShowFloorTooltip(FloorUnlockable floor)
    {
        if (floor != null)
        {
            purchaseTooltip.SetActive(true);

            purchaseTooltip.GetComponentInChildren<Text>().text = "Price: " + curprice.ToString("F1");

            purchaseTooltip.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
            purchaseTooltip.GetComponentInChildren<Button>().onClick.AddListener(() => floor.PurchaseFloor(curprice));

            purchaseTooltip.transform.position = Camera.main.WorldToScreenPoint(floor.transform.position + Vector3.up *-0.5f);
        }    


        else
        {
            
            purchaseTooltip.SetActive(false);
        }


    }
}
