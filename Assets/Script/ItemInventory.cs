﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemInventory : MonoBehaviour
{
    Spell spellCaster;
    GameObject spellCreater;

    public float timer = 0;
    public float delay = 0.25f;

    public string spellName;

    private int currentWeaponIndex = 0;

    Dictionary<string, int> playerInventory;

    void Awake()
    {
        playerInventory = new Dictionary<string, int>();

        spellCreater = GameObject.FindGameObjectWithTag("script");
        spellCaster = spellCreater.GetComponent<Spell>();
    }

    public void AddItem(string name, int amount)
    {
        List<string> list = new List<string>(playerInventory.Keys);
        int count = 0;
        
        foreach (string obj in list)
        {
            count += playerInventory[obj];
        }
        
        if (count < 4)
        {
            if (playerInventory.ContainsKey(name))
            {
                playerInventory[name] += amount;
                spellName = name;
                string check = name.Substring(1);
                // original is 4
                if (check != "2")
                {
                    int num = int.Parse(name);
                    num += 1;
                    string newNum = num.ToString();
                    if (playerInventory.ContainsKey(newNum))
                    {
                        playerInventory[newNum] += 1;
                    }
                    else
                    {
                        playerInventory.Add(newNum, 1);
                    }
                    spellName = newNum;
                }
            }
            else
            {
                playerInventory.Add(name, amount);
                spellName = name;
            }

            if (playerInventory[name] < 0)
            {
                playerInventory[name] = 0;
            }
        }
        else
        {
            Debug.Log("You can't keep more");
        }
    }

    public void RemoveItem(string name)
    {
        if (playerInventory.ContainsKey(name))
        {
            playerInventory.Remove(name);
            spellName = "";
        }
    }

    public void RemoveItem(string name, int amount)
    {
        if (playerInventory.ContainsKey(name))
        {
            playerInventory[name] -= amount;

            if (playerInventory[name] == 0)
            {
                playerInventory.Remove(name);
            }
        }

        if (playerInventory[name] < 0)
        {
            playerInventory[name] = 0;
        }

        //spellName = "";
    }

    void Update()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        if (playerInventory.Count > 0)
        {
            if (scrollWheel > 0 && timer < 0)
            {
                timer = delay;

                ChangeWeapon(true);
            }
            else if (scrollWheel < 0 && timer < 0)
            {
                timer = delay;

                ChangeWeapon(false);
            }
        }
        

        if (Input.GetButton("Fire1"))
        {
            if (timer <= 0)
            {
                if (playerInventory.Count == 0)
                {
                    spellCaster.setPrefab(null);
                }
                else if (playerInventory.Count >= 1)
                {
                    spellCaster.setPrefab(spellName);
                }

                timer = delay;

                //spellCaster.Use();
            }
        }

        timer -= Time.deltaTime;
    }

    void ChangeWeapon(bool next)
    {
        List<string> weapons = new List<string>(this.playerInventory.Keys);
        string[] spellList = weapons.ToArray();
        
        if (next == true)
        {
            currentWeaponIndex++;
            
            if (currentWeaponIndex >= weapons.Count)
            {
                currentWeaponIndex = 0;
            }
        }
        else if (next == false)
        {
            currentWeaponIndex--;
            
            if (currentWeaponIndex < 0)
            {
                currentWeaponIndex = weapons.Count - 1;
            }
        }

        spellName = spellList[currentWeaponIndex];
    }

    public bool HasItem(string name)
    {
        if (playerInventory.ContainsKey(name))
        {
            if (playerInventory[name] > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
