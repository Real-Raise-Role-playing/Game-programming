using UnityEngine;

using System.Collections;

using System.IO;

using System.Xml;

using System.Text;

using System;



public class ItemParse : MonoBehaviour

{



    // 파싱 할 xml 파일명

    string m_strName = "item.xml";



    // Use this for initialization

    void Start()

    {

        // 파싱 시작해요

        StartCoroutine(Process());

    }



    // Update is called once per frame

    void Update()

    {



    }



    IEnumerator Process()

    {

        string strPath = string.Empty;

        // 플랫폼 별로 다르게

#if ( UNITY_EDITOR || UNITY_STANDALONE_WIN )

        strPath += ("file:///");

        strPath += (Application.streamingAssetsPath + "/" + m_strName);

#elif UNITY_ANDROID

        strPath =  "jar:file://" + Application.dataPath + "!/assets/" + m_strName;

#endif



        WWW www = new WWW(strPath);



        yield return www;



        Debug.Log("Read Content : " + www.text);



        Interpret(www.text);

    }



    private void Interpret(string _strSource)

    {

        // 인코딩 문제 예외처리.

        // 읽은 데이터의 앞 2바이트 제거

        StringReader stringReader = new StringReader(_strSource);



        stringReader.Read();    // BOM 제거 한 데이터로 파싱.



        XmlNodeList xmlNodeList = null;



        XmlDocument xmlDoc = new XmlDocument();

        try

        {

            // XML 로드.

            xmlDoc.LoadXml(stringReader.ReadToEnd());

        }

        catch (Exception e)

        {

            xmlDoc.LoadXml(_strSource);

        }

        // 최 상위 노드 선택.

        xmlNodeList = xmlDoc.SelectNodes("Items");



        // 만들어 놓은 아이템 매니져에다.        

        foreach (XmlNode node in xmlNodeList)

        {

            // 자식이 있을 때에 돈다.

            if (node.Name.Equals("Items") && node.HasChildNodes)

            {

                foreach (XmlNode child in node.ChildNodes)

                {

                    ItemInfo item = new ItemInfo();



                    item.ID = int.Parse(child.Attributes.GetNamedItem("id").Value);



                    item.NAME = child.Attributes.GetNamedItem("name").Value;



                    item.ICON = child.Attributes.GetNamedItem("icon").Value;



                    item.WEIGHT = int.Parse(child.Attributes.GetNamedItem("weight").Value);



                    item.HP = int.Parse(child.Attributes.GetNamedItem("hp").Value);

                    item.HG = int.Parse(child.Attributes.GetNamedItem("hg").Value);

                    item.DELAY = int.Parse(child.Attributes.GetNamedItem("delay").Value);

                    item.TYPE = child.Attributes.GetNamedItem("type").Value;

                    item.DAMAGE = int.Parse(child.Attributes.GetNamedItem("damage").Value);

                    item.BULLET = int.Parse(child.Attributes.GetNamedItem("bullet").Value);

                    item.ONESHOT = int.Parse(child.Attributes.GetNamedItem("one_shot").Value);

                    // 매니저에 넣기.

                    ItemManager.INSTANCE.AddItem(item);

                }

            }

        }

    }

}