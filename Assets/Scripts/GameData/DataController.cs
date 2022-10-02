using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class DataController : MonoBehaviour
{
    public bool isSaveMode;
    [Header("암호화 여부 체크")]
    public bool dataEncryption;

    public static DataController instance = null;

    //게임데이터
    string gameDataFileName = "gameData.json";
    //[HideInInspector]
    public GameData gameData;

    private ParticleSystem rainParticle;
    private Globalvariable globalVar;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(instance!=this)
        {
            //씬 전환이 되었는데 인스턴스가 존재하는 경우
            //이전 씬에서 넘어온 인스턴스를 사용하기 위해 
            //새로운 씬의 게임오브젝트 제거
            Destroy(this.gameObject);
        }
        rainParticle = GameObject.FindGameObjectWithTag("Rain").GetComponent<ParticleSystem>();
        globalVar = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>();
        LoadData();
    }   
    public void LoadData()
    {
        //암호화x
        if (!instance.dataEncryption)
        {
            string filePath = Application.persistentDataPath + gameDataFileName;

            if (File.Exists(filePath))
            {
                //json파일 불러오기
                //Debug.Log(filePath);
                string jsonData = File.ReadAllText(filePath);

                //역직렬화
                gameData =JsonConvert.DeserializeObject<GameData>(jsonData);

                // 비 지속시간 로드
                var main = rainParticle.main;
                main.duration = gameData.rainDuration;

                Debug.Log("비 암호화 된 데이터 불러오기 성공");
            }
            else
            {
                Debug.Log("새로운 데이터 생성");
                gameData = new GameData();
                InitData();
                if(isSaveMode) SaveData();
            }
        }
        else //암호화 O
        {
            string jsonData = DataSecurity.Load(gameDataFileName);

            if (string.IsNullOrEmpty(jsonData))
            {
                Debug.Log("새로운 데이터 생성");
                gameData = new GameData();
                InitData();
                if (isSaveMode) SaveData();
            }
            else
            {
                gameData = JsonConvert.DeserializeObject<GameData>(jsonData);

                // 비 지속시간 로드
                var main = rainParticle.main;
                main.duration = gameData.rainDuration;

                Debug.Log("암호화된 데이터 불러오기 성공");
            }
        }
    }

    public void SaveData()
    {
        string filePath = Application.persistentDataPath + gameDataFileName;

        //데이터 직렬화
        string jsonData = JsonConvert.SerializeObject(gameData);

        //암호화x
        if (!instance.dataEncryption)
        {
            File.WriteAllText(filePath, jsonData);
            Debug.Log("암호화 하지 않고 저장 : " + filePath);
        }
        else
        {
            DataSecurity.Save(gameDataFileName,jsonData);
            Debug.Log("암호화저장");
        }

    }

    public void InitData()
    {
        gameData.cloudSaveTime = new System.DateTime();
        // 재화 변수
        gameData.coin = 200; // 원래 500
        gameData.heart = 20;
        gameData.medal = 0;
        // 누적 변수
        gameData.unlockBerryCnt = 1;
        gameData.totalHarvBerryCnt = 0;
        gameData.accCoin = gameData.coin;
        gameData.accHeart = gameData.heart;
        gameData.accAttendance = 0;
        gameData.mgPlayCnt = 0;

        // 딸기 성장 시간


        //Truck
        gameData.truckBerryCnt = 0;

        //Research
        for (int i = 0; i < gameData.researchLevel.Length; i++) 
        {
            gameData.researchLevel[i] = 0;
        }
        
        //BerryFieldData
        for(int i = 0; i < gameData.berryFieldData.Length; i++)
        {
            gameData.berryFieldData[i] = new BerryFieldData();                       
        }
        
        // 비 지속시간 초기화
        var main = rainParticle.main;
        main.duration = 5.0f;

        //초기 딸기 가격 설정
        InitBerryPrice();

        //=====================================================
        //여기 아래 정리 필요
        //isBerryUnlock
        for (int i = 0; i < 192; i++)
        {   gameData.isBerryUnlock[i] = false;   }
        gameData.isBerryUnlock[0] = true;//첫번째 기본베리는 존재

        //도전과제
        for (int i = 0; i < 6; i++) {   gameData.challengeLevel[i] = 0; gameData.challengeTF[i] = false;  }

        //수집
        for (int i = 0; i < 6; i++) { gameData.isCollectionDone[i] = false; gameData.collectionTF[i] = false; }

        //뉴스
        for (int i = 0; i < 15; i++) {   gameData.newsState[i] = 0;  }

        //PTJ
        for (int i = 0; i < 6; i++) { gameData.PTJNum[i] = 0; }
        gameData.PTJSelectNum[0] = 0;
        gameData.PTJSelectNum[1] = 0;
        gameData.PTJCount = 0;

        //느낌표 !!
        for (int i = 0; i < 15; i++) 
        {    gameData.isNewsEM[i] = false;   }
        for (int i = 0; i < 192; i++)
        {    gameData.isBerryEM[i] = false;    }

        //딸기
        gameData.newBerryResearchAble = 0;
        gameData.newBerryBtnState = 0;

        gameData.newBerryTime = 0;

        gameData.newBerryTime_start = System.DateTime.Now;
        gameData.newBerryTime_end = System.DateTime.Now;
        gameData.newBerryTime_span = gameData.newBerryTime_start - gameData.newBerryTime_end;

        gameData.newBerryIndex = 0;

        gameData.newBerryBangImgBool = false;


        //오디오
        gameData.BGSoundVolume = 0.5f;
        gameData.SFXSoundVolume = 0.5f;

        //가게관련변수
        gameData.isStoreOpend = false;
        for (int i = 0; i < 4; i++) { gameData.highScore[i] = 0; }
        //=====================================================
    }
    void InitBerryPrice()
    {      
        for (int i = 0; i < 192; i++)
        {
            if (globalVar.berryListAll[i] == null) continue;

            if (i < 64)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = (int)((globalVar.CLASSIC_FIRST + i * 3));
            else if (i < 128)
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = (int)((globalVar.SPECIAL_FIRST + (i - 64) * 5));
            else
                globalVar.berryListAll[i].GetComponent<Berry>().berryPrice
                    = (int)((globalVar.UNIQUE_FIRST + (i - 128) * 7));
        }
    }
    void OnApplicationQuit()
    {
        //게임종료시 저장
        if(isSaveMode)SaveData();
    }
    
}
