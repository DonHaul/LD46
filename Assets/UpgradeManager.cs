using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;
    public UpgradeSO[] upgrades;

    public GameObject upgradefab;

    public GameObject upgradeContainer;

    public List<UpgradeItem> buttons;

    public List<GlobEffect> globEffects;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //generate upgrades
        for (int i = 0; i < upgrades.Length; i++)

        {
            upgrades[i].id = i;
            Debug.Log("New Item");
            GameObject go = Instantiate(upgradefab, upgradeContainer.transform);
            go.GetComponent<UpgradeItem>().SetItem(upgrades[i]);

            upgrades[i].effect.name = upgrades[i].name;

            buttons.Add(go.GetComponent<UpgradeItem>());
        }
        
        
    }

    // Update is called once per frame
    public void PurchaseUpgrade(int id)
    {
        Debug.Log(id);
        globEffects.Add(upgrades[id].effect);

        Destroy(buttons[id].gameObject);

        foreach (Placeable item in GameManager.instance.meubles)
        {
            if(upgrades[id].effect.prefabAffected  == null || upgrades[id].effect.prefabAffected==item.stats.prefab)
            {
                item.globEffects.Add(upgrades[id].effect);
                item.ApplyGlobalEffects();
            }
        }

    }


}
