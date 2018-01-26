using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RaderObject
{
    public Image icon { get; set; }
    public GameObject owner { get; set; }
}

public class Rader : MonoBehaviour {

    public Transform playerPos;
    float mapScale = 2.0f;

    public static List<RaderObject> radObjects = new List<RaderObject>();

    public static void RegisterRaderObject(GameObject o, Image i)
    {
        Image image = Instantiate(i);
        radObjects.Add(new RaderObject() { owner = o, icon = image });
    }
    public static void RemoveRaderObject(GameObject o)
    {
        List<RaderObject> newList = new List<RaderObject>();
        for(int i=0;i<radObjects.Count;i++)
        {
            if (radObjects[i].owner == o)
            {
                Destroy(radObjects[i].icon);
                continue;
            }
            else
                newList.Add(radObjects[i]);
        }

        radObjects.RemoveRange(0, radObjects.Count);
        radObjects.AddRange(newList);
    }

    void DrawRaderDots()
    {
        foreach(RaderObject ro in radObjects)
        {
            Vector3 radarPos = (ro.owner.transform.position - playerPos.position);
            float distToObject = Vector3.Distance(playerPos.position, ro.owner.transform.position) * mapScale;
            float deltay = Mathf.Atan2(radarPos.x, radarPos.z) * Mathf.Rad2Deg - 270 - playerPos.eulerAngles.y;
            radarPos.x = distToObject * Mathf.Cos(deltay * Mathf.Deg2Rad) * -1;
            radarPos.z = distToObject * Mathf.Sin(deltay * Mathf.Deg2Rad);

            ro.icon.transform.SetParent(this.transform);
            ro.icon.transform.position = new Vector3(radarPos.x, radarPos.z, 0) + this.transform.position;
        }
    }
	
	void Update () {
        DrawRaderDots();
	}
}
