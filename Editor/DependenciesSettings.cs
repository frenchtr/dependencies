using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace TravisRFrench.Dependencies.Editor
{
    public static class DependenciesSettings
    {
        private const string AssetsPath = "Assets/dependencies/Editor";
        private const string PackagesPath = "Packages/com.travisrfrench.dependencies/Editor";
        private const string VisualTreeAssetFilename = "DependenciesSettings.uxml";
        private const string StyleSheetFilename = "DependenciesSettings.uss";
        
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new("Project/Dependency Injection", SettingsScope.Project)
            {
                label = "Dependency Injection",
                activateHandler = (searchContext, rootElement) =>
                {
                    string editorPath;
                    
                    if (System.IO.Directory.Exists(PackagesPath) )
                    {
                        editorPath = PackagesPath;
                    }
                    else
                    {
                        editorPath = AssetsPath;
                    }
                    
                    var visualTreeAssetPath = $"{editorPath}/{VisualTreeAssetFilename}";
                    var styleSheetPath = $"{editorPath}/{StyleSheetFilename}";
                    
                    var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(visualTreeAssetPath);
                    var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(styleSheetPath);

                    var container = visualTreeAsset.CloneTree();
                    container.styleSheets.Add(styleSheet);

                    var settings = Runtime.Dependencies.Settings;
                    var serializedSettings = new SerializedObject(settings);
                    
                    container.Bind(serializedSettings);
                    
                    rootElement.Add(container);
                }
            };
        }
    }
}
