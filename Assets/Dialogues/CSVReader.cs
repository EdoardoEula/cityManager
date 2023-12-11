using System.Collections;
using UnityEngine;
using System.Text;


public class CSVReader : MonoBehaviour
{
    public TextAsset textAssetData;

    [System.Serializable]
    public class Dialogue
    {
        public string character;
        public string level;
        public string numsentence;
        public string sentence;
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
        int tableSize = data.Length - 1;
        myDialogueList.dialogue = new Dialogue[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            string[] row = data[i + 1].Split(new string[] { "," }, System.StringSplitOptions.None);

            if (row.Length >= 4)
            {
                myDialogueList.dialogue[i] = new Dialogue();
                myDialogueList.dialogue[i].character = row[0].Trim('"');
                myDialogueList.dialogue[i].level = row[1].Trim('"');
                myDialogueList.dialogue[i].numsentence = row[2].Trim('"');

                // Handling multiline strings enclosed in triple double quotes
                myDialogueList.dialogue[i].sentence = CombineMultilineStrings(row, 3).Replace("\"", "").Trim();
            }
            else
            {
                Debug.LogError($"Error parsing row at index {i + 1}. Expected at least 4 fields.");
                myDialogueList.dialogue[i] = null;
            }
        }
    }

    string CombineMultilineStrings(string[] row, int startIndex)
    {
        if (row.Length <= startIndex)
            return "";

        // Initialize a StringBuilder to efficiently concatenate strings
        StringBuilder combinedString = new StringBuilder(row[startIndex].Trim('"'));

        for (int i = startIndex + 1; i < row.Length; i++)
        {
            // Check if it's not the last field in the row before appending the comma
            if (i < row.Length - 1)
            {
                combinedString.Append(",");
            }

            // Append the current field, trimming any leading or trailing quotes
            combinedString.Append(row[i].Trim('"'));

            // Check if the multiline string is complete
            if (combinedString.ToString().EndsWith("\"\"\"") || combinedString.ToString().EndsWith("\""))
            {
                break;
            }
        }

        // Return the final combined string
        return combinedString.ToString();
    }
}
