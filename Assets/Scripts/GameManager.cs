using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.Networking;



public class GameManager : MonoBehaviour
{
    #region 인스펙터
    public static GameManager instance;

    [Header("[ Money ]")]
    public Text CoinText;
    public Text HeartText;

    [Header("[ Object ]")]
    public GameObject stemPrefab; // 프리팹
    public GameObject bugPrefab;

    public List<GameObject> farmObjList = new List<GameObject>();
    public List<Farm> farmList = new List<Farm>();
    public List<Stem> stemList = new List<Stem>();
    public List<Bug> bugList = new List<Bug>();

    [Header("[ Truck ]")]
    public GameObject TruckObj;
    public GameObject TruckPanel;
    Transform target;
    public Text truckCoinText;

    public const float STEM_LEVEL_0 = Globalvariable.STEM_LEVEL_0;
    public const float STEM_LEVEL_1 = Globalvariable.STEM_LEVEL_1;
    public const float STEM_LEVEL_2 = Globalvariable.STEM_LEVEL_2;
    public const float STEM_LEVEL_3 = Globalvariable.STEM_LEVEL_3;
    public const float STEM_LEVEL_MAX = Globalvariable.STEM_LEVEL_MAX;

    public const int TRUCK_CNT_LEVEL_0 = Globalvariable.TRUCK_CNT_LEVEL_0;
    public const int TRUCK_CNT_LEVEL_1 = Globalvariable.TRUCK_CNT_LEVEL_1;
    public const int TRUCK_CNT_LEVEL_2 = Globalvariable.TRUCK_CNT_LEVEL_2;
    public const int TRUCK_CNT_LEVEL_MAX = Globalvariable.TRUCK_CNT_LEVEL_MAX;

    [Header("[ PartTime/Search/Berry List ]")]
    //PTJ 알바
    public GameObject workingCountText;//고용 중인 동물 수
    public GameObject[] working;//고용 중인 동물 리스트 상단에

    //베리 설명창
    public GameObject berryExp;

    //새로운딸기================================
    [Header("[ OBJECT ]")]
    public GameObject priceText_newBerry;
    public GameObject timeText_newBerry;
    public GameObject startBtn_newBerry;
    [Header("[ INFO ]")]
    public float[] time_newBerry;
    public int[] price_newBerry;
    [Header("[ SPRITE ]")]
    public Sprite startImg;
    public Sprite doneImg;
    public Sprite ingImg;
    [Header("[ NEW BERRY ]")]
    public GameObject newBerryTimeReuce;
    public GameObject newBerryAcheive;

    private int index_newBerry = 0; //현재 인덱스
    private bool isStart_newBerry = false; //시작을 눌렀는가
    
    //===========================================

    [Header("[ Check/Settings Panel ]")]
    public GameObject SettingsPanel;
    public GameObject CheckPanel;
    public bool isblackPanelOn = false;

    [Header("[ Check/Day List ]")]
    public GameObject AttendanceCheck;
    public string url = "";

    #endregion

    #region 기본
    void Awake()
    {
        Application.targetFrameRate = 60;
        instance = this; // 게임 매니저의 싱글턴 패턴화 >> GameManager.instance.~~ 로 호출

        target = TruckObj.GetComponent<Transform>();

        //for(int i = 0; i < )
        //출석 관련 호출.
        StartCoroutine(WebCheck());
        AttendanceCheck.GetComponent<AttendanceCheck>().Attendance();
        CheckTime();

        //SetBerryPrice();
        InitDataInGM();
    }
    void InitDataInGM()
    {
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
            if ((0 < creatTimeTemp && creatTimeTemp < STEM_LEVEL_MAX) || DataController.instance.gameData.berryFieldData[i].hasWeed)
            {
                farmList[i].GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
    void Update()
    {
        if (Input.GetMouseButton(0)) // 마우스 왼쪽 버튼으로
        {
            GameObject obj = ClickObj(); // 클릭당한 옵젝을 가져온다
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

        updateInfo(index_newBerry);

        //폰에서 뒤로가기 버튼 눌렀을 때/에디터에서ESC버튼 눌렀을 때 게임 종료
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DataController.instance.SaveData();
            Application.Quit();
        }
    }
    void LateUpdate()
    {
        //CoinText.text = coin.ToString() + " A";
        ShowCoinText(CoinText, DataController.instance.gameData.coin); // 트럭코인 나타낼 때 같이쓰려고 매개변수로 받게 수정했어요 - 신희규
        HeartText.text = DataController.instance.gameData.heart.ToString();
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
                PlantStrawBerry(st, obj); // 심는다                            
                DataController.instance.gameData.berryFieldData[farm.farmIdx].isPlant = true; // 체크 변수 갱신
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
        ShowCoinText(truckCoinText, DataController.instance.gameData.truckCoin);               
    }
    
    Stem GetStem(int idx)
    {
        if (DataController.instance.gameData.berryFieldData[idx].isPlant) return null;

        return stemList[idx];
    }
    public void PlantStrawBerry(Stem stem, GameObject obj)
    {
        BoxCollider2D coll = obj.GetComponent<BoxCollider2D>();
        //stem.transform.position = obj.transform.position; ; // 밭의 Transform에 딸기를 심는다
        stem.gameObject.SetActive(true); // 딸기 활성화              
        coll.enabled = false; // 밭의 콜라이더를 비활성화 (잡초와 충돌 방지)
    }
    void Harvest(Stem stem)
    {
        AudioManager.instance.HarvestAudioPlay();//딸기 수확할때 효과음

        Farm farm = farmList[stem.stemIdx];
        if (DataController.instance.gameData.berryFieldData[stem.stemIdx].isHarvest) return;
       
        DataController.instance.gameData.berryFieldData[stem.stemIdx].isHarvest = true;
        Vector2 pos = stem.transform.position;
        stem.instantBerry.Explosion(pos, target.position, 0.5f);
        stem.instantBerry.GetComponent<SpriteRenderer>().sortingOrder = 4;

        StartCoroutine(HarvestRoutine(farm, stem)); // 연속으로 딸기가 심어지는 현상을 방지

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
        farm.GetComponent<BoxCollider2D>().enabled = false; // 밭을 잠시 비활성화

        yield return new WaitForSeconds(0.75f); // 0.75초 뒤에

        UpdateTruckState(stem);
        DataController.instance.gameData.totalHarvBerryCnt++; // 수확한 딸기의 총 개수 업데이트
        

        yield return new WaitForSeconds(0.25f); // 0.25초 뒤에
        DataController.instance.gameData.berryFieldData[stem.stemIdx].isPlant = false; // 밭을 비워준다
        stem.gameObject.SetActive(false);
        DataController.instance.gameData.berryFieldData[farm.farmIdx].isHarvest = false; // 수확이 끝남              
        if (!DataController.instance.gameData.berryFieldData[farm.farmIdx].hasWeed) // 잡초가 없다면
        {
            farm.GetComponent<BoxCollider2D>().enabled = true; // 밭을 다시 활성화 
        }
    }
    void UpdateTruckState(Stem stem)
    {
        if (DataController.instance.gameData.truckBerryCnt < TRUCK_CNT_LEVEL_MAX)
        {
            DataController.instance.gameData.truckBerryCnt += 1;
            DataController.instance.gameData.truckCoin += stem.instantBerry.berryPrice;
        }
    }

    #endregion

    #region 재화
    /*void SetBerryPrice()
    {
        BerryPrice[0] = 10;     // 클래식의 0번 딸기
        BerryPrice[64] = 20;    // 스페셜의 0번 딸기
        BerryPrice[128] = 30;   // 유니크의 0번 딸기

        for (int i = 0; i < 192; i++)
        {
            if (i < 64)
                BerryPrice[i] = BerryPrice[0] + i * 3;
            else if (i < 128)
                BerryPrice[i] = BerryPrice[64] + (i - 64) * 5;
            else
                BerryPrice[i] = BerryPrice[128] + (i - 128) * 7;
        }

        //Debug.Log("딸기가치 세팅 완료");
        //Debug.Log(BerryPrice[0] + " " + BerryPrice[64] + " " + BerryPrice[128] + " ");
        //Debug.Log(BerryPrice[9] + " " + BerryPrice[73] + " " + BerryPrice[137] + " ");
    }*/

    public void ShowCoinText(Text coinText, int coin)
    {
        //int coin = DataController.instance.gameData.coin;
        if (coin <= 9999)           // 0~9999까지 A
        {
            coinText.text = coin.ToString() + " A";
        }
        else if (coin <= 9999999)   // 10000~9999999(=9999B)까지 B
        {
            coin /= 1000;
            coinText.text = coin.ToString() + " B";
        }
        else                        // 그 외 C (최대 2100C)
        {
            coin /= 1000000;
            coinText.text = coin.ToString() + " C";
        }
    }

    public void GetCoin(int cost) // 코인 획득 함수
    {
        DataController.instance.gameData.coin += cost; // 현재 코인 +
        DataController.instance.gameData.accCoin += cost; // 누적 코인 +
    }

    public void UseCoin(int cost) // 코인 사용 함수 (마이너스 방지 위함)
    {
        int mycoin = DataController.instance.gameData.coin;
        if (mycoin >= cost)
            DataController.instance.gameData.coin -= cost;
        else
            Debug.Log("코인이 부족해요!"); //경고 패널 등장
    }

    public void GetHeart(int cost) // 하트 획득 함수
    {
        DataController.instance.gameData.heart += cost; // 현재 하트 +
        DataController.instance.gameData.accHeart += cost; // 누적 하트 +
    }

    public void UseHeart(int cost) // 하트 획득 함수 (마이너스 방지 위함)
    {
        int myHeart = DataController.instance.gameData.heart;
        if (myHeart >= cost)
            DataController.instance.gameData.heart -= cost;
        else
            Debug.Log("하트가 부족해요!"); //경고 패널 등장
    }
    #endregion

    #region 콜라이더
    public void DisableObjColliderAll() // 모든 오브젝트의 collider 비활성화
    {
        BoxCollider2D coll;
        isblackPanelOn = true;
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
        isblackPanelOn = false;
        for (int i = 0; i < farmList.Count; i++)
        {
            coll = farmList[i].GetComponent<BoxCollider2D>();
            if (!DataController.instance.gameData.berryFieldData[i].isPlant && !DataController.instance.gameData.berryFieldData[i].hasWeed) // 잡초가 없을 때만 빈 밭의 Collider활성화
            {
                coll.enabled = true;
            }
            if (!DataController.instance.gameData.berryFieldData[i].hasBug && !DataController.instance.gameData.berryFieldData[i].hasWeed && DataController.instance.gameData.berryFieldData[i].createTime >= STEM_LEVEL_MAX) // (4)의 상황, 즉 벌레와 잡초 둘 다 없을 때 다 자란 딸기밭의 콜라이더를 켜준다.
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

    #region PTJ
    public void workingApply(List<Sprite> workingList)
    {
        for (int i = 0; i < 3; i++)
        {
            try
            {
                if (workingList[i] == null) { working[i].SetActive(false); }
                else {
                    working[i].SetActive(true);
                    working[i].transform.GetChild(0).transform.GetComponent<Image>().sprite = workingList[i];
                }
            }
            catch {}//여기 에러?
        }
    }
    //PTJ몇명 고용했는지
    public void workingCount(int employCount) 
    { workingCountText.GetComponent<Text>().text = employCount.ToString(); } // + "/3"
    #endregion

    #region New Berry Add
    //새로운 딸기 추가
    public void updateInfo(int index)
    {

        try
        {
            if (isStart_newBerry == true)
            {
                if (time_newBerry[index] > 0) //시간이 0보다 크면 1초씩 감소
                {
                    time_newBerry[index] -= Time.deltaTime;
                    startBtn_newBerry.GetComponent<Image>().sprite = ingImg;//진행중 이미지
                }
                else
                { 
                    startBtn_newBerry.GetComponent<Image>().sprite = doneImg; //완료 이미지
                }
            }

            //현재 price와 time text를 보인다.
            priceText_newBerry.GetComponent<Text>().text 
                = price_newBerry[index].ToString();
            timeText_newBerry.GetComponent<Text>().text 
                = TimeForm(Mathf.CeilToInt(time_newBerry[index])); //정수부분만 출력
        }
        catch
        { Debug.Log("다음 레벨 정보 없음"); }
    }



    //새로운 딸기 버튼을 누르면
    public void newBerryAdd()
    {
        if (startBtn_newBerry.GetComponent<Image>().sprite == startImg) // 시작 버튼 클릭
        {
            Debug.Log("^_^");
            Time.timeScale = 0;//선택하기 전까지 시간 멈춤
            newBerryTimeReuce.SetActive(true);//하트로 시간 줄이겠냐는 패널 띄우기
        }
        else { Time.timeScale = 1; }


        //타이머가 0 이라면 (=타이머 끝나서 베리 얻을 수 있음)
        if (time_newBerry[index_newBerry] < 0.9)
        {

            int newBerryIndex = 0;
            while (DataController.instance.gameData.isBerryUnlock[newBerryIndex] == true)
            {
                //아직 없는 베리 찾아서 true로 변경
                //없는 베리중에 랜덤으로 하나 결정(임의로 0~10)
                newBerryIndex = UnityEngine.Random.Range(0, 11);
            }
            //새로운 딸기가 추가된다.
            DataController.instance.gameData.isBerryUnlock[newBerryIndex] = true;

            //딸기 얻음 효과음(짜잔)
            AudioManager.instance.TadaAudioPlay();

            //얻은 딸기 설명창
            newBerryAcheive.SetActive(true);
            newBerryAcheive.transform.GetChild(0).GetComponent<Image>().sprite 
                = Globalvariable.instance.berryListAll[newBerryIndex].GetComponent<SpriteRenderer>().sprite;
            newBerryAcheive.transform.GetChild(1).GetComponent<Text>().text
                = Globalvariable.instance.berryListAll[newBerryIndex].GetComponent<Berry>().berryName;





            //코인 소비
            UseCoin(price_newBerry[index_newBerry]);

            //업그레이드 레벨 상승 -> 그 다음 업그레이드 내용, 금액 보인다
            index_newBerry++;
            updateInfo(index_newBerry);

            //시작버튼으로 변경
            startBtn_newBerry.GetComponent<Image>().sprite = startImg;
            isStart_newBerry = false;
        }
        //타이머 0이 아니라면 타이머를 시작한다.
        else
        {
            isStart_newBerry = true;
        }

    }

    #endregion

    #region Explanation
    public void Explanation(GameObject berry,int prefabnum)
    {

        try
        {
            if (DataController.instance.gameData.isBerryUnlock[prefabnum] == true)
            {

                //설명창 띄운다
                berryExp.SetActive(true);

                GameObject berryExpImage = berryExp.transform.GetChild(2).gameObject;
                GameObject berryExpName = berryExp.transform.GetChild(3).gameObject;
                GameObject berryExpTxt = berryExp.transform.GetChild(4).gameObject;

                //Explanation 내용을 채운다.
                berryExpImage.GetComponentInChildren<Image>().sprite
                    = berry.GetComponent<SpriteRenderer>().sprite;//이미지 설정

                berryExpName.gameObject.GetComponentInChildren<Text>().text
                    = berry.GetComponent<Berry>().berryName;//이름 설정

                berryExpTxt.transform.gameObject.GetComponentInChildren<Text>().text
                    = berry.GetComponent<Berry>().berryExplain;//설명 설정

                
            }
        }
        catch
        {
            Debug.Log("여기에 해당하는 베리는 아직 없다");
        }
    }

    #endregion

    //활성화 비활성화로 창 끄고 켜고
    public void turnOff(GameObject Obj)
    {
        if (Obj.activeSelf == true)
        { Obj.SetActive(false); }
        else
        { Obj.SetActive(true); }
    }
    public void turnOffAll() {//테스트중
        berryExp.SetActive(false);
        newBerryTimeReuce.SetActive(false);
        newBerryAcheive.SetActive(false);
    }
    public string TimeForm(int time)//초단위 시간을 분:초로 변경
    {
        int M = 0, S = 0;//M,S 계산용
        string Minutes, Seconds;//M,S 텍스트 적용용

        M = (time / 60);
        S = (time % 60);


        //M,S적용
        Minutes = M.ToString();
        Seconds = S.ToString();

        //M,S가 10미만이면 01, 02... 식으로 표시
        if (M < 10 && M > 0) { Minutes = "0" + M.ToString(); }
        if (S < 10 && S > 0) { Seconds = "0" + S.ToString(); }

        //M,S가 0이면 00으로 표시한다.
        if (M == 0) { Minutes = "00"; }
        if (S == 0) { Seconds = "00"; }


        return Minutes + " : " + Seconds;

    }
    #endregion

    #region 출석

    //인터넷 시간 가져오기.

    IEnumerator WebCheck() 
    {
        UnityWebRequest request = new UnityWebRequest();
        using (request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string date = request.GetResponseHeader("date");
                DateTime dateTime = DateTime.Parse(date);
                DataController.instance.gameData.Today = dateTime;
            }
        }
    }

    //자정 체크 및 정보갱신

    public void CheckTime()
    {
        //플레이 도중 자정이 넘어갈 경우 출석 가능
        // 자정시간 구하기.
        DateTime target = new DateTime(DataController.instance.gameData.Today.Year, DataController.instance.gameData.Today.Month, DataController.instance.gameData.Today.Day);
        target = target.AddDays(1);
        // 자정시간 - 현재시간
        TimeSpan ts = target - DataController.instance.gameData.Today;
        // 남은시간 만큼 대기 후 OnTimePass 함수 호출.
        Invoke("OnTimePass", (float)ts.TotalSeconds);
    }

    public void OnTimePass()
    {
        //정보갱신
        DataController.instance.gameData.attendance = false;
        AttendanceCheck.GetComponent<AttendanceCheck>().Attendance();
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

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
    #endregion

}