using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{

    void Start()
    {

    }

    //�ٵ� update�Ȥ����ϴ��溡�� ����..
    void Update()
    {
        if (this.GetComponent<AudioSource>().isPlaying == false)
        { AudioManager.ReturnObject(this); }
    }
}
