using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


[System.Serializable]
public class CollectionAcquire
{ public int[] collectionInfos; }

[System.Serializable]
public class ChallengeAcquire
{
    public int[] challengeCriterions;
}

public class GameManager : MonoBehaviour
{
    #region 인스펙터 및 변수 생성
    public static GameManager instance;

    [Header("[ Money ]")]
    public Text CoinText;
    public Text HeartText;
    public Text MedalText;
    public Text DotoriText;
    public Text coinAnimText;
    public Text heartAnimText;

    [Header("[ Object ]")]
    private Globalvariable globalVar;
    public GameObject stemPrefab; //프리팹
    public GameObject bugPrefab;

    public List<GameObject> farmObjList = new List<GameObject>();
    public List<GameObject> stemObjList = new List<GameObject>();
    public List<Farm> farmList = new List<Farm>();
    public List<Stem> stemList = new List<Stem>();
    public List<Bug> bugList = new List<Bug>();

    [Header("[ Truck ]")]
    public GameObject TruckObj;
    public GameObject TruckPanel;
    Transform target;
    public Text truckCoinText;
    public Text truckCoinBonusText;
    public int bonusTruckCoin;
    public Text truckCountNowText;
    public Text truckCountMaxText;

    /*public const int TRUCK_CNT_LEVEL_0 = Globalvariable.TRUCK_CNT_LEVEL_0;
    public const int TRUCK_CNT_LEVEL_1 = Globalvariable.TRUCK_CNT_LEVEL_1;
    public const int TRUCK_CNT_LEVEL_2 = Globalvariable.TRUCK_CNT_LEVEL_2;
    public const int TRUCK_CNT_LEVEL_MAX = Globalvariable.TRUCK_CNT_LEVEL_MAX;*/

    //PTJ, NEWS================================
    [Header("[ PTJ ]")]
    //PTJ 알바
    public GameObject workingCountText;//고용중인 동물수
    public GameObject PTJList;
    public GameObject[] PTJPref;

    [Header("PTJ === Warning Panel")]
    public GameObject warningBlackPanel;
    public GameObject HireYNPanel;
    public Button HireYNPanel_yes;
    public GameObject confirmPanel;

    [Header("[ MiniGame ]")]
    public GameObject minigame_inside;
    public Text dotoriTimer;

    //NEWS
    [NonSerialized]
    public int NewsPrefabNum;

    //새로운딸기================================
    [Header("[ NEW BERRY === OBJECT ]")]
    public GameObject priceText_newBerry;
    public GameObject timeText_newBerry;
    public GameObject startBtn_newBerry;

    public GameObject TimeReuce_newBerry;
    public GameObject TimeReduceBlackPanel_newBerry;
    public GameObject TimeReducePanel_newBerry;
    public Text TimeReduceText_newBerry;
    public GameObject AcheivePanel_newBerry;
    public Sprite[] AcheiveClassify_newBerry;

    public GameObject NoPanel_newBerry;
    public GameObject BlackPanel_newBerry;

    [Header("[ NEW BERRY === SPRITE ]")]
    public Sprite StartImg;
    public Sprite DoneImg;
    public Sprite IngImg;
    public SpriteRenderer[] stemLevelSprites;

    public GameObject newBerryBangImg;
    public GameObject newBerryBangImg2;

    private int price_newBerry;//이번에 개발되는 베리 가격
    private string BtnState;//지금 버튼 상태
    private int newBerryIndex2;//이번에 개발되는 뉴스 베리 넘버

    [Header("[ NEW BERRY === GLOBAL ]")]
    public GameObject Global;

    //Challenge, Collection================================
    [Header("[ Challenge / Collection]")]
    public GameObject bangIcon;//업적 느낌표 오브젝트
    public GameObject contentChallenge;
    public CollectionAcquire[] collectionInfo;
    public ChallengeAcquire[] challengeCriterion;
    private int[] ChallengeValueNow = new int[6];

    //===========================================
    [Header("[ Check/Settings Panel ]")]
    public GameObject settingsPanel;
    public GameObject checkPanel;


    [Header("[ Check/Day List ]")]
    public string url = "";

    [Header("[ Absence Panel ]")]
    public GameObject absencePanel;
    public GameObject absenceBlackPanel;
    public Text absenceMoneyText;
    public Text absenceTimeText;
    private int revenue;
    public Button add_receive_btn; //부재중수익 2배받기 버튼

    [Header("[ Panel List ]")]
    public Text panelCoinText;
    public Text panelHearText;
    public GameObject noCoinPanel;
    public GameObject noHeartPanel;
    public GameObject blackPanel;
    public GameObject coinAnimManager;
    public GameObject heartAnimManager;
    public GameObject QuitPanel;

    [Header("[ Ad Btn ]")]
    public Button truckAdBtn;
    public Button coinAdBtn;
    public Button heartAdBtn;
    public Button absenceAdBtn;


    [Header("[ Game Flag ]")]
    public bool isGameRunning;
    public bool isBlackPanelOn = false;
    private int coinUpdate;
    public bool isStart;
    public bool isMiniGameMode = false;

    public Action OnOnline;
    public Action OnOffline;
    static bool isOnline = false;
    #endregion

    #region 기본 기능

    private void Awake()
    {
        setChallenge();


    }
    void Start()
    {
        Application.targetFrameRate = 60;
        instance = this;

        target = TruckObj.GetComponent<Transform>();

        //for(int i = 0; i < )
        //TimerStart += Instance_TimerStart;

        DisableObjColliderAll();

        isGameRunning = true;

        //NEW BERRY
        NewBerryUpdate();

        ShowCoinText(CoinText, DataController.instance.gameData.coin);
        HeartText.text = DataController.instance.gameData.heart.ToString();
        invokeDotori();

        globalVar = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>();

        isStart = true;

        InitDataInGM();

        //PrintTime();

    }

    public void StartPrework()
    {
        DataController.instance.gameData.isPrework = false;
        StartCoroutine(PreWork());
    }

    public void GameStart()
    {
        isGameRunning = true;
        Invoke("EnableObjColliderAll", 4.5f);


    }

    void InitDataInGM()
    {
        // 게임 시작시 딸기 가격 한번 업데이트 해주기
        float researchCoeffi = (DataController.instance.gameData.researchLevel[0]) * globalVar.getEffi();
        for (int i = 0; i < 192; i++)
        {
            if (globalVar.berryListAll[i] == null) continue;

            if (i < 64)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((globalVar.CLASSIC_FIRST + i) * (1 + researchCoeffi));
            else if (i < 128)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((globalVar.SPECIAL_FIRST + (i - 64) * 2) * (1 + researchCoeffi));
            else
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((globalVar.UNIQUE_FIRST + (i - 128) * 3) * (1 + researchCoeffi));
            /*if (i < 64)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((globalVar.CLASSIC_FIRST + i) * (1 + researchCoeffi));
            else if (i < 128)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((50) * (1 + researchCoeffi));
            else
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((1000) * (1 + researchCoeffi));*/
        }

        for (int i = 0; i < 16; i++)
        {
            if (DataController.instance.gameData.berryFieldData[i].isStemEnable)
            {
                stemList[i].gameObject.SetActive(true);
            }
            if (DataController.instance.gameData.berryFieldData[i].isWeedEnable)
            {
                farmList[i].weed.gameObject.SetActive(true);
            }
            if (DataController.instance.gameData.berryFieldData[i].isBugEnable)
            {
                bugList[i].gameObject.SetActive(true);
            }
            float creatTimeTemp = DataController.instance.gameData.berryFieldData[i].createTime;
            if ((0 < creatTimeTemp && creatTimeTemp < DataController.instance.gameData.stemLevel[4]) || DataController.instance.gameData.berryFieldData[i].hasWeed)
            {
                farmList[i].GetComponent<BoxCollider2D>().enabled = false;
            }
        }

    }
    void Update()
    {


        //PTJ
        workingCountText.GetComponent<Text>().text = DataController.instance.gameData.PTJCount.ToString();//알바중인 인원수

        //NEW BERRY 개발
        switch (DataController.instance.gameData.newBerryBtnState)
        {
            case 0: BtnState = "start"; startBtn_newBerry.GetComponent<Image>().sprite = StartImg; break;
            case 1: BtnState = "ing"; startBtn_newBerry.GetComponent<Image>().sprite = IngImg; break;
            case 2:
                BtnState = "done";
                newBerryBangImg.SetActive(true);
                newBerryBangImg2.SetActive(true);
                startBtn_newBerry.GetComponent<Image>().sprite = DoneImg; break;
        }

        if (Input.GetMouseButton(0)) // 마우스 왼쪽 버튼으로
        {
            GameObject obj = ClickObj(); // 클릭당한 오브젝트 가져옴
            if (obj != null)
            {

                if (obj.CompareTag("Farm"))
                {
                    ClickedFarm(obj);
                }
                else if (obj.CompareTag("Bug"))
                {
                    ClickedBug(obj);
                }
                else if (obj.CompareTag("Weed"))
                {
                    ClickedWeed(obj);
                }
            }
        }



        //폰에서 뒤로가기 버튼 눌렀을 때/에디터에서 ESC 버튼 눌렀을 때 게임 종료
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!blackPanel.activeSelf)
            {
                DisableObjColliderAll();
                BlackPanelOn();
                QuitPanel.GetComponent<PanelAnimation>().OpenScale();
            }
            else
            {
                EnableObjColliderAll();
                blackPanel.GetComponent<PanelAnimation>().FadeOut();
                QuitPanel.GetComponent<PanelAnimation>().CloseScale();
            }
        }


    }
    private void FixedUpdate()
    {
        ChallengeValueNow[0] = DataController.instance.gameData.unlockBerryCnt;
        ChallengeValueNow[1] = DataController.instance.gameData.totalHarvBerryCnt;
        ChallengeValueNow[2] = DataController.instance.gameData.accCoin;
        ChallengeValueNow[3] = DataController.instance.gameData.accHeart;
        ChallengeValueNow[4] = DataController.instance.gameData.accAttendance;
        ChallengeValueNow[5] = DataController.instance.gameData.mgPlayCnt;

        BangIconSearch();

    }
    public void QuitOkBtn()
    {
        isStart = false;
        DataController.instance.gameData.lastExitTime = DataController.instance.gameData.currentTime;

        if (MiniGameManager.isOpen == true)
        {
            DataController.instance.gameData.lastMinigameExitTime = DataController.instance.gameData.currentTime;
        }
        DataController.instance.SaveData();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    void LateUpdate()
    {
        //CoinText.text = coin.ToString() + " A";
        //ShowCoinText(CoinText, DataController.instance.gameData.coin); //트럭코인 나타낼 때 같이쓰려고 매개변수로 받게 수정했어요 - 신희규
        //HeartText.text = DataController.instance.gameData.heart.ToString();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            if (DataController.instance.gameData.isPrework)
            {
                DataController.instance.gameData.lastExitTime = DataController.instance.gameData.currentTime;

                if (MiniGameManager.isOpen)
                {
                    DataController.instance.gameData.lastMinigameExitTime = DataController.instance.gameData.currentTime;
                    MiniGameManager.instance.StopAllCoroutines();
                    MiniGameManager.instance.isTimerOn = false;
                }
            }

            DataController.instance.SaveData();
        }
        else
        {
            if (isStart && Intro.isEnd)
            {
                StartCoroutine(CheckElapseTime());
            }


        }

    }


    #endregion

    #region 딸기밭
    void ClickedFarm(GameObject obj)
    {
        Farm farm = obj.GetComponent<Farm>();

        if (!DataController.instance.gameData.berryFieldData[farm.farmIdx].isPlant)
        {
            Stem st = GetStem(farm.farmIdx);
            if (st != null)
            {
                //PlantStrawBerry(st, obj); // 심는다

                DataController.instance
                    .gameData.berryFieldData[farm.farmIdx].isPlant = true; // 체크 변수 갱신

                PlantStrawBerry(st, obj); // 심는다


            }
        }
        else
        {
            if (!DataController.instance.gameData.berryFieldData[farm.farmIdx].canGrow)
            {
                Harvest(stemList[farm.farmIdx]); // 수확
            }
        }
    }
    void ClickedBug(GameObject obj)
    {
        Bug bug = obj.GetComponent<Bug>();
        bug.DieBug();
    }
    void ClickedWeed(GameObject obj)
    {
        Weed weed = obj.GetComponent<Weed>();
        weed.DeleteWeed();
    }
    public void ClickedTruck()
    {
        bonusTruckCoin = (int)(DataController.instance.gameData.truckCoin *
            DataController.instance.gameData.researchLevel[5] * Globalvariable.instance.getEffi());

        // 트럭에 담긴 현재 딸기 개수와 현재 MAX 개수 출력
        truckCountNowText.text = DataController.instance.gameData.truckBerryCnt.ToString();
        truckCountMaxText.text = "/ " + Globalvariable.instance.truckCntLevel[3, DataController.instance.gameData.newBerryResearchAble].ToString();
        ShowCoinText(truckCoinText, DataController.instance.gameData.truckCoin);
        ShowCoinText(truckCoinBonusText, bonusTruckCoin);

        if (!isOnline)
            truckAdBtn.interactable = false;
    }

    Stem GetStem(int idx)
    {
        if (DataController.instance.gameData.berryFieldData[idx].isPlant) return null;

        return stemList[idx];
    }
    public void PlantStrawBerry(Stem stem, GameObject obj)
    {
        BoxCollider2D coll = obj.GetComponent<BoxCollider2D>();
        //stem.transform.position = obj.transform.position; ; // 밭의 Transform에 달기를 심는다
        stem.gameObject.SetActive(true); // 딸기 활성화              
        coll.enabled = false; // 밭의 콜라이더 비활성화 (잡초와 충돌 방지)



        if (!(isMiniGameMode || Blink.instance.gameObject.activeSelf)) // 미니게임 중에는 소리 안나게
            AudioManager.instance.SowAudioPlay();
    }
    public void Harvest(Stem stem)
    {
        Farm farm = farmList[stem.stemIdx];
        if (farm.isHarvest) return;


        if (!(isMiniGameMode || Blink.instance.gameObject.activeSelf)) // 미니게임 중에는 소리 안나게-      
            AudioManager.instance.HarvestAudioPlay();//딸기 수확할 때 효과음

        farm.isHarvest = true;
        Vector2 pos = stem.transform.position;
        stem.getInstantBerryObj().GetComponent<Berry>().Explosion(pos, target.position, 0.5f);
        //stem.getInstantBerryObj().GetComponent<SpriteRenderer>().sortingOrder = 3;

        StartCoroutine(HarvestRoutine(farm, stem)); // 연속으로 딸기가 심어지는 현상 방지

    }
    GameObject ClickObj() // 클릭당한 오브젝트를 반환
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        if (hit.collider == null) return null;

        return hit.collider.gameObject;
    }
    IEnumerator HarvestRoutine(Farm farm, Stem stem)
    {
        farm.GetComponent<BoxCollider2D>().enabled = false; //밭 잠시 비활성화

        yield return new WaitForSeconds(0.75f); // 0.75뒤에

        UpdateTruckState(stem);

        DataController.instance.gameData.totalHarvBerryCnt++; // 수확한 딸기의 총 개수 업데이트           
        DataController.instance.gameData.berryFieldData[stem.stemIdx].isPlant = false; // 밭을 비워둔다

        //줄기에 페이드아웃 적용
        Animator anim = stemObjList[stem.stemIdx].GetComponent<Animator>();
        anim.SetInteger("Seed", 5);

        yield return new WaitForSeconds(0.3f); // 0.3초 뒤에

        stem.gameObject.SetActive(false);

        farm.isHarvest = false; // 수확 끝              
        if (!DataController.instance.gameData.berryFieldData[farm.farmIdx].hasWeed && !isBlackPanelOn) //잡초가 없다면
        {
            farm.GetComponent<BoxCollider2D>().enabled = true; // 밭 다시 활성화
        }
    }
    void UpdateTruckState(Stem stem)
    {
        if (DataController.instance.gameData.truckBerryCnt < Globalvariable.instance.truckCntLevel[3, DataController.instance.gameData.newBerryResearchAble])
        {
            DataController.instance.gameData.truckBerryCnt += 1;
            DataController.instance.gameData.truckCoin += stem.getInstantBerryObj().GetComponent<Berry>().berryPrice;
        }
    }
    #endregion

    #region 재화

    public void StartTutorialRewardCo()
    {
        StartCoroutine(TutorialReward());
    }

    public IEnumerator TutorialReward()
    {
        yield return new WaitForSeconds(0.3f);
        GameManager.instance.GetCoin(200);
        GameManager.instance.GetHeart(20);
    }

    IEnumerator CountAnimation(int cost, String text, int num) //재화 증가 애니메이션

    {
        if (num == 0)
        {

            coinAnimManager.GetComponent<HeartMover>().CountMoney(102f, cost, text, num);
            Invoke("invokeCoin", 0.3f);
        }
        else
        {
            heartAnimManager.GetComponent<HeartMover>().CountMoney(442f, cost, text, num);
            Invoke("invokeHeart", 0.3f);
        }
        yield return null;
    }

    public void invokeCoin()
    {
        ShowCoinText(CoinText, DataController.instance.gameData.coin);
    }

    public void invokeHeart()
    {
        HeartText.text = DataController.instance.gameData.heart.ToString();
    }
    public void invokeDotori()
    {
        DotoriText.text = DataController.instance.gameData.dotori.ToString() + " / 5";
    }
    public void ShowCoinText(Text coinText, int coin)
    {
        //int coin = DataController.instance.gameData.coin;
        if (coin <= 9999)           // 0~9999까지 A
        {
            coinText.text = coin.ToString() + "A";
        }
        else if (coin <= 9999999)   // 10000~9999999(=9999B)까지 B
        {
            coin /= 1000;
            coinText.text = coin.ToString() + "B";
        }
        else                        //  그 외 C (최대 2100C)
        {
            coin /= 1000000;
            coinText.text = coin.ToString() + "C";
        }
    }

    public void GetCoin(int cost) // 코인 획득 함수
    {
        StartCoroutine(CountAnimation(cost, "+", 0));
        DataController.instance.gameData.coin += cost; // 현재 코인 +
        DataController.instance.gameData.accCoin += cost; // 누적 코인 +
        AudioManager.instance.CoinAudioPlay();
    }

    public void UseCoin(int cost) // 코인 사용 함수(마이너스 방지 위함)
    {
        int mycoin = DataController.instance.gameData.coin;
        if (mycoin >= cost)
        {
            StartCoroutine(CountAnimation(cost, "-", 0));
            DataController.instance.gameData.coin -= cost;
        }
        else
        {
            //경고 패널 등장
            ShowCoinText(panelCoinText, DataController.instance.gameData.coin);
            blackPanel.GetComponent<PanelAnimation>().Fadein();
            noCoinPanel.GetComponent<PanelAnimation>().OpenScale();

            AudioManager.instance.Cute4AudioPlay(); // 효과음
        }
    }

    public void GetHeart(int cost) // 하트 획득 함수
    {
        StartCoroutine(CountAnimation(cost, "+", 1));
        DataController.instance.gameData.heart += cost; // 현재 하트 +
        DataController.instance.gameData.accHeart += cost; // 누적 하트 +
        AudioManager.instance.HeartAudioPlay();
    }

    public void UseHeart(int cost) // 하트 획득 함수 (마이너스 방지 위함)
    {
        int myHeart = DataController.instance.gameData.heart;

        if (myHeart >= cost)
        {
            DataController.instance.gameData.heart -= cost;
            StartCoroutine(CountAnimation(cost, "-", 1));
        }
        else
        {
            //경고 패널 등장
            panelHearText.text = DataController.instance.gameData.heart.ToString() + "개";
            blackPanel.GetComponent<PanelAnimation>().Fadein();
            noHeartPanel.GetComponent<PanelAnimation>().OpenScale();

            AudioManager.instance.Cute4AudioPlay(); // 효과음

        }
    }

    public void GetMedal(int cost)
    {
        DataController.instance.gameData.medal += cost;
        ShowMedalText();
    }

    public void UseMedal(int cost)
    {
        int myMedal = DataController.instance.gameData.medal;
        if (myMedal >= cost)
        {
            DataController.instance.gameData.medal -= cost;
            ShowMedalText();
        }
        else
        {
            //메달이 모자를 떄 뜨는 경고
            AudioManager.instance.Cute4AudioPlay(); // 효과음
        }
    }
    public void ShowMedalText()
    {
        MedalText.GetComponent<Text>().text = DataController.instance.gameData.medal.ToString();
    }


    public void UseDotori()
    {
        if (DataController.instance.gameData.dotori > 0)
        {
            --DataController.instance.gameData.dotori;
            DataController.instance.gameData.totalDotoriTime = DataController.instance.gameData.totalDotoriTime.Add(TimeSpan.FromMinutes(30));

            if (DataController.instance.gameData.totalDotoriTime.TotalMinutes >= 150) // 최대치 150분 고정
                DataController.instance.gameData.totalDotoriTime = TimeSpan.FromMinutes(150);

            if (DataController.instance.gameData.nextDotoriTime.TotalSeconds <= 0)
            {
                DataController.instance.gameData.nextDotoriTime = TimeSpan.FromSeconds(1800f);
                dotoriTimer.text = "30:00";
            }
        }
    }

    public void BlackPanelOn()
    {
        blackPanel.GetComponent<PanelAnimation>().Fadein();
    }

    #endregion

    #region 콜라이더
    public void DisableObjColliderAll() // 모든 오브젝트의 콜라이더 비활성화
    {
        BoxCollider2D coll;
        isBlackPanelOn = true;
        for (int i = 0; i < farmList.Count; i++)
        {
            coll = farmList[i].GetComponent<BoxCollider2D>();
            coll.enabled = false;
            //stemList[i].canGrow = false;
            bugList[i].GetComponent<CircleCollider2D>().enabled = false;
            farmList[i].weed.GetComponent<CapsuleCollider2D>().enabled = false;
            // Weed의 Collider 제거
            //farmList[i].canGrowWeed = false;
        }
    }
    public void EnableObjColliderAll() // 모든 오브젝트의 collider 활성화
    {
        BoxCollider2D coll;
        isBlackPanelOn = false;
        for (int i = 0; i < farmList.Count; i++)
        {
            coll = farmList[i].GetComponent<BoxCollider2D>();
            if (!DataController.instance.gameData.berryFieldData[i].isPlant && !DataController.instance.gameData.berryFieldData[i].hasWeed) // 잡초가 없을 때만 빈 밭의 Collider활성화
            {
                coll.enabled = true;
            }
            if (!DataController.instance.gameData.berryFieldData[i].hasBug && !DataController.instance.gameData.berryFieldData[i].hasWeed && DataController.instance.gameData.berryFieldData[i].createTime >= DataController.instance.gameData.stemLevel[4]) //(4)의 상황, 즉 벌레와 잡초 둘 다 없을 때 다 자란 딸기밭의 콜라이더를 켜준다.
            {
                coll.enabled = true;
            }
            bugList[i].GetComponent<CircleCollider2D>().enabled = true;
            farmList[i].weed.GetComponent<CapsuleCollider2D>().enabled = true; // 잡초의 Collider 활성화
            //farmList[i].canGrowWeed = true;
        }
    }
    #endregion

    #region 리스트

    #region ===PTJ

    //고용 버튼 클릭시
    public void PTJEmployButtonClick(int prefabNum)
    {
        //효과음
        AudioManager.instance.Cute1AudioPlay();

        //고용중이 아닌 상태다
        if (DataController.instance.gameData.PTJNum[prefabNum] == 0)
        {
            if (DataController.instance.gameData.PTJCount < 3)
            {
                int cost = PTJ.instance.Info[prefabNum].Price * DataController.instance.gameData.PTJSelectNum[1];

                // PTJ 스크립트도 변경
                if (DataController.instance.gameData.researchLevelAv < 5) // 연구 레벨이 5레벨 이하라면
                    cost *= 1;
                else if (DataController.instance.gameData.researchLevelAv < 10) // 연구 레벨이 10레벨 이하라면
                    cost *= 2;
                else
                    cost *= 4;

                if (cost <= DataController.instance.gameData.coin)
                {
                    int ID = DataController.instance.gameData.PTJSelectNum[0];
                    //HIRE

                    //코인 사용
                    UseCoin(cost);

                    //고용
                    DataController.instance.gameData.PTJNum[prefabNum] = DataController.instance.gameData.PTJSelectNum[1];

                    //고용중인 알바생 수 증가
                    DataController.instance.gameData.PTJCount++;

                    for (int i = 0; i < 6; i++)
                        PTJPref[i].GetComponent<PanelAnimation>().CloseScale();
                }
                else
                {
                    //효과음
                    AudioManager.instance.Cute4AudioPlay();
                    //재화 부족 경고 패널
                    ShowCoinText(panelCoinText, DataController.instance.gameData.coin);
                    noCoinPanel.GetComponent<PanelAnimation>().OpenScale();
                    GameManager.instance.BlackPanelOn();
                }
            }
            else
            {
                //효과음
                AudioManager.instance.Cute4AudioPlay();
                //3명 이상 고용중이라는 패널 등장
                confirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "고용 가능한 알바 수를\n넘어섰어요!";
                confirmPanel.GetComponent<PanelAnimation>().OpenScale();
                GameManager.instance.BlackPanelOn();
            }
        }
        //고용중인 상태이다
        else
        {
            //FIRE
            //확인창 띄우기
            HireYNPanel.GetComponent<PanelAnimation>().OpenScale();
            GameManager.instance.BlackPanelOn();
        }


    }

    public void Fire()
    {
        int ID = DataController.instance.gameData.PTJSelectNum[0];
        //고용 해제
        DataController.instance.gameData.PTJNum[ID] = 0;
        //고용 중인 알바생 수 감소
        //PTJ에서 구현됨

        //확인창 내리기
        HireYNPanel.GetComponent<PanelAnimation>().CloseScale();
        warningBlackPanel.GetComponent<PanelAnimation>().FadeOut();

        for (int i = 0; i < 6; i++)
            PTJPref[i].GetComponent<PanelAnimation>().CloseScale();
    }
    #endregion

    #region ===New Berry Add

    public void NewBerryUpdate()
    {
        //새 딸기 개발======

        //PRICE
        price_newBerry = 80 + 20 * (BerryCount("classic", true) + BerryCount("special", true) * 2 + BerryCount("unique", true) * 5);

        ShowCoinText(priceText_newBerry.GetComponent<Text>(), price_newBerry);

        if (DataController.instance.gameData.newBerryBtnState == 1)//개발 가능한 딸기
        { StartCoroutine(Timer()); }
        else
        {
            if (isNewBerryAble() == true)//개발 가능한 딸기 있는지 검사
            {

                DataController.instance.gameData.newBerryIndex = selectBerry();//얻을딸기, 시간 정해진다
                timeText_newBerry.GetComponent<Text>().text = "??:??";//TIME (미공개)

                //베리 없음 지우기
                NoPanel_newBerry.SetActive(false);
            }
            else { NoPanel_newBerry.SetActive(true); }
        }
    }
    public void NewBerryUpdate2() //개발 가능한 딸기 범위 넓어질때 실행되는 거. 위에 합치자
    {

        if (isNewBerryAble() == true)//한번 더 확인
        {
            if (DataController.instance.gameData.newBerryBtnState == 0)//딸기 얻고 있는 상태 아니면
            {
                //얻을 딸기가 정해진다 -> 시간, 값도 정해진다
                //PRICE
                price_newBerry = 10 * (BerryCount("classic", true) + BerryCount("special", true) + BerryCount("unique", true));
                //priceText_newBerry.GetComponent<Text>().text = price_newBerry.ToString();
                ShowCoinText(priceText_newBerry.GetComponent<Text>(), price_newBerry);
                //TIME
                DataController.instance.gameData.newBerryIndex = selectBerry();
                timeText_newBerry.GetComponent<Text>().text = "??:??";//가격 시간 아직 미공개 "?"

            }
            //베리 없음 지우기
            NoPanel_newBerry.SetActive(false);
        }
        else { NoPanel_newBerry.SetActive(true); }

    }

    public bool isNewBerryAble()
    {
        //지금 새 딸기 개발 할 수 있나
        switch (DataController.instance.gameData.newBerryResearchAble)
        {
            case 0://classic 개발가능
                if (BerryCount("classic", false) == BerryCount("classic", true))
                { return false; }
                break;
            case 1://classic, special 개발가능
                if (BerryCount("classic", false) + BerryCount("special", false) ==
                    BerryCount("classic", true) + BerryCount("special", true))
                { return false; }
                break;
            case 2: //classic, special, unique 개발가능
                if (BerryCount("classic", false) + BerryCount("special", false) + BerryCount("unique", false) ==
                    BerryCount("classic", true) + BerryCount("special", true) + BerryCount("unique", true))
                { return false; }
                break;
        }
        return true;
    }


    //isUnlock-> false=현재 값이 존재하는 딸기 갯수들을 반환 / true=현재 unlock된 딸기 갯수들을 반환한다.
    private int BerryCount(string berryClssify, bool isUnlock)
    {
        int countIsExsist = 0;
        int countIsUnlock = 0;
        switch (berryClssify)
        {
            case "classic":
                for (int i = 0; i < Global.GetComponent<Globalvariable>().classicBerryList.Count; i++)
                {
                    if (DataController.instance.gameData.isBerryUnlock[i] == true) { countIsUnlock++; }
                    if (Global.GetComponent<Globalvariable>().classicBerryList[i] == true) { countIsExsist++; }
                }
                break;

            case "special":
                for (int i = 0; i < Global.GetComponent<Globalvariable>().specialBerryList.Count; i++)
                { if (Global.GetComponent<Globalvariable>().specialBerryList[i] == true) { countIsExsist++; } }
                for (int i = 64; i < 64 + 64; i++)
                { if (DataController.instance.gameData.isBerryUnlock[i] == true) { countIsUnlock++; } }
                break;

            case "unique":
                for (int i = 0; i < Global.GetComponent<Globalvariable>().uniqueBerryList.Count; i++)
                { if (Global.GetComponent<Globalvariable>().uniqueBerryList[i] == true) { countIsExsist++; } }
                for (int i = 128; i < 128 + 64; i++)
                { if (DataController.instance.gameData.isBerryUnlock[i] == true) { countIsUnlock++; } }
                break;
                //default:Debug.Log("잘못된 값 받았다");break;
        }


        if (isUnlock == true)
        { return countIsUnlock; }
        else { return countIsExsist; }
    }



    //새로운 딸기 개발 버튼 누르면
    public void NewBerryButton()
    {

        switch (BtnState)
        {
            case "start":
                //이번에 새딸기 개발에 필요한 가격과 시간
                //priceText_newBerry.GetComponent<Text>().text = price_newBerry.ToString();//가격
                ShowCoinText(priceText_newBerry.GetComponent<Text>(), price_newBerry);

                if (DataController.instance.gameData.coin >= price_newBerry)
                {
                    timeText_newBerry.GetComponent<Text>().text
                        = TimeForm(DataController.instance.gameData.newBerryTime);//시간

                    //돈소비
                    UseCoin(price_newBerry);

                    //버튼상태 ing로
                    DataController.instance.gameData.newBerryBtnState = 1;

                    //타이머 시작
                    DataController.instance.gameData.newBerryTime_start = DateTime.Now;
                    DataController.instance.gameData.newBerryTime_end
                        = DataController.instance.gameData.newBerryTime_start.AddSeconds(DataController.instance.gameData.newBerryTime);
                    DataController.instance.gameData.newBerryTime_span
                        = DataController.instance.gameData.newBerryTime_end - DataController.instance.gameData.newBerryTime_start;
                    StartCoroutine(Timer());

                    //시간 감소여부 묻는 패널 띄움.
                    TimeReduceBlackPanel_newBerry.GetComponent<PanelAnimation>().Fadein(); //시원 건드림
                    TimeReducePanel_newBerry.GetComponent<PanelAnimation>().OpenScale(); //시원 건드림
                    TimeReduceText_newBerry.GetComponent<Text>().text //시원 건드림
                        = "하트로 시간을\n단축하시겠습니까??\n";
                }
                else//돈 부족
                { UseCoin(price_newBerry); }
                break;

            case "ing":
                if (DataController.instance.gameData.newBerryTime_start < DataController.instance.gameData.newBerryTime_end)
                {
                    //시간 감소 여부 묻는 패널 띄움.
                    TimeReduceBlackPanel_newBerry.GetComponent<PanelAnimation>().Fadein(); //시원 건드림
                    TimeReducePanel_newBerry.GetComponent<PanelAnimation>().OpenScale();
                    TimeReduceText_newBerry.GetComponent<Text>().text
                        = "하트로 시간을\n단축하시겠습니까?\n";
                }
                break;

            case "done": //딸기 개발
                GetNewBerry();
                newBerryBangImg.SetActive(false);
                newBerryBangImg2.SetActive(false);
                break;

        }

    }

    //TimeReucePanel_newBerry
    //하트 써서 시간을 줄인지 여부 패널
    public void TimeReduce(int heartNum)
    {
        //하트 써서 시간을 줄일거면

        if (DataController.instance.gameData.heart >= heartNum)
        {
            //시간을 줄여준다.
            if ((int)DataController.instance.gameData.newBerryTime_span.TotalSeconds < heartNum * 60)
            {
                timeText_newBerry.GetComponent<Text>().text = TimeForm(Mathf.CeilToInt(0));
                DataController.instance.gameData.newBerryTime_end = DateTime.Now;

            }
            else
            {
                //Debug.Log("간소전=" + (int)DataController.instance.gameData.newBerryTime_span.TotalSeconds);
                DataController.instance.gameData.newBerryTime_end
                    = DataController.instance.gameData.newBerryTime_end.AddSeconds(-heartNum * 60);
                //Debug.Log("간소후=" + (int)DataController.instance.gameData.newBerryTime_span.TotalSeconds);
                DataController.instance.gameData.newBerryTime_span
                    = DataController.instance.gameData.newBerryTime_end - DataController.instance.gameData.newBerryTime_start;
                //Debug.Log(DataController.instance.gameData.newBerryTime_span);
            }

            timeText_newBerry.GetComponent<Text>().text
                = TimeForm(Mathf.CeilToInt(DataController.instance.gameData.newBerryTime_span.Seconds));

            //하트를 소비한다.
            UseHeart(heartNum);



            //StartCoroutine(Timer());
        }
        else
        { UseHeart(heartNum); }



    }
    public void TimeReduceClose()
    {
        //창 끄기
        TimeReduceBlackPanel_newBerry.GetComponent<PanelAnimation>().FadeOut();
        TimeReducePanel_newBerry.GetComponent<PanelAnimation>().CloseScale();
    }

    IEnumerator Timer()
    {

        while (true)
        {

            //1초씩 감소
            DataController.instance.gameData.newBerryTime_start = DateTime.Now;
            DataController.instance.gameData.newBerryTime_span
                = DataController.instance.gameData.newBerryTime_end - DataController.instance.gameData.newBerryTime_start;
            yield return new WaitForSeconds(0.005f);


            //타이머
            if (DataController.instance.gameData.newBerryTime_start >= DataController.instance.gameData.newBerryTime_end)//타이머 끝남
            {
                DataController.instance.gameData.newBerryBtnState = 2;//Done상태로
                timeText_newBerry.GetComponent<Text>().text = TimeForm(Mathf.CeilToInt(0));
                break;
            }
            else//타이머 안끝남
            {
                timeText_newBerry.GetComponent<Text>().text
                    = TimeForm(Mathf.CeilToInt((int)DataController.instance.gameData.newBerryTime_span.TotalSeconds));
            }
        }
        StopCoroutine(Timer());

    }


    private int selectBerry()
    {
        int newBerryIndex = 1;

        while (true)
        {
            switch (DataController.instance.gameData.newBerryResearchAble)
            {
                case 0:
                    newBerryIndex = berryPercantage(64);
                    /*newBerryIndex = UnityEngine.Random.Range(1, 64);
                    DataController.instance.gameData.newBerryTime = 10 * 60;*/
                    break;
                case 1:
                    newBerryIndex = berryPercantage(128);
                    break;
                case 2:
                    newBerryIndex = berryPercantage(192);
                    break;
            }

            if (DataController.instance.gameData.isBerryUnlock[newBerryIndex] == false
            && Global.GetComponent<Globalvariable>().berryListAll[newBerryIndex] != null)
            { break; }
        }
        return newBerryIndex;
    }


    private void GetNewBerry()
    {

        //새로운 딸기가 추가된다
        DataController.instance.gameData.isBerryUnlock[DataController.instance.gameData.newBerryIndex] = true;
        DataController.instance.gameData.unlockBerryCnt++;

        //느낌표 표시
        DataController.instance.gameData.isBerryEM[DataController.instance.gameData.newBerryIndex] = true;

        //딸기 얻음 효과(짜잔)
        AudioManager.instance.TadaAudioPlay();

        //얻은딸기 설명창
        AcheivePanel_newBerry.SetActive(true);
        AcheivePanel_newBerry.transform.GetChild(1).GetComponent<Image>().sprite //얻은 딸기 이미지
            = Global.GetComponent<Globalvariable>().berryListAll[DataController.instance.gameData.newBerryIndex].GetComponent<SpriteRenderer>().sprite;
        AcheivePanel_newBerry.transform.GetChild(1).GetComponent<Image>().preserveAspect = true;

        AcheivePanel_newBerry.transform.GetChild(2).GetComponent<Text>().text //얻은 딸기 이름
            = Global.GetComponent<Globalvariable>().berryListAll[DataController.instance.gameData.newBerryIndex].GetComponent<Berry>().berryName;
        //베리 분류 이미지
        if (DataController.instance.gameData.newBerryIndex < 64)
        { AcheivePanel_newBerry.transform.GetChild(3).GetComponent<Image>().sprite = AcheiveClassify_newBerry[0]; }
        else if (DataController.instance.gameData.newBerryIndex < 128)
        { AcheivePanel_newBerry.transform.GetChild(3).GetComponent<Image>().sprite = AcheiveClassify_newBerry[1]; }
        else
        { AcheivePanel_newBerry.transform.GetChild(3).GetComponent<Image>().sprite = AcheiveClassify_newBerry[2]; }


        //검정창 띄우기
        BlackPanel_newBerry.GetComponent<PanelAnimation>().Fadein();


        DataController.instance.gameData.newBerryBtnState = 0;

        NewBerryUpdate();

    }

    private int berryPercantage(int endIndex)
    {
        int randomNum = 0;
        int newBerryIndex = 0;

        //RANDOM NUM -> classic(45)=0~44  special(35)=45~79  unique(20)=80~101
        if (endIndex == 128) { randomNum = UnityEngine.Random.Range(0, 80); }//지금 클래식이랑 스페셜만 가능하다면
        else if (endIndex == 192) { randomNum = UnityEngine.Random.Range(0, 100 + 1); }//지금 전부 다 가능하면



        if (randomNum < 45)
        {
            newBerryIndex = UnityEngine.Random.Range(1, 64);
            DataController.instance.gameData.newBerryTime = 10 * 60;
        }//classic
        else if (randomNum < 80)
        {
            newBerryIndex = UnityEngine.Random.Range(64, 128);
            DataController.instance.gameData.newBerryTime = 20 * 60;
        }//special
        else if (randomNum <= 100)
        {
            newBerryIndex = UnityEngine.Random.Range(128, 192);
            DataController.instance.gameData.newBerryTime = 30 * 60;
        }//unique


        return newBerryIndex;
    }


    public bool newsBerry()
    {
        if (isNewBerryAble())
        {
            do { newBerryIndex2 = selectBerry(); }
            while (newBerryIndex2 == DataController.instance.gameData.newBerryIndex);

            //새로운 딸기가 추가된다.
            DataController.instance.gameData.isBerryUnlock[newBerryIndex2] = true;
            DataController.instance.gameData.unlockBerryCnt++;
            //느낌표 표시
            DataController.instance.gameData.isBerryEM[newBerryIndex2] = true;

            //새딸기 얻음 팝업창
            GetNewBerry();

            return true;

        }
        else { return false; }

    }

    #endregion

    #region ===Explanation
    /*
    public void Explanation(GameObject berry,int prefabnum)
    {

        try
        {
            if (DataController.instance.gameData.isBerryUnlock[prefabnum] == true)
            {

                //설명창 띄우기
                berryExp_BlackPanel.SetActive(true); //시원 건드림
                berryExp_Panel.GetComponent<PanelAnimation>().OpenScale(); //시원 건드림

                //GameObject berryExpImage = berryExp.transform.GetChild(1).GetChild(1).gameObject; //시원 건드림
                //GameObject berryExpName = berryExp.transform.GetChild(1).GetChild(2).gameObject; //시원 건드림
                //GameObject berryExpTxt = berryExp.transform.GetChild(1).GetChild(3).gameObject; //시원 건드림


                //Explanation 내용을 채운다.
                berryExpImage.GetComponentInChildren<Image>().sprite
                    = berry.GetComponent<SpriteRenderer>().sprite;//이미지 설정

                berryExpName.gameObject.GetComponentInChildren<Text>().text
                    = berry.GetComponent<Berry>().berryName;//이름설정

                berryExpTxt.transform.gameObject.GetComponentInChildren<Text>().text
                    = berry.GetComponent<Berry>().berryExplain;//설명 설정   
            }
        }
        catch
        {
            Debug.Log("여기에 해당하는 베리는 아직 없다");
        }
    }
    */
    #endregion

    #region ===Challenge BangIcon
    public void BangIconSearch()
    {

        if (CollectionBangIconSearch() == true || ChallengeBangIconSearch() == true)
        { bangIcon.SetActive(true); }
        else { bangIcon.SetActive(false); }
    }

    private bool CollectionBangIconSearch()
    {
        if (Array.IndexOf(DataController.instance.gameData.isCollectionDone, false) == -1)
        { return false; }
        else
        {

            for (int i = 0; i < collectionInfo.Length; i++)
            {
                if (DataController.instance.gameData.isCollectionDone[i] == false)
                {
                    for (int j = 0; j < collectionInfo[i].collectionInfos.Length; j++)
                    {
                        if (DataController.instance.gameData.isBerryUnlock[collectionInfo[i].collectionInfos[j]] == false)
                        { break; }
                        if (j == collectionInfo[i].collectionInfos.Length - 1)
                        { return true; }

                    }
                }

            }

            return false;
        }
    }
    private bool ChallengeBangIconSearch()
    {

        for (int i = 0; i < 6; i++)
        {
            if (DataController.instance.gameData.isChallengeMax[i] == false)
            {
                if (ChallengeValueNow[i] >=
                    challengeCriterion[i].challengeCriterions
                    [DataController.instance.gameData.challengeLevel[i]])
                { return true; }

            }
        }
        return false;
    }

    private void setChallenge()
    {

        int MaxLevel = 6;


        challengeCriterion[0].challengeCriterions[0] = 10;
        for (int i = 1; i < MaxLevel; i++)
        {
            challengeCriterion[0].challengeCriterions[i] = challengeCriterion[0].challengeCriterions[0] + 10 * i;
        }


        challengeCriterion[1].challengeCriterions[0] = 80;
        for (int i = 1; i < MaxLevel; i++)
        {
            challengeCriterion[1].challengeCriterions[i] = challengeCriterion[1].challengeCriterions[i - 1] * 3;
        }


        challengeCriterion[2].challengeCriterions[0] = 3000;
        for (int i = 1; i < MaxLevel; i++)
        {
            challengeCriterion[2].challengeCriterions[i] = challengeCriterion[2].challengeCriterions[i - 1] * 3;
        }


        challengeCriterion[3].challengeCriterions[0] = 100;
        for (int i = 1; i < MaxLevel; i++)
        {
            challengeCriterion[3].challengeCriterions[i] = challengeCriterion[3].challengeCriterions[i - 1] + 100;
        }


        challengeCriterion[4].challengeCriterions[0] = 2;
        for (int i = 1; i < MaxLevel; i++)
        {
            challengeCriterion[4].challengeCriterions[i] = challengeCriterion[4].challengeCriterions[i - 1] + 3;
        }

        challengeCriterion[5].challengeCriterions[0] = 4;
        for (int i = 1; i < MaxLevel; i++)
        {
            challengeCriterion[5].challengeCriterions[i] = challengeCriterion[5].challengeCriterions[i - 1] + 8;
        }

    }
    #endregion

    public void NewsUnlock()
    {
        News.instance.NewsUnlock(NewsPrefabNum);
    }

    #region 기타
    //활성화 비활성화로 창 끄고 켜고
    public void turnOff(GameObject Obj)
    {

        if (Obj.activeSelf == true)
        { Obj.SetActive(false); }
        else
        { Obj.SetActive(true); }

    }

    public string TimeForm(int time)//초단위 시간을 분:초로
    {
        int M = 0, S = 0;//M,S 계산용
        string Minutes, Seconds;//M,S 텍스트 적용용

        M = (time / 60);
        S = (time % 60);


        //M,S 적용
        Minutes = M.ToString();
        Seconds = S.ToString();

        //M,S가 10미만이면 01, 02... 식으로 표시
        if (M < 10 && M > 0) { Minutes = "0" + M.ToString(); }
        if (S < 10 && S > 0) { Seconds = "0" + S.ToString(); }

        //M,S가 0이면 00으로 표시
        if (M == 0) { Minutes = "00"; }
        if (S == 0) { Seconds = "00"; }


        return Minutes + " : " + Seconds;

    }
    #endregion

    #endregion

    #region 출석

    //인터넷 시간 가져오기.

    public static IEnumerator InternetCheck()
    {
        UnityWebRequest uwr = UnityWebRequest.Get("http://naver.com");
        yield return uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success)
        {
            isOnline = false;
        }
        else
        {
            isOnline = true;
        }
    }

    public bool GetisOnline()
    {
        return isOnline;
    }

    public void GoodsAdBtnInternetCheck()
    {
        if (isOnline)
            OnOnline();
        else
            OnOffline();
    }

    public void TruckInternetCheck()
    {
        if (!isOnline)
            truckAdBtn.interactable = false;
    }

    public static IEnumerator UpdateCurrentTime()
    {
        while (isOnline)
        {
            yield return new WaitForSeconds(30f);
            UnityWebRequest request = new UnityWebRequest();
            using (request = UnityWebRequest.Get("https://naver.com"))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(request.error + "오류 났다 야");
                }
                else
                {
                    string date = request.GetResponseHeader("date");
                    DateTime dateTime = DateTime.Parse(date);
                    DataController.instance.gameData.currentTime = dateTime;
                }

                if (request.result != UnityWebRequest.Result.Success)
                    isOnline = false;
            }

        }

        while (!isOnline)
        {
            yield return new WaitForSeconds(30f);
            DateTime dateTime = DateTime.Now;
            DataController.instance.gameData.currentTime = dateTime;
            InternetCheck();
        }

        if (!AdsInitializer.instance.isInitialize)
            AdsInitializer.instance.InitializeAds();
    }

    public static IEnumerator TryGetCurrentTime()
    {
        while (DataController.instance.gameData.isPrework == false)
        {
            if (isOnline)
            {
                UnityWebRequest request = new UnityWebRequest();
                using (request = UnityWebRequest.Get("https://naver.com"))
                {
                    yield return request.SendWebRequest();

                    if (request.result == UnityWebRequest.Result.ConnectionError)
                    {
                        DataController.instance.gameData.isPrework = false;
                    }
                    else
                    {
                        string date = request.GetResponseHeader("date");
                        DateTime dateTime = DateTime.Parse(date);
                        DataController.instance.gameData.currentTime = dateTime;
                        DataController.instance.gameData.isPrework = true;
                    }
                }
            }
            else
            {
                DateTime dateTime = DateTime.Now;
                DataController.instance.gameData.currentTime = dateTime;
                DataController.instance.gameData.isPrework = true;
            }
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    //자정 체크 및 정보갱신
    void ResetTime() // 자정 지날 시 정보 갱신
    {
        AttendanceCheck.Inst.Attendance();
        DataController.instance.gameData.nextMidnightTime = DataController.instance.gameData.currentTime.Date.AddDays(1);

        //자동타이머
        Invoke(nameof(ResetTime),
            (float)(DataController.instance.gameData.nextMidnightTime
            - DataController.instance.gameData.currentTime).TotalSeconds);
    }

    IEnumerator CheckElapseTime() // 게임 복귀할때 
    {
        yield return StartCoroutine(InternetCheck());
        DataController.instance.gameData.isPrework = false;
        yield return StartCoroutine(TryGetCurrentTime());

        if (MiniGameManager.isOpen)
        {
            MiniGameManager.instance.DotoriInit();
        }

        TimeSpan gap = DataController.instance.gameData.currentTime - DataController.instance.gameData.lastExitTime;

        if (gap.TotalSeconds > 60f) //재접속 부재중 시간이 1분이상이면 Calculate 추가 계산
        {
            yield return StartCoroutine(CalculateTime());
        }

        //InternetCheck();
        MidNightCheck();
        AbsenceCheck();
        AttendanceCheck.Inst.Attendance();
    }

    IEnumerator PreWork() // 접속할 때 
    {
        yield return StartCoroutine(InternetCheck());
        yield return StartCoroutine(TryGetCurrentTime()); //현재시간 불러오기 체크
        yield return StartCoroutine(CalculateTime());

        StartCoroutine(UpdateCurrentTime()); //30초마다 시간 갱신 시작
        //InternetCheck();
        MidNightCheck(); // 자정시간 체크
        invokeDotori(); // 도토리 텍스트 갱신
        AbsenceCheck(); // 부재중 시간 체크

        AttendanceCheck.Inst.Attendance(); // 출석 확인
    }

    public void MidNightCheck() // 자정 체크 
    {
        DateTime temp = new DateTime();
        if (temp != DataController.instance.gameData.nextMidnightTime && temp != DataController.instance.gameData.currentTime)
        {
            //예외처리
            TimeSpan test = DataController.instance.gameData.nextMidnightTime - DataController.instance.gameData.currentTime.Date;
            if (test.Days >= 2)
                DataController.instance.gameData.nextMidnightTime = DataController.instance.gameData.currentTime.Date.AddDays(1);

            //자정시간을 지났다면
            TimeSpan gap = DataController.instance.gameData.currentTime - DataController.instance.gameData.nextMidnightTime;
            if (gap.TotalSeconds >= 0)
            {
                DataController.instance.gameData.nextMidnightTime = DataController.instance.gameData.currentTime.Date.AddDays(1);
                AttendanceCheck.Inst.Attendance();

                //광고시간 초기화
                DataController.instance.gameData.coinAdCnt = 3;
                DataController.instance.gameData.heartAdCnt = 3;
            }
            //자정시간을 지나지 않았다면
            gap = DataController.instance.gameData.nextMidnightTime - DataController.instance.gameData.currentTime;
            if (gap.TotalSeconds >= 0)
                Invoke(nameof(ResetTime), (float)gap.TotalSeconds);
        }
    }

    public void AbsenceCheck()
    {
        double checkTime = DataController.instance.gameData.rewardAbsenceTime.TotalMinutes;

        if (checkTime >= 60 && DataController.instance.gameData.isStoreOpend) // 60분 이상 && 인트로가 끝났을 때 && 미니게임 오픈
        {
            AbsenceTime(); // 부재중 패널
        }
        else if (checkTime < 60 && DataController.instance.gameData.isStoreOpend) // 60분 미만
        {
            DataController.instance.gameData.rewardAbsenceTime = TimeSpan.FromSeconds(0);
            //Debug.Log("부재중 60분 미만");
        }
    }

    public static bool CheckFirstGame()
    {
        if (!DataController.instance.gameData.isFirstGame)
        {
            DataController.instance.gameData.
                isFirstGame = true;

            DataController.instance.gameData.nextMidnightTime = DataController.instance.gameData.currentTime.Date.AddDays(1);
            DataController.instance.gameData.lastExitTime = DataController.instance.gameData.currentTime;
            DataController.instance.gameData.rewardAbsenceTime = TimeSpan.FromSeconds(0);
            DataController.instance.gameData.atdLastday = DataController.instance.gameData.currentTime.Date.AddDays(-1);
            DataController.instance.gameData.accDays = 0;
            return true;
        }
        return false;
    }

    public static IEnumerator CalculateTime() //부재중 시간
    {
        if (CheckFirstGame() == true) yield break;

        TimeSpan gap = DataController.instance.gameData.currentTime - DataController.instance.gameData.lastExitTime;

        DataController.instance.gameData.lastExitTime = DataController.instance.gameData.currentTime;

        if ((DataController.instance.gameData.rewardAbsenceTime + gap).TotalMinutes >= 1440) //부재중 수익 최대치 고정 24시간
            DataController.instance.gameData.rewardAbsenceTime = TimeSpan.FromMinutes(1440);
        else
            DataController.instance.gameData.rewardAbsenceTime += gap;


        //알바 남은 시간 갱신 자리
    }

    void PrintTime()
    {
        Debug.Log("현재 시간: " + DataController.instance.gameData.currentTime);
        Debug.Log("다음 자정시간: " + DataController.instance.gameData.nextMidnightTime);
        Debug.Log("다음 자정시간까지 남은시간: " + (DataController.instance.gameData.nextMidnightTime - DataController.instance.gameData.currentTime));
        Debug.Log("마지막 종료 시간: " + DataController.instance.gameData.lastExitTime);
        Debug.Log("부재중 시간: " + (DataController.instance.gameData.currentTime - DataController.instance.gameData.lastExitTime));
        Debug.Log("누적출석:" + DataController.instance.gameData.accDays);
        Debug.Log("마지막 출석:" + DataController.instance.gameData.atdLastday);
    }

    public void AbsenceTime() // 부재중 패널
    {
        int minute = (int)DataController.instance.gameData.rewardAbsenceTime.TotalMinutes;
        int hour = 0;

        revenue = minute * DataController.instance.gameData.researchLevelAv / 2; // 평균 7렙 기준 10분에 35A, 1시간 210A, 24시간 5040A
        // 한시간이 안넘거나 연구레벨이 하나라도 0 레벨이면 시간 초기화 하고 종료
        // 가게 오픈 가능 레벨이 모든 연구 레벨 7 이상임. 가게가 안열렸으면 부재중 보상도 오픈 X

        // 예외처리
        if (revenue <= 0)
        {
            DataController.instance.gameData.rewardAbsenceTime = TimeSpan.FromSeconds(0);
            return;
        }
        else
        {
            ShowCoinText(absenceMoneyText, revenue);
            // ShowCoin 안 쓰는 이유가 있음? (-우연) -> 아 맞네 만들 당시에 생각나는대로 적다보니 잊어버림
        }

        // 시간 텍스트 갱신 
        if (minute >= 60)
        {
            hour = minute / 60;
            minute %= 60;
            absenceTimeText.text = string.Format("{0:D2}:{1:D2}", hour, minute);
        }
        else
        {
            DataController.instance.gameData.rewardAbsenceTime = TimeSpan.FromSeconds(0);
            return; // 예외처리 2
        }

        // 보상 패널 띄우기

        absenceBlackPanel.GetComponent<PanelAnimation>().Fadein();
        absencePanel.GetComponent<PanelAnimation>().OpenScale();
        Invoke("DisColDelay", 0.5f);


        if (!isOnline)
            add_receive_btn.interactable = false;
    }

    void DisColDelay()
    {
        DisableObjColliderAll();
    }

    //광고보고 2배받기
    public void OnclickAdBtn()
    {
        RewardAd.instance.OnAdComplete += ReceiveCoin2Times;
        RewardAd.instance.OnAdFailed += OnFailedAd;
        RewardAd.instance.ShowAd();
        add_receive_btn.interactable = false; // 광고 보고 받기 버튼을 비활성화
    }

    void ReceiveCoin2Times()
    {
        GetCoin(revenue * 2);

        RewardAd.instance.OnAdComplete -= ReceiveCoin2Times;
        add_receive_btn.interactable = true; // 광고 보고 받기 버튼 활성
        absenceBlackPanel.SetActive(false);
        absencePanel.SetActive(false);
        InitAbsenceReward();
    }

    void OnFailedAd()
    {
        RewardAd.instance.OnAdComplete -= ReceiveCoin2Times;
        RewardAd.instance.OnAdFailed -= OnFailedAd;
        add_receive_btn.interactable = true; // 광고 보고 받기 버튼 활성
    }

    public void AbsenceBtn()
    {
        GetCoin(revenue);
        InitAbsenceReward();
        absenceBlackPanel.GetComponent<PanelAnimation>().FadeOut();
        absencePanel.GetComponent<PanelAnimation>().CloseScale();
    }

    public void InitAbsenceReward()
    {
        DataController.instance.gameData.rewardAbsenceTime = TimeSpan.FromSeconds(0); // 부재중 시간 초기화
        EnableObjColliderAll();
    }




    #endregion

    #region 메인 메뉴
    public void OnclickStart()
    {
    }

    public void OnclickOption()
    {

    }

    public void OnclickQuit()
    {
        isStart = false;
        DataController.instance.gameData.lastExitTime = DataController.instance.gameData.currentTime;
        if (MiniGameManager.isOpen == true)
        {
            DataController.instance.gameData.lastMinigameExitTime = DataController.instance.gameData.currentTime;
        }
        DataController.instance.SaveData();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else
        Application.Quit();

#endif
    }
    #endregion

}