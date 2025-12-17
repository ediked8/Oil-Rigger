using UnityEngine;

public class StorageData
{
    public string item_type;
    public string item_id;
    public string item_name;
    public string item_desc;
    public int item_count;
    public int item_cost;
    public int[] setProbability;
    public Sprite item_img;

    public StorageData(CrudeOilScript oilItem)
    {
        item_type = oilItem.crudeOilData.itemtype.ToString();
        item_id = oilItem.crudeOilData.oil_id;
        item_desc = oilItem.crudeOilData.item_desc;
        item_name = oilItem.crudeOilData.oil_name;
        item_count = 1; // 기본 카운트는 1로 설정
        item_cost = oilItem.crudeOilData.oil_cost;
        setProbability = oilItem.crudeOilData.setProbability;
        // item_img = oilItem.crudeOilData.oil_img;
    }



}