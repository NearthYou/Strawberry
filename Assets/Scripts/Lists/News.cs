using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class News : MonoBehaviour
{
    public static News instance;

    [Serializable]
    public class NewsStruct
    {
        public string Title;//����
        public string Exp;//���� ����
        public int Price;
        public bool isShort;
        public NewsStruct(string Title, string Exp,int Price,bool isShort)
        {
            this.Title = Title;
            this.Exp = Exp;
            this.Price = Price;
            this.isShort = isShort;
        }
    }

    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    NewsStruct[] Info;

    [Header("==========OBJECT==========")]
    [SerializeField]
    private GameObject TitleText;
    [SerializeField]
    private GameObject CountText;
    [SerializeField]
    private GameObject Lock;//���� ���
    [SerializeField]
    private GameObject Unlockable;//���� ��� ���� ����


    //�߰� �� Prefab ��
    static int Prefabcount = 0;
    //�ڽ��� ���° Prefab����
    int prefabnum;

    GameObject newsExp;
    GameObject newsContent;
    GameObject LockNum;

    //��� â �гε�
    GameObject WarningPanel;
    GameObject YNPanel;
    GameObject ConfirmPanel;
    GameObject WarningPanelBlack;
    //=======================================================================================================================
    //=======================================================================================================================
    private void Awake()
    {
        instance = this;

        //�����յ鿡�� ���� ��ȣ ���̱�
        if (Prefabcount >= 15) { Prefabcount %= 15; }
        prefabnum = Prefabcount;
        Prefabcount++;
    }
    private void Start()
    {
        newsExp = GameObject.FindGameObjectWithTag("NewsExplanation").transform.GetChild(0).gameObject;
        newsContent = GameObject.FindGameObjectWithTag("NewsContent");
        LockNum = Lock.transform.GetChild(1).gameObject;


        WarningPanel = GameObject.FindGameObjectWithTag("WarningPanel");
        WarningPanelBlack= WarningPanel.transform.GetChild(0).gameObject;
        YNPanel = WarningPanel.transform.GetChild(2).gameObject;
        ConfirmPanel= WarningPanel.transform.GetChild(7).gameObject;

        //�޴�
        GameManager.instance.ShowMedalText();
     
        //����
        TitleText.GetComponent<Text>().text = Info[prefabnum].Title;
        //���� ��ȣ ����
        if (prefabnum < 9) { CountText.GetComponent<Text>().text = "0" + (prefabnum + 1); }
        else { CountText.GetComponent<Text>().text = (prefabnum + 1).ToString(); }
        //�رݿ� �ʿ��� ���� ��
        LockNum.GetComponent<Text>().text = "x"+Info[prefabnum].Price.ToString();

        //���� ���� ������Ʈ
        InfoUpdate();
    }
    //==================================================================================================================
    //==================================================================================================================
    
    public void InfoUpdate()
    {
        //���� ���¿� ���� lock, unlockable, unlock���� ���̱� 
        switch (DataController.instance.gameData.newsState[prefabnum]) 
        {
            case 0://LOCK
                CountText.SetActive(false);
                Lock.SetActive(true);
                Unlockable.SetActive(false);
                break;
            case 1://UNLOCK ABLE
                CountText.SetActive(false);
                Lock.SetActive(false);
                Unlockable.SetActive(true);
                break;
            case 2://UNLOCK
                CountText.SetActive(true);
                Lock.SetActive(false);
                Unlockable.SetActive(false);
                break;
        }

    }


    public void NewsButton()
    {
        AudioManager.instance.Cute1AudioPlay();
        switch (DataController.instance.gameData.newsState[prefabnum])
        {
            case 0://LOCK
                GameManager.instance.NewsPrefabNum = prefabnum;
                YNPanel.GetComponent<PanelAnimation>().OpenScale();
                YNPanel.transform.GetChild(1).GetComponent<Text>().text
                    = "����" + Info[prefabnum].Price + "���� �Ҹ��Ͽ�\n������ �ر��ұ��?";
                WarningPanelBlack.SetActive(true);
                break;
            case 1://UNLOCK ABLE
                //ó�� ������ ��Ȳ
                Explantion();
                DataController.instance.gameData.newsState[prefabnum] = 2;
                break;
            case 2://UNLOCK
                //���� â�� ����.
                Explantion();
                break;
        }

        InfoUpdate();
    }

    //���� �ر�
    public void NewsUnlock(int ID) //ID==PrefabNum
    {

        if (DataController.instance.gameData.medal >= Info[ID].Price)//�޴��� ����ϸ�
        {
            //�޴� �Һ�
            GameManager.instance.UseMedal(Info[ID].Price);

            //unlock���·� ����
            DataController.instance.gameData.newsState[ID] = 1;
            InfoUpdate();
            GameObject thisNews = newsContent.transform.GetChild(ID).gameObject;
            thisNews.transform.GetChild(3).gameObject.SetActive(false);//Lock
            thisNews.transform.GetChild(4).gameObject.SetActive(true);//Lock


            int RandomNum = UnityEngine.Random.Range(1, 101);
            if (RandomNum <= Info[ID].Price * 10 && GameManager.instance.isNewBerryAble()) //price*10%Ȯ���� ���ʽ� ���� ȹ��
            {
                //���� ȹ��
                WarningPanelBlack.SetActive(true);
                ConfirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "������ ���ʽ� ���Ⱑ �رݵǾ����!";
                ConfirmPanel.GetComponent<PanelAnimation>().OpenScale();
                GameManager.instance.newsBerry();
            }

            else //������ �ر�
            {
                //�ȳ�â
                WarningPanelBlack.SetActive(true);
                ConfirmPanel.GetComponent<PanelAnimation>().ScriptTxt.text = "������ �رݵǾ����!";
                ConfirmPanel.GetComponent<PanelAnimation>().OpenScale();
            }
            YNPanel.SetActive(false);

        }
        else
        {
            //�޴��� ������ ��
            YNPanel.GetComponent<PanelAnimation>().CloseScale();
            WarningPanelBlack.SetActive(true);
            ConfirmPanel.GetComponent<PanelAnimation>().OpenScale();
            ConfirmPanel.transform.GetChild(0).transform.GetComponent<Text>().text = "�޴��� �����ؿ�!";
            
        }
    }

    //���� ����â
    private void Explantion()
    {
        //����â�� ����.
        newsExp.SetActive(true);


        Info[prefabnum].Exp=Info[prefabnum].Exp.Replace("\\n", "\n");//\n����

        newsExp.transform.GetChild(2).GetChild(1).GetComponent<Scrollbar>().value = 1; //Scrollbar Vertical ��ũ�� ������ �̵�


        if (Info[prefabnum].isShort == true) //���� ª���� content������ ����
        {
            newsExp.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta 
                = new Vector2(-81, 900);
        }
        else 
        {
            newsExp.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta 
                = new Vector2(-81, 1260);
        }

        newsExp.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text//ȯ��
            = Info[prefabnum].Title;//����
        newsExp.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text
            = Info[prefabnum].Exp;//����


    }

}
