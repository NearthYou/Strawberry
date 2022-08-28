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

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        basketRect = basket.GetComponent<RectTransform>();     
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        minigame_3_src_rndtime = 1.0f;
        minigame_3_dst_rndtime = 1.5f;
        
        basketRect.anchoredPosition = new Vector3(425f, 560f, 0f);
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

        if (score >= 100) // ������ ���� ���� �����ֱ� ����
        {
            minigame_3_src_rndtime = 0.5f;
            minigame_3_dst_rndtime = 1.0f;
        }
        randTime = UnityEngine.Random.Range(minigame_3_src_rndtime, minigame_3_dst_rndtime);

        // �巡���ؼ� �ٱ��� �ű��!
        if (isDrag)
        {                     
            Vector3 mousePos = Input.mousePosition;          
            
            float leftBorder = 0f;
            float rightBorder = bgRect.rect.width - basketRect.rect.width;           
            
            mousePos.y = 560;
            mousePos.z = 0;
            if (mousePos.x < leftBorder) mousePos.x = leftBorder;
            else if (mousePos.x > rightBorder) mousePos.x = rightBorder;
            else mousePos.x = mousePos.x - basketRect.rect.width / 2;

            basketRect.anchoredPosition = Vector3.Lerp(basketRect.anchoredPosition, mousePos, 0.2f);
        }
        accTime += Time.deltaTime;

        // ������ �ð����� ���� ����
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
                    Debug.Log("Bug!!");
                    berryPool[i].transform.GetChild(0).gameObject.SetActive(true);
                }
                return berryPool[i].GetComponent<MinigameBerry>();
            }
        }
        return MakeMiniGameBerry(); // ��Ȱ��ȭ�� ���Ⱑ ���ٸ� ���� �����.
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

    public override void ReStart() //�ٽ��ϱ�
    {
        base.ReStart();
        basketRect.anchoredPosition = new Vector3(425f, 560f, 0f);
    }
    protected override void FinishGame()
    {
        base.FinishGame();
        AudioManager.instance.EndAudioPlay();

        int score = GetComponent<MiniGame3>().score;
        //�ְ��� ����
        if (DataController.instance.gameData.highScore[2] < score)
        {
            DataController.instance.gameData.highScore[2] = score;
        }

        //����г�
        resultPanel.SetActive(true);
        result_txt.text = "�ְ��� : " + DataController.instance.gameData.highScore[2] + "\n�������� : " + score;
       
        // �̴ϰ��� 3 ���� ��Ʈ ����(�̴ϰ��� 3�� �ر� ��Ʈ�� 20�̴�)
        float gain_coin = score * research_level_avg * ((100 + 20 * 2) / 100f);
        
        Debug.Log("���� ����:" + Convert.ToInt32(gain_coin));
        //��Ʈ����
        GameManager.instance.GetCoin(Convert.ToInt32(gain_coin));

        StopGame();
    }
}
