using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class StorageManager : MonoBehaviour
{
    public Dictionary<string, StorageData> storage = new Dictionary<string, StorageData>();

    public void AddItem(CrudeOilScript crudeOilitem)
    {
        string itemId = crudeOilitem.crudeOilData.oil_id;
        if (storage.ContainsKey(itemId))
        {

            storage[itemId].item_count++;
        }

        else
        {

            storage.Add(itemId, new StorageData(crudeOilitem));

        }

    }

    public void AddComsumeItem(CrudeOilScript comsumeItem)
    {
        if (comsumeItem.crudeOilData.itemtype != Itemtype.comsume)
        {
            Debug.Log("소비아이템이 아닙니다.- 에러 -");
            return;
        }
        string itemId = comsumeItem.crudeOilData.item_id.ToString();
        if (GameManager.Instance.playerGold > comsumeItem.crudeOilData.oil_cost)
        {
            if (storage.ContainsKey(itemId))
            {

                storage[itemId].item_count++;
                GameManager.Instance.uiManager.slotList[itemId].itemName_Count.text = $"{storage[itemId].item_name} x{storage[itemId].item_count}";
            }

            else
            {

                storage.Add(itemId, new StorageData(comsumeItem));
                //+구매소리 

                Debug.Log("소비아이템 추가됨");
                GameManager.Instance.uiManager.itemSlotAdd(comsumeItem);
            }
            GameManager.Instance.playerGold -= comsumeItem.crudeOilData.oil_cost;
            GameManager.Instance.uiManager.PlayerGoldUpdate();
        }
        else
        {
            //+오류소리
            Debug.Log("플레이어 골드가 부족합니다");
        }
        GameManager.Instance.uiManager.ImsiUI();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
    }

    public void ReduceComsumeItem(string id)
    {
        if (storage[id].item_type != "comsume")
        {
            Debug.Log(storage[id].item_type);
            Debug.Log("소비아이템이 아닙니다.- 에러 -");
            return;
        }
        string itemId = (storage[id].item_id.ToString());
        if (storage.ContainsKey(itemId))
        {

            storage[itemId].item_count--;
            GameManager.Instance.uiManager.slotList[itemId].itemCount = storage[itemId].item_count;
            GameManager.Instance.uiManager.slotList[itemId].itemName_Count.text = $"{storage[itemId].item_name} x{storage[itemId].item_count}";
            Debug.Log("소비아이템 소비됨.");
            if (storage[itemId].item_count <= 0)
            {
                Debug.Log("삭제절차 진행");
                RemoveSpecifyitem(itemId);
            }
        }
    }

    public void RemoveOilItem()
    {  //원유 제거 로직
        List<string> keyList = new List<string>();
        foreach (string s in storage.Keys.ToList())
        {
            if (int.Parse(s) < 10)
            {
                keyList.Add(s);
            }
        }
        foreach (string key in keyList)
        {
            storage.Remove(key);
        }
        GameManager.Instance.uiManager.ImsiUI();
    }

    public void RemoveSpecifyitem(string itemid)
    {
        if(int.Parse(itemid)>9 ) //게임메니져가  아이템슬롯을 참고하고 있어서 스토리지 삭제 시 비어있으면 키참조오류발생.
        {
            GameManager.Instance.isItem = !GameManager.Instance.isItem ;
        }
        List<string> keyList = new List<string>();
        keyList.Add(itemid);
        foreach (string key in keyList)
        {
            storage.Remove(key);
            GameManager.Instance.uiManager.itemSlotRemove(itemid);
        }
    }


}
