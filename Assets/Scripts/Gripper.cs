using UnityEngine;

public class Gripper : MonoBehaviour
{
    public bool isObjectLocated = false;
    public Transform touchObj;
    public Transform touchObj2;
    Vector3 originPos;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Contains("�ݼ�") || other.tag.Contains("�ö�ƽ"))
        {
            touchObj = other.transform;
            isObjectLocated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("�ݼ�") || other.tag.Contains("�ö�ƽ"))
        {
            touchObj = null;
            isObjectLocated = false;
        }
    }
    
    // TeachData�� ���� ���� �� isGripperOn�� True��� �ε��� ��ü�� �ڽĿ�����Ʈ�� �����
    public void SetChild(bool isGripperOn)
    {
        if (touchObj == null && !isObjectLocated) return;

        if (isGripperOn)
        {
            touchObj.transform.SetParent(transform);
            touchObj.GetComponent<Rigidbody>().useGravity = false;
            touchObj.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            touchObj.GetComponent<Rigidbody>().useGravity = true;
            touchObj.GetComponent<Rigidbody>().isKinematic = false;
            touchObj.transform.SetParent(null);
        }
    }
}
