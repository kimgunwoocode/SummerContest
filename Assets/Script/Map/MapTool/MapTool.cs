using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System;


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
            AssignIDsAndSaveToInitData();
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

    private const string ScenePath = "Assets/Scenes/Map";
    private const string InitSaveDataPath = "Assets/InitData/InitData.asset";
    private const string DumpPath = "Assets/InitData/InitData_ID_Dump.json";

    public static void AssignIDsAndSaveToInitData()
    {
        var finalMapData = new MapData();

        int semiID = 1001;
        int mainID = 2001;
        int shopID = 3001;
        int interactionID = 4001;
        int pushObjectID = 5001;

        int totalMain = 0, totalSemi = 0, totalShop = 0, totalInteraction = 0, totalPushObject = 0;

        var dumpEntries = new List<DumpEntry>();
        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { ScenePath });

        foreach (string guid in sceneGuids)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guid);
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

            // 여기서 다시 로드해야 함!!
            InitSaveData initData = AssetDatabase.LoadAssetAtPath<InitSaveData>(InitSaveDataPath);
            if (initData == null)
            {
                Debug.LogError($"InitSaveData를 {scene.name} 씬에서 다시 로드하는 데 실패했습니다.");
                continue;
            }

            List<string> changedObjects = new();
            bool sceneModified = false;

            GameObject mapRoot = GameObject.FindWithTag("Map");

            if (mapRoot == null)
            {
                Debug.LogWarning($"씬 \"{scene.name}\"에 Map 오브젝트가 없습니다. 해당 씬은 건너뜁니다.");
                continue;
            }

            SavePoint[] savePoints = mapRoot.GetComponentsInChildren<SavePoint>(true);
            ShopObject[] shopObjects = mapRoot.GetComponentsInChildren<ShopObject>(true);
            Interaction[] interactions = mapRoot.GetComponentsInChildren<Interaction>(true);
            PushObject[] pushObjects = mapRoot.GetComponentsInChildren<PushObject>(true);

            foreach (var sp in savePoints)
            {
                int assignedID = sp.SavePoint_type == SavePoint.SP_type.Main ? mainID++ : semiID++;
                if (sp.ID != assignedID)
                {
                    sp.ID = assignedID;
                    sceneModified = true;
                    EditorUtility.SetDirty(sp);
                    changedObjects.Add($"SavePoint ({sp.SavePoint_type}) → ID {assignedID} [{sp.name}]");
                }

                finalMapData.SpawnPoints[sp.ID] = sp.SavePointEnabled;

                dumpEntries.Add(new DumpEntry
                {
                    ID = sp.ID,
                    Type = $"SavePoint-{sp.SavePoint_type}",
                    Name = sp.name,
                    Position = sp.transform.position,
                    Scene = scene.name
                });

                if (sp.SavePoint_type == SavePoint.SP_type.Main) totalMain++;
                else totalSemi++;
            }

            foreach (var shop in shopObjects)
            {
                int assignedID = shopID++;
                if (shop.ID != assignedID)
                {
                    shop.ID = assignedID;
                    sceneModified = true;
                    EditorUtility.SetDirty(shop);
                    changedObjects.Add($"ShopObject → ID {assignedID} [{shop.name}]");
                }

                var shopData = new ShopData
                {
                    ID = shop.ID,
                    isOpened = shop.isOpened,
                    Items = new Dictionary<int, bool>()
                };

                finalMapData.Shops.Add(shopData);

                dumpEntries.Add(new DumpEntry
                {
                    ID = shop.ID,
                    Type = "ShopObject",
                    Name = shop.name,
                    Position = shop.transform.position,
                    Scene = scene.name
                });

                totalShop++;
            }

            foreach (var inter in interactions)
            {
                int assignedID = interactionID++;
                if (inter.ID != assignedID)
                {
                    inter.ID = assignedID;
                    sceneModified = true;
                    EditorUtility.SetDirty(inter);
                    changedObjects.Add($"Interaction → ID {assignedID} [{inter.name}]");
                }

                finalMapData.InteractionObjects[inter.ID] = inter.isInteracted;

                dumpEntries.Add(new DumpEntry
                {
                    ID = inter.ID,
                    Type = "Interaction",
                    Name = inter.name,
                    Position = inter.transform.position,
                    Scene = scene.name
                });

                totalInteraction++;
            }

            foreach (var push in pushObjects)
            {
                int assignedID = pushObjectID++;
                if (push.ID != assignedID)
                {
                    push.ID = assignedID;
                    sceneModified = true;
                    EditorUtility.SetDirty(push);
                    changedObjects.Add($"PushObject → ID {assignedID} [{push.name}]");
                }

                finalMapData.PushObjects[push.ID] = (Vector2)push.transform.position;

                dumpEntries.Add(new DumpEntry
                {
                    ID = push.ID,
                    Type = "PushObject",
                    Name = push.name,
                    Position = push.transform.position,
                    Scene = scene.name
                });

                totalPushObject++;
            }

            if (sceneModified)
            {
                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);
                Debug.Log($"씬 저장됨: {scene.name}");
                foreach (var line in changedObjects)
                    Debug.Log($"  - {line}");
            }
            else
            {
                Debug.Log($"씬 변경 없음: {scene.name}");
            }
        }

        // 마지막에 한 번만 원본 InitSaveData에 MapData 넣고 저장
        InitSaveData finalInitData = AssetDatabase.LoadAssetAtPath<InitSaveData>(InitSaveDataPath);
        if (finalInitData != null)
        {
            finalInitData.InitData.MapData = finalMapData;
            SaveInitSaveDataAsset(finalInitData);
            DumpToJson(dumpEntries, totalMain, totalSemi, totalShop, totalInteraction, totalPushObject);
        }
        else
        {
            Debug.LogError("최종 InitSaveData 저장 실패: InitSaveData를 다시 불러올 수 없습니다.");
        }
    }

    public static void SaveInitSaveDataAsset(InitSaveData data)
    {
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
        Debug.Log("InitSaveData.asset 저장 완료");
    }

    private static void DumpToJson(List<DumpEntry> entries, int totalMain, int totalSemi, int totalShop, int totalInteraction, int totlaPushObject)
    {
        var wrapper = new DumpWrapper
        {
            Entries = entries,
            Summary = new DumpSummary
            {
                TotalMainSavePoints = totalMain,
                TotalSemiSavePoints = totalSemi,
                TotalShops = totalShop,
                TotalInteractions = totalInteraction,
                TotalPushObjects = totlaPushObject
            }
        };

        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(DumpPath, json);
        AssetDatabase.Refresh();

        Debug.Log($"상세 정보 JSON으로 저장됨: {DumpPath}");
    }

    [System.Serializable]
    private class DumpEntry
    {
        public int ID;
        public string Type;
        public string Name;
        public Vector3 Position;
        public string Scene;
    }

    [System.Serializable]
    private class DumpSummary
    {
        public int TotalMainSavePoints;
        public int TotalSemiSavePoints;
        public int TotalShops;
        public int TotalInteractions;
        public int TotalPushObjects;
    }

    [System.Serializable]
    private class DumpWrapper
    {
        public List<DumpEntry> Entries;
        public DumpSummary Summary;
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
            mapRoot.tag = "Map";
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
