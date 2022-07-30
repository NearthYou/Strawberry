using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame1 : MiniGame
{
    [Header("MiniGame1")]
    public Image quiz_img;     //������� �̹���
    public int quizIndex;      //������� �ε���
    public int[] answerIndex;  //������� �ε���(4��)
    public Button[] answer_btn;//������� ��ư(4��)
    public Image[] answer_img; //������� �̹���(4��)
    public GameObject O;       //O �̹���
    public GameObject X;       //X �̹���

    protected override void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            int _i = i;
            answer_btn[_i].onClick.AddListener(() => OnClickAnswerButton(_i));
        }
        answerIndex = new int[4];
        base.Awake();
    }

    protected override void MakeGame()
    {
        quiz_img.gameObject.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            answer_img[i].gameObject.SetActive(true);
            answer_btn[i].enabled = true;
        }

        //������� ����� �̹��� ��ġ
        quizIndex = unlockList[Random.Range(0, unlockList.Count)];
        quiz_img.sprite = global.berryListAll[quizIndex].GetComponent<SpriteRenderer>().sprite;

        //������ ������� �ε���(0~4)�� ������� ��ġ
        int randomAnswerIndex = Random.Range(0, 4);
        for (int i = 0; i < 4; i++)
        {
            if (randomAnswerIndex == i)
            {
                answer_img[i].sprite = quiz_img.sprite;
                answerIndex[i] = quizIndex;
            }
            else
            {
                //�����ε����� �ٸ� ���������̶� �ٸ� �����ȣ ���ö����� ������ȣ�� �̾Ƽ� ������⿡ ��ġ
                answerIndex[i] = unlockList[Random.Range(0, unlockList.Count)];
                while (CheckIndex(i))
                {
                    answerIndex[i] = unlockList[Random.Range(0, unlockList.Count)] ;
                }
                answer_img[i].sprite = global.berryListAll[answerIndex[i]].GetComponent<SpriteRenderer>().sprite;
            }
        }

        bool CheckIndex(int idx)
        {
            if (answerIndex[idx] == quizIndex) return true;
            for (int i = 0; i < 4; i++)
            {
                if (i == idx) continue;
                if (answerIndex[idx] == answerIndex[i]) return true;
            }
            return false;
        }
    }

    public void OnClickAnswerButton(int index)
    {
        //���� : 10�� �߰�, �������� ����
        if (answerIndex[index] == quizIndex)
        {
            O.SetActive(true);
            score += 10;
            score_txt.text = score.ToString() + "��";
        }
        //���� : 10�� �ٱ�
        else
        {
            X.SetActive(true);
            scrollbar.size -= size * 10;
            time -= 10;
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
        quiz_img.gameObject.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            answer_img[i].gameObject.SetActive(false);
            answer_btn[i].enabled = false;
        }
        O.SetActive(false);
        X.SetActive(false);
    }
}
