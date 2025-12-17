using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{

    public Dictionary<string, int> PriceList; //시세용.
    public TextMeshProUGUI priceTable; //시세표기.


    public Dictionary<string, CrudeOilScript> ComsumeList; //소비아이템리스트
    public CrudeOilScript[] list;   //아이템리스트


    public GameObject itemButtonPrefab; //버튼프리팹
    public Transform parent;
    CrudeOilScript data;




    private void Start()//아이템 구매버튼 활성화 //구매는 버튼프리 프리팹에 기능 포함.
    {
        PriceList = new Dictionary<string, int>();
        ComsumeList = new Dictionary<string, CrudeOilScript>();
        for (int i = 0; i < list.Length; i++)
        {
            ComsumeList.Add(list[i].crudeOilData.oil_name, list[i]);
            GameObject obj = Instantiate(itemButtonPrefab, parent);
            obj.GetComponent<ComsumePrefab>().data = list[i];


        }
    }

    public void PayDay()//원유시세표 활성화
    {

        PriceList.Clear();
        for (int i = 0; i < GameManager.Instance.stageManager.OilNodes.Length; i++)
        {
            data = GameManager.Instance.stageManager.OilNodes[i];
            PriceList.Add(data.crudeOilData.oil_name, data.crudeOilData.oil_cost);
            priceTable.text += $"{data.crudeOilData.oil_name} : {data.crudeOilData.oil_cost}\n";
        }

    }

    public void SellAllOil()
    {
        int sum = 0;
        Dictionary<string, StorageData> imsi = GameManager.Instance.storageManager.storage;
        foreach (StorageData data in GameManager.Instance.storageManager.storage.Values)
        {
            if (data.item_type == "oil")
            {
                sum += data.item_cost * data.item_count;
            }
        }
        GameManager.Instance.playerGold += sum;
        GameManager.Instance.uiManager.PlayerGoldUpdate();
        GameManager.Instance.storageManager.RemoveOilItem();

    }

    public void DriilUpgrade()
    {
        if (GameManager.Instance.currentDrillLvL < 2)
        {
            if (GameManager.Instance.stageManager.drillLevel[GameManager.Instance.currentDrillLvL+1].drill_iteminfo.upgradeCost > GameManager.Instance.playerGold)
            {
                //오류소리
                Debug.Log("금액부족");
                return;
            }
            GameManager.Instance.currentDrillLvL++;
            switch (GameManager.Instance.currentDrillLvL)
            {
                case 1:
                    GameManager.Instance.stageManager.currentDrillLevel = GameManager.Instance.stageManager.drillLevel[1];
                    GameManager.Instance.playerGold -= GameManager.Instance.stageManager.currentDrillLevel.drill_iteminfo.upgradeCost;
                    //구매소리
                    break;
                case 2:
                    GameManager.Instance.stageManager.currentDrillLevel = GameManager.Instance.stageManager.drillLevel[2];
                    GameManager.Instance.playerGold -= GameManager.Instance.stageManager.currentDrillLevel.drill_iteminfo.upgradeCost;
                    //구매소리
                    break;

            }
        }
        else
        {
            //오류소리
            Debug.Log("드릴 레벨이 최대치입니다");
        }

    }







}
