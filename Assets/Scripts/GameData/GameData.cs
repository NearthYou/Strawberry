using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/* ���̺�Ǿ�� �� ������ �־��ּ���! */

public class GameData
{
    //Ʃ�丮�� �ߴ���
    public bool isTutorialDone = false;

    //Ŭ���� ���� ��¥
    public DateTime cloudSaveTime;

    //��ȭ
    public int coin;
    public int heart;
    public int medal;
    public int dotori = 5;

    //Truck
    public int truckBerryCnt; // Ʈ�� ���� ����
    public int truckCoin;

    //��, ����

    // ���� ���� �ð�
    public float[] stemLevel = { 0f, 5f, 10f, 15f, 20f };

    //�̰� �ٲٷ��� �������� ���� �����ؼ� �ٲ�ߵ� ����� �ʱ�ȭ�κ�
    public float bugProb = 20f;
    public float weedProb = 20f; 

    //���� ���� �ֱ�
    public float weedPeriod = 60f;

    // ���� ���� �ð�
    public float rainDuration = 5.0f;

    //���� �� ������ ����
    public BerryFieldData[] berryFieldData = new BerryFieldData[16];

    //�رݵ� ����
    //�迭 ũ�� ���� �ȵǴ°� ���� C#�迭�� Ư���̴�.
    //List�� ����ϰų� Linq����ؾ� ��

    //=====================================================================================
    public bool[] isBerryUnlock = new bool[192];

    //���ο� ���� ����
    public int newBerryBtnState;//���� ���� ��ư ����
    public int newBerryIndex;
    public int newBerryTime;//���� ���� �ð�
    public DateTime newBerryTime_start;
    public DateTime newBerryTime_end;
    public TimeSpan newBerryTime_span;

    //���� ���� ���� ����
    //0�̸� classic��, 1�̸� unique����, 2�̸� ���� ���߰���
    public int newBerryResearchAble;

    //=====================================================================================
    //���� ����
    public int[] researchLevel = new int[6];
    public int researchLevelAv;
    //=====================================================================================

    //��������
    public int[] challengeLevel = new int[6];//���� �޼� ��ġ
    public bool[] isChallengeMax = new bool[6];//�������� max level���� ����
    //����
    public int[] newsState = new int[15];//0=Lock, 1=Unlock Able, 2=Unlock

    //����
    public bool[] isCollectionDone = new bool[6];

    //=====================================================================================
    //PTJ
    public int[] PTJNum = new int[6];//��� Ƚ��
    public int PTJCount;//n�� �����
    public int[] PTJSelectNum = new int[2];//���� ���õ� (prefabNum , �����̴� �ѹ�)
    public bool[] PTJIsWorking = new bool[6];//���ϴ� '����'����
    //=====================================================================================

    //����ǥ !
    public bool[] isBerryEM = new bool[192];
    //=====================================================================================

    //�����
    public float SFXSoundVolume;
    public float BGSoundVolume;


    //=====================================================================================
    //���������� ���� ���� ���� ���� (���� : accumulate)
    public int unlockBerryCnt; // �ر� ���� ���� (����)
    public int totalHarvBerryCnt; // ��Ȯ ���� ���� (����)
    public int accCoin; // ���� ����
    public int accHeart; // ���� ��Ʈ
    public int accAttendance; // ���� �⼮
    public int mgPlayCnt; // �̴ϰ��� �÷��� Ƚ��
    //���±��� ���� ���� �� ����
    //public int totalBerryCnt; => �̰� ��Ȯ�Ѱ� ����?
    //=====================================================================================

    //������ �⼮ ��¥ ����.
    public DateTime atdLastday = new DateTime();
    public DateTime currentTime = new DateTime();
    public DateTime lastExitTime = new DateTime();
    public DateTime nextMidnightTime = new DateTime();
    public DateTime nextDotoriTimer = new DateTime();
    public DateTime lastMinigameExitTime = new DateTime();
    public TimeSpan totalDotoriTime = TimeSpan.FromSeconds(0);
    public TimeSpan nextDotoriTime = TimeSpan.FromSeconds(0);
    public int resultDotori = 0;
    public TimeSpan rewardAbsenceTime = TimeSpan.FromSeconds(0);
    //�⼮ �� ��
    public int accDays = 0;
    public int weeks = 0;
    //���� �⼮ ���� �Ǵ�
    public bool isAttendance = false;
    public bool isPrework = false;
    public bool isFirstGame = false;

    //����&�̴ϰ���
    public bool isStoreOpend;
    public int[] highScore = new int[4];

    //���� Ƚ�� ����
    public int coinAdCnt = 3;
    public int heartAdCnt = 3;

    // ���� ����� ������ �ڵ� �޾Ҵ���
    public bool[] userCode = new bool[10];

}
