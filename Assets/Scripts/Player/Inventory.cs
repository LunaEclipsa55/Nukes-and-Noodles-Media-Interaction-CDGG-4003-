using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public class Entry { public string name; public int amount; public Entry(string n, int a) { name = n; amount = a; } }

    public static Inventory Instance { get; private set; }

    [Header("Player References (for item effects)")]
    //public PlayerStats playerStats;
    //public Gun playerGun;

    [Header("Inventory Settings")]
    public KeyCode toggleInventoryKey = KeyCode.I;
    public bool hubKey;

    //Max inventory
    public int maxUnique = 4;

    [Header("UI Settings")]
    public bool showInventory;
    public bool showhud;
    public Rect invRect = new Rect(20, 20, 680, 560);
    public Rect quickRect = new Rect(20, Screen.height - 100, 520, 80);
    string popupMsg = "";
    float popupTimer;
    Vector2 scrollPos;

    //inventory data
    public LinkedList<Entry> items = new LinkedList<Entry>();          // unique, sorted by insert order
    public List<string> quickbar = new List<string> { "Empty", "Empty", "Empty", "Empty" };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        showInventory = false;
        showhud = true;
        //if (playerStats == null) playerStats = FindFirstObjectByType<PlayerStats>();
        //if (playerGun == null) playerGun = FindFirstObjectByType<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(toggleInventoryKey))
        {
            showInventory = !showInventory;

        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) Use(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Use(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Use(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) Use(3);

        if (popupTimer > 0) popupTimer -= Time.deltaTime;
    }

    LinkedListNode<Entry> FindNodeByName(string name)
    {
        for (var n = items.First; n != null; n = n.Next)
            if (n.Value.name == name) return n;
        return null;
    }

    public int UniqueCount => CountUnique();
    int CountUnique()
    {
        int c = 0;
        for (var n = items.First; n != null; n = n.Next) c++;
        return c;
    }

    public Entry GetAt(int index)
    {
        int i = 0;
        for (var n = items.First; n != null; n = n.Next, i++)
            if (i == index) return n.Value;
        return null;
    }

    public bool AddToInventory(int amount, string item)
    {
        if (amount <= 0 || string.IsNullOrEmpty(item)) return false;

        var node = FindNodeByName(item);
        if (node != null)
        {
            node.Value.amount += amount;
            return true;
        }

        //unique items
        if (UniqueCount >= maxUnique)
        {
            ShowPopup("Storage is full! Cannot take more items.");
            return false; // do NOT delete the world object; let pickup handle staying put
        }

        items.AddLast(new Entry(item, amount));
        return true;
    }

    public void RemoveFromInventory(int amount, string name)
    {
        if (amount <= 0 || string.IsNullOrEmpty(name)) return;

        var node = FindNodeByName(name);
        if (node == null) return;

        node.Value.amount -= amount;
        if (node.Value.amount <= 0)
        {
            items.Remove(node);

            // remove from quickbar if gone
            for (int i = 0; i < quickbar.Count; i++)
                if (quickbar[i] == name) quickbar[i] = "Empty";
        }
    }

    public int AmountInInventory(string itemName)
    {
        var node = FindNodeByName(itemName);
        return (node != null) ? node.Value.amount : 0;
    }

    void SetQuickItem(int slot, string name)
    {
        if (slot < 0 || slot >= 4) return;
        if (string.IsNullOrEmpty(name)) return;
        if (FindNodeByName(name) == null) return; // must exist in inventory
        quickbar[slot] = name;
    }

    void Use(int spot)
    {
        if (spot < 0 || spot >= 4) return;
        string n = quickbar[spot];
        if (n == "Empty") return;
        UseItem(n, 1);
    }

    void UseItem(string name, int num)
    {
        if (num <= 0) return;

        // Execute effect first; only consume if effect runs
        bool consumed = ApplyItemEffect(name);
        if (!consumed) return;

        RemoveFromInventory(num, name);
    }

    //shows what if inventory or health is full
    void ShowPopup(string msg)
    {
        popupMsg = msg;
        popupTimer = 3f;
    }

    bool ApplyItemEffect(string name)
    {
        //if (playerStats == null) playerStats = FindFirstObjectByType<PlayerStats>();
        //if (playerGun == null) playerGun = FindFirstObjectByType<Gun>();

        if (name == "Health10" || name == "Health20" || name == "Health30")
        {
            int amt = 0;
            if (name == "Health10") amt = 10;
            else if (name == "Health20") amt = 20;
            else if (name == "Health30") amt = 30;
            if (HealIfPossible(amt))
            {
                ShowPopup($"Healed {amt} HP");
                return true;
            }
            ShowPopup("Health is full");
            return false;
        }

        //no consume on equip, but if it fails (no gun) then don’t consume
        //if (name == "Ammo Red" || name == "Ammo Green" || name == "Ammo Yellow")
        //{
        //    if (playerGun)
        //    {
        //        playerGun.Equip(name);
        //        ShowPopup($"Equipped {name} ammo");
        //        return false;
        //    }
        //    return false;
        //}
        return false;
    }

    bool HealIfPossible(int amount)
    {
        //if (!playerStats) return false;
        //if (playerStats.health >= playerStats.healthMax) return false; // don’t waste it
        //playerStats.Heal(amount);
        return true;
    }

    private void OnGUI()
    {
        if (showhud)
        {
            //IMGUI
            if (popupTimer > 0)
            {
                //GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height - 140, 200, 30), popupMsg);
                var style = GUI.skin.box;
                style.wordWrap = true;

                float w = Mathf.Min(640f, Screen.width * 0.9f);
                float h = style.CalcHeight(new GUIContent(popupMsg), w);

                var rect = new Rect((Screen.width - w) * 0.5f, Screen.height - (h + 120f), w, h + 8f);
                GUI.Box(rect, popupMsg, style);
            }
        }

        //always visible
        DrawQuickbar();

        // ----- Inventory Window -----
        if (showInventory)
        {
            invRect = GUI.Window(100, invRect, InventoryGUI, "Inventory(Linked List)");
        }
    }

    void DrawQuickbar()
    {
        // ----- Quickbar (always visible) -----
        Rect quickRect = new Rect(20, Screen.height - 100, 520, 80);
        GUI.Box(quickRect, "Quick Items (1-4)");

        GUILayout.BeginArea(new Rect(quickRect.x + 8, quickRect.y + 20, quickRect.width - 16, quickRect.height - 28));
        GUILayout.BeginHorizontal();

        for (int i = 0; i < 4; i++)
        {
            GUILayout.BeginVertical(GUILayout.Width(120));

            string n = (i < quickbar.Count) ? quickbar[i] : "Empty";
            int amt = (n == "Empty") ? 0 : AmountInInventory(n); ;

            if (GUILayout.Button($"{i + 1}. {(n == "Empty" ? "(Empty)" : $"{n} x{amt}")}", GUILayout.Height(40)))
            {
                if (n != "Empty") UseItem(n, 1); // mouse click use
            }
            GUILayout.EndVertical();
        }

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    void InventoryGUI(int windowID)
    {
        int unique = UniqueCount;
        int rows = (unique <= 4) ? 4 : Mathf.Min(unique, maxUnique);

        GUILayout.Space(4);
        GUILayout.Label($"Unique Items: {unique} / {maxUnique}");

        //scroll view
        float scrollHeight = invRect.height - 60f;
        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(scrollHeight));

        for (int i = 0; i < rows; i++)
        {
            GUILayout.BeginHorizontal("box", GUILayout.Height(32));
            var e = GetAt(i);
            if (e != null)
            {
                GUILayout.Label(e.name, GUILayout.Width(180));
                GUILayout.Label($"x{e.amount}", GUILayout.Width(60));

                if (GUILayout.Button("Use", GUILayout.Width(50)))
                    UseItem(e.name, 1);

                GUILayout.Label("Assign:", GUILayout.Width(55));
                for (int s = 0; s < 4; s++)
                {
                    if (GUILayout.Button($"{s + 1}", GUILayout.Width(30)))
                        SetQuickItem(s, e.name);
                }
            }
            else
            {
                GUILayout.Label("(Empty)", GUILayout.Width(240));
                GUILayout.FlexibleSpace();
            }

            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }
}