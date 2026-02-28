using NUnit;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Abilities : MonoBehaviour
{
    [Header("Ammo Prefabs")]
    public Bullets red;
    public Bullets yellow;
    public Bullets purple;
    //public AmmoEntry[] extra;

    [Header("Firing")]
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float fireCool = 0.2f;
    public bool enableMouse = true;

    public bool aimAtMouse = false;

    float nextFireTime;
    static int lastShotFrame = -1;

    Dictionary<string, Bullets> ammoMap = new();
    string currentAmmo;
    Bullets currentBullet;

    void Awake()
    {
        if (red) ammoMap["Ammo"] = red;
        if (yellow) ammoMap["Ammo Yellow"] = yellow;
        if (purple) ammoMap["Ammo Green"] = purple;

        //if (extra != null)
        //    foreach (var e in extra)
        //        if (!string.IsNullOrEmpty(e.name) && e.prefab)
        //            ammoMap[e.name] = e.prefab;
    }

    // Update is called once per frame
    void Update()
    {
        if (enableMouse && Input.GetMouseButtonDown(1))
            TryFire();
    }

    Bullets TryAutoLoad(string ammoName)
    {
        var pb = Resources.Load<Bullets>($"Projectiles/{ammoName}");
        if (pb) ammoMap[ammoName] = pb;
        return pb;
    }

    public void Equip(string ammoName)
    {
        if (!ammoMap.TryGetValue(ammoName, out var pb) || !pb)
            pb = TryAutoLoad(ammoName);

        if (!pb)
        {
            Debug.LogError($"[Gun2D] Equip failed for '{ammoName}'.");
            return;
        }

        currentAmmo = ammoName;
        currentBullet = pb;
    }

    public bool TryFire()
    {
        Debug.Log("Trying to fire...");
        if (!currentBullet || string.IsNullOrEmpty(currentAmmo)) return false;
        if (!firePoint) return false;
        if (Time.time < nextFireTime) return false;
        if (Time.frameCount == lastShotFrame) return false;

        var inv = Inventory.Instance;
        if (!inv || inv.AmountInInventory(currentAmmo) <= 0) return false;

        // Direction (2D)
        Vector2 dir = firePoint.right; // default for 2D (sprite facing right)
        if (aimAtMouse && Camera.main)
        {
            Vector3 mw = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mw.z = 0f;
            dir = ((Vector2)(mw - firePoint.position)).normalized;

            // optional: rotate bullet to face direction
            //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            //firePoint.rotation = Quaternion.Euler(0, 0, angle);
        }

        // spawn
        var bullet = Instantiate(currentBullet, firePoint.position, firePoint.rotation);

        // Ignore self collisions (2D)
        var bCol = bullet.GetComponent<Collider2D>();
        if (bCol)
        {
            foreach (var c in GetComponentsInChildren<Collider2D>())
                Physics2D.IgnoreCollision(bCol, c, true);
        }

        // launch (2D): pass velocity vector
        bullet.Launch(dir * bulletSpeed);

        inv.RemoveFromInventory(1, currentAmmo);

        lastShotFrame = Time.frameCount;
        nextFireTime = Time.time + fireCool;
        return true;
    }
}
