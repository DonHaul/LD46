using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{

    public bool placed= false;
    public bool curPosInvalid = false;

    public SpriteRenderer sr;
    // Start is called before the first frame update

    public ObjectM stats;

    public Effect[] outgoingEffects;

    public Color originalColor;

    public Vector3 posb4Drag;
    public Vector3 reldragpos;

    public List<Effect> incomingEffects;

    public double cps;

    public float sellprice;
    


    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        


    }
    public void RefreshEffects()
    {


        //update incomming effect
        /*
        for (int i = 0; i < incomingEffects.Count; i++)
        {
            

            //origin was removed
            if (incomingEffects[i].origin==null)
            {
                Debug.Log("Effect Removed");
                incomingEffects.RemoveAt(i);
            }
            

            //origin is to far

            
        }*/
        incomingEffects.Clear();


        //update outgoing in radius
        for (int i = 0; i < outgoingEffects.Length; i++)
        {
            outgoingEffects[i].origin = this;


            Collider2D[] colss = Physics2D.OverlapCircleAll(transform.position, outgoingEffects[i].radius);

            foreach (var col in colss)
            {
                Placeable p = col.gameObject.GetComponent<Placeable>();

                //dont add effect to itself
                if (p != null && p != this)
                {
                    Effect e = outgoingEffects[i];
                    if (e != null)
                    {
                        //if effect doesnot yet exist

                        if (p.incomingEffects.IndexOf(e) == -1)
                        {
                            p.incomingEffects.Add(e);
                        }

                    }
                }


            }
        }


        
    }

    public void Configure(ObjectM objm)
    {
        stats = objm;
        placed = true;
        cps = stats.virgincps;

        sellprice = stats.curcost * stats.sellingPercent;

        MoneyManager.instance.rawCPS += cps;

        objm.curcost = objm.basecost * objm.priceMultiplier;

        //because off effects
        //RecalculateCps();
    }

    public void RecalculateCps()
    {

        MoneyManager.instance.rawCPS -= cps;


        //update cps
        cps = stats.virgincps;
        foreach (var eff in incomingEffects)
        {
            if(eff.operation==Effect.Type.Add)
            {
                cps += eff.improvement;
            }
           
        }
        foreach (var eff in incomingEffects)
        {
            if (eff.operation == Effect.Type.Multiply)
            {
                cps *= eff.improvement;
            }
        }

        MoneyManager.instance.rawCPS += cps;
    }


    void OnMouseDown()
    {

        if (GameManager.instance.mode == GameManager.Mode.Play)
        {
            //replace so that all placeable have a click money value
            
                //MoneyManager.instance.Transaction(obj.clickMoney);
            
        }
        else if (GameManager.instance.mode == GameManager.Mode.Deleting)
        {
            GameManager.instance.SupprimerMeuble(this);
        }

        if (GameManager.instance.mode == GameManager.Mode.Move)
        {
            posb4Drag = transform.position;
            reldragpos = transform.position - GameManager.instance.mousepos;
            placed = false;
            sr.sortingLayerName = "Foreground";
        }
            

    }

    public void HightlightDelete()
    {
        
        sr.color = Color.red;
    }
    public void UnhightlightDelete()
    {
        sr.color = originalColor;
    }

    private void OnMouseEnter()
    {
        
        GameManager.instance.curSelected = this;

        if (GameManager.instance.mode == GameManager.Mode.Deleting)
        {
            HightlightDelete();
        }

        //dont show tooltip on preview object
        if(GameManager.instance.object2place!= GameManager.instance.curSelected.gameObject)
        {
            GameManager.instance.ToggleTooltip(this);
        }
            

    }

    private void OnMouseUp()
    {
        if (GameManager.instance.mode == GameManager.Mode.Move)
        {
            if (GameManager.instance.canPlace == false)
            {
                transform.position = posb4Drag;
                sr.color = originalColor;
            }else
            {
                GameManager.instance.update = true;
            }
            placed = true;
            sr.sortingLayerName = "Default";
        }
    }

    private void OnMouseDrag()
    {
        if (GameManager.instance.mode == GameManager.Mode.Move)
        {
            transform.position = reldragpos + GameManager.instance.mousepos;
           
        }
    }

    private void OnMouseExit()
    {

        GameManager.instance.curSelected = null;

        if (GameManager.instance.mode == GameManager.Mode.Deleting)
        {
            UnhightlightDelete();
        }
        GameManager.instance.ToggleTooltip(null);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
  
        if(collision.tag !="Interaction")
        {
            if (!placed)
            {
                sr.color = Color.red;
                curPosInvalid = true;


            }
            GameManager.instance.canPlace = false;
        }




    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Interaction")
        {
            if (!placed)
        {
            curPosInvalid = false;
            sr.color = originalColor;
        }
        Debug.Log("Can Place");
        GameManager.instance.canPlace = true;
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (outgoingEffects.Length > 0)
            Gizmos.DrawWireSphere(transform.position, outgoingEffects[0].radius);
    }


}

