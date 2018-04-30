using System.Collections;
using UnityEngine;

/// <summary>
/// 모든 아이템이 갖고 있어야 할 스크립트.
/// </summary>

public class ItemScript : MonoBehaviour {
    //아이템 이름 구분
    private string m_strName;
    //아이템 아이콘 스프라이트 
    private string m_strSpriteName;
    //아이콘 표시 sprite 클래스
    public UISprite m_sprIcon;
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //아이템 정보 설정
    public void SetInfo(string spriteName)
    {
        //같은 아틀라스안에 아이템 sprite 찾아서 사용
        m_sprIcon.spriteName = spriteName;
        //이름 설정
        m_strName = spriteName;
    }

    void OnClick()
    {
        Debug.Log(m_strName + "클릭 됨 ");
    }
}
