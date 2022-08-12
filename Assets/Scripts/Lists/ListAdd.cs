using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListAdd : MonoBehaviour
{

    [SerializeField]
    private GameObject elementPrefab = null;//������
    
    [SerializeField]
    private Transform[] content = null;//�������� �� ����Ʈ

    [SerializeField]
    private int count=0;//������ ����

    [SerializeField]
    private bool isBerry;

    //===================================================================================
    private void Start()
    {
        //����
        if (isBerry == true) 
        {
            for (int t = 0; t < 3; t++)//classic, special unique 3��
            {
                for (int j = 0; j < content.Length; j++)//layer ������ŭ ������ 2��
                {
                    for (int i = 0; i < 16; i++)//content �� ������ŭ
                    {
                        AddElement(content[j]);
                    }
                }
            }
        }
        //�� ��
        else
        {
            for (int i = 0; i < count; i++)
            {
                AddElement(content[0]);
            }
        }

    }

    public void AddElement(Transform content) 
    {
        var instance = Instantiate(elementPrefab);//�ش� �������� �ν��Ͻ�ȭ�ؼ� �����.
        instance.transform.SetParent(content);//�θ� content�� �Ѵ�. �� ������ ����.
    }

}
