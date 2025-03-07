using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PTJWorking : MonoBehaviour
{
    //========알바생 정보==========
    public Sprite[] FacePicture;
    public Sprite[] WorkingFacePicture;
    public GameObject face;
    public GameObject employCount;


    //==========Prefab별로 숫자 부여==========
    static int Prefabcount = 0;
    int prefabnum;
    //==========고용 횟수==========
    private int PTJ_NUM_NOW;
    private bool PTJ_IS_WORKING;

    private bool isOnetime;//첫번째에만 scrollView맨 앞으로 이동하게 한다.

    private void Awake()
    {
        //프리팹들에게 고유 번호 붙이기
        if (Prefabcount >= 6) { Prefabcount %= 6; }
        prefabnum = Prefabcount;
        Prefabcount++;

    }
    void Start()
    {
        //얼굴 이미지 적용
        face.GetComponent<Image>().sprite = FacePicture[prefabnum];
        
        isOnetime = true;
    }


    void Update()
    {
        //자신의 고용횟수값 변경 파악
        PTJNumNow = DataController.instance.gameData.PTJNum[prefabnum];
        PTJIsWorking = DataController.instance.gameData.PTJIsWorking[prefabnum];
    }

    int PTJNumNow
    {
        set
        {
            if (PTJ_NUM_NOW == value) return;
            PTJ_NUM_NOW = value;


            //알바 끝남
            if (PTJ_NUM_NOW == 0)
            {
                face.SetActive(false);
                employCount.SetActive(false);
                DataController.instance.gameData.PTJCount--;
                isOnetime = true;
                gameObject.transform.SetAsLastSibling();
                AudioManager.instance.DisappearAudioPlay();//알바 끝나면 효과음. 바꿀거임 임시임 // 바꿔봄(-우연)
            }
            //알바 하는중
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

    bool PTJIsWorking
    {
        set
        {
            if (PTJ_IS_WORKING == value) return;
            PTJ_IS_WORKING = value;

            if (PTJ_IS_WORKING == true) 
            {
                StartCoroutine("workingFace");
                
            }
        }
        get { return PTJ_IS_WORKING; }
    
    }

    private IEnumerator workingFace() 
    {
        face.GetComponent<Image>().sprite = WorkingFacePicture[prefabnum];
        yield return new WaitForSeconds(2f);
        face.GetComponent<Image>().sprite = FacePicture[prefabnum];
        DataController.instance.gameData.PTJIsWorking[prefabnum] = false;
    }


}
