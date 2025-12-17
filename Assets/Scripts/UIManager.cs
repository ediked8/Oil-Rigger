using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Dictionary<string, ItemSlot> slotList;
    public GameObject prefab;
    public Transform parent;
    StorageManager storageManager;
    CrudeOilScript info;
    public ItemSlot itemSlot;
    public bool isUIOn = false;
    public TextMeshProUGUI imsiTextUI;
    //public Button imsiBtn;
    public TextMeshProUGUI PlayerGold;

    


    private void Awake()
    {
        slotList = new Dictionary<string, ItemSlot>();
    }

    private void Start()
    {
        storageManager = GameManager.Instance.storageManager;
        PlayerGold.text = GameManager.Instance.playerGold.ToString();

    }

    public void ImsiUI()
    {

        StringBuilder sb = new StringBuilder();
        var sorted = storageManager.storage.OrderBy(s => s.Value.item_id);
        foreach (var pair in sorted)
        {
            var item = pair.Value;

            sb.Append(item.item_name);
            sb.Append("x");
            sb.Append(item.item_count);
            sb.Append(" °¡°Ý:");
            sb.Append(item.item_cost);
            sb.Append("\n");
        }
        imsiTextUI.text = sb.ToString();
    }

    public void itemSlotAdd(CrudeOilScript item)
    {
        var exist = storageManager.storage.ContainsKey(item.crudeOilData.item_id.ToString());
        if (exist)
        {

            GameObject obj = Instantiate(prefab, parent);
            itemSlot = obj.GetComponent<ItemSlot>();
            itemSlot.Getiteminfo(item);
            slotList.Add(itemSlot.itemid, itemSlot);
            Debug.Log($"{itemSlot.itemid} Å°·Î ½½·Ô Ãß°¡µÊ");
            ImsiUI();

        }
    }

    public void itemSlotRemove(string itemid)
    {
        var exist = storageManager.storage.ContainsKey(itemid);
        if (!exist)
        {
            slotList[itemid].RemoveSlot();
            slotList.Remove(itemid);
            ImsiUI();


        }
    }

    public void PlayerGoldUpdate() => PlayerGold.text = GameManager.Instance.playerGold.ToString();

}
