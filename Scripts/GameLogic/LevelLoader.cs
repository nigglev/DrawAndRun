using UnityEditor;
using UnityEngine;
public class LevelLoader : MonoBehaviour
{
    private void Awake()
    {
        LoadLevel(0);
    }



    public void LoadLevel(int in_level_index)
    {
        GameObject level = AdressablesManager.GetLevelByIndex(in_level_index);
        Instantiate(level);
        
    }
}
