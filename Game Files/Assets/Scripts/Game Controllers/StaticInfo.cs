using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StaticInfo : MonoBehaviour {
    //infoForDataBase - Look LoadOutInfo.cs for more information
    public static LoadOutInfo[] infoForDataBase = new LoadOutInfo[5];
    [SerializeField] private Unit[] unitChoices = new Unit[3];
    public Unit[] unitsSelected = new Unit[5];

    public void Start()
    {
        Init();
    }



    public Unit[] LoadUnits()
    {
        if(!readFromFile())
        {
            SceneManager.LoadScene("Scenes/TitleScene");
        }
        for(int i = 0; i < 5; i++)
        {
            unitsSelected[i] = (Unit)Instantiate(unitChoices[(int)infoForDataBase[i].classSelected]);
            for(int j = 0; j < 3; j++)
            {
                unitsSelected[i].setAbility(infoForDataBase[i].skills[j], j);
                unitsSelected[i].getCharacter().GetComponent<Unit>().setAbility(infoForDataBase[i].skills[j], j);
                //Debug.Log(unitsSelected[i].getCharacter().GetComponent<Unit>().getAbility()[j].getName());
            }
        }
        return unitsSelected;
    }

    public bool readFromFile()
    {
        try
        {
            StreamReader fileIn = new StreamReader("PlayerData.txt");
            for (int i = 0; i < 5; i++)
            {
                string[] line = fileIn.ReadLine().Split(' ');
                infoForDataBase[i].classSelected = short.Parse(line[0]);
                infoForDataBase[i].skills[0] = short.Parse(line[1]);
                infoForDataBase[i].skills[1] = short.Parse(line[2]);
                infoForDataBase[i].skills[2] = short.Parse(line[3]);
            }
            fileIn.Close();
            return true;
        }
        catch (System.Exception e)
        {
            return false;
        }
    }

    public static void Init()
    {
        for(int i = 0; i < 5; i++)
        {
            infoForDataBase[i] = new LoadOutInfo();
        }
    }
}
