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
                DataController.instance.gameData.PTJNum[1] > 0)
            {

                StartCoroutine(HarvestbyThomson(i));
                //Debug.Log("�轼 ���� Ƚ��: " + DataController.instance.gameData.PTJNum[1]);
                break;
            }
        }
    }
    IEnumerator HarvestbyThomson(int idx)
    {
        yield return new WaitForSeconds(0.25f);

        GameManager.instance.Harvest(GameManager.instance.stemList[idx]); // ��Ȯ�Ѵ�
        DataController.instance.gameData.PTJNum[1]--;
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
                StartCoroutine(DeleteWeedByFubo(GameManager.instance.farmList[i]));
                break;
            }
        }
    }
    IEnumerator DeleteWeedByFubo(Farm farm)
    {
        yield return new WaitForSeconds(0.75f);
        farm.weed.DeleteWeed();
    }
    public float Pigma() // ���� 4
    {
        if (DataController.instance.gameData.PTJNum[4] > 0)
        {
            float coEffi = Random.Range(0.7f, 1.8f);
            DataController.instance.gameData.PTJNum[4]--;

            Debug.Log(coEffi);
            return coEffi;
        }
        else return 1.0f;
    }
    public int lluvia() // ������ 5
    {
        if (DataController.instance.gameData.PTJNum[5] > 0)
        {
            DataController.instance.gameData.PTJNum[5]--;           
            return 4;
        }
        else
        {           
            return 2;
        }
    }
}
