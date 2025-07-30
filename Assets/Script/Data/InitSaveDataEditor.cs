#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(InitSaveData))]
public class InitSaveDataEditor : Editor
{
    private bool showMap = true;
    private bool showPlayer = true;
    private bool showInteraction = true;
    private bool showPush = true;
    private bool showShops = true;
    private bool showSpawnPoints = true;
    private bool showSkills = true;
    private bool showItems = true;

    public override void OnInspectorGUI()
    {
        InitSaveData init = (InitSaveData)target;
        if (init == null || init.InitData == null)
        {
            EditorGUILayout.HelpBox("InitSaveData or InitData is null.", MessageType.Error);
            return;
        }

        EditorGUILayout.LabelField("Basic Info", EditorStyles.boldLabel);
        init.InitData.Slot = EditorGUILayout.IntField("Slot", init.InitData.Slot);
        init.InitData.Name = EditorGUILayout.TextField("Name", init.InitData.Name);
        init.InitData.Day = EditorGUILayout.TextField("Day", init.InitData.Day);

        EditorGUILayout.Space(10);
        showMap = EditorGUILayout.Foldout(showMap, "Map Data", true);
        if (showMap)
        {
            EditorGUI.indentLevel++;

            var map = init.InitData.MapData;

            showInteraction = EditorGUILayout.Foldout(showInteraction, $"InteractionObjects ({map.InteractionObjects.Count})");
            if (showInteraction)
                DrawDictionary(map.InteractionObjects, "ID", "Done");

            showPush = EditorGUILayout.Foldout(showPush, $"PushObjects ({map.PushObjects.Count})");
            if (showPush)
                DrawDictionary(map.PushObjects, "ID", "Position");

            showShops = EditorGUILayout.Foldout(showShops, $"Shops ({map.Shops.Count})");
            if (showShops)
                foreach (var shop in map.Shops)
                {
                    EditorGUILayout.BeginVertical("box");
                    EditorGUILayout.LabelField($"Shop ID: {shop.ID} | Opened: {shop.isOpened}");
                    if (shop.Items != null)
                        DrawDictionary(shop.Items, "ItemID", "Sold");
                    EditorGUILayout.EndVertical();
                }

            showSpawnPoints = EditorGUILayout.Foldout(showSpawnPoints, $"SpawnPoints ({map.SpawnPoints.Count})");
            if (showSpawnPoints)
                DrawDictionary(map.SpawnPoints, "ID", "Active");

            map.SpawnPoint = EditorGUILayout.IntField("Spawn Point", map.SpawnPoint);

            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space(10);
        showPlayer = EditorGUILayout.Foldout(showPlayer, "Player Data", true);
        if (showPlayer)
        {
            EditorGUI.indentLevel++;

            var player = init.InitData.PlayerData;

            player.MaxHP = EditorGUILayout.IntField("Max HP", player.MaxHP);
            player.CurrentHP = EditorGUILayout.IntField("Current HP", player.CurrentHP);
            player.ATK = EditorGUILayout.IntField("ATK", player.ATK);
            player.MaxBreathGauge = EditorGUILayout.FloatField("Max Breath", player.MaxBreathGauge);
            player.CurrentBreathGauge = EditorGUILayout.FloatField("Current Breath", player.CurrentBreathGauge);
            player.Money = EditorGUILayout.IntField("Money", player.Money);

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Equip Skills");
            if (player.EquipSkill != null)
            {
                for (int i = 0; i < player.EquipSkill.Count; i++)
                    player.EquipSkill[i] = EditorGUILayout.IntField($"Slot {i}", player.EquipSkill[i]);
            }

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Player Abilities");
            if (player.PlayerAbility != null)
            {
                string[] abilityNames = { "Dash", "Breath", "Double Jump", "Down Attack", "Glide", "Wall Climb" };
                for (int i = 0; i < player.PlayerAbility.Count; i++)
                {
                    string label = i < abilityNames.Length ? abilityNames[i] : $"Ability {i}";
                    player.PlayerAbility[i] = EditorGUILayout.Toggle(label, player.PlayerAbility[i]);
                }
            }

            showSkills = EditorGUILayout.Foldout(showSkills, $"Unlocked Skills ({player.PlayerSkill.Count})");
            if (showSkills)
                DrawDictionary(player.PlayerSkill, "SkillID", "Unlocked");

            showItems = EditorGUILayout.Foldout(showItems, $"Owned Items ({player.GettedItems.Count})");
            if (showItems)
                DrawDictionary(player.GettedItems, "ItemID", "Count");

            EditorGUI.indentLevel--;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void DrawDictionary<T>(Dictionary<int, T> dict, string keyLabel, string valueLabel)
    {
        EditorGUI.indentLevel++;
        foreach (var kv in dict)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"{keyLabel}: {kv.Key}", GUILayout.Width(80));
            EditorGUILayout.LabelField($"{valueLabel}: {kv.Value?.ToString()}");
            EditorGUILayout.EndHorizontal();
        }
        EditorGUI.indentLevel--;
    }
}
#endif