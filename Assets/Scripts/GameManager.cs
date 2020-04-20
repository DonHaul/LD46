using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public enum Mode { Play,Editing, Deleting,Move };
    [SerializeField]
    public Mode mode =Mode.Play;

    public static GameManager instance;

    public GameObject object2place;

    public Placeable curSelected;

    public GameObject[] prefabs;

    int curmeubleId = 0;

    public ObjectM[] objArr;

    public List<Placeable> meubles;

    public bool update=false;

    public Text toolmodetext;

    public Vector3 mouseprevpos;
    public Vector3 mousepos;

    public Vector3 mouseposb4drag;
    public Vector3 camposb4drag;
    public float dragSpeed = 1;
    public float dragLim = 10;

    public Tooltip tooltip;


    public bool canPlace = true;

    public GameObject shopContainer;
    public GameObject shopItemFab;

    public Camera cam;

    public GameObject shopTooltip;

    public Transform map;

    public GameObject manualgo;

    public List<ShopItem> shopitems;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        
        objArr = new ObjectM[prefabs.Length];
        for (int i = 0; i < prefabs.Length; i++)
        {
            objArr[i] = prefabs[i].GetComponent<Placeable>().stats;

            objArr[i].curcost = prefabs[i].GetComponent<Placeable>().stats.basecost;
            objArr[i].prefab = prefabs[i];

            prefabs[i].GetComponent<Placeable>().stats.id = i;
            objArr[i].id = i;
        }

        meubles = map.GetComponentsInChildren<Placeable>().ToList();

        for (int i = 0; i < meubles.Count; i++)
        {
            meubles[i].placed = true;

            for (int j = 0; j < objArr.Length; j++)
            {
                if (meubles[i].stats.prefab==objArr[j].prefab)
                {
                    Debug.Log("SET");
                    meubles[i].stats = objArr[j];
                    break;
                }
            }
            
        }


        //generate shop
        foreach (var item in prefabs)
        {

            GameObject go = Instantiate(shopItemFab, shopContainer.transform);
            go.GetComponent<ShopItem>().SetItem(item.GetComponent<Placeable>());

            shopitems.Add(go.GetComponent<ShopItem>());
        }

        cam = Camera.main;

        //generate upgrades
        
    }

    public void ToggleShopTooltip(string description)
    {
        if (description != "")
        {
            shopTooltip.SetActive(true);
            shopTooltip.GetComponentInChildren<Text>().text = description;
            shopTooltip.transform.position = new Vector3(shopTooltip.transform.position.x, Input.mousePosition.y, shopTooltip.transform.position.z) ;

        }
        else
        {

            shopTooltip.SetActive(false);
        }
    }

    public void ToggleTooltip(Placeable placeab = null)
    {
        if(placeab != null)
        {
            tooltip.gameObject.SetActive(true);
            tooltip.SetTooltip(placeab);

        }else
        {

            tooltip.gameObject.SetActive(false);
        }
    }


    public void AjouterMeuble(int idx)
    {

        if (MoneyManager.instance.Transaction(-objArr[idx].curcost))
        {
            Placeable placeab = object2place.GetComponent<Placeable>();

            meubles.Add(placeab);
            objArr[idx].count++;
            
            object2place = null;

            placeab.Configure(objArr[idx]);

            objArr[idx].curcost = objArr[idx].curcost * objArr[idx].priceMultiplier;

            update = true;

            shopitems[idx].UpdateItem(objArr[idx]);

        }
        else
        {
            Debug.Log("Looks Likes Your are Poor");
            Destroy(object2place);
        }

    }

    private void FixedUpdate()
    {
        toolmodetext.text = mode.ToString();
    }

    private void LateUpdate()
    {
        if(update)
        {
            Debug.Log("Updating Effects");
            //it has to be done here, because Destroy, takes a bit and ends just b4 this call
            update = false;
            RecalculateCps();
        }
    }

    void RecalculateCps()
    {
        //goes thorugh all thing and updates effect
        foreach (var m in meubles)
        {
            m.RefreshEffects();
            
        }

        foreach (var m in meubles)
        {
            m.RecalculateCps();

        }

        foreach (var m in meubles)
        {
            m.ApplyGlobalEffects();

        }
       
    }

   
    public void SupprimerMeuble(Placeable  plac)
    {
        //MoneyManager.instance.Transaction(obj.se);

        MoneyManager.instance.Transaction(plac.sellprice);

        MoneyManager.instance.rawCPS -= plac.cps;
        objArr[plac.stats.id].count--;


        //rollback price
        objArr[plac.stats.id].curcost = objArr[plac.stats.id].curcost / objArr[plac.stats.id].priceMultiplier;


            //remove self

        meubles.Remove(plac);

        Destroy(plac.gameObject);
        //recalculate others
        update = true;
        tooltip.gameObject.SetActive(false);


        shopitems[plac.stats.id].UpdateItem(plac.stats);


    }

    public void CreatePreviewMeuble(int idx)
    {
        mode = Mode.Play;

        object2place = Instantiate(objArr[idx].prefab);

        curmeubleId = idx;

        Effect[] effects = object2place.GetComponent<Placeable>().outgoingEffects;
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].area.localScale = Vector3.one * effects[i].radius*2;
        }
    }


    // Update is called once per frame
    void Update()
    {

        Vector3 p = Input.mousePosition;
        p.z = 20;
        mousepos = Camera.main.ScreenToWorldPoint(p);

        if(Input.GetKeyDown(KeyCode.D))
        {
            
            if(mode!=Mode.Deleting)
            {
                mode = Mode.Deleting;
                if (object2place != null)
                {
                    Destroy(object2place);

                }

                if (curSelected!=null)
                {
                    curSelected.HightlightDelete();
                }
                
            }
            else
            {
                mode = Mode.Play;
                if (curSelected != null)
                {
                    curSelected.UnhightlightDelete();
                }
               
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (mode != Mode.Move)
            {
                mode = Mode.Move;
                if (object2place != null)
                {
                    Destroy(object2place);

                }

                if (curSelected != null)
                {
                   
                }

            }
            else
            {
                mode = Mode.Play;
                if (curSelected != null)
                {
                   
                }

            }
        }


        if (object2place == null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                mode = Mode.Play;

                CreatePreviewMeuble(0);




            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                mode = Mode.Play;
                CreatePreviewMeuble(1);

            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                mode = Mode.Play;
                CreatePreviewMeuble(2);

            }
        }

        if (mode == Mode.Play)
        { 
            if (object2place != null)
        {
            object2place.transform.position = mousepos;

            if(Input.GetMouseButtonDown(0) && canPlace)
            {
                AjouterMeuble(curmeubleId);

            }
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            //hide all tooltips
            FloorUnlockManager.instance.purchaseTooltip.SetActive(false);
            //tooltip.gameObject.SetActive(false);

            mouseposb4drag = mousepos;
        }
       if( Input.GetMouseButton(1))
            {
            float mag = Vector3.Magnitude(mousepos - mouseprevpos); 
            if(mag>dragLim)
            {
                mag = dragLim;
            }
            cam.transform.position += (mousepos - mouseprevpos).normalized * mag;
        }

        mouseprevpos = mousepos;
    }

    public void ToggleManual(bool val)
    {
        manualgo.SetActive(val);
    }

}
