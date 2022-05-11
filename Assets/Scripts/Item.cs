using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    upgrade,
    invincible,
    healHp,
    healGotong,
    bomb,
    healAll
}

public class Item : MonoBehaviour
{
    public ItemType type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<Player>();
            switch (type)
            {
                case ItemType.upgrade:
                    player.UpgradeWeapon();
                    Destroy(gameObject);
                    break;
                case ItemType.invincible:
                    player.InvinsibleByItem();
                    Destroy(gameObject);
                    break;
                case ItemType.healHp:
                    player.HealHp(10);
                    Destroy(gameObject);
                    break;
                case ItemType.healGotong:
                    player.HealGotong(10);
                    Destroy(gameObject);
                    break;
                case ItemType.bomb:
                    player.FireBomb();
                    Destroy(gameObject);
                    break;
                case ItemType.healAll:
                    player.HealHp(10);
                    player.HealGotong(10);
                    Destroy(gameObject);
                    break;
            }
            player.AddScore(100);
            player.manager.itemManager.spawnedItems.Remove(this);
        }
    }
}
