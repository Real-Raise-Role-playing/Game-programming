using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TerrainDistance : MonoBehaviour
{

    public float distance = 250; // 250은 터레인 기본으로 제한해둔 최댓값
    Terrain terrain;

    void Start()
    {
        terrain = GetComponent<Terrain>();
        if (terrain == null)
        {
            Debug.LogError("This gameobject is not terrain, disabling forced details distance", gameObject); //터레인이 아닌 다른 오브젝트에 스크립트가 붙었을 때.
            this.enabled = false;
            return;
        }

    }

    void Update()
    {
        terrain.detailObjectDistance = distance;
    }
}