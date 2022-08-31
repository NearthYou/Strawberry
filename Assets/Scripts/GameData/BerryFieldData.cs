using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryFieldData
{
    //Bug
    
    public float scale; // �ű�
    public bool isBugEnable = false; // true���� �־����Ŵϱ� ���ְ� false���� �����Ŵϱ� �ǵ�������

    //Weed   
    public float xPos = 0f;   
    public int weedSpriteNum; 
    public bool isWeedEnable = false; // true���� �־����Ŵϱ� ���ְ� false���� �����Ŵϱ� �ǵ�������

    //Farm
    public bool isPlant = false;       
    public bool hasWeed = false;
    public bool canGrowWeed = true;
    public float weedTime = 0f;   

    //Stem
    public float createTime = 0f;
    public bool canGrow = true;
    public bool hasBug = false;
    
    public int berryPrefabNowIdx;
    public bool isStemEnable = false; // // true���� �־����Ŵϱ� ���ְ� false���� �����Ŵϱ� �ǵ�������
    public float randomTime = 0f;
}
