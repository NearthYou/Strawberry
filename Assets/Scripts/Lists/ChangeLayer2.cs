using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ChangeLayer2 : MonoBehaviour
{
    #region Inspector / Variable

    [SerializeField]
	private bool isBerry;

	[Header("====Buttons====")]
	[SerializeField]
	private GameObject[] tagButtons;//버튼 gameObject
	public Sprite[] tagButtons_Image;//눌리지 않은 버튼 이미지
	public Sprite[] tagButtons_selectImage;//눌린 버튼 이미지

	[Header("====ScrollView====")]
	//베리 content들 겸 Challenge News 오브젝트
	[SerializeField]
	private GameObject[] content;

	[Header("========Swipe=======")]
	[SerializeField]
	private Scrollbar scrollBar;                    // Scrollbar의 위치를 바탕으로 현재 페이지 검사
	[SerializeField]
	private float swipeTime = 0.1f;         // 페이지가 Swipe 되는 시간
	[SerializeField]
	private float swipeDistance = 1.0f;        // 페이지가 Swipe되기 위해 움직여야 하는 최소 거리
	public Button[] swipeBtn;

	[Header("=====Berry Lock=====")]
	public GameObject berryLockSpecial;
	public GameObject LockSpecialBtn;

	public GameObject berryLockUnique;
	public GameObject LockUniqueBtn;

	[Header("=====Panel Black=====")]
	[SerializeField]
	private GameObject panelBlack;

	//============================================================================
	//SWIPE 변수들

	private float[] scrollPageValues;           // 각 페이지의 위치 값 [0.0 - 1.0]
	private float valueDistance = 0;            // 각 페이지 사이의 거리
	private int currentPage = 0;            // 현재 페이지
	private int maxPage = 2;                // 최대 페이지 2로 설정

	private float startTouchX;              // 터치 시작 위치
	private float endTouchX;                    // 터치 종료 위치

	private bool isSwipeMode = false;       // 현재 Swipe가 되고 있는지 체크

    #endregion
    
	//===========================================================================================================
    //===========================================================================================================
    
	#region Event Function
    private void Awake()
	{
		if (isBerry == true)
		{
			// 스크롤 되는 페이지의 각 value 값을 저장하는 배열 메모리 할당
			scrollPageValues = new float[2];

			// 스크롤 되는 페이지 사이의 거리
			valueDistance = 1f / (scrollPageValues.Length - 1f);

			// 스크롤 되는 페이지의 각 value 위치 설정 [0 <= value <= 1]
			for (int i = 0; i < scrollPageValues.Length; ++i)
			{	scrollPageValues[i] = valueDistance * i;	}

			
		}
	}
	void Start()
	{
		TagImageChange(0);

		if (isBerry == true)
		{
			//처음에는 berry classic
			selectBerryTag("berry_classic");

			// 처음 시작할 때 0번 페이지 보인다.
			SetScrollBarValue(0);
			swipeBtn[0].interactable = false;
			swipeBtn[1].interactable = true;
		}
	}

	private void Update()
	{
		if (isBerry == true)
        {


			UpdateInput(); //스와이프 관련

			// 잠금해제 버튼
			if (berryLockSpecial.activeInHierarchy && ResearchLevelCheck(5))
			{
				LockSpecialBtn.GetComponent<Button>().interactable = true;
			}
			if (berryLockUnique.activeInHierarchy && ResearchLevelCheck(10))
			{
				LockUniqueBtn.GetComponent<Button>().interactable = true;
			}



			// 스와이프 버튼
			if (currentPage == 0)
			{
				swipeBtn[0].interactable = false;
				swipeBtn[1].interactable = true;
			}
			else if (currentPage == maxPage - 1)
			{
				swipeBtn[0].interactable = true;
				swipeBtn[1].interactable = false;
			}
			else
			{
				swipeBtn[0].interactable = true;
				swipeBtn[1].interactable = true;
			}

		}
		
	}

    #endregion

    
	//===========================================================================================================
    //===========================================================================================================

    #region SWIPE

    private void UpdateInput()
	{
		// 현재 Swipe를 진행중이면 터치 불가
		if (isSwipeMode == true) return;

		//현재 swipe불가한 상태이면 터치 불가
		if ((berryLockSpecial.activeSelf == true && tagButtons[1].GetComponent<Image>().sprite == tagButtons_selectImage[1]) ||
		   (berryLockUnique.activeSelf == true && tagButtons[2].GetComponent<Image>().sprite == tagButtons_selectImage[2]))
		{
			swipeBtn[0].GetComponent<Button>().interactable = false;
			swipeBtn[1].GetComponent<Button>().interactable = false;
			return;
		}
		else
		{
			swipeBtn[0].GetComponent<Button>().interactable = true;
			swipeBtn[1].GetComponent<Button>().interactable = true;
		}




#if UNITY_EDITOR
		// 마우스 왼쪽 버튼을 눌렀을 때 1회
		if (Input.GetMouseButtonDown(0))
		{
			// 터치 시작 지점 (Swipe 방향 구분)
			startTouchX = Input.mousePosition.x;

		}
		else if (Input.GetMouseButtonUp(0))
		{
			// 터치 종료 지점 (Swipe 방향 구분)
			endTouchX = Input.mousePosition.x;


			UpdateSwipe();
		}
#endif

#if UNITY_ANDROID
		if (Input.touchCount == 1)
		{
			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Began)
			{
				// 터치 시작 지점 (Swipe 방향 구분)
				startTouchX = touch.position.x;

			}
			else if (touch.phase == TouchPhase.Ended)
			{
				// 터치 종료 지점 (Swipe 방향 구분)
				endTouchX = touch.position.x;


				UpdateSwipe();
			}
		}
#endif


	}

	private void UpdateSwipe()
	{

		// 너무 작은 거리를 움직였을 때는 Swipe 안된다.
		if (Mathf.Abs(startTouchX - endTouchX) < swipeDistance)
		{
			// 원래 페이지로 Swipe해서 돌아간다
			StartCoroutine(OnSwipeOneStep(currentPage));
			return;
			
		}



		// Swipe 방향
		bool isLeft = startTouchX < endTouchX ? true : false;

		// 이동 방향이 왼쪽일 때
		if (isLeft == true)
		{
			// 현재 페이지가 왼쪽 끝이면 종료
			if (currentPage == 0) return;

			// 왼쪽으로 이동을 위해 현재 페이지를 1 감소
			currentPage--;
		}
		// 이동 방향이 오른쪽일 떄
		else if (isLeft == false)
		{
			// 현재 페이지가 오른쪽 끝이면 종료
			if (currentPage == maxPage - 1) return;

			// 오른쪽으로 이동을 위해 현재 페이지를 1 증가
			currentPage++;
		}


		// currentIndex번째 페이지로 Swipe해서 이동
		StartCoroutine(OnSwipeOneStep(currentPage));

	}


	// 페이지를 한 장 옆으로 넘기는 Swipe 효과 재생
	private IEnumerator OnSwipeOneStep(int index)
	{
		float start = scrollBar.value;
		float current = 0;
		float percent = 0;

		isSwipeMode = true;

		while (percent < 1)
		{
			current += Time.deltaTime;
			percent = current / swipeTime;

			scrollBar.value = Mathf.Lerp(start, scrollPageValues[index], percent);

			yield return null;
		}

		isSwipeMode = false;


	}

	#endregion

	//스트롤바 위치 변경
	public void SetScrollBarValue(int index)
	{
		currentPage = index;
		scrollBar.value = scrollPageValues[index];
	}

	//좌우에 있는 세모 모양 스와이프 버튼
	public void swipeButton(string value)
	{
		switch (value)
		{
			case "left":
				// 현재 페이지가 왼쪽 끝이면 종료
				if (currentPage == 0)
					return;
				currentPage--;	// 왼쪽으로 이동 = 현재 페이지 1 감소
				SetScrollBarValue(currentPage);
				break;

			case "right":
				// 현재 페이지가 오른쪽 끝이면 종료
				if (currentPage == maxPage - 1)
					return;
				currentPage++;	// 오른쪽으로 이동 = 현재 페이지 1 증가
				SetScrollBarValue(currentPage);
				break;
		}
	}


	//버튼 누르는 효과
	public void TagImageChange(int index)
	{

		//버튼 스프라이트 다 안눌린거로
		for (int i = 0; i < tagButtons_Image.Length; i++)
		{
			tagButtons[i].GetComponent<Image>().sprite = tagButtons_Image[i];
		}

		//해당 버튼 스프라이트만 눌린거로
		tagButtons[index].GetComponent<Image>().sprite = tagButtons_selectImage[index];

		if (isBerry == true) 
		{
			SetScrollBarValue(0);
			startTouchX = 0;
			endTouchX = 0;
		}

	}
	//=================================================================================================
	//=================================================================================================

	//collection challenge News 창 변경
	public void TurnOn(GameObject obj)
	{
		for (int i = 0; i < content.Length; i++) { content[i].SetActive(false); } //challenge, news , content 다 비활성화
		obj.SetActive(true); //해당 창만 활성화
	}

    //=================================================================================================
    //=================================================================================================

    #region Berry Lock
	private bool ResearchLevelCheck(int level) 
	{
		for (int i = 0; i < DataController.instance.gameData.researchLevel.Length; i++)
		{
			if (DataController.instance.gameData.researchLevel[i] < level) 
			{ return false; }
		}
		return true;//모든 연구가 level이상이면 true를 반환한다.
	
	}

	//조건이 충족되어서 유니크, 스페셜 개발 가능
	public void BerryUnlockBtn() 
	{
		switch (DataController.instance.gameData.newBerryResearchAble) 
		{
			case 0:
				//현재 클래식만 개발가능
				DataController.instance.gameData.newBerryResearchAble++;
				berryLockSpecial.SetActive(false);
				break;
			case 1:
				//현재 클래식, 스페셜 개발가능
				DataController.instance.gameData.newBerryResearchAble++;
				berryLockUnique.SetActive(false);
				break;
		}
		GameManager.instance.NewBerryUpdate2();
		panelBlack.SetActive(false);
	}
    #endregion

    //=================================================================================================
    //=================================================================================================

    //[분류별 베리 띄우기]
    //버튼을 눌렀을 때 해당 분류의 딸기를 보인다
    public void selectBerryTag(string name)
	{
		
		//모든 베리들 보이게 활성화
		AllActive();

		//다른 베리들 안보이게 비활성화  /  Lock 걸기
		switch (name)
		{
			case "berry_classic":
				//LOCK
				berryLockSpecial.SetActive(false); 
				berryLockUnique.SetActive(false);
				//BERRY
				inActive("berry_special"); inActive("berry_unique"); 
				break;


			case "berry_special":
				//LOCK
				berryLockUnique.SetActive(false);
				LockSpecialBtn.GetComponent<Button>().interactable = false;
				switch (DataController.instance.gameData.newBerryResearchAble)
				{
					case 0: berryLockSpecial.SetActive(true); break;
					case 1:	berryLockSpecial.SetActive(false); break;
					case 2:	berryLockSpecial.SetActive(false); break;
				}
				//BERRY
				inActive("berry_classic"); inActive("berry_unique"); 
				break;


			case "berry_unique":
				//LOCK
				berryLockSpecial.SetActive(false);
				LockUniqueBtn.GetComponent<Button>().interactable = false;
				switch (DataController.instance.gameData.newBerryResearchAble)
				{
					case 0: berryLockUnique.SetActive(true);	break;
					case 1:	berryLockUnique.SetActive(true);	break;
					case 2:	berryLockUnique.SetActive(false);	break;
				}
				//BERRY
				inActive("berry_special"); inActive("berry_classic");
				break;
		}


		SetScrollBarValue(0);

	}


	public void inActive(string name)
	{
		int index = 0;
		switch (name)
		{
			case "berry_classic": index = 0; break;
			case "berry_special": index = 16; break;
			case "berry_unique": index = 32; break;
		}

		//비활성화
		for (int j = 0; j < content.Length; j++)
		{
			for (int i = index; i < index + 16; i++)
			{
				Transform trChild = content[j].transform.GetChild(i);
				trChild.gameObject.SetActive(false);
			}
		}

	}


	public void AllActive()
	{
		//모든 베리 오브젝트 활성화
		for (int j = 0; j < content.Length; j++)
		{
			for (int i = 0; i < content[j].transform.childCount; i++)
			{
				Transform trChild = content[j].transform.GetChild(i);
				trChild.gameObject.SetActive(true);
			}
		}
	}
}
