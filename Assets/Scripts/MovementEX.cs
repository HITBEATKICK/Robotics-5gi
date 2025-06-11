using System;
using UnityEngine;

// 큐브를 CylinderA -> CylinderB 로 이동시킨다.
// 속성: 물체의 속도, 시작점, 목적지
public class MovementEX : MonoBehaviour
{
    [SerializeField] private float speed;
    public float Speed { get; set; }
    public GameObject cylinderA;
    public GameObject cylinderB;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveAtoB(transform.gameObject, cylinderB);
    }

    private void MoveAtoB(GameObject start, GameObject end)
    {
        // A에서 B를 향하는 벡터 -> 단위벡터 (크기가 1인 ㄴ벡터) -> 플레이어에게 단위벡터를 더해줌
        Vector3 direction = end.transform.position - start.transform.position;
        Vector3 normalizedDir = direction.normalized;

        // 어디까지 갈 것인가? cylinderB 까지 -> 거리
        float distance = direction.magnitude;
        print(distance);

        if (distance < 0.1f)
            return;

        transform.position += normalizedDir * speed * Time.deltaTime;
    }
}
