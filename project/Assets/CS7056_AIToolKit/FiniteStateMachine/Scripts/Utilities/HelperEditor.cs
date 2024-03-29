﻿using UnityEngine;
using System.Collections;
//using UnityEditor;

namespace CS7056_AIToolKit
{
    public static class HelperEditor : object
    {
        private static Texture2D _staticRectTexture;
        private static GUIStyle _staticRectStyle;

        //---------------------------------------------------------------------------------
        public static void DrawColorBox(Rect position, Color color, string text)
        {
            if (_staticRectTexture == null)
            {
                _staticRectTexture = new Texture2D(1, 1);
            }

            if (_staticRectStyle == null)
            {
                _staticRectStyle = new GUIStyle();
            }
            _staticRectTexture.SetPixel(0, 0, color);
            _staticRectTexture.Apply();

            _staticRectStyle.normal.background = _staticRectTexture;
            _staticRectStyle.normal.textColor = HelperConstants.cyan;
            _staticRectStyle.alignment = TextAnchor.UpperCenter;

            GUI.Box(position, text, _staticRectStyle);
        }

        public static void DrawColorBox(Rect position, Color color)
        {
            DrawColorBox(position, color, "");
        }

        public static void DrawColorBox(Rect position, Color color, string name_, string body)
        {
            DrawColorBox(position, color, name_ + body);
            // GUI.TextArea(position,name_+body);
        }


        //---------------------------------------------------------------------------------


    }
}