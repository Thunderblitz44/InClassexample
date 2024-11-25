using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
public class PluginTester : MonoBehaviour
{
    public TextMeshProUGUI uiText;

    private Dictionary<string, Dictionary<string, string>> languages;


    public int JumpHeight = 1;
    public int MovementSpeed = 1;
    void Start()
    {
        Debug.Log("PluginTester has started!");
       
        LoadLanguagesFromCSV();
   
        ChangeLanguage("en");
    }
  
    void LoadLanguagesFromCSV()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Presets.csv");
        Debug.Log("Attempting to load CSV from: " + filePath);
        if (File.Exists(filePath))
        {
            try
            {
             
                string[] lines = File.ReadAllLines(filePath);
                Debug.Log("CSV file loaded. Number of lines: " + lines.Length);
              
                string[] headers = lines[0].Split(',');
                Debug.Log("Language codes found: " + string.Join(", ", headers.Skip(1)));
                                                                                          
                languages = new Dictionary<string, Dictionary<string, string>>();
          
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] columns = lines[i].Split(',');
                    string key = columns[0].Trim(); 
                    Debug.Log("Found key: " + key);
                    
             
for (int j = 1; j < headers.Length; j++)
                    {
                        string languageCode = headers[j].Trim();
                        string translation = columns[j].Trim(); 
                        if (!languages.ContainsKey(languageCode))
                        {
                            languages[languageCode] = new Dictionary<string, string>();
                        }
                        languages[languageCode][key] = translation;
                        Debug.Log("Added translation: " + languageCode + " -> " + key + " = " + translation);
                    }
                }
                Debug.Log("CSV file loaded and parsed successfully.");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error loading or parsing CSV file: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("CSV file not found at: " + filePath);
        }
    }
    void ChangeLanguage(string languageCode)
    {
        if (languages == null)
        {
            Debug.LogError("Languages dictionary is null! CSV might not have been loaded correctly.");
            return;
        }
        if (!languages.ContainsKey(languageCode))
        {
            Debug.LogWarning("Language code not found: " + languageCode);
            return;
        }
        if (uiText == null)
        {
            Debug.LogError("UI Text reference is missing! Please assign the UI Text in the inspector.");
            return;
        }
        
  
if (languages[languageCode].ContainsKey("greeting"))
        {
            uiText.text = languages[languageCode]["greeting"];
            Debug.Log("Language changed to: " + languageCode + " with text: " + uiText.text);

            ChangeStats(uiText.text);
        }
        else
        {
            Debug.LogWarning("Key 'greeting' not found for language: " + languageCode);
        }
    }
    public void OnLanguageChangeButtonClicked(string newLanguageCode)
    {
        ChangeLanguage(newLanguageCode);
    }


    public void ChangeStats(string PresetToHandle)
    {
        switch (PresetToHandle)
        {
            case "med":
                JumpHeight = 5;
                MovementSpeed = 5;
                break;

            case "high":
                JumpHeight = 10;
                MovementSpeed = 10;
                break;

            case "low":
                JumpHeight = 1;
                MovementSpeed = 1;
                break;


            default:
                Debug.LogWarning("Preset not recognized: " + PresetToHandle);
                break;
        }
    }
}