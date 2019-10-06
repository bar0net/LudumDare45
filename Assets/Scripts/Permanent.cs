using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Permanent : MonoBehaviour
{
    [System.Serializable]
    public struct DeathData
    {
        public Vector3 position;
        public Quaternion rotation;

        
        public DeathData(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }
    }

    public List<DeathData> deathPositions = new List<DeathData>();

    void Awake()
    {
        Debug.Log("Awake");
        Permanent[] p = FindObjectsOfType<Permanent>();
        if (p.Length > 1)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void AddDeath(Transform value)
    {
        if (deathPositions.Count > 16) deathPositions.RemoveAt(0);
        deathPositions.Add(new DeathData(value.position, value.rotation) );
    }

}
