using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Play.Common;
using Google.Play.AppUpdate;
using System;

public class GooglePlayManager : MonoBehaviour
{
    // ��ó1: https://dodnet.tistory.com/4779
    // ��ó2: https://wonjuri.tistory.com/entry/unity-%EA%B5%AC%EA%B8%80%ED%94%8C%EB%A0%88%EC%9D%B4-%EC%9D%B8%EC%95%B1-%EC%97%85%EB%8D%B0%EC%9D%B4%ED%8A%B8-%EB%B0%8F-%EC%9D%B8%EC%95%B1-%EB%A6%AC%EB%B7%B0-%EC%97%B0%EB%8F%99-%ED%81%B4%EB%9E%98%EC%8A%A4
    public static GooglePlayManager Instance { get; private set; }

    bool Updating = false;

    public void Awake()
    {
        Instance = this;
    }

    public void OnDestroy()
    {
        Instance = null;
    }

    // �ξ� ������Ʈ ȣ�� �Լ�
    public IEnumerator UpdateApp()
    {
        // �Ŵ��� ����
        AppUpdateManager appUpdateManager = new AppUpdateManager();

        // ������ ������Ʈ�� �ִ��� Ȯ��
        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation =
            appUpdateManager.GetAppUpdateInfo();

        // ������Ʈ ������ �޾ƿ������ ��ٸ�
        yield return appUpdateInfoOperation;

        // ������Ʈ ������ ���������� �޾ƿ�
        if (appUpdateInfoOperation.IsSuccessful)
        {
            // ������Ʈ ���� ����
            var appUpdateInfoResult = appUpdateInfoOperation.GetResult();

            // ������Ʈ ���� ���� Ȯ��
            if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                // ���� ������Ʈ ����
                var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();
                var startUpdateRequest = appUpdateManager.StartUpdate(appUpdateInfoResult, appUpdateOptions);

                yield return startUpdateRequest;
            }

        }
        else
        {
            Debug.Log(appUpdateInfoOperation.Error);
        }
    }
}