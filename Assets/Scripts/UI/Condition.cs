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
        curValue = Mathf.Min(curValue + amount, maxValue); // 컬밸류 + 값/ 맥스밸류중 작은거(맥스 이상 안가게)
    }

    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f); //컬밸류 - 값 / 0 중 큰거 (0이하 안돼게)
    }
    public float GetPercentage()
    {
        return curValue / maxValue;
    }
}