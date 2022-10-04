using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PTJWorking : MonoBehaviour
{
    //========�˹ٻ� ����==========
    public Sprite[] FacePicture;
    public Sprite[] WorkingFacePicture;
    public GameObject face;
    public GameObject employCount;


    //==========Prefab���� ���� �ο�==========
    static int Prefabcount = 0;
    int prefabnum;
    //==========��� Ƚ��==========
    private int PTJ_NUM_NOW;

    private bool isOnetime;//ù��°���� scrollView�� ������ �̵��ϰ� �Ѵ�.

    private void Awake()
    {
        //�����յ鿡�� ���� ��ȣ ���̱�
        if (Prefabcount >= 6) { Prefabcount %= 6; }
        prefabnum = Prefabcount;
        Prefabcount++;

    }
    void Start()
    {
        //�� �̹��� ����
        face.GetComponent<Image>().sprite = FacePicture[prefabnum];
        
        isOnetime = true;
    }


    void Update()
    {
        //�ڽ��� ���Ƚ���� ���� �ľ�
        PTJNumNow = DataController.instance.gameData.PTJNum[prefabnum];
    }

    int PTJNumNow
    {
        set
        {
            if (PTJ_NUM_NOW == value) return;
            PTJ_NUM_NOW = value;


            //�˹� ����
            if (PTJ_NUM_NOW == 0)
            {
                face.SetActive(false);
                employCount.SetActive(false);
                DataController.instance.gameData.PTJCount--;
                isOnetime = true;
                gameObject.transform.SetAsLastSibling();
            }
            //�˹� �ϴ���
            else
            {
                face.SetActive(true);
                employCount.SetActive(true);

                if (isOnetime==true) 
                { gameObject.transform.SetAsFirstSibling(); isOnetime = false; }

                employCount.GetComponent<Text>().text = DataController.instance.gameData.PTJNum[prefabnum].ToString();
            }

        }
        get { return PTJ_NUM_NOW; }
    }

}
