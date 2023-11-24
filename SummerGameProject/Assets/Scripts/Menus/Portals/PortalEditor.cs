using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using static UnityEngine.Animations.AimConstraint;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Portal))]
[CanEditMultipleObjects]

/*
    This script changes how the 'Portal' Component apears in the inspector.
    It adds functioning buttons to test the portal functionality while using the editor.
*/

public class PortalEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Portal portal = (Portal)target;

        portal.worldType = (WorldType)EditorGUILayout.EnumPopup("World Type", portal.worldType);
        EditorGUILayout.LabelField($"Portal ID: {portal.PortalID}");

        EditorGUILayout.Space();

        portal.firstUseActions = (FirstUseActions)EditorGUILayout.EnumPopup("First Use Action", portal.firstUseActions);

        switch (portal.firstUseActions)
        {
            case FirstUseActions.None: break;
            case FirstUseActions.TeleportPlayer:
                portal.FA_SceneName = EditorGUILayout.TextField("Scene Name", portal.FA_SceneName);
                portal.FA_WaypointName = EditorGUILayout.TextField("Waypoint Name", portal.FA_WaypointName);
                break;
            default: break;
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField($"Portal Save Data");
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Unlock Portal")) portal.UnlockPortal();
        if (GUILayout.Button("Lock Portal")) portal.LockPortal();
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Open Portal List")) 
        {
            Debug.LogWarning("This file will not update live while opened! Please close and reopen the file to see changes.");
            Application.OpenURL(portal.SaveDataPath);
        }
        
        if (GUILayout.Button("Clear List"))
        {
            if (EditorUtility.DisplayDialog("Are you sure?", "Clearing this data will remove all portals from the unlocked list. This action cannot be undone.", "Yes", "No"))
            {
                portal.ClearPortalData();
            }
        }


        if (GUI.changed && !Application.isPlaying)
        {
            EditorUtility.SetDirty(portal);
            EditorSceneManager.MarkSceneDirty(portal.gameObject.scene);
        }
    }
}
