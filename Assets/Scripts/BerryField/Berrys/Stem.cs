using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Stem : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sprite;

    int animlevel;
    [SerializeField]
    private Vector3 farmPos; 
    private Bug stemBug;

    public int stemIdx;
    private GameObject instantBerryObj;
  
    RainCtrl rainCtrl;
    Globalvariable global;

    private int[] rank = { 6, 9, 10 };
    void Awake()
    {       
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        stemBug = GameManager.instance.bugList[stemIdx];

        rainCtrl = GameObject.FindGameObjectWithTag("Rain").GetComponent<RainCtrl>();
        global = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>();
    }
    private void OnEnable()
    {       

        DataController.instance.gameData.berryFieldData[stemIdx].isStemEnable = true;
        
        if (DataController.instance.gameData.berryFieldData[stemIdx].createTime >= DataController.instance.gameData.stemLevel[1])
        {
            sprite.sortingOrder = 0;
        }
        else sprite.sortingOrder = 2;

        if (DataController.instance.gameData.berryFieldData[stemIdx].createTime > 0f)
        {
            MakeBerry();
            updateStem();
            return;
        }  
        
        DataController.instance.gameData.berryFieldData[stemIdx].randomTime = 
            Random.Range(DataController.instance.gameData.stemLevel[1] + 2.0f, DataController.instance.gameData.stemLevel[4] - 2.0f);
        DefineBerry();
        MakeBerry();
        SetAnim(0);
    }
    private void OnDisable()
    {
        DataController.instance.gameData.berryFieldData[stemIdx].isStemEnable = false;
        // ���� �ʱ�ȭ       
        //if (DataController.instance.gameData.berryFieldData[stemIdx].isPlant) return;

        DataController.instance.gameData.berryFieldData[stemIdx].isPlant = false;
        DataController.instance.gameData.berryFieldData[stemIdx].canGrow = true;
        DataController.instance.gameData.berryFieldData[stemIdx].createTime = 0f;
        DataController.instance.gameData.berryFieldData[stemIdx].randomTime = 0f;

        // ���� Ʈ������ �ʱ�ȭ
        transform.localPosition = new Vector3(0, 0.4f, 0);
        transform.localRotation = Quaternion.identity;

        sprite.sortingOrder = 2;
        Destroy(instantBerryObj);
    }
    void Update() // �ð��� ���� ���� ���� // ���⿡ �̴ϰ��� ������ Ȯ���ϴ� ���� �߰� �Ҽ���?
    {       
        if (GameManager.instance.isGameRunning && DataController.instance.gameData.berryFieldData[stemIdx].canGrow) 
        {
            DataController.instance.gameData.berryFieldData[stemIdx].createTime += Time.deltaTime * rainCtrl.mult;
            updateStem();
        }
    }
    private void updateStem()
    {      
        if (DataController.instance.gameData.berryFieldData[stemIdx].randomTime <= DataController.instance.gameData.berryFieldData[stemIdx].createTime)
        {
            stemBug.GenerateBug();
            DataController.instance.gameData.berryFieldData[stemIdx].randomTime = 200f;
        }
        if (DataController.instance.gameData.stemLevel[1] <= DataController.instance.gameData.berryFieldData[stemIdx].createTime
            && DataController.instance.gameData.berryFieldData[stemIdx].createTime < DataController.instance.gameData.stemLevel[2])
        {
            if (animlevel == 1) return;
            transform.localPosition = new Vector3(0, 0.72f, 0);
            SetAnim(1);
        }
        else if (DataController.instance.gameData.stemLevel[2] <= DataController.instance.gameData.berryFieldData[stemIdx].createTime 
            && DataController.instance.gameData.berryFieldData[stemIdx].createTime < DataController.instance.gameData.stemLevel[3])
        {
            if (animlevel == 2) return;
            transform.localPosition = new Vector3(0, 0.72f, 0);
            SetAnim(2);
        }
        else if (DataController.instance.gameData.stemLevel[3] <= DataController.instance.gameData.berryFieldData[stemIdx].createTime
            && DataController.instance.gameData.berryFieldData[stemIdx].createTime < DataController.instance.gameData.stemLevel[4])
        {
            if (animlevel == 3) return;
            transform.localPosition = new Vector3(0, 0.72f, 0);
            SetAnim(3);
        }
        else if (DataController.instance.gameData.berryFieldData[stemIdx].createTime >= DataController.instance.gameData.stemLevel[4])
        {            
            DataController.instance.gameData.berryFieldData[stemIdx].canGrow = false;
            if(!GameManager.instance.isBlackPanelOn)
                GameManager.instance.farmList[stemIdx].GetComponent<BoxCollider2D>().enabled = true; // ���� �ݶ��̴� �ٽ� Ȱ��ȭ
            transform.localPosition = new Vector3(0, 0.72f, 0);
            SetAnim(4);
        }
    }
    public void SetAnim(int level)
    {
        animlevel = level;
        anim.SetInteger("Seed", animlevel);

        if (animlevel == 0)
        {
            //transform.position = new Vector2(farmPos.x, farmPos.y + 0.02f);
        }
        else if (animlevel == 1)
        {
            sprite.sortingOrder = 0;       
        }
        else if (animlevel == 2)
        {                      
            //instantBerry.gameObject.SetActive(true);
            //instantBerry.GetComponent<Animator>().SetInteger("berryLevel", level);
            //instantBerry.transform.localPosition = new Vector3(transform.localPosition.x + 0.3f, transform.localPosition.y - 0.5f, transform.localPosition.z);
        }
        else if (animlevel == 3)
        {                      
            //instantBerry.gameObject.SetActive(true);
            //instantBerry.GetComponent<Animator>().SetInteger("berryLevel", level);
            //instantBerry.transform.localPosition = new Vector3(transform.localPosition.x + 0.3f, transform.localPosition.y - 0.48f, transform.localPosition.z);
        }
        else if (animlevel == 4)
        {
            instantBerryObj.gameObject.SetActive(true);
            //instantBerryObj.GetComponent<SpriteRenderer>().sprite = global.berryListAll[DataController.instance.gameData.berryFieldData[stemIdx].berryPrefabNowIdx].GetComponent<SpriteRenderer>().sprite;
            instantBerryObj.GetComponent<Animator>().SetInteger("berryLevel", level);
            instantBerryObj.transform.localPosition = new Vector2(0.32f, -0.27f);
        }
    }
    void DefineBerry() // ���� Ȯ�������� ������ ���� ����
    {
        int cumulativeKind = 0, probSum = 0;
        int begin = 0, end = 196;
        int berryRandomChance;
        int rankRandom = Random.Range(0, rank[DataController.instance.gameData.newBerryResearchAble]);
        if(rankRandom < 6)
        {
            begin = 0; end = 64;      
        }
        else if(rankRandom < 9)
        {
            begin = 64; end = 128;
        }
        else
        {
            begin = 128; end = 192;
        }
        for (int i = begin; i < end; i++)
        {
            if(DataController.instance.gameData.isBerryUnlock[i])
            {
                probSum += global.berryListAll[i].GetComponent<Berry>().berrykindProb; // �رݵ� ���⿡�� ������ �߻�Ȯ���� ������
            }           
        }
        berryRandomChance = Random.Range(0, probSum + 1);

        for (int i = begin; i < end; i++)
        {
            if (DataController.instance.gameData.isBerryUnlock[i])
            {
                cumulativeKind += global.berryListAll[i].GetComponent<Berry>().berrykindProb; // �رݵ� ���⿡�� ������ �߻�Ȯ���� ������
                if (berryRandomChance <= cumulativeKind)
                {
                    DataController.instance.gameData.berryFieldData[stemIdx].berryPrefabNowIdx = i;
                    break;
                }
            }                       
        }
    }
    void MakeBerry() // ���� ����
    {
        // �ε�ɶ��� ���������ϱ� �ε� Ŭ�������� �ٽ� �����ؾߵ�(MakeBerry() ȣ���ϸ� ��)

        instantBerryObj = Instantiate(global.berryListAll[DataController.instance.gameData.berryFieldData[stemIdx].berryPrefabNowIdx], this.transform);

        instantBerryObj.name = global.berryListAll[DataController.instance.gameData.berryFieldData[stemIdx].berryPrefabNowIdx].name;     
        instantBerryObj.gameObject.SetActive(false);
    }

    public GameObject getInstantBerryObj()
    {
        return this.instantBerryObj;
    }
}
