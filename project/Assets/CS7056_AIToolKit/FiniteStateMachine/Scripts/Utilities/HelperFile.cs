using UnityEngine;
using System.Collections;
using UnityEditor;

namespace CS7056_AIToolKit
{
    public static class HelperFile : object
    {

        //------------------------------------------------------
        public static string getTextFileFromResource(string filename)
        {
            TextAsset textAsset = (TextAsset)Resources.Load("FSM/" + filename, typeof(TextAsset));
            if (textAsset != null)
                return textAsset.text;
            else return "";
        }
        //------------------------------------------------------


        //------------------------------------------------------
        public static void saveToFile(string filename, string value)
        {
            System.IO.File.WriteAllText(filename, value);
            AssetDatabase.Refresh();
            //AssetDatabase.ImportAsset(filename);
        }
        //------------------------------------------------------

    }
}