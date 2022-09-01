using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame2 : MiniGame
{
    [Header("MiniGame2")]
    public Berry[] rotten_berry; // ����(����) ���� �ε���
    public int normalIndex;      //������� �ε���
    public int[] answerIndex_4x4;  //������� �ε���(16��)
    public Button[] answer_btn_4x4;//������� ��ư(16��)
    public Image[] answer_img_4x4; //������� �̹���(16��)
    public GameObject O;       //O �̹���
    public GameObject X;       //X �̹���
    public int rottenIndex; // 4(����) or 8(����)�� �ε���
    public Image Sandy;
    public Sprite[] SandySprite;

    private float[] shaded = { 0.5f, 0.75f, 0.9f};
    private int shade_idx = 0;
    private int randomAnswerIndex = 0;

    protected override void Awake()
    {
        for (int i = 0; i < 16; i++)
        {
            int _i = i;
            answer_btn_4x4[_i].onClick.AddListener(() => OnClickAnswerButton(_i));
        }
        answerIndex_4x4 = new int[16];       
        base.Awake();
    }

    protected override void MakeGame()
    {
        // ���� ����� ������ ���� ���ϱ�
        if(score < 100) shade_idx = 0;
        else if (100 <= score && score <= 200) shade_idx = 1;
        else if(score >= 200) shade_idx = 2;

        answer_img_4x4[randomAnswerIndex].color = new Vector4(1, 1, 1, 1);
        for (int i = 0; i < 16; i++)
        {
            answer_img_4x4[i].gameObject.SetActive(true);
            answer_btn_4x4[i].enabled = true;
        }

        //���� ���� ����� �̹��� ��ġ
        while (true) // ����, ���� ���⸦ �����ϰ� ����
        {
            normalIndex = unlockList[UnityEngine.Random.Range(0, unlockList.Count)];
            if (normalIndex != 4 && normalIndex != 8) break;
        }
        Debug.Log("normalIndex: " + normalIndex);

        //������ ���� ���� �ε���(0~16)�� ���� ���� ��ġ
        randomAnswerIndex = UnityEngine.Random.Range(0, 16);
        Debug.Log("randomAnswerIndex: " + randomAnswerIndex);
        for (int i = 0; i < 16; i++)
        {
            answerIndex_4x4[i] = normalIndex; // ���� ��ġ
            answer_img_4x4[i].sprite = global.berryListAll[answerIndex_4x4[i]].GetComponent<SpriteRenderer>().sprite;
            if (randomAnswerIndex == i)
            {
                // ���� or ���� ���� ��ġ(��������)               
                float rgb = shaded[shade_idx];
                answer_img_4x4[i].color = new Vector4(rgb, rgb, rgb, 1);
                Debug.Log("answer_img_4x4[i].color.r: " + answer_img_4x4[i].color.r);
            }               
        }
    }

    public void OnClickAnswerButton(int index)
    {
        Color color = answer_img_4x4[index].color;
        //���� : 10�� �߰�, �������� ����
        if (!color.Equals(new Vector4(1, 1, 1, 1)))
        {
            O.SetActive(true);
            score += 10;
            score_txt.text = score.ToString();
            AudioManager.instance.RightAudioPlay();

            Sandy.GetComponent<Image>().sprite = SandySprite[0];
        }
        //���� : 10�� �ٱ�
        else
        {
            X.SetActive(true);
            //scrollbar.size -= size * 10;
            scroll.fillAmount -= size * 10;
            time -= 10;
            AudioManager.instance.WrongAudioPlay();

            Sandy.GetComponent<Image>().sprite = SandySprite[1];
        }
        if (time > 0)
        {                  
            Invoke("MakeNextQuiz", 0.3f);
        }
    }

    void MakeNextQuiz()
    {
        O.SetActive(false);
        X.SetActive(false);
        MakeGame();
    }

    public override void StopGame()
    {
        base.StopGame();
        //���� �Ⱥ��̰�
        for (int i = 0; i < 16; i++)
        {
            answer_img_4x4[i].gameObject.SetActive(false);
            answer_btn_4x4[i].enabled = false;
        }
        O.SetActive(false);
        X.SetActive(false);


    }

    protected override void FinishGame()
    {
        base.FinishGame();
        AudioManager.instance.EndAudioPlay();

        ManageScore(1, score);

        //����г�
        resultPanel.SetActive(true);
        result_cur_score_txt.text = score + "��";
        result_highscore_txt.text = DataController.instance.gameData.highScore[1].ToString();



        // �̴ϰ��� 2 ���� ��Ʈ ����(�̴ϰ��� 2�� �ر� ��Ʈ�� 40�̴�)
        float gain_coin = score * research_level_avg * ((100 + 40 * 2) / 100f);
        //result_coin_txt.text = gain_coin.ToString();
        GameManager.instance.ShowCoinText(result_coin_txt.GetComponent<Text>(), Convert.ToInt32(gain_coin));

        Debug.Log("���� ����:" + Convert.ToInt32(gain_coin));

        //��������
        GameManager.instance.GetCoin(Convert.ToInt32(gain_coin));

        StopGame();
    }
}
