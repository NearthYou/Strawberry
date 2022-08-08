using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    public GameObject Store;
    public Sprite[] StoreSprite;
    public GameObject popup;
    public GameObject inside;
    public Button UnlockBtn;
    public Button startButton;
    public Text infoText;
    public List<GameObject> miniGameList;

    void Awake()
    {
        if (DataController.instance.gameData.isStoreOpend == true)
            Store.GetComponent<Image>().sprite = StoreSprite[1];

    }
    
    public void EnterStore()
    {
        if (DataController.instance.gameData.isStoreOpend == true)
            inside.SetActive(true);
        else
        {
            //Debug.Log(DataController.instance.gameData.isStoreOpend);
            ////�ر����� - �������� 15�̻�, 700A �Ҹ� ���ɻ���
            //if (DataController.instance.gameData.coin >= 700 && ResearchLevelCheck(15))
            //{
            //    UnlockBtn.interactable = true;
            //}
            //else
            //{
            //    UnlockBtn.interactable = false;
            //}
        }
    }

    public void ClickUnlockStore()
    {
        DataController.instance.gameData.isStoreOpend = true;
        popup.SetActive(false);
        Store.GetComponent<Image>().sprite = StoreSprite[1];
    }

    private bool ResearchLevelCheck(int level)
    {
        for (int i = 0; i < DataController.instance.gameData.researchLevel.Length; i++)
        {
            if (DataController.instance.gameData.researchLevel[i] < level)
            { return false; }
        }
        return true;
    }

    public void SetInfo(int btnIdx)
    {
        switch(btnIdx)
        {
            case 1:
                infoText.text = "���� â�� ������ �Ǿ����! ���� ������� �з��ؼ� �Ⱦƾ� �ϴµ�... ���� �����ϰ� ��Ͼ���� �ٱ��� ����� ������. ������ �츮�� �Ϸ� ��Ʋ ���⸦ ��Ȯ�� �� �ƴ���! ��� �ӿ��� �ִ��� ���� ������� �з��� ����!";
                break;
            case 2:
                infoText.text = "�� ���� ����� ���� ��Ƶ׾��µ�, ���� ���������� ��� ������� ��¦ ���� �� ���ƿ�. ������ �츮���� ���� �ų��� ����. ���� �����ϰ� ���� �̽��� ������� ����! �׷��� ���� ũ�� �߰� ���� ������� ��󳻺���!";
                break;
            case 3:
                infoText.text = "���� �翡 ��ǳ�� ���������! �翡 �����ִ� ����鵵, ��Ȯ�ص� ����鵵 ��� ��� ���ư� ���Ⱦ��... ���? �ٵ� ���ư��� ������� �ϴÿ��� �������׿�?���� ���� ���� �ƴ���! �ٱ��Ϸ� ������ ����鸸 �ִ��� ��ƺ���!";
                break;
            case 4:
                infoText.text = "���̰� ���⸦ ����� ���Ⱑ �ټ����� �и��غ���";
                break;
        }
    }
    public void OnclickStartBtn()
    {
        string info_str = infoText.text.Substring(0, 5);
        Debug.Log(info_str);
        if (info_str == "���� â��")
        {
            miniGameList[0].gameObject.SetActive(true);
        }
        else if (info_str == "�� ���� ")
        {
            miniGameList[1].gameObject.SetActive(true);
        }
        else if(info_str == "���� �翡")
        {
            miniGameList[2].gameObject.SetActive(true);
        }  
        else if (info_str == "���̰� ��")
        {
            miniGameList[3].gameObject.SetActive(true);
        }
    }
}
