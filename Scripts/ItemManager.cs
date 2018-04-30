using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemManager : MonoBehaviour {

    private static ItemManager m_plnstance;

    private static object m_pLock = new object();

    public static ItemManager INSTANCE
    {
        get
        {
            lock(m_pLock)
            {
                if(m_plnstance==null)
                {
                    m_plnstance = (ItemManager)FindObjectOfType(typeof(ItemManager));

                    if (FindObjectsOfType(typeof(ItemManager)).Length > 1)
                    {
                        return m_plnstance;
                    }
                    if(m_plnstance==null)
                    {
                        GameObject singleton = new GameObject();
                        m_plnstance = singleton.AddComponent<ItemManager>();
                        singleton.name = typeof(ItemManager).ToString();

                        DontDestroyOnLoad(singleton);
                    }
                }
            }

            return m_plnstance;
        }
    }

    //파싱 정보 저장
    Dictionary<int, ItemInfo> m_dicData = new Dictionary<int, ItemInfo>();
    //아이템 추가
    public void AddItem(ItemInfo _cInfo)
    {
        //아이템 고유값
        if (m_dicData.ContainsKey(_cInfo.ID)) return;

        //아이템 추가
        m_dicData.Add(_cInfo.ID, _cInfo);

    }
    //하나의 아이템 얻기

    public ItemInfo GetItem(int _nID)

    {

        // 있는지 체크 

        if (m_dicData.ContainsKey(_nID))

            return m_dicData[_nID];



        // 없으면 null 리턴

        return null;

    }
    // 전체 리스트 얻기

    public Dictionary<int, ItemInfo> GetAllItems()

    {

        return m_dicData;

    }

    // 전체 갯수 얻기

    public int GetItemsCount()

    {
        return m_dicData.Count;
    }

}
public class ItemInfo
{
    private int m_nID;
    private string m_strName;
    private string m_strIcon;
    private int m_weight;
    private int m_hp;
    private int m_hg;
    private int m_delay;
    private string m_strType;
    private int m_damage;
    private int m_bullet;
    private int m_one_shot;

    public int ID
    {
        set { m_nID = value; }
        get { return m_nID; }
    }
    public string NAME
    {
        set { m_strName = value; }
        get { return m_strName; }
    }

    public string ICON
    {
        set { m_strName = value; }
        get { return m_strName; }
    }

    public int WEIGHT
    {
        set { m_weight = value; }
        get { return m_weight; }
    }

    public int HP
    {
        set { m_hp = value; }
        get { return m_hp; }
    }

    public int HG
    {
        set { m_hg = value; }
        get { return m_hg; }
    }

    public int DELAY
    {
        set { m_delay = value; }
        get { return m_delay; }
    }

    public string TYPE
    {
        set { m_strType = value; }
        get { return m_strType; }
    }
    public int DAMAGE
    {
        set { m_damage = value; }
        get { return m_damage; }
    }

    public int BULLET
    {
        set { m_bullet = value; }
        get { return m_bullet; }
    }

    public int ONESHOT
    {
        set { m_one_shot = value; }
        get { return m_one_shot; }
    }

}

