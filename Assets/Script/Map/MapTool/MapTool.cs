using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using static UnityEngine.Rendering.DebugUI;
using System.Xml.Linq;


public class MapTool : EditorWindow
{
    Dictionary<string, List<GameObject>> categorizedPrefabs;
    Dictionary<string, bool> foldoutStates;
    GameObject selectedPrefab;
    Vector2 scroll;

    [MenuItem("Window/MapTool")]
    public static void ShowWindow()
    {
        GetWindow<MapTool>("Map Tool");
    }

    void OnEnable()
    {
        LoadPrefabs();
        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    void LoadPrefabs()
    {
        categorizedPrefabs = new Dictionary<string, List<GameObject>>();
        foldoutStates = new Dictionary<string, bool>();

        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefab/Map" });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            string category = GetCategoryFromPath(prefab);
            if (!categorizedPrefabs.ContainsKey(category))
            {
                categorizedPrefabs[category] = new List<GameObject>();
                foldoutStates[category] = true;
            }

            categorizedPrefabs[category].Add(prefab);
        }
    }

    void OnGUI()
    {
        if (categorizedPrefabs == null || categorizedPrefabs.Count == 0)
        {
            EditorGUILayout.HelpBox("No prefabs found in Assets/Prefab/Map", MessageType.Info);
            return;
        }

        EditorGUILayout.LabelField("Map Prefabs", EditorStyles.boldLabel);
        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach (var kvp in categorizedPrefabs)
        {
            string category = kvp.Key;
            List<GameObject> prefabs = kvp.Value;

            foldoutStates[category] = EditorGUILayout.Foldout(foldoutStates[category], category, true);
            if (foldoutStates[category])
            {
                EditorGUI.indentLevel++;
                foreach (GameObject prefab in prefabs)
                {
                    if (prefab == null) continue;

                    EditorGUILayout.BeginHorizontal();

                    string label = selectedPrefab == prefab ? $"▶ {prefab.name}" : prefab.name;
                    if (GUILayout.Button(label))
                    {
                        selectedPrefab = prefab;
                    }

                    // 프리팹 위치로 이동 버튼
                    if (GUILayout.Button("E", GUILayout.Width(30)))
                    {
                        EditorGUIUtility.PingObject(prefab);
                        Selection.activeObject = prefab;
                    }

                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUILayout.Space(15);
        if (GUILayout.Button("맵 오브젝트 ID 일괄 설정"))
        {
            AssignIDs();
        }

        EditorGUILayout.Space();
        GUI.enabled = selectedPrefab != null;
        if (GUILayout.Button("선택 해제"))
        {
            selectedPrefab = null;
        }
        GUI.enabled = true;
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (selectedPrefab == null) return;

        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        Plane plane = new Plane(Vector3.forward, 0);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 point = ray.origin + ray.direction * distance;
            Vector3 gridPoint = SnapToGrid(point, 1f);

            Handles.color = Color.green;
            Handles.DrawWireCube(gridPoint, Vector3.one);

            if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
            {
                GameObject placed = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);
                Undo.RegisterCreatedObjectUndo(placed, "Place Map Element");
                placed.transform.position = gridPoint;

                string category = GetCategoryFromPath(selectedPrefab);
                Transform parent = EnsureHierarchy(category);
                placed.transform.SetParent(parent);

                // 설치한 프리팹 선택 상태로 유지
                Selection.activeGameObject = placed;

                selectedPrefab = null;
                e.Use();
            }

            SceneView.RepaintAll();
        }
    }




    #region 맵의 다양한 오브젝트 ID 자동 부여 및 설정, 저장까지!
    void AssignIDs()
    {
        /*
        SavePoint_list spList = AssetDatabase.LoadAssetAtPath<SavePoint_list>(SavePointListPath);
        if (spList == null)
        {
            Debug.LogError("SavePoint_list ScriptableObject를 찾을 수 없습니다.");
            return;
        }

        // Dictionary 초기화
        spList.SavePoint_IDlist.Clear();
        */


        int mainID = 2001;
        int semiID = 1001;
        int totalMain = 0;
        int totalSemi = 0;

        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes/Map" });

        foreach (string guid in sceneGuids)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guid);
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

            SavePoint[] savePoints = GameObject.FindObjectsByType<SavePoint>(FindObjectsSortMode.None);

            foreach (var sp in savePoints)
            {
                if (sp == null) continue;

                int assignedID = -1;

                switch (sp.SavePoint_type)
                {
                    case SavePoint.SP_type.Main:
                        assignedID = mainID++;
                        totalMain++;
                        break;
                    case SavePoint.SP_type.Semi:
                        assignedID = semiID++;
                        totalSemi++;
                        break;
                }

                if (assignedID != -1)
                {
                    sp.ID = assignedID;
                    //spList.SavePoint_IDlist[assignedID] = sp;
                    EditorUtility.SetDirty(sp);

                }
            }

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
        }

        //EditorUtility.SetDirty(spList);
        AssetDatabase.SaveAssets();

        Debug.Log($"SavePoint ID 부여 완료! Main: {totalMain}, Semi: {totalSemi}");

        //DumpSavePointIDsToJson();
    }
    /*
    void DumpSavePointIDsToJson()
    {
        SavePoint_list spList = AssetDatabase.LoadAssetAtPath<SavePoint_list>(SavePointListPath);

        if (spList == null)
        {
            Debug.LogError("SavePoint_list ScriptableObject를 불러오지 못했습니다.");
            return;
        }

        if (spList.SavePoint_IDlist == null || spList.SavePoint_IDlist.Count == 0)
        {
            Debug.LogWarning("SavePoint_IDlist가 비어 있습니다.");
            return;
        }

        var entries = new List<SavePointEntry>();
        Debug.Log(spList.SavePoint_IDlist[1001].name);
        foreach (var kv in spList.SavePoint_IDlist)
        {
            int id = kv.Key;
            SavePoint sp = kv.Value;

            //Debug.Log(sp.SavePoint_type.ToString() + ' ' + sp.transform.position + ' ' + sp.name + ' ' + sp.GetInstanceID());

            if (sp == null)
            {
                Debug.LogWarning($"ID {id}에 대응되는 SavePoint가 null입니다.");
                continue;
            }

            var entry = new SavePointEntry
            {
                ID = id,
                Type = sp.SavePoint_type.ToString(),
                Position = sp.transform.position,
                Name = sp.name,
                InstanceID = sp.GetInstanceID()
            };

            entries.Add(entry);
        }

        string json = JsonUtility.ToJson(new SavePointEntryWrapper { entries = entries }, true);
        string jsonPath = Path.Combine("Assets/MapData", "SavePoint_detailed.json");

        File.WriteAllText(jsonPath, json);
        AssetDatabase.Refresh();

        Debug.Log("상세 SavePoint 정보가 JSON으로 저장되었습니다!");
    }

    [System.Serializable]
    private class SavePointEntry
    {
        public int ID;
        public string Type;
        public Vector3 Position;
        public string Name;
        public int InstanceID;
    }

    [System.Serializable]
    private class SavePointEntryWrapper
    {
        public List<SavePointEntry> entries;
    }
    */

    #endregion


    Vector3 SnapToGrid(Vector3 pos, float size)
    {
        pos.x = Mathf.Round(pos.x / size) * size;
        pos.y = Mathf.Round(pos.y / size) * size;
        pos.z = 0;
        return pos;
    }

    string GetCategoryFromPath(GameObject prefab)
    {
        string path = AssetDatabase.GetAssetPath(prefab); // Assets/Prefab/Map/...
        string[] parts = path.Split('/');
        int mapIndex = System.Array.IndexOf(parts, "Map");
        if (mapIndex >= 0 && mapIndex + 1 < parts.Length)
        {
            return parts[mapIndex + 1];
        }
        return "Uncategorized";
    }

    Transform EnsureHierarchy(string category)
    {
        GameObject mapRoot = GameObject.Find("Map");
        if (mapRoot == null)
        {
            mapRoot = new GameObject("Map");
            Undo.RegisterCreatedObjectUndo(mapRoot, "Create Map Root");
        }

        Transform categoryObj = mapRoot.transform.Find(category);
        if (categoryObj == null)
        {
            GameObject newGroup = new GameObject(category);
            Undo.RegisterCreatedObjectUndo(newGroup, "Create Category Group");
            newGroup.transform.SetParent(mapRoot.transform);
            categoryObj = newGroup.transform;
        }

        return categoryObj;
    }
}
