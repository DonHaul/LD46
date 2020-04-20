using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorUnlockable : MonoBehaviour
{

    public BoxCollider2D bc;

    public Color lockedColor;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = lockedColor;
        }
    }

    // Update is called once per frame
    public void PurchaseFloor(float val)
    {
        if(MoneyManager.instance.Transaction(-val))
        {

        
        //remove collider;
        Destroy(bc);

        

        foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = Color.white;
        }
        }

        FloorUnlockManager.instance.curprice *= FloorUnlockManager.instance.priceMultiplier;

        FloorUnlockManager.instance.purchaseTooltip.SetActive(false);
    }

    private void OnMouseOver()
    {
        FloorUnlockManager.instance.ShowFloorTooltip(this);
    }

    private void OnMouseExit()
    {
        FloorUnlockManager.instance.ShowFloorTooltip(null);
    }
   
}
