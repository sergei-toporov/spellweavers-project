using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "new_scenes_list", menuName = "Custom Assets/Scenes Management/Scenes list", order = 53)]
public class ScenesListSO : ScriptableObject
{
    [SerializeField] protected GenericDictionary<string, string> scenesList = new GenericDictionary<string, string>();
    public GenericDictionary<string, string> ScenesList { get => scenesList; }

    public string GetSceneNameByKey(string key)
    {
        if (scenesList.TryGetValue(key, out string sceneName))
        {
            return sceneName;
        }

        return null;
    }
}
