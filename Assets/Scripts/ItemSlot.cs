using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public CrudeOilScript info;
    public string itemid = "test";
    public TextMeshProUGUI itemName_Count;
    public int itemCount = 1;
    public Toggle toggle;
    

    

    private void Awake()
    {

        toggle = GetComponentInChildren<Toggle>();
        toggle.group = GetComponentInParent<ToggleGroup>();
        itemName_Count = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        
    }
    public void Getiteminfo(CrudeOilScript item)
    {
        info = item;
        itemid = info.crudeOilData.item_id.ToString();
        itemName_Count.text = $"{item.crudeOilData.oil_name.ToString()} x{itemCount.ToString()}";

    }



    public void RemoveSlot()
    {
        if (itemCount <= 0)
            Destroy(gameObject);
    }



    public void ItemSelect()
    {
        if (toggle.isOn)
        {
            GameManager.Instance.isItem = true;
            GameManager.Instance.toUseitem = new StorageData(info);
        }
        else
        {
            GameManager.Instance.isItem = false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created


}
