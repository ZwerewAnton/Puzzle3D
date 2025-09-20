using Infrastructure;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils.BootstrapLoading
{
    public class BootstrapEditorSceneSwitcher : MonoBehaviour
    {
        [SerializeField] private Bootstrap bootstrap;
        
        private void Awake()
        {
            bootstrap.LoadingCompleted += LoadEditedScene;
        }
        
        private void OnDestroy()
        {
            bootstrap.LoadingCompleted -= LoadEditedScene;
        }

        private static void LoadEditedScene()
        {
#if UNITY_EDITOR
            var startScene = EditorPrefs.GetString(SceneConfigEditor.StartScenePrefsKey, null);
            switch (startScene)
            {
                case null:
                    startScene = SceneConfigEditor.MenuSceneName;
                    Debug.LogWarning("Couldn't find saved scene name");
                    break;
                case SceneConfigEditor.BootSceneName:
                    return;
            }
            SceneManager.LoadSceneAsync(startScene, LoadSceneMode.Single);
#endif
        }
    }
}