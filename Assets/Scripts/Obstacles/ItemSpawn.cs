using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    [SerializeField] private GameObject mushroom;
    [SerializeField] private Transform blockTransform;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 spawnPosition = blockTransform.position + Vector3.up * 1f;
            Instantiate(mushroom, spawnPosition, Quaternion.identity, blockTransform);
        }

    }
}
