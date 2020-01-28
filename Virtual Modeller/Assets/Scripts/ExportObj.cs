﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SFB; // StandaloneFileBrowser
using UnityEditor;
using UnityEngine;

public class ExportObj : MonoBehaviour
{
    public void SaveFile()
    {
        MeshFilter[] filters = FindObjectsOfType<MeshFilter>();
        ExtensionFilter[] extensionList = new[] {
            new ExtensionFilter("Waveform obj", "obj")
        };
        var path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "MySaveFile", extensionList);
        Debug.Log(path);
        bool flag = false;
        Stack<string> reverseName = new Stack<string>();
        string fileName = "";
        for(int i = path.Length-1; i>0; i--)
        {
            if(!flag)
            {
                if (path[i].ToString().Contains("\\"))
                {
                    foreach (string item in reverseName)
                    {

                        fileName = fileName + item;
                    }
                    path = path.Remove(path.Length - fileName.Length - 1);
                    fileName = fileName.Remove(fileName.Length-4,4);
                    SaveModels(fileName, path);
                    return;
                }
                reverseName.Push(path[i].ToString());
            }
        }
    }

    struct ObjMaterial
    {
        public string name;
        public string textureName;
    }

    public int vertexOffset = 0;
    public int normalOffset = 0;
    public int uvOffset = 0;
    public string targetFolder = "ExportedObj";

    string MeshToString(MeshFilter mf, Dictionary<string, ObjMaterial> materialList)
    {
        Mesh m = mf.sharedMesh;
        Material[] mats = mf.GetComponent<Renderer>().sharedMaterials;

        StringBuilder sb = new StringBuilder();

        sb.Append("g ").Append(mf.name).Append("\n");
        foreach (Vector3 lv in m.vertices)
        {
            Vector3 wv = mf.transform.TransformPoint(lv);

            //This is sort of ugly - inverting x-component since we're in
            //a different coordinate system than "everyone" is "used to".
            sb.Append(string.Format("v {0} {1} {2}\n", -wv.x, wv.y, wv.z));
        }
        sb.Append("\n");

        foreach (Vector3 lv in m.normals)
        {
            Vector3 wv = mf.transform.TransformDirection(lv);

            sb.Append(string.Format("vn {0} {1} {2}\n", -wv.x, wv.y, wv.z));
        }
        sb.Append("\n");

        foreach (Vector3 v in m.uv)
        {
            sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
        }

        for (int material = 0; material < m.subMeshCount; material++)
        {
            sb.Append("\n");
            sb.Append("usemtl ").Append(mats[material].name).Append("\n");
            sb.Append("usemap ").Append(mats[material].name).Append("\n");

            //See if this material is already in the materiallist.
            try
            {
                ObjMaterial objMaterial = new ObjMaterial();

                objMaterial.name = mats[material].name;

                if (mats[material].mainTexture)
                    objMaterial.textureName = EditorUtility.GetAssetPath(mats[material].mainTexture);
                else
                    objMaterial.textureName = null;

                materialList.Add(objMaterial.name, objMaterial);
            }
            catch (ArgumentException)
            {
                //Already in the dictionary
            }


            int[] triangles = m.GetTriangles(material);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                //Because we inverted the x-component, we also needed to alter the triangle winding.
                sb.Append(string.Format("f {1}/{1}/{1} {0}/{0}/{0} {2}/{2}/{2}\n",
                                       triangles[i] + 1 + vertexOffset, triangles[i + 1] + 1 + normalOffset, triangles[i + 2] + 1 + uvOffset));
            }
        }

        vertexOffset += m.vertices.Length;
        normalOffset += m.normals.Length;
        uvOffset += m.uv.Length;

        return sb.ToString();
    }

    void Clear()
    {
        vertexOffset = 0;
        normalOffset = 0;
        uvOffset = 0;
    }

    Dictionary<string, ObjMaterial> PrepareFileWrite()
    {
        Clear();

        return new Dictionary<string, ObjMaterial>();
    }

    void MeshToFile(MeshFilter mf, string folder, string filename)
    {
        Dictionary<string, ObjMaterial> materialList = PrepareFileWrite();

        using (StreamWriter sw = new StreamWriter(folder + "/" + filename + ".obj"))
        {
            sw.Write("mtllib ./" + filename + ".mtl\n");

            sw.Write(MeshToString(mf, materialList));
        }

    }

    void MeshesToFile(MeshFilter[] mf, string folder, string filename)
    {
        Dictionary<string, ObjMaterial> materialList = PrepareFileWrite();

        using (StreamWriter sw = new StreamWriter(folder + "/" + filename + ".obj"))
        {
            sw.Write("mtllib ./" + filename + ".mtl\n");

            for (int i = 0; i < mf.Length; i++)
            {
                sw.Write(MeshToString(mf[i], materialList));
            }
        }

    }

    bool CreateTargetFolder()
    {
        try
        {
            System.IO.Directory.CreateDirectory(targetFolder);
        }
        catch
        {
            EditorUtility.DisplayDialog("Error!", "Failed to create target folder!", "");
            return false;
        }

        return true;
    }

    public void SaveModels(string fileName, string path)
    {
        MeshFilter[] filters = FindObjectsOfType<MeshFilter>();

        if (filters.Length == 0) return;

        //MeshesToFile(filters, Application.dataPath, fileName + ".obj");
        MeshesToFile(filters, path, fileName + ".obj");
    }

    //public void SaveModels(Transform[] models)
    //{
    //    if (models.Length == 0) return;

    //    int exportedObjects = 0;

    //    ArrayList mfList = new ArrayList();

    //    for (int i = 0; i < models.Length; i++)
    //    {
    //        Component[] meshfilter = models[i].GetComponentsInChildren(typeof(MeshFilter));

    //        for (int m = 0; m < meshfilter.Length; m++)
    //        {
    //            exportedObjects++;
    //            mfList.Add(meshfilter[m]);
    //        }
    //    }

    //    if (exportedObjects > 0)
    //    {
    //        MeshFilter[] mf = new MeshFilter[mfList.Count];

    //        for (int i = 0; i < mfList.Count; i++)
    //        {
    //            mf[i] = (MeshFilter)mfList[i];
    //        }

    //        string filename = "Exported_Objects.obj";

    //        MeshesToFile(mf, Application.dataPath, filename);


    //        Debug.Log("Objects exported == Exported " + exportedObjects + " objects to " + filename);
    //    }
    //    else
    //        Debug.Log("Objects not exported == Make sure at least some of your selected objects have mesh filters!");
    //}
}
