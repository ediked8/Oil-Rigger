using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI[] oilCounts;
    public TextMeshProUGUI[] oilPercents;
    public Slider[] sliders;
    public Toggle toggle;
    public Slider drillSlider;
    public float drillpoint = 0.115f;
    public float spd;

    public GameObject onCoin;
    public GameObject offCoin;
    public GameObject[] uiToggleObj;
    public Slider coinSL;

    public Dictionary<string, ItemSlot> slotList;
    public GameObject prefab;
    public Transform parent;
    StorageManager storageManager;
    CrudeOilScript info;
    public ItemSlot itemSlot;
    public bool isUIOn = false;
    public TextMeshProUGUI imsiTextUI;

    public TextMeshProUGUI PlayerGold;
    public TextMeshProUGUI GameMessage;

    public Image PlayerHud;


    private void Awake()
    {
        slotList = new Dictionary<string, ItemSlot>();
    }

    private void Start()
    {
        storageManager = GameManager.Instance.storageManager;
        PlayerGold.text = GameManager.Instance.playerGold.ToString();
        spd = 0.1f;
        oilPercent();
        StartCoroutine(CoinSLupdate());
        
    }

    public void StoredOilCountRefresh()
    {

        int i = GameManager.Instance.oil_Index;
        oilCounts[i].text = storageManager.storage[$"{i}"].item_count.ToString();


    }

    /*public void itemSlotAdd(CrudeOilScript item)
    {
        var exist = storageManager.storage.ContainsKey(item.crudeOilData.item_id.ToString());
        if (exist)
        {

            GameObject obj = Instantiate(prefab, parent);
            itemSlot = obj.GetComponent<ItemSlot>();
            itemSlot.Getiteminfo(item);
            slotList.Add(itemSlot.itemid, itemSlot);
            Debug.Log($"{itemSlot.itemid} 키로 슬롯 추가됨");
            //ImsiUI(); 슬롯 시스템 삭제됨 참조없음.

        }
    }*/

    public void CallSlider()
    {
        StartCoroutine(SliderMove());
    }

    public IEnumerator SliderMove()
    {

        Slider slider = sliders[GameManager.Instance.stageManager.drillCount];

        float duration = 5.0f; // 5초 동안 이동
        float currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime; // 프레임 간격만큼 시간 누적
            float t = currentTime / duration; // 0에서 1 사이의 비율 계산

            slider.value = Mathf.Lerp(0, 1, t); // 계산된 비율 적용
            yield return null; // 다음 프레임까지 대기
        }
        slider.value = 1;
        if (GameManager.Instance.stageManager.doneDrill)
        {
            duration = 2f;
            currentTime = 0f;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime; // 프레임 간격만큼 시간 누적
                float t = currentTime / duration; // 0에서 1 사이의 비율 계산

                slider.value = Mathf.Lerp(1, 0, t); // 계산된 비율 적용
                yield return null; // 다음 프레임까지 대기
            }
            slider.value = 0;

        }



    }

    public void oilPercent()
    {
        if (!GameManager.Instance.isItem)
        {
            for (int i = 0; i < oilPercents.Length; i++)
            {
                oilPercents[i].text = $"{GameManager.Instance.stageManager.OilNodes[i].crudeOilData.probability.ToString()}%";
            }
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.toUseitem.setProbability.Length; i++)
            {
                oilPercents[i].text = $"{GameManager.Instance.toUseitem.setProbability[i].ToString()}%";
            }
        }

    }

    public IEnumerator SliderClear()
    {

        float duration = 2f;
        float currentTime = 0f;
        foreach (Slider s in sliders)
        {
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime; // 프레임 간격만큼 시간 누적
                float t = currentTime / duration; // 0에서 1 사이의 비율 계산

                s.value = Mathf.Lerp(1, 0, t); // 계산된 비율 적용
                yield return null; // 다음 프레임까지 대기
            }
            s.value = 0;
        }


    }


    public void itemSlotRemove(string itemid)
    {
        var exist = storageManager.storage.ContainsKey(itemid);
        if (!exist)
        {
            slotList[itemid].RemoveSlot();
            slotList.Remove(itemid);
            // ImsiUI();슬롯 시스템 삭제됨 참조없음.


        }
    }
    public void ItemSelect()
    {
        if (toggle.isOn)
        {

            GameManager.Instance.isItem = true;

            oilPercent();
        }
        else
        {

            GameManager.Instance.isItem = false;
            oilPercent();
        }
    }

    public IEnumerator DrillSliderMove()
    {



        float duration = 2.0f; // 5초 동안 이동
        float currentTime = 0f;



        while (currentTime < duration)
        {
            currentTime += Time.deltaTime; // 프레임 간격만큼 시간 누적
            float t = currentTime / duration; // 0에서 1 사이의 비율 계산

            drillSlider.value = Mathf.Lerp(drillSlider.value, drillpoint, t); // 계산된 비율 적용
            yield return null; // 다음 프레임까지 대기
        }
        if (GameManager.Instance.stageManager.doneDrill)
        {
            duration = 2f;
            currentTime = 0f;
            yield return new WaitForSeconds(3);
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime; // 프레임 간격만큼 시간 누적
                float t = currentTime / duration; // 0에서 1 사이의 비율 계산

                drillSlider.value = Mathf.Lerp(drillpoint, 0, t); // 계산된 비율 적용
                yield return null; // 다음 프레임까지 대기
            }
            drillSlider.value = 0;
            drillpoint = 0.142f;
            yield break;
        }

        drillSlider.value = drillpoint;
        drillpoint += drillpoint;
    }

    public void storeUI()
    {
        
        if (GameManager.Instance.isShop)
        {
            foreach (GameObject o in uiToggleObj)
            {
                o.SetActive(true);
            }
            if (GameManager.Instance.currentDay % 30 == 0)
            {
                onCoin.SetActive(true);
                offCoin.SetActive(false);
            }
            else
            {
                onCoin.SetActive(false);
                offCoin.SetActive(true);
            }
        }

        if (!GameManager.Instance.isShop)
        {
            foreach (GameObject o in uiToggleObj)
            {
                o.SetActive(false);
            }
        }
    }

    IEnumerator CoinSLupdate()
    {
        yield return new WaitForSeconds(5);
        //coinSL.value  = GameManager.Instance.playerGold / GameManager.Instance.DebtGoldArray[(GameManager.Instance.currentDay / GameManager.Instance.monthlyDay) - 1];
        

    }
    public void PlayerGoldUpdate() => PlayerGold.text = GameManager.Instance.playerGold.ToString();




}
