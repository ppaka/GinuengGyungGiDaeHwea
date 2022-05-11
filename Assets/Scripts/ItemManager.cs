using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Item[] itemPrefabs;
    public List<Item> spawnedItems = new List<Item>();

    public void DestroyAll()
    {
        foreach (var i in spawnedItems)
        {
            Destroy(i.gameObject);
        }
        spawnedItems.Clear();
    }

    public void SpawnRandom(Vector3 position)
    {
        var c = Instantiate(itemPrefabs[Random.Range(0, itemPrefabs.Length)], position, Quaternion.identity);
        spawnedItems.Add(c);
    }
}