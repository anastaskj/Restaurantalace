using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class XP : MonoBehaviour {
    [SerializeField]
    private int xp;
    private int currentLevel;
    private float fillAmount;
    private int xpnextlevel;
    private int differencexp;
    private int totaldifference;
    public UnityEvent OnValueChanged = new UnityEvent();

    public void Update()
    {
       // this.UpdateXp(5);
    }

    public void UpdateXp(int amount)
    {
        xp += amount;

        int curlvl = (int)(0.1f * Mathf.Sqrt(xp));

        if (curlvl != currentLevel)
        {
            currentLevel = curlvl;
            //Add animation to show leveling up
        }

        // Getting proper xp scaling for next level
        xpnextlevel = 100 * (currentLevel + 1) * (currentLevel + 1);
        // The xp difference between this level and next level (how much is left)
        differencexp = xpnextlevel - xp;
        //Difference between this level and next level (fixed)
        totaldifference = xpnextlevel - (100 * currentLevel * currentLevel);

        //differencexp / totaldifference -> how much the bar should be full
        fillAmount = 1 - (float)differencexp / (float)totaldifference;

        OnValueChanged.Invoke();
    }

    public void Save()
    {
        this.xp = 0;
        UpdateXp(5000000);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Open(Application.persistentDataPath + "/xpInfo.dat", FileMode.OpenOrCreate);

        XpInfo saveData = new XpInfo();
        saveData.xp = this.xp;

        bf.Serialize(fs, saveData);
        fs.Close();
        UpdateXp(-5000000);

        //Debug.Log(Application.persistentDataPath);
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/xpInfo.dat"))
        {
            Thread.Sleep(2000);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + "/xpInfo.dat", FileMode.Open);
            XpInfo loadData = (XpInfo)bf.Deserialize(fs);
            fs.Close();
            this.xp = 0;
            UpdateXp(loadData.xp);
        }
    }

    public double GetFillAmount()
    {
        return this.fillAmount;
    }

    [Serializable]
    class XpInfo {
        public int xp;
    }


    //Unused mapping function, might use later
    //private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    //{
    //    return (value - inMin) * (outMax - outMin) / (inMax - inMin) - outMin;
    //}
}
