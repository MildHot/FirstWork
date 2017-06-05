using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace Builder
{

    /// <summary>
    /// 一键打包工具
    /// </summary>
    public class AssetBundleEditor : Editor
    {

        public static readonly string[] searchPaternList = new string[]
    {
        //图片格式
        "png","tga","jpg",
        //二进制格式
        "bytes",
        //声音格式
        "mp3","wav","ogg",
        //文档格式
        "json","txt","xml",
        //assetbundle
        "prefab","mat","ttf","fontsettings","shader","fbx"
    };

        /// <summary>
        /// D:/.../  +  AssetSources
        /// </summary>
        public static readonly string resourceRoot = Application.dataPath + "/AssetSources";

        /// <summary>
        ///  D:/.../Assets-6  =>  D:/.../
        /// </summary>
        public static readonly string projectRoot = Application.dataPath.Substring(0, Application.dataPath.Length - 6);

        public static readonly string publishName = "publish";

        public static string targetAssetBundlePathName = "AssetBundles";

        public static string targetRawResourePathName = "RawResource";

        public static readonly string assetbundlePattern = ".ab";

        public static readonly string assetbundleMD5FileName = "assetbundleConfiglist.ab";

        public static readonly string rawAssetMD5FileName = "rawresourceconfiglist.ab";

        [MenuItem("Build/AssetBundle/根据当前平台打包")]
        private static void CreateAssetBundle()
        {
            CreateAssetBundleAssets();
        }


        private static void FindAllAssets(string root, Dictionary<string, bool> paternDictionary, Dictionary<string, string[]> togetherDictionary, Dictionary<string, string[]> togetherEachOutputDictionar, List<string> assetBundleFiles)
        {
            var topDirList = Directory.GetDirectories(root);
            foreach (string path in topDirList)
            {
                var name = path.Substring(resourceRoot.Length + 1).Replace(Path.DirectorySeparatorChar, '/');

                //判断是否要打包在一起
                if (togetherDictionary.ContainsKey(name))
                {
                    //获取该目录下的所有文件
                    var allFiles = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                    if (allFiles.Length == 0)
                    {
                        continue;
                    }
                    List<string> checkedFiles = new List<string>(256);
                    foreach (string file in allFiles)
                    {
                        //过滤打包文件
                        var patern = file.Substring(file.LastIndexOf('.') + 1).ToLower();
                        if (paternDictionary.ContainsKey(patern))
                        {
                            checkedFiles.Add(file);
                        }
                    }
                    if (checkedFiles.Count > 0)
                    {
                        togetherDictionary[name] = checkedFiles.ToArray();
                    }
                }
                else if (BuilderConfig.togetherEachInputList.Contains(name))
                {
                    CollectToghterEachList(path, paternDictionary, togetherEachOutputDictionar);
                }
                else
                {
                    //获取该目录下的多有文件
                    var allFiles = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
                    foreach (string file in allFiles)
                    {
                        //过滤打包文件
                        var patern = file.Substring(file.LastIndexOf('.') + 1).ToLower();
                        if (paternDictionary.ContainsKey(patern))
                        {
                            assetBundleFiles.Add(file);
                        }
                    }
                    FindAllAssets(path, paternDictionary, togetherDictionary, togetherEachOutputDictionar, assetBundleFiles);
                }
            }
        }

        private static void CollectToghterEachList(string root, Dictionary<string, bool> paternDictionary, Dictionary<string, string[]> togetherEachOutputDictionar)
        {
            var name = root.Substring(resourceRoot.Length + 1).Replace(Path.DirectorySeparatorChar, '/');
            //获取该目录下的所有文件
            var allFiles = Directory.GetFiles(root, "*.*", SearchOption.TopDirectoryOnly);
            List<string> checkedFiles = new List<string>(256);
            foreach (string file in allFiles)
            {
                //过滤打包文件
                var patern = file.Substring(file.LastIndexOf('.') + 1).ToLower();
                if (paternDictionary.ContainsKey(patern))
                {
                    checkedFiles.Add(file);
                }
            }

            if (checkedFiles.Count > 0)
            {
                togetherEachOutputDictionar[name] = checkedFiles.ToArray();
            }

            var topDirList = Directory.GetDirectories(root);
            foreach (string path in topDirList)
            {
                CollectToghterEachList(path, paternDictionary, togetherEachOutputDictionar);
            }
        }

        private static void CreateAssetBundleAssets(string uploadResDir = null, BuildPlatform buildPlatform = null)
        {
            //空合并运算符(??)
            //例如：a??b 当a为null时则返回b，a不为null时则返回a本身。
            //空合并运算符为右结合运算符，即操作时从右向左进行组合的。如，“a??b??c”的形式按“a??(b??c)”计算。
            uploadResDir = uploadResDir ?? defaultUploadResDir;
            buildPlatform = buildPlatform ?? BuildPlatform.current;
            //生成部分容器
            Dictionary<string, bool> paternDictionary = new Dictionary<string, bool>(100);
            foreach (string patern in searchPaternList)
            {
                paternDictionary[patern] = true;
            }

            Dictionary<string, string[]> togetherDictionary = new Dictionary<string, string[]>();
            Dictionary<string, string[]> togetherEachOutputDictionary = new Dictionary<string, string[]>();

            foreach (string  together in BuilderConfig.buildinTogetherList)
            {
                togetherDictionary[together] = null;
            }

            var rootLength = resourceRoot.Length + 1;

            List<string> assetBundleFiles = new List<string>(2048);
            //获取顶级目录，根据bundleTogetherList判断是否单独打包
            FindAllAssets(resourceRoot, paternDictionary, togetherDictionary, togetherEachOutputDictionary, assetBundleFiles);
            //构建打包设置项目
            List<AssetBundle> buildes = new List<AssetBundle>();
            // Dictionary<string,ResInfo>buildFiles

        }



        static string defaultUploadResDir
        {
            get
            {
                if (Platform.isEditorOsx)
                    return "/../...";
                else
                    return @"\\....";
            }
        }


















    }
}