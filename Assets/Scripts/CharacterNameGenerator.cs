using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data object that can generate a number of dancer names
/// 
/// TODO:
///     Needs to generate the number of requested <b>unique</b> character names for the dancers in our battle
/// </summary>
[CreateAssetMenu(menuName = "Battle Objects/Character Name Generator")]
[System.Serializable]

public class CharacterNameGenerator : ScriptableObject
{
    [Header("Possible first names")]
    [SerializeField]
    public List<string> firstNames;
    [Header("Possible last names")]
    [SerializeField]
    public List<string> lastNames;
    [Header("Possible nicknames")]
    [SerializeField]
    public List<string> nicknames;
    [Header("Possible adjectives to describe the character")]
    [SerializeField]
    public List<string> descriptors;
    //this code basically has a list of names for each category which it then goes into and chooses a random one then assigns it to a character
    public CharacterName[] GenerateNames(int namesNeeded)
    {
        CharacterName[] names = new CharacterName[namesNeeded];

        //TODO - filling this with empty names so the rest of our code is safe to run without need for many null checks

        for (int i = 0; i < names.Length; i++)
        {
            int randomFirstNameNumber = Random.Range(0, firstNames.Count);
            int randomLastNameNumber = Random.Range(0, lastNames.Count);
            int randomDescriptorNameNumber = Random.Range(0, descriptors.Count);
            int randomNickNameNumber = Random.Range(0, nicknames.Count);

            CharacterName Lebron = new CharacterName(firstNames[randomFirstNameNumber], lastNames[randomLastNameNumber], nicknames[randomNickNameNumber], descriptors[randomDescriptorNameNumber]);
            names[i] = Lebron;

        }
        //this code is a formula which the code uses to choose between the names in the category to assign randomly. it then shows you how its done ^
        // Debug.LogWarning("CharacterNameGenerator called, it needs to fill out the names array with unique randomly constructed character names");

        return names;
    }
}