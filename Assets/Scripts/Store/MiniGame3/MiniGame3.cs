using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame3 : MiniGame
{
    [Header("MiniGame3")]
    public GameObject basket;
    private RectTransform basketRect;
    public RectTransform bgRect;
    public GameObject miniGameBerryPref;
    public List<GameObject> berryPool = new List<GameObject>();
    private float minigame_3_src_rndtime;
    private float minigame_3_dst_rndtime;
    private bool isDrag;
    float accTime, randTime;
    public RectTransform berryGroup;
    public new Camera camera;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        basketRect = basket.GetComponent<RectTransform>();     
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        minigame_3_src_rndtime = 0.8f;
        minigame_3_dst_rndtime = 1.2f;
        
        //바구니의 위치 초기화
        basketRect.anchoredPosition = new Vector3(bgRect.rect.width/2 - 125f, bgRect.rect.height*0.25f - 75f, 0f);
    }
    public void PointDown()
    {
        isDrag = true;
    }
    public void PointUp()
    {
        isDrag = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isGameRunning) return;

        int score = GetComponent<MiniGame3>().score;

        if (score >= 100) // 점수에 따라 딸기 생성주기 변경
        {
            minigame_3_src_rndtime = 0.3f;
            minigame_3_dst_rndtime = 0.6f;
        }
        randTime = UnityEngine.Random.Range(minigame_3_src_rndtime, minigame_3_dst_rndtime);

        // 드래그해서 바구니 옮기기!
        if (isDrag)
        {                     
            Vector3 mousePos = Input.mousePosition;

            mousePos = camera.ScreenToViewportPoint(mousePos);
            mousePos.x = mousePos.x * bgRect.rect.width;

            //Debug.Log("mousePos.x = " + mousePos.x);
            float leftBorder = 0f;
            float rightBorder = bgRect.rect.width - basketRect.rect.width;           
            
            mousePos.y = bgRect.rect.height * 0.25f - 75f; // y좌표 고정
            mousePos.z = 0;
            if (mousePos.x < leftBorder) mousePos.x = leftBorder;
            else if (mousePos.x > rightBorder) mousePos.x = rightBorder;
            else mousePos.x = mousePos.x - basketRect.rect.width / 2;

            basketRect.anchoredPosition = Vector3.Lerp(basketRect.anchoredPosition, mousePos, 0.2f);
        }
        accTime += Time.deltaTime;

        // 랜덤한 시간마다 딸기 생성
        if(accTime >= randTime)
        {
            MinigameBerry miniBerry = GetMiniGameBerry();
            miniBerry.gameObject.SetActive(true);
            accTime = 0f;
            randTime = UnityEngine.Random.Range(0.5f, 1.0f);
        }
    }
    MinigameBerry GetMiniGameBerry()
    {
        for (int i = 0; i < berryPool.Count; i++)
        {
            if (!berryPool[i].gameObject.activeSelf)
            {
                int rndId = unlockList[UnityEngine.Random.Range(0, unlockList.Count)];
                berryPool[i].GetComponent<Image>().sprite = global.berryListAll[rndId].GetComponent<SpriteRenderer>().sprite;

                float bugrnd = UnityEngine.Random.Range(0f, 10f);
                if(bugrnd <= 2f)
                {
                    //Debug.Log("Bug!!");
                    berryPool[i].transform.GetChild(0).gameObject.SetActive(true);
                }
                return berryPool[i].GetComponent<MinigameBerry>();
            }
        }
        return MakeMiniGameBerry(); // 비활성화된 딸기가 없다면 새로 만든다.
    }
    MinigameBerry MakeMiniGameBerry()
    {
        GameObject instantMiniBerryObj = Instantiate(miniGameBerryPref, berryGroup);
        int rndId = unlockList[UnityEngine.Random.Range(0, unlockList.Count)];

        instantMiniBerryObj.GetComponent<Image>().sprite = global.berryListAll[rndId].GetComponent<SpriteRenderer>().sprite;
        instantMiniBerryObj.name = "MiniBerry " + berryPool.Count;

        berryPool.Add(instantMiniBerryObj);

        MinigameBerry instantMiniBerry = instantMiniBerryObj.GetComponent<MinigameBerry>();
        instantMiniBerry.bgRect = bgRect;
        instantMiniBerry.basketRect = basketRect;

        instantMiniBerry.transform.GetChild(0).gameObject.SetActive(false);

        return instantMiniBerry;
    }
    public override void OnClickPauseButton()
    {
        base.OnClickPauseButton();        
    }
    public override void OnClickKeepGoingButton()
    {
        base.OnClickKeepGoingButton();     
    }

    public override void StopGame()
    {
        base.StopGame();
        for (int i = 0; i < berryPool.Count; i++)
        {
            berryPool[i].SetActive(false);
        }       
    }

    public override void ReStart() //다시하기
    {
        base.ReStart();       
    }
    protected override void FinishGame()
    {
        base.FinishGame();
        AudioManager.instance.EndAudioPlay();

        int score = GetComponent<MiniGame3>().score;
        ManageScore(2, score);

        //결과패널
        resultPanel.SetActive(true);
        result_dotori_txt.text = DataController.instance.gameData.dotori + " / 5";
        result_cur_score_txt.text = score + "점";
        result_highscore_txt.text = DataController.instance.gameData.highScore[2].ToString();

        // 미니게임3 보상 코인
        float gain_coin = score * DataController.instance.gameData.researchLevelAv * (110 / 100f);
        //result_coin_txt.text = gain_coin.ToString();
        GameManager.instance.ShowCoinText(result_coin_txt.GetComponent<Text>(), Convert.ToInt32(gain_coin));
        //Debug.Log("얻은 코인:" + Convert.ToInt32(gain_coin));
        
        // 코인 지급
        GameManager.instance.GetCoin(Convert.ToInt32(gain_coin));

        StopGame();
    }
}
