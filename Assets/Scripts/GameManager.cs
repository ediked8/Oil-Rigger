using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("ScriptConnect")]
    public StageManager stageManager;
    public StorageManager storageManager;
    public ExtractionManager extractionManager;
    public EconomyManager economyManager;
    public UIManager uiManager;
   

    //플레이어: 주차 정보, 원금 금액, 남은 원금 금액, 주의 최대 채굴 가능 횟수, 주의 남은 채굴 횟수
    [Header("PlayerInfo")]
    public int monthlyDay;   //한달 스테이지
    public int currentDay;   //현재 일자
    public long totalDebt; // 총 원금
    public long remainingDebt; //남은 원금
    public int monthlyDayCount; //이번 달 남은 시추 가능 횟수
    public TextMeshProUGUI DayCountText;
    public int playerGold;
    public bool isItem= false;
    public StorageData toUseitem;
    public int repairCost;
    public bool isFail = false;
    public int currentDrillLvL;
    public bool isInside = true;




    [Header("Oil and Dril Info")]
    public int oil_Index;





    public static GameManager Instance { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != null)
            {
                Destroy(gameObject);
            }
        }

        monthlyDay = 30;
        currentDay = 1;
        totalDebt = 1000;
        remainingDebt = totalDebt;
        playerGold = 2000;
        currentDrillLvL = 0;

    }

   

    public void UseDrillAction()
    {
        /*채굴 시 호출
        currentDrillCount를 줄이고,
        StageManager에 지층 정보 갱신 요청*/
    }
    public void StoreTime()
    {
        economyManager.PayDay();
    }

    public void CheckMonthEnd()
    {
        monthlyDayCount = currentDay % monthlyDay;

        if (monthlyDayCount == 1)
        {
            //이자 납입 이벤트 실행.
        }
    }

    public void RepayDebt()
    {
        /* 빚 상환 로직
          상환 후 remainingDebt가  0 이하일 때 엔딩 호출*/
    }

    public void CheckGameOver()
    {
        /*35 / 70 / 105일차마다 정산금액을
        못 넘겼을 시 혹은 장비 수리 못할 시 게임 오버 호출*/
    }
}
