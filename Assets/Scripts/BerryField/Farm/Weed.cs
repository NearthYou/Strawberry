using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weed : MonoBehaviour
{
    public int weedIdx;  
    public float xPos = 0f;   
    public int weedSpriteNum; // �ű�
  
    private Farm farm;
    private Animator anim;
    private BoxCollider2D farmColl;
    void Awake()
    {        
        anim = GetComponent<Animator>();
        farm = transform.parent.gameObject.GetComponent<Farm>();        
        farmColl = farm.GetComponent<BoxCollider2D>();
    }
    void OnEnable()
    {
        DataController.instance.gameData.berryFieldData[weedIdx].isWeedEnable = true;
        DataController.instance.gameData.berryFieldData[weedIdx].weedSpriteNum = Random.Range(0, 3);

        xPos = Random.Range(-0.35f, 0.35f); // ���� X���� ������ ��ġ�� ���� ����
        transform.position = new Vector2(farm.transform.position.x + xPos, farm.transform.position.y + 0.07f);

        anim.SetInteger("Generate", weedSpriteNum);
    }
    void OnDisable()
    {
        DataController.instance.gameData.berryFieldData[weedIdx].isWeedEnable = false;
    }
    
    public void GenerateWeed() // ���� ����
    {
        float prob = Random.Range(0, 100);
        
        //scale = Random.Range(1.3f, 1.8f);
        if (prob < DataController.instance.gameData.weedProb)
        {
            this.gameObject.SetActive(true); // �� �ڽ�(����)�� Ȱ��ȭ

            farmColl.enabled = false; // ���� �ݶ��̴� ��Ȱ��ȭ
            DataController.instance.gameData.berryFieldData[weedIdx].hasWeed = true; // ���ʺ������θ� Ȯ���ϴ� ����
            DataController.instance.gameData.berryFieldData[weedIdx].canGrow = false; // ������ ���� ����

            xPos = Random.Range(-0.35f, 0.35f); // ���� X���� ������ ��ġ�� ���� ����
            transform.position = new Vector2(farm.transform.position.x + xPos, farm.transform.position.y + 0.07f);            
        }
    }
    public void DeleteWeed()
    {
        anim.SetTrigger("Delete");

        if (this.gameObject.activeSelf)
        {
            StartCoroutine(DisableWeed(0.25f)); // �ִϸ��̼��� ���� �� ��Ȱ��ȭ
        }      
    }
    IEnumerator DisableWeed(float time)
    {
        yield return new WaitForSeconds(time);
     
        float creatTime = DataController.instance.gameData.berryFieldData[weedIdx].createTime; // ���Ⱑ ������ �ð����� ����
        if ((creatTime == 0f || (creatTime >= DataController.instance.gameData.stemLevel[4])) && !GameManager.instance.isBlackPanelOn) // BP�� �����ְ� �� ���̰ų� ���Ⱑ ��Ȯ������ ���¶��
        {
            farmColl.enabled = true; // ���� Collider�� �Ҵ�.
        }
        else // �ƴ϶��
        {
            farmColl.enabled = false; // ����.
        }
        DataController.instance.gameData.berryFieldData[weedIdx].hasWeed = false; // ���� ���ŵ�
        if (!DataController.instance.gameData.berryFieldData[weedIdx].hasBug) // ������ ���ٸ�
        {
            DataController.instance.gameData.berryFieldData[weedIdx].canGrow = true; // ����� �ٽ� �ڶ� �� �ִ�.
        }
        this.gameObject.SetActive(false); // ���� ��Ȱ��ȭ

        if (!(GameManager.instance.isMiniGameMode || Blink.instance.gameObject.activeSelf))
            AudioManager.instance.RemoveAudioPlay();
    }
}
