using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using UnityEngine.EventSystems;


public class UpgradeItem : MonoBehaviour
{
    public Image imag;

    public Button btn;

    public EventTrigger trigger;

    public string name;
    public string description;
    public int id;

    // Start is called before the first frame update
    public void SetItem(UpgradeSO u)
    {
        imag.sprite = u.sprite;
        name = u.name;
        id = u.id;
        int idx = id;
        description = u.description;
        Debug.Log(idx);
        btn.onClick.AddListener(() => UpgradeManager.instance.PurchaseUpgrade(idx));


        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { GameManager.instance.ToggleShopTooltip(name + "\n\n" + description); });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((eventData) => { GameManager.instance.ToggleShopTooltip(""); });
        trigger.triggers.Add(entry);
    }

}
