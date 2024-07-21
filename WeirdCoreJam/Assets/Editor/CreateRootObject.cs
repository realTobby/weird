using UnityEditor;
using UnityEngine;

public class CreateRootObject : EditorWindow
{
    [MenuItem("Tools/Create Root Object")]
    static void CreateWindow()
    {
        // Show existing window instance. If one doesn't exist, create one.
        var window = GetWindow<CreateRootObject>();
        window.titleContent = new GUIContent("Create Root Object");
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Select objects in Hierarchy and click 'Create Root Object'");

        if (GUILayout.Button("Create Root Object"))
        {
            // Get selected objects
            GameObject[] selectedObjects = Selection.gameObjects;

            if (selectedObjects.Length > 0)
            {
                // Create an empty GameObject as the root
                GameObject rootObject = new GameObject("RootObject");

                // Parent selected objects under the root object
                foreach (GameObject obj in selectedObjects)
                {
                    obj.transform.SetParent(rootObject.transform, true);
                }

                // Select the root object in the Hierarchy
                Selection.activeGameObject = rootObject;

                Debug.Log("Created Root Object and reparented selected objects.");
            }
            else
            {
                Debug.LogWarning("No objects selected. Please select objects in the Hierarchy.");
            }
        }
    }
}
