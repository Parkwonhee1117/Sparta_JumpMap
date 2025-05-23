using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = CharacterManager.Instance.Player.playerController.rigidbody;
    }

    void OnCollisionEnter(Collision collision)
    {
        _rigidbody.AddForce(transform.up.normalized * 300f, ForceMode.Impulse);
    }
}   
