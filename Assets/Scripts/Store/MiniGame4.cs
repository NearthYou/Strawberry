using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MiniGame4 : MiniGame
{
    [Header("MiniGame 4")]
    public GameObject leftImage;
    public GameObject rightImage;
    public GameObject content;
    public GameObject correctTxt;
    public GameObject fakeImage;

    Sprite leftSprite;
    Sprite rightSprite;


    List<GameObject> berryListAll;//global�� berryListAll

    protected override void Awake()
    {

        berryListAll = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>().berryListAll;

        SetGame();
        base.Awake();

    }
    protected override void OnEnable()
    {
        base.OnEnable();
       
    }
    void Start()
    {
        
    }



    void SetGame() 
    {
        //���ʰ� ������ ���� ����
        int leftOne;
        int rightOne;
        do { leftOne = UnityEngine.Random.Range(0, 10); } 
        while (DataController.instance.gameData.isBerryUnlock[leftOne] == false);
        
        do { rightOne = UnityEngine.Random.Range(0, 10); } 
        while (leftOne == rightOne || DataController.instance.gameData.isBerryUnlock[rightOne] == false);

        leftSprite = berryListAll[leftOne].GetComponent<SpriteRenderer>().sprite;
        rightSprite = berryListAll[rightOne].GetComponent<SpriteRenderer>().sprite;

        
        //�ش� ������� ���δ�.
        leftImage.GetComponent<Image>().sprite = leftSprite;
        rightImage.GetComponent<Image>().sprite = rightSprite;

        leftImage.GetComponent<Image>().preserveAspect = true;
        rightImage.GetComponent<Image>().preserveAspect = true;


        for (int i = 0; i < content.transform.childCount; i++) 
        {
            int ran = UnityEngine.Random.Range(0, 4);

            if (ran == 0 || ran == 1)
            { content.transform.GetChild(i).GetComponent<Image>().sprite = leftSprite; }
            else 
            { content.transform.GetChild(i).GetComponent<Image>().sprite = rightSprite; }

            content.transform.GetChild(i).GetComponent<Image>().preserveAspect = true;

        }

    }


    public void clickAnswer(bool isLeft) 
    {
        //ȿ�� �ʱ�ȭ
        StopCoroutine("FadeCoroutine");
        correctTxt.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0f);

        StopCoroutine(MoveCoroutine(true, content.transform.GetChild(content.transform.childCount - 1).gameObject));
        fakeImage.SetActive(false);
        fakeImage.GetComponent<RectTransform>().position = new Vector3(50f,-597f,0);
        

        //���俩�� �Ǻ�=======================================
        Sprite answerSprite;
        if (isLeft == true) { answerSprite = leftSprite; }
        else { answerSprite = rightSprite; }


        //����!!
        if (content.transform.GetChild(content.transform.childCount - 1).GetComponent<Image>().sprite == answerSprite)
        {
            score += 10;
            score_txt.text = score.ToString();

            //���� ȿ��//////////////////////////�ǵ���ʿ�////////////////////////////////////
            correctTxt.GetComponent<Text>().color=new Color(1f,1f,1f,1f);
            StartCoroutine("FadeCoroutine");
        }
        //����!!
        else
        {
            scrollbar.size -= size * 10;
            time -= 10;

            //ȭ�� ����?////////////////////////////////

        }
        //�¿� �̵�==========================================
        StartCoroutine(MoveCoroutine(isLeft, content.transform.GetChild(content.transform.childCount - 1).gameObject));




        //+)���� �ø�������?. �¿� �����ϱ�?/////////////////////////




        //updateContent ��������Ʈ�� ������Ʈ=================================

        //�� �տ� �ִ� ��������Ʈ�� �ڷ� �ڷ� �̵���Ų��.
        content.transform.GetChild(content.transform.childCount - 1).GetComponent<RectTransform>().SetAsFirstSibling();

        //��� �� �� �ڷ� �� ��������Ʈ�� ���ο� ��������Ʈ�� �����Ѵ�.
        int ran = UnityEngine.Random.Range(0, 4);
        if (ran == 0 || ran == 1)
            content.transform.GetChild(0).GetComponent<Image>().sprite = leftSprite;
        else
            content.transform.GetChild(0).GetComponent<Image>().sprite = rightSprite;
    }

    protected override void FinishGame()
    {
        base.FinishGame();

        //�ְ��� ����
        if (DataController.instance.gameData.highScore[3] < score)
        {
            DataController.instance.gameData.highScore[3] = score;
        }

        //����г�
        resultPanel.SetActive(true);
        result_txt.text = "�ְ��� : " + DataController.instance.gameData.highScore[3] + "\n�������� : " + score;

        // �̴ϰ��� 3 ���� ��Ʈ ����(�̴ϰ��� 3�� �ر� ��Ʈ�� 20�̴�)
        float gain_coin = score * research_level_avg * ((100 + 80 * 2) / 100f);

        Debug.Log("���� ����:" + Convert.ToInt32(gain_coin));
        //��Ʈ����
        GameManager.instance.GetCoin(Convert.ToInt32(gain_coin));

        base.StopGame();
    }


    IEnumerator FadeCoroutine()
    {
        float fadeCount = 1;
        while (fadeCount > -0.1f) 
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            correctTxt.GetComponent<Text>().color = new Color(1f, 1f, 1f, fadeCount);
        }
    }
    IEnumerator MoveCoroutine(bool isLeft,GameObject content)
    {
        fakeImage.GetComponent<Image>().sprite = content.GetComponent<Image>().sprite;
        fakeImage.GetComponent<Image>().preserveAspect = true;
        fakeImage.SetActive(true);

        Vector3 moveCount = content.GetComponent<RectTransform>().position;
        float fadeCount = 1;

        while (fadeCount > -0.1f)
        {
            //���������
            fadeCount -= 0.05f;
            fakeImage.GetComponent<Image>().color = new Color(1f, 1f, 1f, fadeCount);

            //�����̵���
            if (isLeft == true)
            { moveCount.x -= 0.05f; }
            else
            { moveCount.x += 0.05f; }
            fakeImage.GetComponent<RectTransform>().position = moveCount;

            yield return new WaitForSeconds(0.01f);

           
        }
        fakeImage.SetActive(false);
        fakeImage.GetComponent<RectTransform>().position = new Vector3(50f, -597f, 0);
    }

}
