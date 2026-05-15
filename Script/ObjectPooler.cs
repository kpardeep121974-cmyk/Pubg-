using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public string tag;           // पूल का नाम (जैसे: "Bullet", "BloodVFX")
    public GameObject prefab;    // जो ऑब्जेक्ट रीसाइकिल करना है
    public int size;             // शुरुआत में कितने ऑब्जेक्ट्स बनाकर रखने हैं
}

public class ObjectPooler : MonoBehaviour
{
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        // इसे Service Locator में रजिस्टर करें ताकि WeaponShooting इसे एक्सेस कर सके
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<ObjectPooler>(this);
        }

        InitializePools();
    }

    void InitializePools()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false); // शुरुआत में छुपा कर रखें
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
            Debug.Log($"[ObjectPooler] {pool.tag} पूल तैयार है। साइज़: {pool.size}");
        }
    }

    // इंस्टेंटिएट करने के बजाय इस मेथड को कॉल करें
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"[ObjectPooler] {tag} नाम का कोई पूल नहीं मिला!");
            return null;
        }

        // पूल में से सबसे पहला ऑब्जेक्ट निकालें
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        // ऑब्जेक्ट को वापस कतार (Queue) के पीछे लगा दें ताकि इसे दोबारा इस्तेमाल किया जा सके
        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
