using UnityEngine;
using System.Collections;
using UnityEditor;
namespace Builder
{
    public class BuildPlatform
    {

        public static BuildPlatform current = null;

        static BuildPlatform()
        {
            BuildTarget target;
            if (Platform.isEdiorWin)
                target = BuildTarget.StandaloneWindows;
            else if (Platform.isOsx)
                target = BuildTarget.StandaloneOSXIntel;
            else if (Platform.isLinux)
                target = BuildTarget.StandaloneLinux;
            else if (Platform.isAndroid)
                target = BuildTarget.Android;
            else if (Platform.isIOS)
                target = BuildTarget.iOS;
            else
                target = BuildTarget.StandaloneWindows;

            current = new BuildPlatform(target);
        }


        public BuildTarget target { get; private set; }
        public string name { get; private set; }

        public BuildPlatform(BuildTarget target)
        {
            this.target = target;
            if (target == BuildTarget.StandaloneWindows)
                name = "Windows";
            else if (target == BuildTarget.StandaloneOSXIntel)
                name = "Mac";
            else if (target == BuildTarget.StandaloneLinux)
                name = "Linux";
            else if (target == BuildTarget.Android)
                name = "Android";
            else if (target == BuildTarget.iOS)
                name = "iOS";
            else
                name = "Windows";
        }

    }

}

