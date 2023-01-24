using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbeitMgr : MonoBehaviour
{   
    public bool OnRachel;
    public bool OnThomson;
    public bool OnHamsworth;
    public bool OnFubo;

    private float delay;
    void FixedUpdate()
    {
        // ���⿡ �̴ϰ��� ������ Ȯ���ϴ� ���� �߰� �Ҽ���?
        delay += Time.deltaTime;
        if(delay >= 0.5f)
        {
            delay = 0f;
            Rachel();
            Thomson();
            Fubo();
            Hamsworth();
        }
    }
    void Rachel()//�䳢 0
    {
        for (int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if (!DataController.instance.gameData.berryFieldData[i].isPlant &&
                !DataController.instance.gameData.berryFieldData[i].hasWeed &&
                !DataController.instance.gameData.berryFieldData[i].hasBug &&
                DataController.instance.gameData.PTJNum[0] > 0)
            {
                GameManager.instance.PlantStrawBerry(GameManager.instance.stemList[i], GameManager.instance.farmObjList[i]); // �ɴ´�                            
                DataController.instance.gameData.berryFieldData[i].isPlant = true; // üũ ���� ����
                DataController.instance.gameData.PTJNum[0]--;
                DataController.instance.gameData.PTJIsWorking[0] = true;//������
                //Debug.Log("����ÿ ���� Ƚ��: " + DataController.instance.gameData.PTJNum[0]);
                break;
            }
        }
    }
    void Thomson()//�� 1
    {
        for (int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if (DataController.instance.gameData.berryFieldData[i].createTime >= DataController.instance.gameData.stemLevel[4] &&
                !GameManager.instance.farmList[i].isHarvest &&
                !DataController.instance.gameData.berryFieldData[i].hasWeed &&
                !DataController.instance.gameData.berryFieldData[i].hasBug &&
                DataController.instance.gameData.PTJNum[1] > 0 &&
                DataController.instance.gameData.truckBerryCnt != Globalvariable.instance.truckCntLevel[3, DataController.instance.gameData.newBerryResearchAble])
            {
                DataController.instance.gameData.PTJNum[1]--;
                StartCoroutine(HarvestbyThomson(i));
                //Debug.Log("�轼 ���� Ƚ��: " + DataController.instance.gameData.PTJNum[1]);
                break;
            }
        }
    }
    IEnumerator HarvestbyThomson(int idx)
    {
        yield return new WaitForSeconds(0.25f);
        DataController.instance.gameData.PTJIsWorking[1] = true;//������
        GameManager.instance.Harvest(GameManager.instance.stemList[idx]); // ��Ȯ�Ѵ�    
    }  
    void Hamsworth()//�ܽ��� 2
    {
        for (int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if (DataController.instance.gameData.berryFieldData[i].hasBug &&
                DataController.instance.gameData.PTJNum[2] > 0)
            {
                DataController.instance.gameData.berryFieldData[i].hasBug = false;
                DataController.instance.gameData.PTJNum[2]--;
                //Debug.Log("Ǫ�� ���� Ƚ��: " + DataController.instance.gameData.PTJNum[4]);
                StartCoroutine(DeleteBugByHamsworth(GameManager.instance.bugList[i]));
                break;
            }
        }
    }
    IEnumerator DeleteBugByHamsworth(Bug bug)
    {
        yield return new WaitForSeconds(0.75f);
        DataController.instance.gameData.PTJIsWorking[2] = true;//������
        bug.DieBug();
    }

    void Fubo()//������ 3
    {
        for (int i = 0; i < GameManager.instance.farmList.Count; i++)
        {
            if (DataController.instance.gameData.berryFieldData[i].hasWeed &&
                DataController.instance.gameData.PTJNum[3] > 0)
            {
                DataController.instance.gameData.berryFieldData[i].hasWeed = false;
                DataController.instance.gameData.PTJNum[3]--;
                //Debug.Log("�ܽ����� ���� Ƚ��: " + DataController.instance.gameData.PTJNum[3]);
                StartCoroutine(DeleteWeedByFubo(GameManager.instance.farmObjList[i]));
                break;
            }
        }
    }
    IEnumerator DeleteWeedByFubo(GameObject obj)
    {
        yield return new WaitForSeconds(0.75f);
        DataController.instance.gameData.PTJIsWorking[3] = true;//������
        Farm farm = obj.GetComponent<Farm>();
        farm.weed.DeleteWeed();
        //obj.GetComponent<BoxCollider2D>().enabled = false;
    }
    public float Pigma() // ���� 5
    {
        if (DataController.instance.gameData.PTJNum[5] > 0)
        {
            float coEffi = Random.Range(1.0f, 1.4f);
            DataController.instance.gameData.PTJNum[5]--;
            DataController.instance.gameData.PTJIsWorking[5] = true;//������

            Debug.Log(coEffi);
            return coEffi;
        }
        else return 1.0f;
    }
    public int lluvia() // ����� 4
    {
        if (DataController.instance.gameData.PTJNum[4] > 0)
        {
            DataController.instance.gameData.PTJNum[4]--;
            DataController.instance.gameData.PTJIsWorking[4] = true;//������

            return 3;
        }
        else
        {           
            return 2;
        }
    }
}
