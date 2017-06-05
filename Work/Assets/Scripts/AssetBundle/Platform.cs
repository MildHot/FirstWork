using UnityEngine;
using System.Collections;

namespace Builder
{
    public class Platform 
    {
        public static bool isWindows { get; private set; }
        public static bool isLinux { get; private set; }
        public static bool isAndroid { get; private set; }
        public static bool isOsx { get; private set; }
        public static bool isIOS { get; private set; }
        public static bool isEditorOsx { get; private set; }
        public static bool isEdiorWin { get; private set; }

        static Platform()
        {
#if UNITY_STANDALONE_WIN
            isWindows =true;
#endif

#if UNITY_STANDALONE_OSX
            isOsx = true;
#endif

#if UNITY_STANDALONE_LINUX
            isLinux = true;
#endif

#if UNITY_ANDROID
            isAndroid = true;
#endif

#if UNITY_IOS
            isIOS = true;
#endif

#if UNITY_EDITOR_OSX
            isEditorOsx = true;
#endif

#if UNITY_EDITOR_WIN
            isEdiorWin = true;
#endif
        }

    }
}


