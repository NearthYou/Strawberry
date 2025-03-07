using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Research : MonoBehaviour
{

    [Serializable]
    public class ResearchStruct
    {
        public string Name;//연구 제목
        public Sprite Picture;//사진
        public string Explanation;//설명
        public int Price;//가격

        public ResearchStruct(string Name, string Explanation, int Price, Sprite Picture)
        {
            this.Name = Name;
            this.Explanation = Explanation;
            this.Price = Price;
            this.Picture = Picture;
        }
    }

    [Header("==========INFO STRUCT==========")]
    [SerializeField]
    ResearchStruct[] Info;//구조체

    //Research Info  적용할 것들
    [Header("==========INFO 적용할 대상=========")]
    [SerializeField]
    public GameObject titleText;
    public GameObject Picture;
    public GameObject explanationText;
    public GameObject coinNum;
    public GameObject levelNum;



    //Prefab별로 숫자 부여
    static int Prefabcount = 0;
    int prefabnum;


    //비 파티클
    private ParticleSystem rainParticle;


    // 글로벌 변수
    private Globalvariable globalVar;

    //===================================================================================================
    //===================================================================================================

    void Start()
    {
        InfoUpdate();      
        rainParticle = GameObject.FindGameObjectWithTag("Rain").GetComponent<ParticleSystem>();
        globalVar = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>();       
    }


    //===================================================================================================
    //===================================================================================================
    public void InfoUpdate()
    {
        //!!!!!!!!!!!!!!주의!!!!!!!!!!!!!숫자 6은 프리팹 숫자와 관련되어 있다!!! 같이 조절 . 변수설정하기
        //프리팹들에게 고유 번호 붙이기
        if (Prefabcount >= 6) { Prefabcount %= 6; }
        prefabnum = Prefabcount;

        //타이틀, 그림, 설명, 비용 텍스트에 표시
        titleText.GetComponent<Text>().text = Info[prefabnum].Name;//제목(이름) 표시

        Picture.GetComponent<Image>().sprite = Info[prefabnum].Picture;//그림 표시

        levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString();

        if (DataController.instance.gameData.researchLevel[prefabnum] >= 25)
        {
            coinNum.GetComponent<Text>().text = "Max";
            explanationText.GetComponent<Text>().text =
                Info[prefabnum].Explanation + "\n" +
                (DataController.instance.gameData.researchLevel[prefabnum] * 2) + "%";//설명 텍스트 표시
        }
        else
        {
            if (DataController.instance.gameData.researchLevel[prefabnum] < 5) // 연구 레벨이 5레벨 이하라면
                Info[prefabnum].Price = (1 + DataController.instance.gameData.researchLevel[prefabnum]) * 200;
            else if (DataController.instance.gameData.researchLevel[prefabnum] < 10) // 연구 레벨이 10레벨 이하라면
                Info[prefabnum].Price = 1000 + (DataController.instance.gameData.researchLevel[prefabnum] - 4) * 400;
            else // 연구 레벨이 15레벨 이하라면
                Info[prefabnum].Price = 3000 + (DataController.instance.gameData.researchLevel[prefabnum] - 9) * 600;

            explanationText.GetComponent<Text>().text = Info[prefabnum].Explanation + "\n" +
            ((DataController.instance.gameData.researchLevel[prefabnum] * 2) + "% →" +
            (DataController.instance.gameData.researchLevel[prefabnum] + 1) * 2 + "%");//설명 텍스트 표시

            GameManager.instance.ShowCoinText(coinNum.GetComponent<Text>(), Info[prefabnum].Price); //비용 표시
        }
        Prefabcount++;
        
    }

    //=============================================================================================================================

    //연구 레벨
    public void clickCoin_Research() {
        AudioManager.instance.Cute1AudioPlay();

        //Debug.Log("prefabnum: " + prefabnum);
        if (DataController.instance.gameData.researchLevel[prefabnum] < 25)//레벨 25로 한계두기
        {
            //해당 금액이 지금 가진 코인보다 적으면
            if (DataController.instance.gameData.coin >= Info[prefabnum].Price)
            {
                //해당 금액의 코인이 감소하고 레벨업
                GameManager.instance.UseCoin(Info[prefabnum].Price);
                DataController.instance.gameData.researchLevel[prefabnum]++;//레벨업
                //Info[prefabnum].Price = (1 + DataController.instance.gameData.researchLevel[prefabnum]) * 200 * (DataController.instance.gameData.newBerryResearchAble + 1);//가격 업데이트

                //가격 업데이트
                if (DataController.instance.gameData.researchLevel[prefabnum] < 5) // 연구 레벨이 5레벨 이하라면
                    Info[prefabnum].Price = (1 + DataController.instance.gameData.researchLevel[prefabnum]) * 200;
                else if (DataController.instance.gameData.researchLevel[prefabnum] < 10) // 연구 레벨이 10레벨 이하라면
                    Info[prefabnum].Price = 1000 + (DataController.instance.gameData.researchLevel[prefabnum] - 4) * 400;
                else // 연구 레벨이 15레벨 이하라면
                    Info[prefabnum].Price = 3000 + (DataController.instance.gameData.researchLevel[prefabnum] - 9) * 600;
                

                levelNum.GetComponent<Text>().text = DataController.instance.gameData.researchLevel[prefabnum].ToString();//레벨 보이기
                
                switch (Info[prefabnum].Name)
                {
                    case "딸기 값이 금값": UpdateBerryPrice(); break;
                    case "딸기가 쑥쑥": DecreaseBerryGrowTime(); break;                   
                    case "도와줘요 세스콩": DecreaseBugGenerateProb(); break;
                    case "잡초 바이바이": DecreaseWeedGenerateProb(); break;
                    case "시원한 소나기": AccessRainDuration(); break;
                    case "부르는 게 값": break;
                }

                if (DataController.instance.gameData.researchLevel[prefabnum] >= 25)
                {
                    coinNum.GetComponent<Text>().text = "Max";
                    explanationText.GetComponent<Text>().text = 
                        Info[prefabnum].Explanation + "\n" +
                        (DataController.instance.gameData.researchLevel[prefabnum] * 2) + "%";//설명 텍스트 표시
                }
                else
                {
                    GameManager.instance.ShowCoinText(coinNum.GetComponent<Text>(), Info[prefabnum].Price);

                    explanationText.GetComponent<Text>().text = 
                        Info[prefabnum].Explanation + "\n" +
                        (DataController.instance.gameData.researchLevel[prefabnum] * 2) + "%" + "→" +
                        (DataController.instance.gameData.researchLevel[prefabnum] + 1) * 2 + "%";//설명 텍스트 표시
                }

                // 연구레벨 평균값 업데이트
                DataController.instance.gameData.researchLevelAv = 0;
                for (int i = 0; i < 6; i++)
                {
                    DataController.instance.gameData.researchLevelAv += DataController.instance.gameData.researchLevel[i];
                }
                DataController.instance.gameData.researchLevelAv /= 6;
            }
            else
            {
                //재화 부족 경고 패널 등장
                GameManager.instance.UseCoin(Info[prefabnum].Price);

            }
        }
            

    }
    public void UpdateBerryPrice()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[0]) * globalVar.getEffi();     
        for (int i = 0; i < 192; i++)
        {
            if (globalVar.berryListAll[i] == null) continue;

            if (i < 64)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((globalVar.CLASSIC_FIRST + i) * (1 + researchCoeffi));
            else if (i < 128)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((globalVar.SPECIAL_FIRST + (i - 64) * 2) * (1 + researchCoeffi));
            else
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((globalVar.UNIQUE_FIRST + (i - 128) * 3) * (1 + researchCoeffi));
            /*if (i < 64)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((5) * (1 + researchCoeffi));
            else if (i < 128)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((50) * (1 + researchCoeffi));
            else
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = Convert.ToInt32((1000) * (1 + researchCoeffi));*/
        }
    }


    //=============================================================================================================================
    //=============================================================================================================================
    //연구 기능 구현
    public void DecreaseBerryGrowTime()
    {
        
        float researchCoeffi = (DataController.instance.gameData.researchLevel[1]) * Globalvariable.instance.getEffi();

        for (int i = 0; i < DataController.instance.gameData.stemLevel.Length; i++)
        {
            DataController.instance.gameData.stemLevel[i] = (Globalvariable.instance.STEM_LEVEL[i] * (1 - researchCoeffi));
        }

    }
    public void DecreaseBugGenerateProb()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[2]) * Globalvariable.instance.getEffi();     
        DataController.instance.gameData.bugProb = (Globalvariable.BUG_PROB * (1 - researchCoeffi));
    }
    public void DecreaseWeedGenerateProb()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[3]) * Globalvariable.instance.getEffi();
        DataController.instance.gameData.weedProb = Globalvariable.WEED_PROB * (1 - researchCoeffi);
    }
    public void AccessRainDuration()
    {
        var main = rainParticle.main;

        if (rainParticle.isPlaying)
        {
            Invoke("IncreaseRainDuration", rainParticle.main.duration + 1.0f);
        }
        else
        {
            IncreaseRainDuration();
        }  
    }
    public void IncreaseRainDuration()
    {
        float researchCoeffi = (DataController.instance.gameData.researchLevel[4]) * Globalvariable.instance.getEffi();

        var main = rainParticle.main;
        main.duration = Globalvariable.RAIN_DURATION * (1 + researchCoeffi);

        DataController.instance.gameData.rainDuration = Globalvariable.RAIN_DURATION * (1 + researchCoeffi);
    }
}
