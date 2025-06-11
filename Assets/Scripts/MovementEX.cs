using System;
using UnityEngine;

// ť�긦 CylinderA -> CylinderB �� �̵���Ų��.
// �Ӽ�: ��ü�� �ӵ�, ������, ������
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
        // A���� B�� ���ϴ� ���� -> �������� (ũ�Ⱑ 1�� ������) -> �÷��̾�� �������͸� ������
        Vector3 direction = end.transform.position - start.transform.position;
        Vector3 normalizedDir = direction.normalized;

        // ������ �� ���ΰ�? cylinderB ���� -> �Ÿ�
        float distance = direction.magnitude;
        print(distance);

        if (distance < 0.1f)
            return;

        transform.position += normalizedDir * speed * Time.deltaTime;
    }
}
