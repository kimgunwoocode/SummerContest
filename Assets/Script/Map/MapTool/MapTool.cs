using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System.Reflection;


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
        if (GUILayout.Button("SavePoint ID 일괄 설정"))
        {
            AssignIDsToSavePoints();
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




#region 맵의 세이브포인트 ID 자동 부여 및 설정, 저장까지!
    void AssignIDsToSavePoints()
    {
        string folderPath = "Assets/Scenes/test";
        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { folderPath });

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            Debug.LogWarning("변경된 씬 저장 안 해서 중단됨");
            return;
        }

        // SavePoint_list ScriptableObject 불러오기
        string[] guids = AssetDatabase.FindAssets("t:SavePoint_list", new[] { "Assets/MapData" });
        if (guids.Length == 0)
        {
            Debug.LogError("SavePoint_list ScriptableObject를 찾을 수 없습니다.");
            return;
        }
        string spListPath = AssetDatabase.GUIDToAssetPath(guids[0]);
        SavePoint_list spList = AssetDatabase.LoadAssetAtPath<SavePoint_list>(spListPath);
        if (spList == null)
        {
            Debug.LogError("SavePoint_list 불러오기 실패");
            return;
        }

        // Dictionary 필드 접근 (리플렉션)
        var dictField = typeof(SavePoint_list).GetField("SavePoint_IDlist", BindingFlags.NonPublic | BindingFlags.Instance);
        var dict = dictField?.GetValue(spList) as Dictionary<int, SavePoint>;
        if (dict == null)
        {
            Debug.LogError("SavePoint_IDlist 접근 실패");
            return;
        }

        // 초기화
        dict.Clear();


        int mainID = 2001;
        int semiID = 1001;

        int totalMain = 0;
        int totalSemi = 0;

        foreach (string guid in sceneGuids)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guid);
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

            SavePoint[] savePoints = GameObject.FindObjectsByType<SavePoint>(FindObjectsSortMode.None);

            Undo.RecordObjects(savePoints, "Bulk Assign SavePoint IDs");

            foreach (var sp in savePoints)
            {
                if (sp == null) continue;

                int assignedID = -1;

                switch (sp.SavePoint_type)
                {
                    case SavePoint.SP_type.Main:
                        sp.SavePoint_ID = mainID++;
                        assignedID = mainID;
                        totalMain++;
                        break;
                    case SavePoint.SP_type.Semi:
                        sp.SavePoint_ID = semiID++;
                        assignedID = semiID;
                        totalSemi++;
                        break;
                }

                if (assignedID != -1)
                {
                    dict[assignedID] = sp;
                }

                EditorUtility.SetDirty(sp);
            }

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);

            Debug.Log($"{scene.name} 씬 처리 완료 ({savePoints.Length}개)");
        }

        Debug.Log($"모든 씬 저장 완료! Main: {totalMain}, Semi: {totalSemi}");
        DumpDictionaryToJson();
    }
    void DumpDictionaryToJson()
    {
        string assetPath = "Assets/MapData";
        string[] guids = AssetDatabase.FindAssets("t:SavePoint_list", new[] { assetPath });

        if (guids.Length == 0)
        {
            Debug.LogWarning("SavePoint_list ScriptableObject가 없습니다.");
            return;
        }

        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        SavePoint_list spList = AssetDatabase.LoadAssetAtPath<SavePoint_list>(path);
        if (spList == null) return;


        var dictField = typeof(SavePoint_list).GetField("SavePoint_IDlist", BindingFlags.NonPublic | BindingFlags.Instance);
        var dict = dictField?.GetValue(spList) as Dictionary<int, SavePoint>;
        Debug.Log($"딕셔너리 항목 수: {dict.Count}");


        if (dict == null)
        {
            Debug.LogWarning("Dictionary 접근 실패");
            return;
        }

        // 키 리스트만 추출
        List<int> keys = new(dict.Keys);

        // 래퍼로 감싸서 JSON 직렬화
        string json = JsonUtility.ToJson(new KeyListWrapper { keys = keys }, true);

        string fullPath = Path.Combine(assetPath, "SavePoint_keys.json");
        File.WriteAllText(fullPath, json);
        AssetDatabase.Refresh();

        Debug.Log("SavePoint ID(Key) 목록이 JSON으로 저장되었습니다!");
    }

    [System.Serializable]
    private class KeyListWrapper
    {
        public List<int> keys;
    }
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
