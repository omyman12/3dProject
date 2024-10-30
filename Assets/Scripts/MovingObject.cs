using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public GameObject gameobject;
    private bool isMovingRight;
    public float moveSpeed = 1f;
    public float limitDistance = 2f;

    private void Update()
    {
        MoveObject();
    }

    void MoveObject()
    {
        // �̵� ���⿡ ���� ��ġ ����
        if (isMovingRight)
        {
            gameobject.transform.localPosition += Vector3.right * moveSpeed * Time.deltaTime;
            if (gameobject.transform.localPosition.x >= limitDistance)
            {
                isMovingRight = false; // ������ �Ѱ迡 ���� �� ���� ��ȯ
            }
        }
        else
        {
            gameobject.transform.localPosition += Vector3.left * moveSpeed * Time.deltaTime;
            if (gameobject.transform.localPosition.x <= 0)
            {
                isMovingRight = true; // ���� �Ѱ迡 ���� �� ���� ��ȯ
            }
        }
    }
}
