using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    [SerializeField] private bool isRunning = false;

    public UICondition uICondition;
    public event Action onTakeDamage;

    Condition Health {get {return uICondition.health;}}
    Condition Stamina {get {return uICondition.stamina;}}

    void Start()
    {
        StaminaEvent();
    }

    void Update()
    {
        Stamina.Add(Stamina.PassiveValue * Time.deltaTime);

        if (Health.CurValue <= 0f)
        {
            Die();
        }

        if (isRunning)
        {
            if(Stamina.CurValue > 0)
            {
                UseStamina(10f * Time.deltaTime);
            }
            else
            {
                isRunning = false;
            }
        }
    }

    public void Heal(float amount)
    {
        Health.Add(amount);
    }

    public void TakeDamage(float amount)
    {
        Health.Subtract(amount);
        onTakeDamage?.Invoke(); // onTakeDamage가 null이 아니면 실행
    }

    public void Die()
    {
        Debug.Log("죽었습니다");
    }

    void StaminaEvent()
    {
        CharacterManager.Instance.Player.playerController.onRunning += StartRunning;
        CharacterManager.Instance.Player.playerController.StopRunning += StopRunning;
    }

    // void OnDisable()
    // {
    //     CharacterManager.Instance.Player.playerController.onRunning -= StartRunning;
    //     CharacterManager.Instance.Player.playerController.StopRunning -= StopRunning;
    // }

    public void StartRunning()
    {
        isRunning = true;
    }

    public void StopRunning()
    {
        isRunning = false;
    }

    public void UseStamina(float amount)
    {
        Stamina.Subtract(amount);
    }

    


}
