using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class HeartMover : MonoBehaviour
{
    public HeartAcquireFx prefab;
    public GameObject Day;
    public GameObject to;
    public RectTransform rect;
    public Transform target;

    public int daysCompare;
    private int days;
    private bool isAtd;
    [SerializeField] RectTransform heartPoint;

    private void Start()
    {
        days = DataController.instance.gameData.accDays;
        isAtd = DataController.instance.gameData.isAttendance;
    }

    public void HeartMove(int num)
    {

        if (days > 6)
            days %= 6;

        Vector2 vec = new Vector2(153, 714);

        /*      
                Vector2 vec = new Vector2(153, 714);
                if (j == 1)
                    vec = new Vector2(-133, 714);
                else if (j == 2)
                    vec = new Vector2(-410, 724);
                else if (j == 3)
                    vec = new Vector2(153, 1014);
                else if (j == 4)
                    vec = new Vector2(-133, 1014);
                else if (j == 5)
                    vec = new Vector2(-410, 1014);
                else if (j == 6)
                    vec = new Vector2(-133, 1314);*/

        if (isAtd == false && days == num)
        {
            int randCount = 10;//Random.Range(5, 10);
            for (int i = 0; i < randCount; ++i)
            {
                var itemFx = GameObject.Instantiate<HeartAcquireFx>(prefab, Day.transform);
                itemFx.Explosion(Day.transform.position, heartPoint.transform.position, 120.0f);
            }
            isAtd = true;
        }
    }

    public void BadgeMover(float range)
    {
        int randCount = 1;
        for (int i = 0; i < randCount; ++i)
        {
            var itemFx = GameObject.Instantiate<HeartAcquireFx>(prefab, this.transform);
            itemFx.Explosion2(Day.transform.position, range);
        }
    }

    public void HeartChMover(float range)
    {
        int randCount = 5;
        for (int i = 0; i < randCount; ++i)
        {
            var itemFx = GameObject.Instantiate<HeartAcquireFx>(prefab, this.transform);
            itemFx.Explosion2(Day.transform.position, range);
        }
    }


/*    public void CollMover(int i)
    {
        if (i == 0)
            HeartChMover(0.3467f, -0.043f, 0.2f);
        else
            BadgeMover(0.3467f, -0.043f, 0.2f);
    }*/

    public void CountCoin(float dis)
    {
        var itemFx2 = GameObject.Instantiate<HeartAcquireFx>(prefab, transform);
        itemFx2.Coin(Day.transform.position,dis);
    }
}
