using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DBManager : MonoBehaviour
{
    public Text consoleText;

    public void CreateNewDB()
    {
        StartSync();
    }

    void StartSync()
    {
        DataService ds = new DataService("newTempDatabase.db");
        ds.CreateDB();
        IEnumerable<DataEntry> datas = ds.GetPersons();
        ClearConsole();
        ToConsole("Dropped table and created new");
        ToConsole(datas);
    }

    public void AddNewEntry()
    {
        DataService ds = new DataService("newTempDatabase.db");
        DataEntry newPerson = ds.CreateDataEntry();
        ToConsole("Adding new entry...");
        ToConsole(newPerson.ToString());
    }

    private void ToConsole(IEnumerable<DataEntry> people)
    {
        foreach (DataEntry person in people) ToConsole(person.ToString());
    }

    private void ToConsole(string msg)
    {
        string doAddBreak = (consoleText.text != "") ? Environment.NewLine : "";
        this.consoleText.text += doAddBreak + msg;
        Debug.Log(msg);
    }

    private void ClearConsole()
    {
        this.consoleText.text = "";
    }
}
