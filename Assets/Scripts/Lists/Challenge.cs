using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Challenge : MonoBehaviour
{
    [Serializable]
    public class ChallengeStruct
    {
        public string Title;    //����
        public int rewardMedal; //�޴� ����
        public int rewardHeart; //��Ʈ ����
        public int[] clearCriterion;  //�޼� ����
        public Sprite challengeImage;

        public ChallengeStruct(string Title, int rewardMedal, int rewardHeart,int[] clearCriterion,Sprite challengeImage)
        {
            this.Title = Title;
            this.rewardMedal = rewardMedal;
            this.rewardHeart = rewardHeart;
            this.clearCriterion = clearCriterion;
            this.challengeImage = challengeImage;

        }
    }
    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    ChallengeStruct[] Info;

    [Header("==========OBJECT==========")]
    public GameObject levelText;
    [SerializeField]
    private GameObject titleText;
    [SerializeField]
    private GameObject achieveCondition; //�������� �޼� ���� �ؽ�Ʈ
    [SerializeField]
    private GameObject nowCondition; //���� �������� �޼� ��ġ �ؽ�Ʈ
    [SerializeField]
    private GameObject Button;
    [SerializeField]
    private GameObject image;//������ ���� �׸�
    public GameObject medalTxt;
    public GameObject heartTxt;
    public GameObject FinBtn;
    public GameObject FinBG;
    public GameObject heart;
    public GameObject medal;


    [Header("==========Gauge==========")]
    [SerializeField]
    private RectTransform Gauge;

    [Header("==========SPRITE==========")]
    [SerializeField]
    private Sprite IngButton;
    [SerializeField]
    private Sprite DoneButton;


    [Header("==========Animation==========")]
    public GameObject heartMover;
    public GameObject medalMover;


    //==========prefab num===========
    static int Prefabcount = 0; //�߰� �� Prefab ��
    private int prefabnum; //�ڽ��� ���° Prefab����


    //==========��������==========
    private int LevelNow;//���� �� ������ �ִ�.
    
    private int[] ChallengeValue = new int[6];//������ ��

    private int ValueNow;

    private int MaxLevel = 6;


    //=======================================================================================================================
    //=======================================================================================================================
    
    private void Awake()
    {
        GameManager.instance.ShowMedalText();//���� �޴��� ���δ�.

        //�����յ鿡�� ���� ��ȣ ���̱�
        if (Prefabcount >= 6) { Prefabcount %= 6; }
        prefabnum = Prefabcount;
        Prefabcount++;
 
        InfoInit();
    }

    private void Update()
    {
        
        ChallengeValue[0] = DataController.instance.gameData.unlockBerryCnt;
        ChallengeValue[1] = DataController.instance.gameData.totalHarvBerryCnt;
        ChallengeValue[2] = DataController.instance.gameData.accCoin;
        ChallengeValue[3] = DataController.instance.gameData.accHeart;
        ChallengeValue[4] = DataController.instance.gameData.accAttendance;
        ChallengeValue[5] = DataController.instance.gameData.mgPlayCnt;

        LevelNow= DataController.instance.gameData.challengeLevel[prefabnum];
        ValueNow = ChallengeValue[prefabnum];

        if (LevelNow == MaxLevel)
        {  FinishChallenge(); }


        InfoUpdate();

    }

    //==================================================================================================================
    //==================================================================================================================
    
    private void InfoInit() 
    {
        //����ǥ��
        titleText.GetComponent<Text>().text = Info[prefabnum].Title;

        //���� ����
        Info[prefabnum].rewardMedal = 1; // 1 ����
        Info[prefabnum].rewardHeart = (DataController.instance.gameData.challengeLevel[prefabnum]+1) * 5;//����X5 ��Ʈ

        //�׸� ����
        image.GetComponent<Image>().sprite = Info[prefabnum].challengeImage;


        //���� ���� ����
        Info[prefabnum].clearCriterion = new int[MaxLevel];

        switch (prefabnum)
        {
            case 0: // ���� ����
                Info[prefabnum].clearCriterion[0] = 10;
                for (int i = 1; i < MaxLevel; i++)
                {
                    Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[0] + 10 * i;
                }
                break;

            case 4: // ���� �⼮
                Info[prefabnum].clearCriterion[0] = 2;
                for (int i = 2; i < MaxLevel; i++)
                {
                    // ����
                    Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i - 1] + 2;
                }
                break;

            case 1: // ���� ��Ȯ
                Info[prefabnum].clearCriterion[0] = 100;
                for (int i = 1; i < MaxLevel; i++)
                {
                    // �ø�
                    Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i - 1] * 3;
                }
                break;

            case 2: // ���� ����
                Info[prefabnum].clearCriterion[0] = 1000;
                for (int i = 1; i < MaxLevel; i++)
                {
                    // �ø�
                    Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i - 1] * 5;
                }
                break;

            case 3: // ���� ��Ʈ
                Info[prefabnum].clearCriterion[0] = 100;
                for (int i = 1; i < MaxLevel; i++)
                {
                    // ����
                    Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i - 1] + 100;
                }
                break;

            case 5: //�̴ϰ��� �÷���
                Info[prefabnum].clearCriterion[0] = 5;
                for (int i = 1; i < MaxLevel; i++)
                {
                    // ����
                    Info[prefabnum].clearCriterion[i] = Info[prefabnum].clearCriterion[i - 1] + 10;
                }
                break;

        }
    }






    public void InfoUpdate() {

        //text ����===========
        if (LevelNow != MaxLevel)
        {
            levelText.GetComponent<Text>().text = "Lv." + LevelNow.ToString();  //����
            achieveCondition.GetComponent<Text>().text = "/" + Info[prefabnum].clearCriterion[LevelNow].ToString();//�������� ������ �޼� ���� ����

            //�������� ���� ���δ�.
            medalTxt.GetComponent<Text>().text = "X" + Info[prefabnum].rewardMedal.ToString();
            heartTxt.GetComponent<Text>().text = "X" + Info[prefabnum].rewardHeart.ToString();



            nowCondition.GetComponent<Text>().text = ValueNow.ToString();


            //������===============���� ���� ���� ���� ��ǥ�� �̻����� ���ִ�.
            if (ValueNow >= Info[prefabnum].clearCriterion[LevelNow])
            {
                //�������� ������ == ���� �� ���·�
                Gauge.GetComponent<Image>().fillAmount = 1;
                //�������� ��ư �̹��� == Done
                Button.GetComponent<Image>().sprite = DoneButton;
                //�������� ���� ���� ���� ��ǥ���� ����
                nowCondition.GetComponent<Text>().text = Info[prefabnum].clearCriterion[LevelNow].ToString();

            }
            else
            {
                //�������� ������ == ValueNow ��ŭ ����
                Gauge.GetComponent<Image>().fillAmount = float.Parse(nowCondition.GetComponent<Text>().text) / Info[prefabnum].clearCriterion[LevelNow];
                
            }
        }
    }
    //==================================================================================================================
    //==================================================================================================================


    //�������� �޼��� �Ϸ� ��ư ������
    public void ChallengeSuccess() 
    {

        //�������� �޼��ߴ��� Ȯ��
        if (ValueNow >= Info[prefabnum].clearCriterion[LevelNow])
        {
            //ȿ����, ȿ�� �ִϸ��̼�
            AudioManager.instance.RewardAudioPlay();
            heartMover.GetComponent<HeartMover>().RewardMover("heart");
            medalMover.GetComponent<HeartMover>().RewardMover("medal");

            //����ȹ��
        

            GameManager.instance.GetMedal(Info[prefabnum].rewardMedal); //�޴� ���� ȹ��
            GameManager.instance.GetHeart(Info[prefabnum].rewardHeart); //��Ʈ ���� ȹ��

            //���� ������ �̵�
            if (LevelNow < MaxLevel)
            {
                /*
                switch (prefabnum) 
                {
                    case 0:
                        DataController.instance.gameData.unlockBerryCnt-= Info[prefabnum].clearCriterion[LevelNow];
                        break;
                    case 1:
                        DataController.instance.gameData.totalHarvBerryCnt -= Info[prefabnum].clearCriterion[LevelNow];
                        break;
                    case 2:
                        DataController.instance.gameData.accCoin -= Info[prefabnum].clearCriterion[LevelNow];
                        break;
                    case 3:
                        DataController.instance.gameData.accHeart -= Info[prefabnum].clearCriterion[LevelNow];
                        break;
                    case 4:
                        DataController.instance.gameData.accAttendance -= Info[prefabnum].clearCriterion[LevelNow];
                        break;
                    case 5:
                        DataController.instance.gameData.mgPlayCnt -= Info[prefabnum].clearCriterion[LevelNow];
                        break;
                }
                */
                Button.GetComponent<Image>().sprite = IngButton; //�������� ��ư �̹��� ����

                DataController.instance.gameData.challengeLevel[prefabnum]++; //Level����
                Info[prefabnum].rewardHeart = (DataController.instance.gameData.challengeLevel[prefabnum] + 1) * 5;//��Ʈ ����

            }
            else
            {
                FinishChallenge();

            }
        }
    }


    public void FinishChallenge()
    {
        levelText.GetComponent<Text>().text = "Lv.Max";
        Button.SetActive(false);
        heart.SetActive(false);
        medal.SetActive(false);
        FinBtn.SetActive(true);
        FinBG.SetActive(true);

        // ������ ����
        Gauge.GetComponent<Image>().fillAmount = 1;
        // ���� Max
        nowCondition.GetComponent<Text>().text = Info[prefabnum].clearCriterion[MaxLevel-1].ToString();
        achieveCondition.GetComponent<Text>().text = "/" + Info[prefabnum].clearCriterion[MaxLevel-1].ToString();//�������� ������ �޼� ���� ����

        DataController.instance.gameData.isChallengeMax[prefabnum] = true;
    
    }
}
