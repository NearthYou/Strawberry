using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class MiniGame4 : MiniGame
{
    [Header("MiniGame 4")]
    public GameObject[] correctImage;
    public GameObject[] boxImage;

    public GameObject content;
    public GameObject correctTxt; //����� ������ �ؽ�Ʈ
    public GameObject leftBtn;
    public GameObject rightBtn;


    int[] correctNum;
    Sprite[] correctSprite;
    Sprite[] answerSprite;

    

    bool isUpgrade;

    List<GameObject> berryListAll;//global�� berryListAll

    //0:�� �Ʒ�    1:�� �Ʒ�    2:�� ��    3:�� ��

    protected override void Awake()
    {

        berryListAll = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>().berryListAll;
        
        correctNum = new int[4] { 0,0,0,0};
        correctSprite = new Sprite[4];
        answerSprite = new Sprite[4];

        base.Awake();
        
    }
    protected override void OnEnable()
    {

        base.OnEnable();
        leftBtn.GetComponent<Button>().interactable = false;
        rightBtn.GetComponent<Button>().interactable = false;
        SetGame();
        
        //upgrade�ʱ�ȭ
        isUpgrade = false;
        boxImage[2].SetActive(false);
        boxImage[3].SetActive(false);
        correctImage[2].SetActive(false);
        correctImage[3].SetActive(false);
    }
    protected override void MakeGame()
    {
        leftBtn.GetComponent<Button>().interactable = true;
        rightBtn.GetComponent<Button>().interactable = true;
    }

    void SetGame()
    {

        //���ʰ� ������ ������ �������� ������(������ 4�� ����)
        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                do { correctNum[i] = UnityEngine.Random.Range(1, 192); }
                while(DataController.instance.gameData.isBerryUnlock[correctNum[i]] == false);
            }

            if (correctNum.Length == correctNum.Distinct().Count())//�ߺ��Ǵ°��� ������
            { break; }
            //correctNum = correctNum.Distinct().ToArray();//�迭 �ߺ� �����ϴ� ��ɾ�
            
        }



        for (int i = 0; i < 4; i++)
        {
            //���� ���� ��������Ʈ 
            correctSprite[i]= berryListAll[correctNum[i]].GetComponent<SpriteRenderer>().sprite;


            //������ ���δ�.
            correctImage[i].GetComponent<Image>().sprite = correctSprite[i];
            correctImage[i].GetComponent<Image>().preserveAspect = true;
        }




        for (int i = 0; i < content.transform.childCount; i++) 
        {
            int ran = UnityEngine.Random.Range(0, 4);

            if (ran == 0 || ran == 1)
            { content.transform.GetChild(i).GetComponent<Image>().sprite = correctSprite[0]; }//left
            else 
            { content.transform.GetChild(i).GetComponent<Image>().sprite = correctSprite[1]; }//right

            content.transform.GetChild(i).GetComponent<Image>().preserveAspect = true;

        }

    }


    public void clickAnswer(bool isLeft) 
    {
        //�ִϸ��̼� ȿ�� �ʱ�ȭ
        StopCoroutine("FadeCoroutine");
        correctTxt.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0f);

        StopCoroutine(MoveCoroutine(true, content.transform.GetChild(content.transform.childCount - 1).gameObject));
        

        //���俩�� �Ǻ�=======================================
        if (isUpgrade == true) //������4���϶�
        {
            if (isLeft == true) //left
            {
                answerSprite[0] = correctSprite[0];
                answerSprite[1] = correctSprite[2];
            }
            else //right
            { 
                answerSprite[0] = correctSprite[1];
                answerSprite[1] = correctSprite[3];
            }
        }
        else
        {
            if (isLeft == true) { answerSprite[0] = correctSprite[0]; }
            else { answerSprite[0] = correctSprite[1]; }
        }
       


        //����!!
        if (isUpgrade==false && (content.transform.GetChild(content.transform.childCount - 1).GetComponent<Image>().sprite == answerSprite[0])||
            isUpgrade == true && 
            (content.transform.GetChild(content.transform.childCount - 1).GetComponent<Image>().sprite == answerSprite[0]|| 
            content.transform.GetChild(content.transform.childCount - 1).GetComponent<Image>().sprite == answerSprite[1])
            )
        {
            score += 5;
            score_txt.text = score.ToString();

            //���� ȿ��
            correctTxt.GetComponent<Text>().color=new Color(1f,1f,1f,1f);
            StartCoroutine("FadeCoroutine");
            AudioManager.instance.Cute1AudioPlay();
        }
        //����!!
        else
        {
            scroll.fillAmount -= size * 10;
            time -= 10;
            AudioManager.instance.Cute5AudioPlay();
        }

        //�¿� �̵�==========================================
        StartCoroutine(MoveCoroutine(isLeft, content.transform.GetChild(content.transform.childCount - 1).gameObject));




        // UPGRADE!! ���� �ø���
        if (score > 200 && isUpgrade==false)
        { 
            isUpgrade = true;
            boxImage[2].SetActive(true);
            boxImage[3].SetActive(true);
            correctImage[2].SetActive(true);
            correctImage[3].SetActive(true);
        }
        




        //updateContent ��������Ʈ�� ������Ʈ=================================

        //�� �տ� �ִ� ��������Ʈ�� �ڷ� �ڷ� �̵���Ų��.
        content.transform.GetChild(content.transform.childCount - 1).GetComponent<RectTransform>().SetAsFirstSibling();


        //��� �� �� �ڷ� �� ��������Ʈ�� ���ο� ��������Ʈ�� �����Ѵ�.
        int ran = UnityEngine.Random.Range(0, 4);
        if (isUpgrade == true)
        {
            switch (ran)
            {
                case 0: content.transform.GetChild(0).GetComponent<Image>().sprite = correctSprite[ran]; break;
                case 1: content.transform.GetChild(0).GetComponent<Image>().sprite = correctSprite[ran]; break;
                case 2: content.transform.GetChild(0).GetComponent<Image>().sprite = correctSprite[ran]; break;
                case 3: content.transform.GetChild(0).GetComponent<Image>().sprite = correctSprite[ran]; break;
            }

        }
        else 
        {
            if (ran == 0 || ran == 1)
                content.transform.GetChild(0).GetComponent<Image>().sprite = correctSprite[0];
            else
                content.transform.GetChild(0).GetComponent<Image>().sprite = correctSprite[1];
        }
        
    }

    protected override void FinishGame()
    {
        base.FinishGame();
        AudioManager.instance.EndAudioPlay();

        ManageScore(3, score);

        //����г�
        resultPanel.SetActive(true);
        result_cur_score_txt.text = score + "��";
        result_highscore_txt.text = DataController.instance.gameData.highScore[3].ToString();

        // �̴ϰ��� 4 ���� ��Ʈ ����(�̴ϰ��� 3�� �ر� ��Ʈ�� 20�̴�)
        float gain_coin = score * research_level_avg * ((100 + 80 * 2) / 100f);

        // ���� �����ְ� ����
        //result_coin_txt.text = gain_coin.ToString();
        // �ִ� �Լ� ���ž��� �� ��
        GameManager.instance.ShowCoinText(result_coin_txt.GetComponent<Text>(), Convert.ToInt32(gain_coin));
        Debug.Log("���� ����:" + Convert.ToInt32(gain_coin));
        GameManager.instance.GetCoin(Convert.ToInt32(gain_coin));

        base.StopGame();
    }


    IEnumerator FadeCoroutine()
    {
        float fadeCount = 1;
        while (fadeCount > -0.1f) 
        {
            fadeCount -= 0.05f;
            yield return new WaitForSeconds(0.01f);
            correctTxt.GetComponent<Text>().color = new Color(1f, 1f, 1f, fadeCount);
        }
    }
    IEnumerator MoveCoroutine(bool isLeft,GameObject content)
    {

        Vector3 moveCount = content.GetComponent<RectTransform>().position;
        float fadeCount = 1;

        while (fadeCount > -0.1f)
        {
            //���������
            fadeCount -= 0.05f;

            //�����̵���
            if (isLeft == true)
            { moveCount.x -= 0.05f; }
            else
            { moveCount.x += 0.05f; }

            yield return new WaitForSeconds(0.01f);

           
        }
    }

}
