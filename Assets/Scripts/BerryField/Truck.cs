using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class Truck : MonoBehaviour
{
    public GameObject MaxPanel;
    public Button normal_receive_btn;
    public Button add_receive_btn;
    //public int berryCnt = 0; // �ű�
    private Animator anim;
    private ArbeitMgr arbeit;

    public const int CNT_LEVEL_0 = Globalvariable.TRUCK_CNT_LEVEL_0;
    public const int CNT_LEVEL_1 = Globalvariable.TRUCK_CNT_LEVEL_1;
    public const int CNT_LEVEL_2 = Globalvariable.TRUCK_CNT_LEVEL_2;
    public const int CNT_LEVEL_MAX = Globalvariable.TRUCK_CNT_LEVEL_MAX;

    void Awake()
    {
        anim = GetComponent<Animator>();
        arbeit = GameObject.FindGameObjectWithTag("Arbeit").GetComponent<ArbeitMgr>();
    }
    void FixedUpdate()
    {       
        if(DataController.instance.gameData.truckBerryCnt == CNT_LEVEL_0) // Ʈ�� ���� ���� ������ 0�����
        {
            normal_receive_btn.interactable = false; // �ޱ� ��ư�� ��Ȱ��ȭ
            add_receive_btn.interactable = false; // ���� ���� �ޱ� ��ư�� ��Ȱ��ȭ
        }
        else // Ʈ�� ���� ���� ������ 1�� �̻��̶��
        {
            if(!normal_receive_btn.interactable && !normal_receive_btn.interactable) // ��ư�� ��Ȱ��ȭ ���ִٸ�
            {
                normal_receive_btn.interactable = true; // �ޱ� ��ư�� Ȱ��ȭ
                add_receive_btn.interactable = true; // ���� ���� �ޱ� ��ư�� Ȱ��ȭ
            }           
        }
        if(CNT_LEVEL_0 <= DataController.instance.gameData.truckBerryCnt && DataController.instance.gameData.truckBerryCnt < CNT_LEVEL_1)
        {
            if (anim.GetInteger("Truck") == 0) return;          
            SetAnim(0); // Ʈ�� �ִϸ��̼��� �� Ʈ������ ����
            MaxPanel.SetActive(false); // MAX �г� ����
        }
        if (CNT_LEVEL_1 <= DataController.instance.gameData.truckBerryCnt && DataController.instance.gameData.truckBerryCnt < CNT_LEVEL_2)
        {
            if (anim.GetInteger("Truck") == 1) return;

            SetAnim(1);
        }
        if (CNT_LEVEL_2 <= DataController.instance.gameData.truckBerryCnt && DataController.instance.gameData.truckBerryCnt < CNT_LEVEL_MAX)
        {
            if (anim.GetInteger("Truck") == 2) return;
            SetAnim(2);
        }
        if (DataController.instance.gameData.truckBerryCnt == CNT_LEVEL_MAX)
        {
            if (anim.GetInteger("Truck") == 3) return;
            MaxPanel.SetActive(true);
            SetAnim(3);
        }      
    }    
    void SetAnim(int level)
    {
        anim.SetInteger("Truck", level);
    }
    public void ReceiveCoinNormal()
    {
        //DataController.instance.gameData.coin += DataController.instance.gameData.truckCoin;
        float coEffi = arbeit.Pigma();
        float totalCoin = (DataController.instance.gameData.truckCoin
            + GameManager.instance.bonusTruckCoin) * coEffi;

        Debug.Log(totalCoin);
        GameManager.instance.GetCoin((int)totalCoin);

        //Debug.Log("���� �⼮ : " + DataController.instance.gameData.accCoin);      // ���� ���� �׽�Ʈ
        DataController.instance.gameData.truckBerryCnt = 0;
        DataController.instance.gameData.truckCoin = 0;
    }
}
