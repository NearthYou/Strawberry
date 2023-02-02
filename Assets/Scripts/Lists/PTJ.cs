using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PTJ : MonoBehaviour
{
    public static PTJ instance;

    [Serializable]
    public class PrefabStruct
    {
        public string Name;//�˹ٻ� �̸�
        public Sprite Picture;//����
        public Sprite FacePicture;//�˹ٻ� �� ����
        public string Explanation;//����
        public int Price;//����

        public PrefabStruct(string Name, string Explanation, int Price, Sprite Picture, Sprite FacePicture)
        {
            this.Name = Name;
            this.Explanation = Explanation;
            this.Price = Price;
            this.Picture = Picture;
            this.FacePicture = FacePicture;
        }
    }

    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    public PrefabStruct[] Info;//����ü

    [Header("==========INFO ������ ���=========")]
    [SerializeField]
    private GameObject PTJBackground;
    public GameObject nameText;
    public GameObject facePicture;
    public GameObject explanationText;
    public GameObject coinNum;
    public GameObject employTF;


    [Header("==========PTJ select Sprite===========")]
    //PTJ��� ���� ���� ��������Ʈ
    public Sprite selectPTJSprite;
    public Sprite originalPTJSprite;
    [Header("==========PTJ Button Sprite===========")]
    public Sprite FireButtonSprite;
    public Sprite HireButtonSprite;



    //==========Prefab���� ���� �ο�==========
    static int Prefabcount = 0;
    int prefabnum;

    //==========PTJ â==========
    private GameObject PTJPanel;

    //==========���� ��� �� ���==========
    static List<Sprite> workingList = new List<Sprite>();


    //==========PTJ Panel==========
    private GameObject PTJSlider;
    private GameObject PTJSlider10;
    private GameObject PTJToggle;

    private GameObject SliderNum;
    private GameObject price;

    private GameObject PTJButton;

    private GameObject HireNowLock;

    //==========��� Ƚ��==========
    private int PTJ_NUM_NOW;

    //???
    //�� ��ƼŬ
    //private ParticleSystem rainParticle;
    // �۷ι� ����
    //private Globalvariable globalVar;


    //===================================================================================================
    //===================================================================================================
    private void Awake()
    {
        instance = this;
        //�����յ鿡�� ���� ��ȣ ���̱�
        if (Prefabcount >= 6) { Prefabcount %= 6; }
        prefabnum = Prefabcount;
        Prefabcount++;

    }

    void Start()
    {
        //==========PTJ Explanation Panel==========
        PTJPanel = GameObject.FindGameObjectWithTag("PTJExplanation");
        PTJPanel = PTJPanel.transform.GetChild(prefabnum).GetChild(0).gameObject;

        PTJToggle = PTJPanel.transform.GetChild(8).transform.gameObject;//10���� üũ ���
        PTJSlider = PTJPanel.transform.GetChild(9).transform.gameObject;//1���� �����̴�
        PTJSlider10 = PTJPanel.transform.GetChild(10).transform.gameObject;//10���� �����̴�

        SliderNum = PTJPanel.transform.GetChild(11).gameObject;
        price = PTJPanel.transform.GetChild(7).gameObject;

        PTJButton = PTJPanel.transform.GetChild(6).transform.gameObject;
        HireNowLock= PTJPanel.transform.GetChild(12).transform.gameObject;

        
        //??============================
        //rainParticle = GameObject.FindGameObjectWithTag("Rain").GetComponent<ParticleSystem>();
        //globalVar = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>();

    }

    void OnEnable()
    {
        PreafbInfoUpdate();
    }

    private void Update()
    {

        //�ڽ��� ���Ƚ���� ���� �ľ�
        PTJNumNow = DataController.instance.gameData.PTJNum[prefabnum];
        //�˹� ��� ���� �ݿ�
        EmployStateApply();
    }


    int PTJNumNow
    {
        set
        {
            if (PTJ_NUM_NOW == value) return;
            PTJ_NUM_NOW = value;

            //����� ���� 0(�� �˹ٸ� ���´ٸ�)�̸� �Ʒ��� ����
            if (PTJ_NUM_NOW == 0)
            {
                InitSlider();
            }
            else 
            {
                HireNowLock.transform.GetChild(0).transform.GetComponent<Text>().text
                = "���� ��� Ƚ��: " + DataController.instance.gameData.PTJNum[prefabnum].ToString() + "ȸ";
            }
        }
        get { return PTJ_NUM_NOW; }
    }

    //===================================================================================================
    //===================================================================================================
    public void PreafbInfoUpdate()
    {

        //====������ ���� ä���====
        //====�Һ�====
        //�̸�
        nameText.GetComponent<Text>().text = Info[prefabnum].Name;
        //�˹ٻ� ����
        facePicture.GetComponent<Image>().sprite = Info[prefabnum].Picture;
        facePicture.GetComponent<Image>().preserveAspect = true;
        //����
        explanationText.GetComponent<Text>().text = Info[prefabnum].Explanation;
        //���
        if (DataController.instance.gameData.researchLevelAv < 5) // ���� ������ 5���� ���϶��
            GameManager.instance.ShowCoinText(coinNum.GetComponent<Text>(), Info[prefabnum].Price);
        else if (DataController.instance.gameData.researchLevelAv < 10) // ���� ������ 10���� ���϶��
            GameManager.instance.ShowCoinText(coinNum.GetComponent<Text>(), Info[prefabnum].Price * 2);
        else
            GameManager.instance.ShowCoinText(coinNum.GetComponent<Text>(), Info[prefabnum].Price * 4); // ���ӸŴ��� ��ũ��Ʈ�� ����
    }

    //PTJ Explanation Panel ����
    public void PTJPanelActive()
    {
        //ȿ����
        AudioManager.instance.Cute1AudioPlay();

        DataController.instance.gameData.PTJSelectNum[0] = prefabnum;
        //==== �˹� â ====
        //�˹� â�� ����.
        PTJPanel.transform.GetChild(0).gameObject.SetActive(true);
        PTJPanel.GetComponent<PanelAnimation>().OpenScale();


        //�˹� â ä���
        GameObject picture = PTJPanel.transform.GetChild(3).gameObject;
        GameObject name = PTJPanel.transform.GetChild(4).gameObject;
        GameObject explanation = PTJPanel.transform.GetChild(5).gameObject;

        picture.GetComponent<Image>().sprite = Info[prefabnum].Picture;
        picture.GetComponent<Image>().preserveAspect = true;
        name.GetComponent<Text>().text = Info[prefabnum].Name;
        explanation.GetComponent<Text>().text = Info[prefabnum].Explanation;

        //�˹� �����̴�
        InitSlider();//�����̴� �ʱ�ȭ

        //Slider�� ���� ���� -> nȸ, ��� �ݿ�
        PTJSlider.transform.GetComponent<Slider>().onValueChanged.AddListener
            (delegate { SliderApply((int)PTJSlider.GetComponent<Slider>().value); });
        PTJSlider10.transform.GetComponent<Slider>().onValueChanged.AddListener
            (delegate { SliderApply((int)PTJSlider10.GetComponent<Slider>().value); });
        PTJToggle.GetComponent<Toggle>().onValueChanged.AddListener
            (delegate { ToggleChange(); });
       
        
    }


    public void EmployStateApply()
    {

        //������� �ƴϴ�
        if (DataController.instance.gameData.PTJNum[prefabnum] == 0)
        {
            //PANEL============================
            //��ư �̹���, ���� ����
            PTJButton.transform.GetComponent<Image>().sprite = HireButtonSprite;
            PTJButton.transform.GetChild(0).transform.GetComponent<Text>().text = "��� �ϱ�";

            //HireNowLock �����
            HireNowLock.SetActive(false);


            //PREFAB===========================
            PTJBackground.GetComponent<Image>().sprite = originalPTJSprite;
            employTF.GetComponent<Text>().text = "�����";
            employTF.GetComponent<Text>().color = new Color32(164, 164, 164, 255);

        }
        //������̴�
        else
        {
            //PANEL============================
            //��ư �̹���, ���� ����
            PTJButton.transform.GetComponent<Image>().sprite = FireButtonSprite;
            PTJButton.transform.GetChild(0).transform.GetComponent<Text>().text = "";

            //HireNowLock ���̱�
            HireNowLock.SetActive(true);


            //PREFAB===========================
            PTJBackground.GetComponent<Image>().sprite = selectPTJSprite;
            employTF.GetComponent<Text>().text = "�����";
            employTF.GetComponent<Text>().color = new Color32(245, 71, 71, 255);//#F54747
        }

    }



    //====================================================================================================
    //====================================================================================================
    //SLIDER
    public void SliderApply(int value)
    {
        //10�����̸� 10�� �����ش�.
        if (PTJToggle.GetComponent<Toggle>().isOn == true)
        {  value *= 10;  }

        //"nȸ"
        SliderNum.transform.GetComponent<Text>().text = value.ToString() + "ȸ";

        DataController.instance.gameData.PTJSelectNum[0] = prefabnum;
        DataController.instance.gameData.PTJSelectNum[1] = value;
        //����
        if (DataController.instance.gameData.researchLevelAv < 5) // ���� ������ 5���� ���϶��
            GameManager.instance.ShowCoinText(price.GetComponent<Text>(), value * Info[prefabnum].Price);
        else if (DataController.instance.gameData.researchLevelAv < 10) // ���� ������ 10���� ���϶��
            GameManager.instance.ShowCoinText(price.GetComponent<Text>(), value * Info[prefabnum].Price * 2);
        else // ���� ������ 15���� ���϶��
            GameManager.instance.ShowCoinText(price.GetComponent<Text>(), value * Info[prefabnum].Price * 4);
    }
    public void ToggleChange()
    {
        //10����
        if (PTJToggle.GetComponent<Toggle>().isOn == true)
        {
            PTJSlider10.SetActive(true);
            PTJSlider.SetActive(false);
            InitSlider();
        }
        else 
        {
            PTJSlider10.SetActive(false);
            PTJSlider.SetActive(true);
            InitSlider();
        }
    }
    public void InitSlider()
    {

        //10����
        if (PTJToggle.GetComponent<Toggle>().isOn == true)
        {
            //slider
            PTJSlider10.SetActive(true);
            PTJSlider.SetActive(false);

            PTJSlider10.GetComponent<Slider>().value = 1;

            DataController.instance.gameData.PTJSelectNum[0] = prefabnum;
            DataController.instance.gameData.PTJSelectNum[1] = 10;
            //nȸ
            SliderNum.transform.GetComponent<Text>().text = "10ȸ";
            //����
            if (DataController.instance.gameData.researchLevelAv < 5) // ���� ������ 5���� ���϶��
                GameManager.instance.ShowCoinText(price.GetComponent<Text>(), 10 * Info[prefabnum].Price);
            else if (DataController.instance.gameData.researchLevelAv < 10) // ���� ������ 10���� ���϶��
                GameManager.instance.ShowCoinText(price.GetComponent<Text>(), 10 * Info[prefabnum].Price * 2);
            else // ���� ������ 15���� ���϶��
                GameManager.instance.ShowCoinText(price.GetComponent<Text>(), 10 * Info[prefabnum].Price * 4);

        }
        //1����
        else
        {
            //slider
            PTJSlider.SetActive(true);
            PTJSlider10.SetActive(false);

            PTJSlider.GetComponent<Slider>().value = 1;


            DataController.instance.gameData.PTJSelectNum[0] = prefabnum;
            DataController.instance.gameData.PTJSelectNum[1] = 1;
            //nȸ
            SliderNum.transform.GetComponent<Text>().text = "1ȸ";
            //����
            if (DataController.instance.gameData.researchLevelAv < 5) // ���� ������ 5���� ���϶��
                GameManager.instance.ShowCoinText(price.GetComponent<Text>(), Info[prefabnum].Price);
            else if (DataController.instance.gameData.researchLevelAv < 10) // ���� ������ 10���� ���϶��
                GameManager.instance.ShowCoinText(price.GetComponent<Text>(), Info[prefabnum].Price * 2);
            else // ���� ������ 15���� ���϶��
                GameManager.instance.ShowCoinText(price.GetComponent<Text>(), Info[prefabnum].Price * 4);
        }
    }
}
