using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    // �׽�Ʈ������ ��ư������ ���� �� ���ߵǴ°Ÿ� ���� ��ũ��Ʈ �Դϴ�.
    private GameObject global;
    void Start()
    {
        global = GameObject.FindGameObjectWithTag("Global");
    }

    public void Get(int berryClassify) 
    {
        int start=0, end=0;

        switch (berryClassify) 
        {
            case 0: start = 0;end = 64;break;
            case 1: start = 64;end = 128;break;
            case 2: start = 128; end = 192;break;
        }

        for (int i = start; i < end; i++)
        {
            if (global.GetComponent<Globalvariable>().berryListAll[i] != null) 
            {
                if (DataController.instance.gameData.isBerryUnlock[i] == false)
                {
                    DataController.instance.gameData.isBerryUnlock[i] = true;
                    DataController.instance.gameData.unlockBerryCnt++;
                }
            }
        }
    
    }
}
