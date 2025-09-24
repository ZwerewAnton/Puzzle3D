using System;
using Infrastructure;
using Infrastructure.SceneManagement;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Utils.BootstrapLoading
{
    public class BootstrapEditorSceneSwitcher : MonoBehaviour
    {
        [SerializeField] private Bootstrap bootstrap;
        private SceneSwitcher _sceneSwitcher;

        [Inject]
        private void Construct(SceneSwitcher sceneSwitcher)
        {
            _sceneSwitcher = sceneSwitcher;
        }
        
        private void Awake()
        {
            bootstrap.LoadingCompleted += LoadEditedScene;
        }
        
        private void OnDestroy()
        {
            bootstrap.LoadingCompleted -= LoadEditedScene;
        }

        private void LoadEditedScene()
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
            var sceneType = (SceneType)Enum.Parse(typeof(SceneType), startScene);
            _ = _sceneSwitcher.LoadSceneAsync(sceneType);
#endif
        }
    }
}