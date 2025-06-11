using System;
using UnityEngine;

// ť�긦 CylinderA -> CylinderB �� �̵���Ų��.
// �Ӽ�: ��ü�� �ӵ�, ������, ������
public class MovementEX2 : MonoBehaviour
{
    [SerializeField] private float speed;
    public float Speed { get; set; }
    public GameObject cylinderA;
    public GameObject cylinderB;
    public bool directionAB = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveAtoB(transform.gameObject, cylinderB, cylinderA);
    }

    private void MoveAtoB(GameObject Player, GameObject B, GameObject A)
    {
        if (directionAB == true)
        {
            Vector3 direction = B.transform.position - Player.transform.position;
            Vector3 normalizedDir = direction.normalized;

            float distance = direction.magnitude;

            if (distance < 0.1f)
            {
                directionAB = false;
            }

            transform.position += normalizedDir * speed * Time.deltaTime;
        }
        else
        {
            Vector3 direction = A.transform.position - Player.transform.position;
            Vector3 normalizedDir = direction.normalized;

            float distance = direction.magnitude;

            if (distance < 0.1f)
            {
                directionAB = true;
            }

            transform.position += normalizedDir * speed * Time.deltaTime;
        }

    }
}
