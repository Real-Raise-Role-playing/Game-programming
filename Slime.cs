using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //Navigation을 위해 참조



public class Slime : MonoBehaviour {

    Transform player; // 타겟 : 주인공 
    NavMeshAgent agent; // 길잡이 

    Vector3 origPos; // * 초기 위치를 저장하는 변수

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").transform; 

        agent = GetComponent<NavMeshAgent>();
        // 컴포넌트 찾기

    }
	
	// Update is called once per frame
	void Update () {
        // agent.destination = player.position;

        Debug.DrawRay(transform.position, transform.forward * 15, Color.green);

        float dist = Vector3.Distance(transform.position, player.position); //사이의 거리를 계산.

        if(dist < 30) 
        {
            agent.stoppingDistance = 1; // 타겟과 얼마가 가까이 붙을지
            agent.destination = player.position; // 타겟을 향해 이동
            // 길잡이가 타겟을 향해 이동한다.

        }
        else
        {
            agent.stoppingDistance = 0;
            agent.destination = origPos; // 원래 자리로 이동
        }
	}

    
}
