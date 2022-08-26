using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame : MonoBehaviour
{
    static public bool isGameRunning = false;           //������ ���� ������
    static public bool isMiniGameWork = false;           //������ ���� ������

    [Header("UI")]
    public Scrollbar scrollbar;//��ũ�ѹ�
    public Image scroll;
    public Text score_txt;     //����
    public GameObject countImgs;//ī��Ʈ �̹���
    public Button pause_btn;   //�Ͻ����� ��ư
    public GameObject resultPanel;//����г�
    public Text result_txt;    //��� �ؽ�Ʈ

    public float size;                         //��ũ�ѹ� ������
    public int time;                           //��
    public int score;                          //����
    protected List<int> unlockList=new List<int>(); //�رݵ� ���� ��ȣ ����Ʈ

    protected Globalvariable global;
    protected int research_level_sum;
    protected float research_level_avg;


    protected virtual void Awake()
    {
        //size = scrollbar.size / 60f; 
        size = scroll.fillAmount / 60f;
        global = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>();
    }

    protected virtual void OnEnable()
    {
        StartGame();
        isMiniGameWork = true;
    }

    void StartGame()
    {   
        // ����Ʈ �ʱ�ȭ
        unlockList.Clear();

        //������ ���� ����Ʈ ����
        for (int i = 0; i < 192; i++)
        {
            if (DataController.instance.gameData.isBerryUnlock[i] == true)
            {
                unlockList.Add(i);
            }
        }
        Debug.Log("unlockList.Count: " + unlockList.Count);
        //scrollbar.size = 1;
        scroll.fillAmount = 1;
        score = 0;
        time = 64;
        score_txt.text = score.ToString() + "��";
        isGameRunning = false;
        //4�� ī��Ʈ
        StartCoroutine(Count4Second());
    }

    IEnumerator Count4Second()
    {
        countImgs.transform.GetChild(time - 61).gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        countImgs.transform.GetChild(time - 61).gameObject.SetActive(false);
        time -= 1;
        if (time <= 60)
        {
            StartCoroutine(Timer());          
            MakeGame(); //�� ���ӿ��� �������̵� �ϱ�
            isGameRunning = true; // Update �Լ� ������ �׳� ���� ���� ���� ���°� ����
        }
        else
        {
            StartCoroutine(Count4Second());
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        //scrollbar.size -= size;
        scroll.fillAmount -= size;
        time -= 1;
        if (time <= 0)
        {           
            FinishGame();
        }
        else
        {
            StartCoroutine(Timer());
        }
    }

    IEnumerator Timer2()
    {    
        while (time >= 0)
        {
            scroll.fillAmount -= size;
            time -= 1;
            yield return new WaitForSeconds(1);
        }
        FinishGame();
    }

    protected virtual void MakeGame()
    {
        
    }

    protected virtual void FinishGame()
    {
        research_level_sum = 0;

        for (int i = 0; i < DataController.instance.gameData.researchLevel.Length; i++)
        {
            // ���������� ���� ���Ѵ�.
            research_level_sum += DataController.instance.gameData.researchLevel[i];
        }

        // ���������� ����� ���Ѵ�.
        research_level_avg = research_level_sum / DataController.instance.gameData.researchLevel.Length;

        //�̴ϰ��� �÷��� Ƚ�� ����
        DataController.instance.gameData.mgPlayCnt++;       
    }

    public virtual void StopGame()
    {
        score = 0;
        time = 64;
        unlockList.Clear();

        isGameRunning = false;
        isMiniGameWork = false;
    }

    public virtual void ReStart() //�ٽ��ϱ�
    {
        score = 0;
        time = 64;
        unlockList.Clear();
        //StartGame();
        OnEnable();
    }

    public virtual void OnClickPauseButton() //�Ͻ�����
    {
        Time.timeScale = 0;
        isGameRunning = false;
    }

    public virtual void OnClickKeepGoingButton() //�Ͻ����� ����
    {
        Time.timeScale = 1;
        isGameRunning = true;
    }

    public void OnclickExitButton() //���� ������
    {
        OnClickKeepGoingButton();
        StopGame();
    }
}
