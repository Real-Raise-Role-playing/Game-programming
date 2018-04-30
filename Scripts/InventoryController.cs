using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 인벤토리 관리 클래스
/// </summary>
public class InventoryController : MonoBehaviour {
    //DB 연동 전 아이템 삽입
    private List<string> m_lItemNames = new List<string>();
    //아이템 목록
    private List<ItemScript> m_lItems = new List<ItemScript>();
    // 만든 SampleItem을 복사해서 만들기 위해 선언
    public GameObject m_gObjSampleItem;
    //grid 리셋 포지션
    public UIGrid m_grid;

    // Use this for initialization
    void Start () {
        InitItems();
    }

    // Update is called once per frame
    void Update () {
        // I 키를 누르면 아이템이 추가됩니다.

        if (Input.GetKeyDown(KeyCode.I))

        {

            AddItem();

        }

        // C키를 누르면 모두 삭제합니다.

        if (Input.GetKeyDown(KeyCode.C))

        {

        // ClearAll();

        }

        // R키를 눌르면 랜덤으로 하나가 삭제됩니다.

        if (Input.GetKeyDown(KeyCode.R))

        {

          //  ClearRand();

        }

    }
    private void InitItems()
    {
        //m_lItemNames.Add("AKM01");
        //m_lItemNames.Add("AKM02");
        //m_lItemNames.Add("AKM03");
        //m_lItemNames.Add("AKM04");
        //m_lItemNames.Add("AKM05");
        //m_lItemNames.Add("AKM06");

    }

    private void AddItem()
    {
        //새로 만들어서 그리드 자식으로
        GameObject gObjItem = NGUITools.AddChild(m_grid.gameObject, m_gObjSampleItem);
        //active 꺼져있는거 다시 키기
        gObjItem.SetActive(true);
        //이름 아이콘 설정 itemscript
        //오브젝트 컴포넌트 꺼내오기
        ItemScript itemScript = gObjItem.GetComponent<ItemScript>();
        //여기서 아이디 값으로 설정해서 아이템 가져와야함 18.04.30
       //    itemScript.SetInfo(ItemManager.INSTANCE.GetItem());
    }
}
