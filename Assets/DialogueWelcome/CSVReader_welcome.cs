using UnityEngine;

public class CSVReader_welcome : MonoBehaviour
{
    public TextAsset textAssetData;

    [System.Serializable]
    public class Dialogue
    {
        public string numsentence;
        public string sentence;
        public int label;
    }

    [System.Serializable]
    public class DialogueList
    {
        public Dialogue[] dialogue;
    }

    public DialogueList myDialogueList = new DialogueList();

    void Start()
    {
        ReadCSV();
    }

    void ReadCSV()
    {
        string[] data = textAssetData.text.Split(new string[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        int tableSize = data.Length;
        myDialogueList.dialogue = new Dialogue[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            string[] row = data[i].Split(new string[] { ";" }, System.StringSplitOptions.None);

            if (row.Length >= 3)
            {
                myDialogueList.dialogue[i] = new Dialogue();
                myDialogueList.dialogue[i].numsentence = row[0].Trim('"');
                
                // Parse "label" as an integer
                if (int.TryParse(row[2].Trim('"'), out int label))
                {
                    myDialogueList.dialogue[i].label = label;
                }
                else
                {
                    Debug.LogError($"Error parsing 'label' at row index {i}. Unable to convert to int.");
                    myDialogueList.dialogue[i].label = 0; // Set a default value or handle the error as needed
                }

                myDialogueList.dialogue[i].sentence = row[1].Trim('"');
            }
            else
            {
                Debug.LogError($"Error parsing row at index {i}. Expected at least 3 fields.");
                myDialogueList.dialogue[i] = null;
            }
        }
    }
}

