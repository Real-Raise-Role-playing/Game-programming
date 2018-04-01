using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour{

    private static T _instance;
    //T 클래스에 인스턴스 저장하고 

    private static bool appIsClosing = false;
    //bool을 통해 닫히고 있는지 체크
    /*
     앱 닫히는중이면 null 반환하고 아무것도 x -> 앱 닫힘
     T 컴포넌트가 씬에 없으면 새로 만들고 사용
     이미 존재한다면 새로 만들지 말고 존재하는거 사용해야함     
     */
    
    public static T Instance
    {
        get
        {
            if (appIsClosing)
            {
                //null 반환
                return null;
            }
            if (_instance == null)                // instance가 할당되지 않았으면

            {
                //인스턴스 있는지 확인
                _instance = (T)FindObjectOfType(typeof(T));
                
                if(_instance==null) //존재하지 않으면 만들자
                {
                    GameObject newSingleTon = new GameObject(); //T컴포넌트 추가

                    newSingleTon.name = typeof(T).ToString(); //이름 T 클래스꺼로 바꿔
                }
                DontDestroyOnLoad(_instance);
            }
            //인스턴스 리턴
            return _instance;
        }
    }

    public void Start()
    {
        T[] allInstances = FindObjectsOfType(typeof(T)) as T[];

        if(allInstances.Length>1)
        {
            foreach(T instanceToCheck in allInstances)
            {
                if(instanceToCheck!=Instance)
                {
                    //파괴
                    Destroy(instanceToCheck.gameObject);
                }
            }
        }
        //dontdestroyonload로 표기
        DontDestroyOnLoad((T)FindObjectOfType(typeof(T)));
    }

    void OnApplicationQuit() //어플리케이션 종료되면 null 반환시키기
    {
        appIsClosing = true;
    }
}
