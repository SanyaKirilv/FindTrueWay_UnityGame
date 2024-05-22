using System.IO;
using UnityEngine;
using FTW.Data;

namespace FTW.Core
{
    public class SaveController : MonoBehaviour
    {
        [Header("Initial data json")]
        public TextAsset InitialData;

        private string FilePath => Path.Combine(Application.persistentDataPath, FileName);
        private string FileName => $"_data.json";
        private bool CheckForExists => File.Exists(FilePath);

        public void SaveData(Save save)
        {
            File.WriteAllText(FilePath, JsonUtility.ToJson(save));
        }

        public Save LoadData()
        {
            return CheckForExists ?
                JsonUtility.FromJson<Save>(File.ReadAllText(FilePath)) :
                JsonUtility.FromJson<Save>(InitialData.text);
        }

        [ContextMenu("Force Delete")]
        private void ForceDeleteFile()
        {
            File.Delete(FilePath);
        }
    }
}
