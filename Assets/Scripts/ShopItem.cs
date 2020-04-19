using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour
{

    public Text name;
    public Text amount;
    public Text cost;
    public Image img;
    public Button btn;

    public EventTrigger trigger;

    public int id = -1;
    public string description;

    // Start is called before the first frame update
    public void SetItem(Placeable p)
    {
        name.text = p.stats.name;
        amount.text = p.stats.count.ToString();
        cost.text = p.stats.curcost.ToString("F2");
        id = p.stats.id;
        img.sprite = p.gameObject.GetComponent<SpriteRenderer>().sprite;
        int idx = id;
        description = p.stats.description + "\n\nClick or Press " + (idx+1).ToString() + " to purchase";
        Debug.Log(idx);
        btn.onClick.AddListener(() => GameManager.instance.CreatePreviewMeuble(idx));


        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { GameManager.instance.ToggleShopTooltip(description); });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((eventData) => { GameManager.instance.ToggleShopTooltip(""); });
        trigger.triggers.Add(entry);
    }

    // Update is called once per frame
    public void UpdateItem(ObjectM p)
    {
        amount.text = p.count.ToString();
        cost.text = p.curcost.ToString("F2");
    }
}
