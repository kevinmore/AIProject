using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace CS7056_AIToolKit
{
    public enum NodeLocation
    {
        right,
        bottom,
        left,
        top
    }

    public class EndPoint
    {
        public Rect location;
        public EndPoint(Rect startLocation)
        {
            location = startLocation;
        }
    }

    //---------------------------------------------------------------------------------
    public class StatePanel
    {
        public List<EndPoint> panelEndPoints;
        public List<int> eventsList = new List<int>();

        public Rect screenRect;
        public string stateName = "newName";
        public string stateDiscription = "";
        public int id = 0;
        public bool selected = false;
        //bool   changed=false;
        public bool markedForDeath = false;

        public EndPoint activeEndPoint;

        //200 X 150
        //name,id,events,locationX,locationY
        //state,   0,  ,212,283
        public StatePanel(string line)
        {
            string[] s = line.Split(',');
            stateName = s[0];
            id = int.Parse(s[1]);
            screenRect = new Rect(float.Parse(s[3]),
                                float.Parse(s[4]),
                                HelperConstants.StateWidth,
                                HelperConstants.StateHeight);
            stateDiscription = s[5];

            string[] eventsLine = s[2].Split(':');

            for (int i = 0; i < eventsLine.Length; ++i)
            {
                if (eventsLine[i].Length > 0)
                {
                    //Debug.Log("EVENT["+eventsLine[i]+"]");
                    eventsList.Add(int.Parse(eventsLine[i]));
                }
            }

        }

        //---------------------------------------------------------------------------------
        public StatePanel(Rect currentLocation, string newName)
        {
            panelEndPoints = new List<EndPoint>();
            screenRect = currentLocation;
            stateName = newName;
        }
        //---------------------------------------------------------------------------------

        public bool IsHandlerHolding(Vector2 point)
        {
            if (activeEndPoint == null) return false;
            return activeEndPoint.location.Contains(point);
        }

        public bool IsHolding(Vector2 point)
        {
            return screenRect.Contains(point);
        }

        public void ShowHighlight()
        {
            float boarder = 2;
            Rect back = new Rect(screenRect.position.x - boarder, screenRect.position.y - boarder, screenRect.width + boarder * 2, screenRect.height + boarder * 2);
            HelperEditor.DrawColorBox(back, HelperConstants.darkOrange, "");
        }

        public void ShowHighlight(Color color)
        {
            float boarder = 2;
            Rect back = new Rect(screenRect.position.x - boarder, screenRect.position.y - boarder, screenRect.width + boarder * 2, screenRect.height + boarder * 2);
            HelperEditor.DrawColorBox(back, color, "");
        }

        //---------------------------------------------------------------------------------
        public void Show()
        {
            Rect back = screenRect;

            float boarder = 8;

            back.width += boarder;
            back.height += boarder;
            back.x -= boarder / 2.0f;
            back.y -= boarder / 2.0f;


            Rect pt = new Rect(screenRect.position.x + screenRect.width - 12, screenRect.position.y + screenRect.height + 2, 10, 10);
            EndPoint ep = new EndPoint(pt);
            Rect resized = new Rect(ep.location.x - 1, ep.location.y - 1, ep.location.width + 2, ep.location.height + 2);

            HelperEditor.DrawColorBox(resized, new Color(.35f, .35f, .35f), "");
            HelperEditor.DrawColorBox(pt, new Color(.15f, .15f, .15f), "");

            activeEndPoint = ep;
        }
        //---------------------------------------------------------------------------------

    }
}