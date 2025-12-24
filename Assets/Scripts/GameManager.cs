using System.Collections;
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
    public AudioManager audioManager;


    //플레이어: 주차 정보, 원금 금액, 남은 원금 금액, 주의 최대 채굴 가능 횟수, 주의 남은 채굴 횟수
    [Header("PlayerInfo")]
    public int monthlyDay;   //한달 스테이지
    public int currentDay;   //현재 일자
    public long totalDebt; // 총 원금
    public long remainingDebt; //남은 원금
    public int monthlyDayCount; //이번 달 남은 시추 가능 횟수
    public TextMeshProUGUI DayCountText;
    public int playerGold;
    public bool isItem = false;
    public StorageData toUseitem;
    public int repairCost;
    public bool isFail = false;
    public int currentDrillLvL;
    public bool isInside = true;
    public int[] DebtGoldArray;
    public bool isShop = false;

    public float timer;
    public float expireTime;


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
        totalDebt = 220000;
        remainingDebt = totalDebt;
        playerGold = 7500;
        currentDrillLvL = 0;
        DebtGoldArray = new int[3] { 50000, 70000, 100000 };

    }


    private void Start()
    {
        StartCoroutine("StartGame");
        toUseitem = new StorageData(GameManager.Instance.economyManager.list[0]);
    }

    public void StoreTime()
    {
        if (currentDay % 15 == 0)
        {
            isShop = true;
            //audioManager.Audio.PlayOneShot(audioManager.audioDic["ShopBoat"]);
            uiManager.storeUI();
        }
        else
        {
            isShop = false;
            uiManager.storeUI();

        }
    }

    public void CheckMonthEnd()
    {
        StartCoroutine("CheckGameOver");
        monthlyDayCount = currentDay % monthlyDay;

        if (monthlyDayCount == 0)
        {
            audioManager.Audio.PlayOneShot(audioManager.audioDic["DebtBoat"]);
            uiManager.GameMessage.text = "수금을 위한 상선 도착.";

        }
    }

    public void RepayDebt() //버튼에 할당할 정산기능
    {
        if (currentDay % 30 == 0)
        {
            playerGold -= DebtGoldArray[(currentDay / monthlyDay) - 1];
            totalDebt -= DebtGoldArray[(currentDay / monthlyDay) - 1];
            audioManager.Audio.PlayOneShot(audioManager.audioDic["Money"]);
            uiManager.PlayerGoldUpdate();
            uiManager.totalDebtUp();
           // uiManager.GameMessage.text = $"{currentDay / monthlyDay}달차 빛 정산 완료!";
        }
        else
        {
            //에러소리
          //  uiManager.GameMessage.text = "수금을 위한 상선이 도착하지 않았습니다";
        }
    }


    IEnumerator CheckGameOver()
    {
        if (playerGold < DebtGoldArray[(currentDay / monthlyDay) - 1])
        {
            uiManager.GameMessage.text = "정산에 실패하여 폐업하게 되었습니다...";
            /*30 / 60 / 90일차마다 정산금액을
       못 넘겼을 시 혹은 장비 수리 못할 시 게임 오버 호출*/
        }

        yield return null;


    }

    IEnumerator StartGame()
    {
        yield return null;
        audioManager.Audio.PlayOneShot(audioManager.audioDic["ShopBoat"]);

    }
}
