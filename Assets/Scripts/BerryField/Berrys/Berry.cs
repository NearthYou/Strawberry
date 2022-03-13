using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public enum BerryRank
{
    Classic,
    Special,
    Unique
} // 이 아이의 위치는 어디여야 할까?
public class Berry : MonoBehaviour
{         
    //여기에 public으로 딸기 정보 기입.
    public string berryName;
    public int berrykindProb;
    public string berryExplain;
    public int berryIdx;
    public BerryRank berryRank;
    
    public void Explosion(Vector2 from, Vector2 to, float exploRange) // DOTWeen 효과
    {
        transform.position = from;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(from + Random.insideUnitCircle * exploRange, 0.25f).SetEase(Ease.OutCubic));
        sequence.Append(transform.DOMove(to, 0.5f).SetEase(Ease.InCubic));
        sequence.AppendCallback(() => { Destroy(gameObject); });
    }   
}
