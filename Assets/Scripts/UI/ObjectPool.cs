using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    RectTransform parent; //���̾��Ű �� �θ��� ��ġ ���� ������

    public static ObjectPool Instance; // �̱���

    [SerializeField]
    private HeartAcquireFx poolingObjectPrefab; //�̸� ������ ������

    Queue<HeartAcquireFx> poolingObjectQueue = new Queue<HeartAcquireFx>(); //ť ����

    void Awake()
    {
        Instance = this;
        Initialize(10); //������Ʈ 10�� �̸� ����
    }


    #region �ؽ�Ʈ �ִϸ��̼� Ǯ��

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject()); //10�� Enqueue
        }
    }

    private HeartAcquireFx CreateNewObject()
    {
        var newObj = GameObject.Instantiate<HeartAcquireFx>(poolingObjectPrefab, transform.position,Quaternion.identity, GameObject.Find("MainGame").transform);
        //��ȭ �̺�Ʈ ���� �� ������Ʈ�� ������ ��ġ ����(MainGame UI ĵ���� �ȿ��ٰ�)�� Instantiate ���� �� �־����� 
        newObj.gameObject.SetActive(false); //�̸� �����Ǿ� ����ϰ� �ִ� �ֵ� SetActive ���ְ�
        newObj.transform.SetParent(transform); //��ũ��Ʈ �ٿ����ִ� ObjectPool �ڽ����� �־���
        return newObj; //�׸��� Queue�� �ְ� ��ȯ
    }

    public static HeartAcquireFx GetObject() // �� �̸� ����� �����ٰ� ����!
    {
        if (Instance.poolingObjectQueue.Count > 0) // �̸� �����Ȱ� �Ⱥ����ϸ�
        {
            var obj = Instance.poolingObjectQueue.Dequeue(); // Dequeue
            obj.gameObject.SetActive(true); // �̸� �����Ǿ��ִ°� ON
            obj.transform.SetParent(Instance.parent); //static �̶� Instance.parent /���̾��Ű �� ObjectPool �̶�� ������Ʈ�� ������ �����ϰ� �θ� ����
            return obj;
        }
        else // �����ϸ�
        {
            var newObj = Instance.CreateNewObject(); // �ϳ� ���� ����
            newObj.gameObject.SetActive(true); // �ؿ��� ���� ����
            newObj.transform.SetParent(Instance.parent); 
            return newObj;
        }
    }

    public static void ReturnObject(HeartAcquireFx obj) //��� �� �ٽ� ��ȯ
     {
        obj.gameObject.SetActive(false); //����
        obj.transform.SetParent(Instance.transform); // �ٽ� ���� �θ�� ���ƿͼ� Object Pool �ڽ����� ����
        Instance.poolingObjectQueue.Enqueue(obj); // �׸��� �ٽ� Enqueue ����
    }

    //�� �θ� �Դٰ��� �ϴ°� �� �߳�? -> �׳� ������Ʈ Ǯ���ϸ� ĵ���� �ۿ��ٰ� �����Ǿ ȭ�� �� ǥ�ð� �ȵȴ�. �� UI�� �����. 
    //����� �Ƹ� �θ� �����ϰ� �ǵ��ƿ��°� ��ó�� ���ص� ���״� ������ ��ũ ����!

    #endregion
}
