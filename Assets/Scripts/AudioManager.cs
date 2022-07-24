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
    public AudioClip[] BgClipList;//0=���ΰ��� 1=�̴ϰ���

    //ȿ���� �����
    public AudioClip SFXclip;
    public AudioClip SowClip;
    public AudioClip HarvestClip;
    public AudioClip Cute1Clip;
    public AudioClip BlopClip;
    public AudioClip Cute2Clip;
    public AudioClip Cute4Clip;
    public AudioClip TadaClip;
    public AudioClip RainClip;
    public AudioClip RightClip;
    public AudioClip WrongClip;
    public AudioClip DoorOpenClip;
    public AudioClip DoorCloseClip;
    // �Ʒ� �쿬�̰� �߰�
    public AudioClip CoinClip;
    public AudioClip RewardClip;
    public AudioClip TimerClip;
    public AudioClip RemoveClip;
    public AudioClip Remove2Clip;

    //instance
    public static AudioManager instance;

    //values
    private float BG_SOUND_VALUE;
    private float SFX_SOUND_VALUE;

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
        //BGSoundVolume();
        //SFXVolume();

        BgSoundPlay2(true);
    }
    private void Update()
    {
        BGSoundValue = BGSoundSlider.GetComponent<Slider>().value;
        SFXSoundValue = SFXSoundSlider.GetComponent<Slider>().value;
    }

    //value update
    float BGSoundValue {
        set
        {
            if (BG_SOUND_VALUE == value) return;
            BG_SOUND_VALUE = value;


            if (BGSoundSlider.GetComponent<Slider>().value == 0) { mixer.SetFloat("BGSoundVolume", -80); }
            else { mixer.SetFloat("BGSoundVolume", Mathf.Log10(BGSoundSlider.GetComponent<Slider>().value) * 20); }

        }
        get { return BG_SOUND_VALUE; }
    }

    float SFXSoundValue {
        set
        {
            if (SFX_SOUND_VALUE == value) return;
            SFX_SOUND_VALUE = value;

            if (SFXSoundSlider.GetComponent<Slider>().value == 0) { mixer.SetFloat("SFXVolume", -80); }
            else { mixer.SetFloat("SFXVolume", Mathf.Log10(SFXSoundSlider.GetComponent<Slider>().value) * 20); }


        }
        get { return SFX_SOUND_VALUE; }
    }
    //========================================================================================================
    //========================================================================================================

    //onvalue changed�� �۵��ϴ� �Լ�. �ȵ�.
    /*
    public void BGSoundVolume() {

        //����� ��������
        if (BGSoundSlider.GetComponent<Slider>().value == 0) { mixer.SetFloat("BGSoundVolume", -80); }
        else { mixer.SetFloat("BGSoundVolume", Mathf.Log10(BGSoundSlider.GetComponent<Slider>().value) * 20); }
        
        float value;
        mixer.GetFloat("BGSoundVolume", out value);
        Debug.Log("bg sound=" + BGSoundSlider.GetComponent<Slider>().value + "->" + value);

    }

    public void SFXVolume() {
        //ȿ���� ��������
        if (SFXSoundSlider.GetComponent<Slider>().value == 0) { mixer.SetFloat("SFXVolume", -80); }
        else { mixer.SetFloat("SFXVolume", Mathf.Log10(SFXSoundSlider.GetComponent<Slider>().value) * 20); }

        float value2;
        mixer.GetFloat("SFXVolume", out value2);
        Debug.Log("sfx sound=" + SFXSoundSlider.GetComponent<Slider>().value + "->" + value2);
    }
    */


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

    public void SowAudioPlay()//���Ѹ��� ȿ����
    { SFXPlay("SowSFX", SowClip); }
    public void HarvestAudioPlay() //��Ȯ ȿ����
    { SFXPlay("HarvestSFX", HarvestClip); }



    public void SFXAudioPlay() //X��ư blop
    { SFXPlay("ButtonSFX", SFXclip); }
    public void Cute1AudioPlay() //��ư ȿ����
    { SFXPlay("Cute1SFX", Cute1Clip); }
    public void Cute2AudioPlay()//�г� ����
    { SFXPlay("Cute2SFX", Cute2Clip); }
    public void Cute4AudioPlay()//�����г�
    { SFXPlay("Cute4SFX", Cute4Clip); }
    public void TadaAudioPlay() //���� ȹ�� ȿ���� 
    { SFXPlay("TadaSFX", TadaClip); }
    public void RainAudioPlay() //�� ȿ����
    { SFXPlay("RainSFX", RainClip); }
    public void RightAudioPlay() //�¾Ҿƿ� 
    { SFXPlay("RightSFX", RightClip); }
    public void WrongAudioPlay() //Ʋ�Ⱦ�� 
    { SFXPlay("WrongSFX", WrongClip); }


    public void DoorOpenAudioPlay() //������
    { SFXPlay("DoorOpenSFX", DoorOpenClip); }
    public void DoorCloseAudioPlay() //���ݱ� 
    { SFXPlay("DoorCloseSFX", DoorCloseClip); }

    public void CoinAudioPlay() // ���� ȹ��
    { SFXPlay("CoinSFX", CoinClip); }
    public void RewardAudioPlay() // ���� ȹ��
    { SFXPlay("RewardSFX", RewardClip); }
    public void TimerCloseAudioPlay() // �̴ϰ��� Ÿ�̸�
    { SFXPlay("TimerSFX", TimerClip); }
    public void RemoveAudioPlay() // ����1
    { SFXPlay("RemoveSFX", RemoveClip); }
    public void Remove2AudioPlay() // ����2
    { SFXPlay("Remove2SFX", Remove2Clip); }

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
