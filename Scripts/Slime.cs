using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //Navigation을 위해 참조

public class Slime : MonoBehaviour
{
    public static Slime instance;
    private PhotonView pv = null;
    //Transform player; // 타겟 : 주인공 
    public bool Attecking = false;
    public List<Transform> playerTrs = new List<Transform>();
    NavMeshAgent agent; // 길잡이 
    public int targetNum = 0;
    Vector3 origPos; // * 초기 위치를 저장하는 변수
    public float dist = 0.0f; //플레이어와의 거리

    void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        pv = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        foreach (GameObject player in PhotonManager.instance.playerObjList)
        {
            playerTrs.Add(player.transform);
        }

        agent = GetComponent<NavMeshAgent>();
        // 컴포넌트 찾기
        origPos = new Vector3(1527.3f, 0, 2247.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // agent.destination = player.position;

        Debug.DrawRay(transform.position, transform.forward * 15, Color.green);

        for (int i = 0; i < playerTrs.Count; i++)
        {
            if (Attecking)
            {
                break;
            }
            dist = Vector3.Distance(transform.position, playerTrs[i].position); //사이의 거리를 계산.
            if (dist < 30)
            {
                Attecking = true;
                targetNum = i;
                break;
            }
        }

        if (Attecking)
        {
            dist = Vector3.Distance(transform.position, playerTrs[targetNum].position);
            if (dist < 30 )
            {
                agent.stoppingDistance = 1; // 타겟과 얼마가 가까이 붙을지
                agent.destination = playerTrs[targetNum].position; // 타겟을 향해 이동
                                                           // 길잡이가 타겟을 향해 이동한다.
            }
            else
            {
                agent.stoppingDistance = 0;
                agent.destination = origPos; // 원래 자리로 이동
                targetNum = 0;
                Attecking = false;
            }
        }

        //float dist = Vector3.Distance(transform.position, player.position); //사이의 거리를 계산.
        //if (dist < 30)
        //{
        //    agent.stoppingDistance = 1; // 타겟과 얼마가 가까이 붙을지
        //    agent.destination = player.position; // 타겟을 향해 이동
        //    // 길잡이가 타겟을 향해 이동한다.

        //}
        //else
        //{
        //    agent.stoppingDistance = 0;
        //    agent.destination = origPos; // 원래 자리로 이동
        //}
    }


}
