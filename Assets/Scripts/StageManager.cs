using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    [Header("원유 확률 관련")]
    ExtractionManager extraction;
    GameManager gameManager;
    AudioManager audioManager;

    public int currentLayerIndex;  //현재 굴착한 레이어인덱스
    public CrudeOilScript[] OilNodes;        //원유정보들 
    public int[] ProList; //원유의 확률 내림차순 정렬되어있음
    public int oilSeed; //랜덤정수
    public int oilIndex; //확률에 맞아 떨어지는 인덱스값
    public int oilCount; //체굴 가능한 횟수.
    public StorageData itemData;

    public TextMeshProUGUI result; //테스트용 출력

    [Header("드릴 확률 관련")]
    public Drill_ItemScriptObj[] drillLevel;
    public Drill_ItemScriptObj currentDrillLevel;
    private int drillLayer = 6;
    public int drillSeed;
    public TextMeshProUGUI result2; //테스트용 출력2
    public bool isFail = false;
    public int drillCount; //체굴 가능한 횟수
    bool doneDrill = true;
    string ProResult;

    [Header("드릴 애니메이션")]
    public float animtime;
    public float donetime;

    private void Start()
    {
        extraction = GameManager.Instance.extractionManager;
        gameManager = GameManager.Instance;
        isFail = gameManager.isFail;
        currentDrillLevel = drillLevel[gameManager.currentDrillLvL];
        audioManager = GameManager.Instance.audioManager;


    }
    public void GetItem()
    {
        
        if (!gameManager.isItem)
        {
            ProList = new int[OilNodes.Length];

            foreach (CrudeOilScript imsi in OilNodes)
            {
                ProList[int.Parse(imsi.crudeOilData.oil_id)] = imsi.crudeOilData.probability;
            }
        }

        else
        {
            
            ProList = gameManager.toUseitem.setProbability;   
            foreach(int i in ProList)
            {
                ProResult =  string.Join(", ", ProList);
            }
            Debug.Log($"아이템이 사용된 원유 확률 : {ProResult}\n 아이템 사용 직전 Bool값 : {gameManager.isItem}");
            Debug.Log(gameManager.toUseitem.item_id);
            gameManager.storageManager.ReduceComsumeItem(gameManager.toUseitem.item_id);
            


        }

            oilSeed = Random.Range(1, 101);

        for (int i = 0; i < ProList.Length; i++)
        {
            oilSeed -= ProList[i];
            if (oilSeed <= 0)
            {
                oilIndex = i;
                GameManager.Instance.oil_Index = oilIndex;               
               

                break;
            }

        }


        //테스트출력
        result.text += $"\n{OilNodes[oilIndex].crudeOilData.oil_name}";
        GameManager.Instance.storageManager.AddItem(OilNodes[oilIndex]); //스토리지에 원유 추가.
        GameManager.Instance.uiManager.ImsiUI();


    }

    public void DrillStep()
    {
        if (doneDrill)
        {
            currentLayerIndex = 0;
            doneDrill = false;
        }
        drillLayer = currentDrillLevel.drill_iteminfo.layerPro.Length;
        ProList = new int[drillLayer];
        for (int i = 0; i < drillLayer; i++)
        {

            ProList[i] = currentDrillLevel.drill_iteminfo.layerPro[i];
        }


        int drillpro = currentDrillLevel.drill_iteminfo.layerPro[currentLayerIndex];
        currentLayerIndex++;
        drillSeed = Random.Range(1, 101);
        Debug.Log(drillSeed);
        drillSeed -= drillpro;
        if (drillSeed <= 0 && !isFail) 
        {
            result2.text = $"지층 {currentLayerIndex} 굴착 성공! \n원유 채취를 시작합니다....";
            audioManager.Audio.PlayOneShot(audioManager.audioDic["OilDrillingSuccess"]); //시추 성공 재생

            GetItem(); //굴착 성공 시 원유 가챠 실행.
            extraction.PlayEffect(oilIndex);
            audioManager.FlamePoint.PlayOneShot(audioManager.audioDic["FlameVFX"]); //화염 재생

        }
        else if (isFail)
        {
            doneDrill = true;// 실패 이벤트 발생. 드릴정보에 있는 eventgold 지급 매소드 버튼 투명도 및 Onclick 활성화.
            Debug.Log("실패로직으로 인한 굴착 불가. 수리 금액을 터미널을 통해 납부하십시오");


        }
        else
        {
            doneDrill = true;
            drillSeed = Random.Range(1, 101);            
            audioManager.Audio.PlayOneShot(audioManager.audioDic["OilDrillingFail"]); //시추 실패 재생
            if (drillSeed <= 5)
            {
                isFail = true;
                gameManager.isFail = true;
                Debug.Log("실패로직 작동. 수리 금액을 터미널을 통해 납부하십시오");
                gameManager.repairCost = currentDrillLevel.drill_iteminfo.eventGold;
            }
            Debug.Log($"{currentLayerIndex - 1}까지 굴착 성공하였습니다.");
            extraction.PlayEffect(4);
            audioManager.FlamePoint.PlayOneShot(audioManager.audioDic["BlowholeVFX"]); //기포 재생
            gameManager.currentDay++;
            gameManager.DayCountText.text = $"{gameManager.currentDay} Day";
            if ((gameManager.currentDay % 5) == 0)
            {
                gameManager.StoreTime();
            }
        }


        // UseDrillAction()로 인해 이 함수가 불리고 GetItem()를 currentLayerIndex수의 맞게 반복실행.

    }

    public void RepairSystem()
    {
        if (gameManager.playerGold > gameManager.repairCost) 
        { 
            gameManager.playerGold -= gameManager.repairCost;
            GameManager.Instance.uiManager.PlayerGoldUpdate();
            isFail = !isFail;
            gameManager.isFail = true;
            Debug.Log($"플레이어 골드 :{gameManager.playerGold} 남음");
            //+ 결제소리
        }
        else
        {
            //오류소리
            Debug.Log("플레이어 골드가 부족합니다");
        }
    }

    IEnumerator Cooldown()
    {
        yield return null;
    }
}
