using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdCoin : MonoBehaviour
{
    [SerializeField] Button adCoinBtn;
    [SerializeField] PanelAnimation panel;
    [SerializeField] GameObject panelBlack;
    public Text adCoinText;
    public Text remainAdText;

    void Awake()
    {
        GameManager.instance.OnOnline += CoinAdCountCheck;
        GameManager.instance.OnOffline += AdBtnOff;
    }

    public void CoinAdCountCheck()
    {
        if (DataController.instance.gameData.coinAdCnt > 0)
        {
            adCoinText.text = "���� ��û�ϰ�\n���� " + (500 * (DataController.instance.gameData.researchLevelAv + 1)) + "A�� �������?";
        }
        else
        {
            adCoinText.text = "���� �� �� �ִ� ����\n��� ��û�Ͽ����!";
            adCoinBtn.interactable = false;
        }
        remainAdText.text = $"���� ���� Ƚ�� : {DataController.instance.gameData.coinAdCnt}";
    }

    public void AdBtnOff()
    {
        adCoinBtn.interactable = false;
    }

    public void OnClickPlusCoinBtn()
    {
        RewardAd.instance.OnAdComplete += ReceiveCoin;
        RewardAd.instance.OnAdFailed += OnAdFail;
        RewardAd.instance.ShowAd();
        adCoinBtn.interactable = false;
    }

    void ReceiveCoin()
    {
        GameManager.instance.GetCoin(500 * (DataController.instance.gameData.researchLevelAv + 1));
        RewardAd.instance.OnAdComplete -= ReceiveCoin;
        RewardAd.instance.OnAdFailed -= OnAdFail;
        adCoinBtn.interactable = true;

        DataController.instance.gameData.coinAdCnt--;
        panelBlack.SetActive(false);
        panel.gameObject.SetActive(false);// �ÿ� ����
    }

    void OnAdFail()
    {
        Debug.Log("����");
        RewardAd.instance.OnAdFailed -= OnAdFail;
        RewardAd.instance.OnAdComplete -= OnClickPlusCoinBtn;
        adCoinBtn.interactable = true;
    }
}
