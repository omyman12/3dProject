using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float maxValue;
    public float startValue;
    public float passiveValue;
    public Image uiBar;

    private void Start()
    {
        curValue = startValue;
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue); // �ù�� + ��/ �ƽ������ ������(�ƽ� �̻� �Ȱ���)
    }

    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f); //�ù�� - �� / 0 �� ū�� (0���� �ȵŰ�)
    }
    public float GetPercentage()
    {
        return curValue / maxValue;
    }
}