using MikeNspired.XRIStarterKit;
using System.Collections;
using TMPro;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    [Header("원유 확률 관련")]
    ExtractionManager extraction;
    GameManager gameManager;
    AudioManager audioManager;
    UIManager uiManager;

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
    [SerializeField] bool isDrill = false;
    public int drillCount; //체굴 가능한 횟수
    public bool doneDrill = true;
    string ProResult;

    public int test;
    [Header("드릴 애니메이션")]
    public XRLever lever;

    private void Start()
    {
        uiManager = GameManager.Instance.uiManager;
        extraction = GameManager.Instance.extractionManager;
        gameManager = GameManager.Instance;
        isFail = gameManager.isFail;
        currentDrillLevel = drillLevel[gameManager.currentDrillLvL];
        audioManager = GameManager.Instance.audioManager;
        drillCount = 0;


    }
    private void Update()
    {
        if (GameManager.Instance.currentDay == test)
            StartCoroutine(Call());
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
            foreach (int i in ProList)
            {
                ProResult = string.Join(", ", ProList);
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

        GameManager.Instance.storageManager.AddItem(OilNodes[oilIndex]); //스토리지에 원유 추가.
        Debug.Log($"{OilNodes[oilIndex].crudeOilData.oil_id} 아이템 추가 과정 지남.");
        GameManager.Instance.uiManager.StoredOilCountRefresh();


    }

    public IEnumerator DrillStep()
    {
        if (isDrill)
        {
            audioManager.Audio.PlayOneShot(audioManager.audioDic["ErrorSFX"]);
            yield break;
        }
        isDrill = !isDrill;
        audioManager.Audio.PlayOneShot(audioManager.audioDic["Lever"]);

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

            uiManager.StartCoroutine(uiManager.SliderMove());//슬라이더 시작
            uiManager.StartCoroutine(uiManager.DrillSliderMove());
            drillCount++;

            if (!gameManager.isItem)
                audioManager.Audio.PlayOneShot(audioManager.audioDic["Lever"]);
            else
                audioManager.Audio.PlayOneShot(audioManager.audioDic["LeverWithCandy"]);

            yield return StartCoroutine(Cooldown(1));
            audioManager.DrillPoint.PlayOneShot(audioManager.audioDic["OilDrilling"]);
            //드릴 애니메이션 나오고 쿨타임 실행 => 5초 지나면 결과실행.
            yield return StartCoroutine(Cooldown(5));
            audioManager.Audio.PlayOneShot(audioManager.audioDic["OilDrillingSuccess"]); //시추 성공 재생
            result2.text = $"지층 {currentLayerIndex} 굴착 성공! \n원유 채취를 시작합니다....";
            Invoke("GetItem", 0.5f); //굴착 성공 시 원유 가챠 실행.
            extraction.PlayEffect(oilIndex);
            audioManager.FlamePoint.PlayOneShot(audioManager.audioDic["FlameVFX"]); //화염 재생
            lever.Value = false;
            isDrill = lever.Value;

        }
        else if (isFail)
        {
            doneDrill = true;// 실패 이벤트 발생. 드릴정보에 있는 eventgold 지급 매소드 버튼 투명도 및 Onclick 활성화.
            Debug.Log("실패로직으로 인한 굴착 불가. 수리 금액을 터미널을 통해 납부하십시오");
            audioManager.Audio.PlayOneShot(audioManager.audioDic["ErrorSFX"]);//오류소리추가


        }
        else
        {

            doneDrill = true;
            uiManager.StartCoroutine(uiManager.SliderMove());
            uiManager.StartCoroutine(uiManager.DrillSliderMove());
            drillCount = 0;
            if (!gameManager.isItem)
                audioManager.Audio.PlayOneShot(audioManager.audioDic["Lever"]);
            else
                audioManager.Audio.PlayOneShot(audioManager.audioDic["LeverWithCandy"]);

            yield return StartCoroutine(Cooldown(1));
            audioManager.DrillPoint.PlayOneShot(audioManager.audioDic["OilDrilling"]);
            //드릴 애니메이션 나오고 쿨타임 실행 => 5초 지나면 결과실행.
            yield return StartCoroutine(Cooldown(5));
            drillSeed = Random.Range(1, 101);
            audioManager.Audio.PlayOneShot(audioManager.audioDic["OilDrillingFail"]); //시추 실패 재생
            if (drillSeed <= 5)
            {
                isFail = true;
                gameManager.isFail = true;
                audioManager.RepairPoint.PlayOneShot(audioManager.audioDic["RaderBreakSiren"]);
                Debug.Log("실패로직 작동. 수리 금액을 터미널을 통해 납부하십시오");
                gameManager.repairCost = currentDrillLevel.drill_iteminfo.eventGold;
            }
            Debug.Log($"{currentLayerIndex - 1}까지 굴착 성공하였습니다.");
            extraction.PlayEffect(4);
            audioManager.FlamePoint.PlayOneShot(audioManager.audioDic["BlowholeVFX"]); //기포 재생
            gameManager.currentDay++;
            gameManager.DayCountText.text = $"{gameManager.currentDay} Day";


            gameManager.StoreTime();
            if (gameManager.currentDay % 30 != 0)
                audioManager.Audio.PlayOneShot(audioManager.audioDic["ShopBoat"]); //뱃고동 재생
            else
                audioManager.Audio.PlayOneShot(audioManager.audioDic["DebtBoat"]); // 빚 상선 재생

            yield return StartCoroutine(Cooldown(2));
            StartCoroutine(uiManager.SliderClear());//슬라이더 초기화.
            lever.Value = false;
            isDrill = lever.Value;
        }

        yield return null;
    }

    public IEnumerator RepairSystem()
    {
        if (gameManager.playerGold > gameManager.repairCost)
        {
            gameManager.playerGold -= gameManager.repairCost;
            GameManager.Instance.uiManager.PlayerGoldUpdate();
            isFail = !isFail;
            gameManager.isFail = true;
            Debug.Log($"플레이어 골드 :{gameManager.playerGold} 남음");
            audioManager.Audio.PlayOneShot(audioManager.audioDic["Money"]);//+ 결제소리
            yield return StartCoroutine(Cooldown(0.4f));
            audioManager.Audio.PlayOneShot(audioManager.audioDic["RaderFix"]);

        }
        else
        {
            audioManager.Audio.PlayOneShot(audioManager.audioDic["ErrorSFX"]);//오류소리
            Debug.Log("플레이어 골드가 부족합니다");
        }

    }

    IEnumerator Call()
    {
        gameManager.StoreTime();
        if (gameManager.currentDay % 30 != 0)
            audioManager.Audio.PlayOneShot(audioManager.audioDic["ShopBoat"]); //뱃고동 재생
        else
            audioManager.Audio.PlayOneShot(audioManager.audioDic["DebtBoat"]); // 빚 상선 재생
        yield return null;
    }
    public void CallDrillStep()
    {
        StartCoroutine(DrillStep());
    }

    public void CallRepairSysteam()
    {
        StartCoroutine(RepairSystem());
    }
    public IEnumerator Cooldown(float time)
    {
        float timer = Time.time + time;
        while (Time.time < timer)
        {

            yield return null;
        }

    }
}
