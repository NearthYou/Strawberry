using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdHeart : MonoBehaviour
{
    [SerializeField] Button adHeartBtn;
    [SerializeField] PanelAnimation panel;
    [SerializeField] GameObject panelBlack;
    public Text adHeartText;
    public Text remainAdText;

    void Awake()
    {
        GameManager.instance.OnOnline += HeartAdCountCheck;
        GameManager.instance.OnOffline += AdBtnOff;
    }

    public void HeartAdCountCheck()
    {
        if (DataController.instance.gameData.heartAdCnt > 0)
        {
            adHeartText.text = "���� ��û�ϰ�\n��Ʈ " + (5 * (DataController.instance.gameData.researchLevelAv + 1)) + "���� �������?";
        }
        else
        {
            adHeartText.text = "���� �� �� �ִ� ����\n��� ��û�Ͽ����!";
            adHeartBtn.interactable = false;
        }
        remainAdText.text = $"���� ���� Ƚ�� : {DataController.instance.gameData.heartAdCnt}";
    }

    public void AdBtnOff()
    {
        adHeartBtn.interactable = false;
    }


    public void OnClickPlusHeartBtn()
    {
        RewardAd.instance.OnAdComplete += ReceiveHeart;
        RewardAd.instance.OnAdFailed += OnFailAd;
        RewardAd.instance.ShowAd();
        adHeartBtn.interactable = false;
    }

    void ReceiveHeart()
    {
        GameManager.instance.GetHeart(5 * (DataController.instance.gameData.researchLevelAv + 1));
        adHeartBtn.interactable = true;
        RewardAd.instance.OnAdComplete -= ReceiveHeart;
        RewardAd.instance.OnAdFailed -= OnFailAd;

        DataController.instance.gameData.heartAdCnt--;
        panel.gameObject.SetActive(false); // �ÿ� ����
        panelBlack.SetActive(false);
    }

    void OnFailAd()
    {
        RewardAd.instance.OnAdComplete -= ReceiveHeart;
        RewardAd.instance.OnAdFailed -= OnFailAd;
        adHeartBtn.interactable = true;
    }
}
