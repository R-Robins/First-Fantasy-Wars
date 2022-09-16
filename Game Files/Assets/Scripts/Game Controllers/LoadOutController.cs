using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadOutController : MonoBehaviour {
    private Unit[] units = new Unit[5];
    private int currentSlot = 0;
    private int skillSlot = 0;
    private int chosenSkillSlot = 0;
    private Text[] Slots;
    private Text[] ChosenSkills;
    private Text[] ChosenSkillsDesc;
    private Text[] AvailableSkills;
    [SerializeField] private Unit[] availUnit = new Unit[3];

    void Start ()
    {
        StaticInfo.Init();
        Slots = new Text[5];
        AvailableSkills = new Text[5];
        ChosenSkillsDesc = new Text[5];
        ChosenSkills = new Text[3];
        //Make Slot[i], AvailableSkills[i], ChosenSkills[i] point to CharacterSlot, Skill, SkillSlot text component
        for(int i = 0; i < 5; i++)
        {
            Slots[i] = transform.parent.Find("CharacterSlot" + (i+1).ToString()).GetComponentInChildren<Text>();
            AvailableSkills[i] = transform.parent.Find("Skill" + (i + 1).ToString()).GetComponentInChildren<Text>();
        }
        for(int i = 0; i < 3; i++)
        {
            ChosenSkills[i] = transform.parent.Find("SkillSlot" + (i + 1).ToString()).Find("SpellName").GetComponent<Text>();
            ChosenSkillsDesc[i] = transform.parent.Find("SkillSlot" + (i + 1).ToString()).Find("Description").GetComponent<Text>();
        }
        readFromFile();
    }

	void Update () {
        
	}

    public void GoBackButton()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }

    public void ChangeModel(Unit Model)
    {
        if(units[currentSlot] != null)
        {
            DestroyObject(units[currentSlot].gameObject);
        }
        units[currentSlot] = (Unit)Instantiate(Model);
        Slots[currentSlot].text = units[currentSlot].getCharacterName();
        PopulateAvailableSkills(units[currentSlot].getAvailAbility(), units[currentSlot].getAbility());
        PopulateChosenSkills(units[currentSlot].getAbility());
        enableAvailSkills(units[currentSlot].getAbility());
        StaticInfo.infoForDataBase[currentSlot].classSelected = (short)Model.getUnitNumber();
    }

    public void CharSlot(int i)
    {
        currentSlot = i;
        if(units[currentSlot] != null)
        {
            PopulateAvailableSkills(units[currentSlot].getAvailAbility(), units[currentSlot].getAbility());
            PopulateChosenSkills(units[currentSlot].getAbility());
            enableAvailSkills(units[currentSlot].getAbility());
        }
        else
        {
            ClearAvailableSkills();
            ClearChosenSkills();
        }
    }

    public void SelectSkillSlot(int i)
    {
        skillSlot = i;
        Ability[] abilities = units[currentSlot].getAbility();
        Ability[] availAbilities = units[currentSlot].getAvailAbility();
        if(abilities[chosenSkillSlot] != null)
        {
            int previousSkillChosen = abilities[chosenSkillSlot].getSkillSlot();
            AvailableSkills[previousSkillChosen].GetComponentInParent<Button>().enabled = true;
        }
        units[currentSlot].setAbility(skillSlot, chosenSkillSlot);
        PopulateChosenSkills(abilities);
        PopulateAvailableSkills(availAbilities, abilities);
        StaticInfo.infoForDataBase[currentSlot].skills[chosenSkillSlot] = (short)skillSlot;
    }

    public void SelectChosenSkillSlot(int i)
    {
        chosenSkillSlot = i;
    }

    private void PopulateAvailableSkills(Ability[] avail, Ability[] chosen)
    {
        for(int i = 0; i < 5; i++)
        {
            AvailableSkills[i].text = avail[i].getName();
        }
        for(int i = 0; i < 3; i++)
        {
            if(chosen[i] != null)
            {
                AvailableSkills[chosen[i].getSkillSlot()].GetComponentInParent<Button>().enabled = false;
            }
        }
    }

    private void PopulateChosenSkills(Ability[] chosen)
    {
        for (int i = 0; i < 3; i++)
        {
            if (chosen[i] != null)
            {
                ChosenSkills[i].text = chosen[i].getName();
                ChosenSkillsDesc[i].text = chosen[i].getDescription();
            }
            else
            {
                ChosenSkills[i].text = "Empty";
                ChosenSkillsDesc[i].text = "";
            }
        }
    }

    private void enableAvailSkills(Ability[] chosen)
    {
        for (int i = 0; i < 5; i++)
        {
            AvailableSkills[i].GetComponentInParent<Button>().enabled = true;
        }
        for (int i = 0; i < 3; i++)
        {
            if (chosen[i] != null)
            {
                AvailableSkills[chosen[i].getSkillSlot()].GetComponentInParent<Button>().enabled = false;
            }
        }
    }

    private void ClearAvailableSkills()
    {
        for (int i = 0; i < 5; i++)
        {
            AvailableSkills[i].text = "";
            AvailableSkills[i].GetComponentInParent<Button>().enabled = true;
        }
    }

    private void ClearChosenSkills()
    {
        for (int i = 0; i < 3; i++)
        {
            ChosenSkills[i].text = "";
            ChosenSkillsDesc[i].text = "";
        }
    }

    public void Done()
    {
        if (!confirm())
        {
            Debug.Log("Something is not selected");
            return;
        }
        writeToFile();
        SceneManager.LoadScene("Scenes/MapOne");
    }

    public bool confirm()
    {
        foreach(Unit unit in units)
        {
            if(unit == null)
            {
                return false;
            }
            foreach(Ability ab in unit.getAbility())
            {
                if(ab == null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void writeToFile()
    {
        File.Delete("PlayerData.txt");
        StreamWriter fileOut = new StreamWriter("PlayerData.txt");
        fileOut.Flush();
        LoadOutInfo[] info = StaticInfo.infoForDataBase;
        for (int i = 0; i < info.Length; i++)
        {
            fileOut.WriteLine(info[i].classSelected.ToString() + " " + info[i].skills[0].ToString() + " " + info[i].skills[1].ToString() + " " + info[i].skills[2].ToString());
        }
        fileOut.Close();
    }

    public void readFromFile()
    {
        try
        {
            StreamReader fileIn = new StreamReader("PlayerData.txt");
            LoadOutInfo[] info = StaticInfo.infoForDataBase;
            for(int i = 0; i < 5; i++)
            {
                string[] line = fileIn.ReadLine().Split(' ');
                info[i].classSelected = short.Parse(line[0]);
                info[i].skills[0] = short.Parse(line[1]);
                info[i].skills[1] = short.Parse(line[2]);
                info[i].skills[2] = short.Parse(line[3]);
            }
            setUp();
            fileIn.Close();
        }
        catch(System.Exception e)
        {
            return;
        }
    }

    public void setUp()
    {
        LoadOutInfo[] info = StaticInfo.infoForDataBase;
        for (int i = 0; i < 5; i++)
        {
            units[i] = (Unit)Instantiate(availUnit[info[i].classSelected]);
            for (int j = 0; j < 3; j++)
            {
                units[i].setAbility(info[i].skills[j], j);
            }
            Slots[i].text = units[i].getCharacterName();
        }
    }
}
