using System;
using System.Collections;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition;
    PlayerController controller;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecay;
    public event Action onTakeDamage;
    bool isSpeedbuff = true;
    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
    }

    private void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (hunger.curValue == 0.0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if (health.curValue == 0.0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }
    public IEnumerator Buff(float amount)
    {
        Debug.Log("buff");
        if (isSpeedbuff)
        {
            controller.curSpeed = controller.moveSpeed;
            controller.moveSpeed *= amount;
            isSpeedbuff = false;
            Debug.Log("1");
            yield return new WaitForSeconds(5);
            controller.moveSpeed = controller.curSpeed;
            Debug.Log("2");
            isSpeedbuff = true;
            Debug.Log(isSpeedbuff);
        }
    }
    public void StartBuff(float amount)
    {
        StartCoroutine(Buff(amount));
    }

    public void Die()
    {
        //Debug.Log("플레이어가 죽었다.");
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        health.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }
    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0)
        {
            return false;
        }
        stamina.Subtract(amount);
        return true;
    }
}