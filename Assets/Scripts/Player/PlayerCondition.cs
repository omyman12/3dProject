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
        RunningStamina(10);
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
            yield return new WaitForSeconds(5);
            controller.moveSpeed = controller.curSpeed;
            isSpeedbuff = true;
        }
    }
    public void StartBuff(float amount)
    {
        StartCoroutine(Buff(amount));
    }

    public void Die()
    {
        //Debug.Log("ÇÃ·¹ÀÌ¾î°¡ Á×¾ú´Ù.");
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
    public void RunningStamina(float stamina) //¶Û¶§ ½ºÅÂ¹Ì³ª ´â´Â
    {
        if (controller.isRunning)
        {
            CharacterManager.Instance.Player.condition.UseStamina(stamina * Time.deltaTime);
            if (!CharacterManager.Instance.Player.condition.UseStamina(stamina * Time.deltaTime))
            {
                controller.isRunning = false;
            }
        }
    }
}