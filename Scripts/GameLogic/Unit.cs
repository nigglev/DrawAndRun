using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public delegate void UnitDeathHandler(ulong in_id);
    public delegate void UnitSpawnHandler(Vector3 in_spawn_pos);
    public event UnitDeathHandler OnDeath;
    public event UnitSpawnHandler OnSpawn;
    private ulong _id; public ulong ID => _id;

    internal void SetID(ulong in_id)
    {
        _id = in_id;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (string.Equals(other.tag, "Mine", StringComparison.Ordinal))
        {
            OnDeath?.Invoke(_id);
            Destroy(other.gameObject);
        }


        if (other.tag == "PointsRomb")
        {
            Debug.Log($"Object with id {_id} collided with romb");
            Destroy(other.gameObject);
        }

    }

}
