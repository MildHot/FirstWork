using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
[CanEditMultipleObjects]
[CustomEditor(typeof(UGUIAltas), true)]
public class UGUIAltasEditor : Editor
{
    Texture2D textureAltas;
    public override void OnInspectorGUI()
    {
        GUILayout.Space(6f);
        DrawCommonProperties();
    }

    protected void DrawCommonProperties()
    {
        UGUIAltas uguiAltas = target as UGUIAltas;

        GUI.changed = false;

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("图集图片:", GUILayout.Width(50f));
        textureAltas = (Texture2D)EditorGUILayout.ObjectField(textureAltas, typeof(Texture2D), true, GUILayout.Width(150f));
        GUILayout.EndHorizontal();

        if (textureAltas != null)
        {
            uguiAltas.names.Clear();
            uguiAltas.sprites.Clear();
            var path = AssetDatabase.GetAssetPath(textureAltas);
            TextureImporter assetImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            if (assetImporter.spritePackingTag != "")
            {
                var parentPath = System.IO.Path.GetDirectoryName(path);
                var files = Directory.GetFiles(parentPath, "*.*", SearchOption.AllDirectories);
                if (files.Length >= 1)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (files[i].EndsWith(".meta"))
                        {
                            continue;
                        }
                        var objs= AssetDatabase.LoadAllAssetRepresentationsAtPath(files[i]);
                        if (objs != null)
                        {
                            var sprite = objs[0] as Sprite;
                            uguiAltas.names.Add(sprite.name);
                            uguiAltas.sprites.Add(sprite);
                        }
                    }
                }
            }
            textureAltas = null;
        }

        for (int i = 0; i < uguiAltas.names.Count; i++)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Name:", GUILayout.Width(50f));
            EditorGUILayout.LabelField(uguiAltas.names[i], GUILayout.Width(80f));
            EditorGUILayout.LabelField("Sprite:", GUILayout.Width(50f));
            EditorGUILayout.ObjectField(uguiAltas.sprites[i], typeof(Sprite), true, GUILayout.Width(150f));
            GUILayout.EndHorizontal();
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(uguiAltas);
        }
    }
}
