using UnityEngine;
using System.Collections;

namespace CS7056_AIToolKit
{
    public class EventConnection
    {
        public EventConnection(StatePanel start, StatePanel end)
        {
            from = start;
            to = end;
        }
        public EventConnection(StatePanel start)
        {
            from = start;
        }

        public StatePanel from;
        public StatePanel to;

        public Vector2 fromPT;
        public Vector2 toPT;

        public string conditions = "";
        public string actions = "";
        public int id = 0;

        private float width = 180;
        private float heigt = 40;
        private float editHeigt = 60;
        private Rect location;

         private Color boxColor = new Color(.2f, .1f, .1f, .01f);
        private Color selecteBoxColor = new Color(.9f, .9f, .2f, .01f);
        private Color boxColorBack = new Color(.45f, .6f, .6f, .01f);
        private Color editboxColorBack = new Color(.1f, .1f, .1f, .9f);
        private Color editboxColorBoarder = new Color(.6f, .6f, .6f, .5f);

        public string eventName = "";

        public bool selected = false;

        public bool markedForDeath = false;

        public int fromToCount = 0;

        public bool IsHoldingPoint(Vector2 point)
        {
            if (location == null) return false;
            return location.Contains(point);
        }

        /// <summary>
        /// Draw the connection box on the screen.
        /// </summary>
        public void ShowSelected()
        {

            Vector2 center = HelperGraphics.quarterPoint(fromPT, toPT);
            Rect box = new Rect(center.x - width / 2, center.y - editHeigt / 2 + fromToCount * 40, width, editHeigt);
            if (from == to)
            {
                //Debug.Log("from = too");
                box = new Rect(center.x - width - 40, center.y - editHeigt + fromToCount * 40, width, editHeigt);
            }



            Rect editbox = new Rect(box.x, box.y, width, editHeigt + 20);
            Rect editboxBoarder = new Rect(box.x - 1, box.y - 1, width + 2, editHeigt + 20 + 2);
            float boarder = 2;
            Rect box2 = new Rect(box.x + boarder, box.y + boarder, box.width - boarder * 2, box.height - boarder * 2);

            location = box;

            if (selected)//we need to add the edit boxes
            {
                HelperEditor.DrawColorBox(editboxBoarder, editboxColorBoarder, "");
                HelperEditor.DrawColorBox(editbox, editboxColorBack, "");

                // Name
                GUILayout.BeginArea(new Rect(box.x + 2, box.y + 15, 40, 20));
                GUILayout.Label("Name:");
                GUILayout.EndArea();
                GUILayout.BeginArea(new Rect(box.x + 65, box.y + 15, 105, 20));
                eventName = GUILayout.TextField(eventName);
                GUILayout.EndArea();

                // Condition
                GUILayout.BeginArea(new Rect(box.x + 2, box.y + 35, 65, 20));
                GUILayout.Label("Condition:");
                GUILayout.EndArea();
                GUILayout.BeginArea(new Rect(box.x + 65, box.y + 35, 105, 20));
                conditions = GUILayout.TextField(conditions);
                GUILayout.EndArea();

                // Action
                GUILayout.BeginArea(new Rect(box.x + 2, box.y + 55, 65, 20));
                GUILayout.Label("Action:");
                GUILayout.EndArea();
                GUILayout.BeginArea(new Rect(box.x + 65, box.y + 55, 105, 20));
                actions = GUILayout.TextField(actions);
                GUILayout.EndArea();

                GUIStyle btnStyle = new GUIStyle();
                btnStyle.stretchHeight = true;
                btnStyle.stretchWidth = true;
                if (GUI.Button(new Rect(box.x + box.width - 15, box.y, 20, 15), (Texture2D)Resources.Load("Editor/close"), btnStyle))
                {
                    markedForDeath = true;
                }
            }
            else//just show the information 
            {
            }

        }
        /// <summary>
        /// Draw the connection box on the screen.
        /// </summary>
        public void ShowNotSelected()
        {

            Vector2 center = HelperGraphics.quarterPoint(fromPT, toPT);
            Rect box = new Rect(center.x - width / 2, center.y - heigt / 2 + fromToCount * 40, width, heigt);
            if (from == to)
            {
                //Debug.Log("from = too");
                box = new Rect(center.x - width - 40, center.y - heigt + fromToCount * 40, width, heigt);
            }

            /*
            Rect    editbox         = new Rect(box.x,box.y,width,heigt+20);
            Rect    editboxBoarder  = new Rect(box.x -1 ,box.y -1,width+2,heigt+20+2);
            */
            float boarder = 2;
            Rect box2 = new Rect(box.x + boarder, box.y + boarder, box.width - boarder * 2, box.height - boarder * 2);

            location = box;

            if (selected)//we need to add the edit boxes
            {

            }
            else//just show the information 
            {
                HelperEditor.DrawColorBox(box, boxColorBack, "");

                if (conditions.Length > 0 && actions.Length > 0)
                    HelperEditor.DrawColorBox(box2, boxColor, "Event " + id + " : " + eventName, "\n[" + conditions + "]\n/" + actions);

                if (conditions.Length == 0 && actions.Length > 0)
                    HelperEditor.DrawColorBox(box2, boxColor, "Event " + id + " : " + eventName, "\n/" + actions);

                if (conditions.Length > 0 && actions.Length == 0)
                    HelperEditor.DrawColorBox(box2, boxColor, "Event " + id + " : " + eventName, "\n[" + conditions + "]");

                if (conditions.Length == 0 && actions.Length == 0)
                    HelperEditor.DrawColorBox(box2, boxColor, "Event " + id + " : " + eventName);
            }

        }
    }
}