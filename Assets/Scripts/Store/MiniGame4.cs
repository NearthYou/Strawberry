using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame4 : MiniGame
{
    [Header("MiniGame 4")]
    public GameObject leftImage;
    public GameObject rightImage;
    public GameObject content;


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
        //���ʰ� �����ʿ� �ش��ϴ� ������ ����.
        int leftOne;
        int rightOne;

        leftOne = Random.Range(0, 10);
        do { rightOne = Random.Range(0, 10); } while (leftOne == rightOne);

        leftSprite = berryListAll[leftOne].GetComponent<SpriteRenderer>().sprite;
        rightSprite = berryListAll[rightOne].GetComponent<SpriteRenderer>().sprite;

        
        //�ش� ������� ���δ�.
        leftImage.GetComponent<Image>().sprite = leftSprite;
        rightImage.GetComponent<Image>().sprite = rightSprite;

        leftImage.GetComponent<Image>().preserveAspect = true;
        rightImage.GetComponent<Image>().preserveAspect = true;


        for (int i = 0; i < content.transform.childCount; i++) 
        {
            int ran = Random.Range(0, 4);

            if (ran == 0 || ran == 1)
            { content.transform.GetChild(i).GetComponent<Image>().sprite = leftSprite; }
            else 
            { content.transform.GetChild(i).GetComponent<Image>().sprite = rightSprite; }

            content.transform.GetChild(i).GetComponent<Image>().preserveAspect = true;

        }

    }


    public void clickAnswer(bool isLeft) 
    {

        //���俩�� �Ǻ�=======================================
        Sprite answerSprite;
        if (isLeft == true) { answerSprite = leftSprite; }
        else { answerSprite = rightSprite; }


        //����!!
        if (content.transform.GetChild(content.transform.childCount - 1).GetComponent<Image>().sprite == answerSprite)
        {
            score += 10;
            score_txt.text = score.ToString();
            //���� ȿ��///////////////////////////////////
        }
        //����!!
        else
        {
            scrollbar.size -= size * 10;
            time -= 10;
            //ȭ�� ����?////////////////////////////////
            
        }
        //�¿� �̵�==========================================
        //���� Ȥ�� ���������� �̵�///////////////////////////////////

        //+)���� �ø�������?. �¿� �����ϱ�?/////////////////////////

        //updateContent ��������Ʈ�� ������Ʈ �ȴ�.=================================

        //�� �տ� �ִ� ��������Ʈ�� �ڷ� �ڷ� �̵���Ų��.
        content.transform.GetChild(content.transform.childCount - 1).GetComponent<RectTransform>().SetAsFirstSibling();

        //��� �� �� �ڷ� �� ��������Ʈ�� ���ο� ��������Ʈ�� �����Ѵ�.
        int ran = Random.Range(0, 4);
        if (ran == 0 || ran == 1)
            content.transform.GetChild(0).GetComponent<Image>().sprite = leftSprite;
        else
            content.transform.GetChild(0).GetComponent<Image>().sprite = rightSprite;
    }

}
