using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Strawberry : MonoBehaviour
{
    //프리팹들 번호 붙여주기
    static int Prefabcount = 0;
    int prefabnum;

    //베리 정보를 담아올 리스트
    List<GameObject> BERRY;

    [Header("======Strawberry=====")]
    [SerializeField]
    private Sprite yesBerryImage;//베리 있을 시 배경 이미지
    [SerializeField]
    private GameObject berryImagePanel;//이미지를 보일 오브젝트 대상
    [SerializeField]
    private GameObject ExclamationMark;//new!표시
    [SerializeField]
    private Sprite[] berryClassifyImage;//베리 분류 이미지

    //베리 설명창
    private GameObject berryExp;
    private GameObject berryExpPanelBlack;
    //=====================================================================================================
    //=====================================================================================================

    private void Awake()
    {
        //프리팹들에게 번호를 붙여 주자
        if (Prefabcount >= 32*3) { Prefabcount %= 32 * 3; }
        prefabnum = Prefabcount;
        if (prefabnum >= 32 && prefabnum <= 63) { prefabnum += 32; }
        else if (prefabnum >= 64 && prefabnum <= 95) { prefabnum += 64; }
        Prefabcount++;
        

    }
    void Start()
    {

        berryExp = GameObject.FindGameObjectWithTag("BerryExplanation");
        berryExpPanelBlack = berryExp.transform.parent.GetChild(7).gameObject; // 시원건드림
        berryExp = berryExp.transform.GetChild(0).gameObject;
        

        //베리 정보 가져오기
        BERRY = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>().berryListAll;

        //베리들을 보인다.
        berryImageChange();

    }
    private void Update()
    {
        //베리들을 보인다. 이거 없앨수있을듯
        berryImageChange();
    }

    //=====================================================================================================
    //=====================================================================================================
    //베리 리스트에 이미지를 보인다
    public void berryImageChange()
    { 
        //베리 정보가 존재하고 && 베리가 unlock 되었다면 
        if (BERRY[prefabnum] != null && DataController.instance.gameData.isBerryUnlock[prefabnum]==true)
        {
            this.transform.GetChild(0).GetComponent<Image>().sprite = yesBerryImage;//배경 이미지 변경
            berryImagePanel.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);//투명 -> 불투명

            berryImagePanel.GetComponent<Image>().sprite 
                = BERRY[prefabnum].GetComponent<SpriteRenderer>().sprite;//해당 베리 이미지 보이기
            berryImagePanel.GetComponent<Image>().preserveAspect = true;

        }

        //베리 아직 한번도 확인하지 않았다면 느낌표 표시. 이미 한 번 봤으면 없애기
        if (DataController.instance.gameData.isBerryEM[prefabnum] == true)
        { ExclamationMark.SetActive(true); }
        else 
        { 
            ExclamationMark.SetActive(false);

            // 베리들 검사해서 새 베리 얻었다는 표시 없앨지 정하기-> 흠이거 너무 ㅁㄶ이 검사하는것같다.
            for (int i = 0; i < DataController.instance.gameData.isBerryEM.Length; i++)
            {
                if (DataController.instance.gameData.isBerryEM[i] == true)
                { return;}
            
            }

        }

    }


    //베리 설명창 띄우기
    public void BerryExplanation() {
        
        try
        {
            
            if (DataController.instance.gameData.isBerryEM[prefabnum] == true) 
            { DataController.instance.gameData.isBerryEM[prefabnum] = false; }

            if (DataController.instance.gameData.isBerryUnlock[prefabnum] == true)
            {
                AudioManager.instance.Cute1AudioPlay();

                //설명창 띄운다
                //berryExp.SetActive(true);
                berryExp.GetComponent<PanelAnimation>().OpenScale();
                berryExpPanelBlack.GetComponent<PanelAnimation>().Fadein();

                GameObject berryExpImage = berryExp.transform.GetChild(2).gameObject;
                GameObject berryExpName = berryExp.transform.GetChild(3).gameObject;
                GameObject berryExpTxt = berryExp.transform.GetChild(4).gameObject;
                GameObject berryExpPrice= berryExp.transform.GetChild(5).gameObject;
                GameObject berryClassify = berryExp.transform.GetChild(6).gameObject;
                

                //Explanation 내용을 채운다.
                berryExpImage.GetComponentInChildren<Image>().sprite
                    = BERRY[prefabnum].GetComponent<SpriteRenderer>().sprite;//이미지 설정
                berryExpImage.GetComponentInChildren<Image>().preserveAspect = true;

                berryExpName.gameObject.GetComponentInChildren<Text>().text
                    = BERRY[prefabnum].GetComponent<Berry>().berryName;//이름 설정

                berryExpTxt.transform.gameObject.GetComponentInChildren<Text>().text
                    = BERRY[prefabnum].GetComponent<Berry>().berryExplain;//설명 설정

                berryExpPrice.transform.gameObject.GetComponentInChildren<Text>().text
                    = BERRY[prefabnum].GetComponent<Berry>().berryPrice.ToString()+"A";//가격 설정

                if (prefabnum < 64)
                { berryClassify.GetComponent<Image>().sprite = berryClassifyImage[0]; }
                else if (prefabnum < 128)
                { berryClassify.GetComponent<Image>().sprite = berryClassifyImage[1]; }
                else
                { berryClassify.GetComponent<Image>().sprite = berryClassifyImage[2]; }
                berryClassify.GetComponent<Image>().preserveAspect = true;



            }
        }
        catch
        {
            Debug.Log("여기에 해당하는 베리는 아직 없다");
        }

    }

}

