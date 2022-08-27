using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public bool isPlayAudio;

    [Header("object")]
    //�Ҹ� ���� �����̴�
    public GameObject BGSoundSlider;
    public GameObject SFXSoundSlider;

    [Header("Sound")]
    public AudioMixer mixer;//����� �ͼ�
    public AudioSource bgSound;//����� �Ŵ���

    //public bool isGameMain;//true=���ΰ��� false=�̴ϰ���

    //����� �����
    public AudioClip[] BgClipList;//0=Ʃ�丮��, 1=�������, 2=����, 3=ũ����, 4~7=�̴ϰ���

    //ȿ���� ����� (���ƾ���)
    public AudioClip HarvestClip;
    public AudioClip SowClip;
    public AudioClip ClickClip; //Cute1Clip;
    public AudioClip OKClip;    //Cute2Clip;
    public AudioClip BackClip;  //SFXclip;
    public AudioClip ErrorClip; //Cute4Clip;
    public AudioClip TadaClip;
    public AudioClip RainClip;
    public AudioClip RightClip;
    public AudioClip WrongClip;
    public AudioClip DoorOpenClip;
    public AudioClip DoorCloseClip;
    public AudioClip CoinClip;
    public AudioClip RewardClip;
    public AudioClip TimerClip;
    public AudioClip FastTimerClip;
    public AudioClip RemoveWeedClip;
    public AudioClip RemoveBugClip;
    public AudioClip CountdownClip;
    public AudioClip EndClip;
    public AudioClip ScrollbarClip;

    //instance
    public static AudioManager instance;


    //========================================================================================================
    //========================================================================================================
    private void Awake()
    {
        //isGameMain = true;
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            //SceneManager.sceneLoaded += OnSceneLoaded;//�� ���۵ɶ����� ����
        }
        else { Destroy(gameObject); }
    }


    private void Start()
    {
        BGSoundSlider.GetComponent<Slider>().value = 0.5f;
        SFXSoundSlider.GetComponent<Slider>().value = 0.5f;

        BgSoundPlay2(true);


        //�����̵尪 ���Ҷ����� �Ʒ� �Լ� ����
        BGSoundSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { BGSoundVolume(); });
        SFXSoundSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { SFXVolume(); });
    }

    
    //========================================================================================================
    //========================================================================================================
    

    public void BGSoundVolume() {

        //����� ��������
        if (BGSoundSlider.GetComponent<Slider>().value == 0) { mixer.SetFloat("BGSoundVolume", -80); }
        else { mixer.SetFloat("BGSoundVolume", Mathf.Log10(BGSoundSlider.GetComponent<Slider>().value) * 20); }
        


    }

    public void SFXVolume() {
        //ȿ���� ��������
        if (SFXSoundSlider.GetComponent<Slider>().value == 0) { mixer.SetFloat("SFXVolume", -80); }
        else { mixer.SetFloat("SFXVolume", Mathf.Log10(SFXSoundSlider.GetComponent<Slider>().value) * 20); }


    }



    //ȿ����
    //�̰� �̿ϼ�
    public void SFXPlay(string sfxName,AudioClip clip) 
    {

        GameObject go = new GameObject(sfxName+"Sound");//�ش��̸��� ���� ������Ʈ�� �Ҹ� �������� �����ȴ�.
        
        //����� ���
        AudioSource audioSource = go.AddComponent<AudioSource>();//�� ������Ʈ�� ����� �ҽ� ������Ʈ �߰�
        audioSource.clip = clip;//����� �ҽ� Ŭ�� �߰�
        audioSource.loop = false;
        audioSource.Play();//���

        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];

        Destroy(go, clip.length);//ȿ���� ��� ��(clip.length �ð� ������) �ı�
    
    }



    //��� ���ϼ��ֱ��ѵ� �׷��� ����� ���ۤ������� �־����

    public void HarvestAudioPlay()  //��Ȯ ȿ����
    { SFXPlay("HarvestSFX", HarvestClip); }
    public void SowAudioPlay()      //���Ѹ��� ȿ����
    { SFXPlay("SowSFX", SowClip); }
    public void Cute1AudioPlay()// Ŭ�� ȿ����
    { SFXPlay("Cute1SFX", ClickClip); }
    public void Cute2AudioPlay()// �г� ���� (Ȯ��)
    { SFXPlay("Cute2SFX", OKClip); }
    public void SFXAudioPlay()  // X��ư blop
    { SFXPlay("ButtonSFX", BackClip); }
    public void Cute4AudioPlay()// ���� �г�
    { SFXPlay("Cute4SFX", ErrorClip); }
    public void TadaAudioPlay() // ���� ȹ��
    { SFXPlay("TadaSFX", TadaClip); }
    public void RainAudioPlay() // �ҳ���
    { SFXPlay("RainSFX", RainClip); }
    public void RightAudioPlay()// �̴ϰ��� ����
    { SFXPlay("RightSFX", RightClip); }
    public void WrongAudioPlay()// �̴ϰ��� ���� 
    { SFXPlay("WrongSFX", WrongClip); }
    public void DoorOpenAudioPlay()     // ���� ����
    { SFXPlay("DoorOpenSFX", DoorOpenClip); }
    public void DoorCloseAudioPlay()    // ���� �ݱ� 
    { SFXPlay("DoorCloseSFX", DoorCloseClip); }
    public void CoinAudioPlay()         // ���� ȹ��
    { SFXPlay("CoinSFX", CoinClip); }
    public void RewardAudioPlay()       // ���� ȹ��
    { SFXPlay("RewardSFX", RewardClip); }
    public void TimerCloseAudioPlay()   // �̴ϰ��� Ÿ�̸�
    { SFXPlay("TimerSFX", TimerClip); }
    public void TimerVeryCloseAudioPlay()   // �̴ϰ��� �� ���� Ÿ�̸�
    { SFXPlay("FastTimerSFX", FastTimerClip); }
    public void RemoveAudioPlay()       // ���� ����
    { SFXPlay("RemoveWeedSFX", RemoveWeedClip); }
    public void Remove2AudioPlay()      // ���� ����
    { SFXPlay("RemoveBugSFX", RemoveBugClip); }
    public void CountdownAudioPlay()      // �̴ϰ��� ī��Ʈ �ٿ�
    { SFXPlay("CountdownSFX", CountdownClip); }
    public void EndAudioPlay()      // �̴ϰ��� ����
    { SFXPlay("EndSFX", EndClip); }
    public void ScrollbarAudioPlay()      // ��ũ�ѹ�
    { SFXPlay("ScrollbarSFX", ScrollbarClip); }


    //���� ����� �ִ� �Լ�
    public void SFXPlayButton(AudioClip clip) 
    {
        SFXPlay("", clip);
    }

    public void BgSoundPlay2(bool isGameMain)
    {
        AudioClip clip;
        if (isGameMain == true) {clip= BgClipList[0]; }
        else { clip = BgClipList[1]; }

        if (isPlayAudio == true)
        {
            bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGSound")[0];
            bgSound.clip = clip;
            bgSound.loop = true;
            bgSound.volume = 0.2f;
            bgSound.Play();
        }
    }

}
