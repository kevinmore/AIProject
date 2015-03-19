using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


//---------------------------------------------------------------------------------



//---------------------------------------------------------------------------------



//---------------------------------------------------------------------------------
namespace CS7056_AIToolKit
{
    public class FSMEditor : EditorWindow
    {
        //Rect windowRect =new Rect (100,100,200,200);
        List<StatePanel> statesPanels = new List<StatePanel>();
        List<EventConnection> events = new List<EventConnection>();
        List<AttributePair> attributes = new List<AttributePair>();
        private bool needToAddControllerScript = false;

        static FSMEditor()
        {
            justRecompiled = true;
            //Debug.Log("test");

        }
        public Object target;
        public Texture2D arrow;
        private int runCounter = 0;

        public StatePanel startStateSelected;
        public StatePanel endStateSelected;
        private string previousState = "";
        private string currentState = "";
        public StateController currentStateController;

        bool dragging_ = false;
        bool dirty = false;
        Vector2 startPos;
        Vector2 currentPos;
        Vector2 deltaPos;
        Vector2 size = new Vector2(150, 100);

        Rect windowRect = new Rect(200, 100, 250, 200);
        //Rect windowRect2 = new Rect (600, 100, 300, 300);
        StatePanel selectedState;
        EventConnection selectedEvent;

        private Vector2 scrollPosition_;
        private string stateName = "state";
        private string eventName = "event";
        private string fromState = "stateFrom";
        private string toState = "stateTo";

        private int clickCounter = 0;
        private Vector2 lastClick = new Vector2();

        private string eventCondition = "";
        private string eventAction = "";
        private string filename = "";
        private string controllerName = "";
        private string resourcesDirectory = "/CS7056_AIToolKit/Resources/FSM";
        //private Color darkGray = new Color(.02f, .02f, .02f, 1.0f);

        //private string attributeLabel="";
        //private string attributeLabel="";

        EventConnection eventConDrag;


        private string stateDiscription = "Enter description...";


        private int screenBoxX = Screen.width;
        private int screenBoxY = Screen.height;

        private int scrollBoxX = Screen.width * 2;
        private int scrollBoxY = Screen.height * 3;

        float hSbarValue = .5f;
        Vector2 scrollPosition = new Vector2(0, 0);
        Rect virtualWindow = new Rect(0, 0, 1000, 1000);
        float maxX = 0;
        float maxY = 0;
        Vector2 atttributScroll = new Vector2(0, 100);
        private static bool justRecompiled;

        GUIStyle btnStyle;

        //---------------------------------------------------------------------------------
        [MenuItem("Finite State Machine/Designer")]
        private static void showEditor()
        {
            EditorWindow.GetWindow<FSMEditor>(false, "FSM Designer", true);
        }
        //---------------------------------------------------------------------------------


        //---------------------------------------------------------------------------------
        [MenuItem("Finite State Machine/Designer", true)]
        private static bool showEditorValidator()
        {
            return true;
        }
        //---------------------------------------------------------------------------------



        //---------------------------------------------------------------------------------
        //for working in scene view
//         void OnSelectionChange()
//         {
//             Debug.Log("Selection Changed");
//         }
        //---------------------------------------------------------------------------------


        //---------------------------------------------------------------------------------
        [ExecuteInEditMode]
        void OnEnable()
        {
            //myDelegate=DoWindow;
            if (PlayerPrefs.HasKey("controllerName"))
            {
                controllerName = PlayerPrefs.GetString("controllerName");

            }
            if (PlayerPrefs.HasKey("target"))
            {
                string targetName = PlayerPrefs.GetString("target");
                GameObject o = GameObject.Find(targetName);

                if (o != null)
                    target = GameObject.Find(targetName);

            }
            if (PlayerPrefs.HasKey("resourceFilename"))
            {
                filename = PlayerPrefs.GetString("resourceFilename");
            }

            btnStyle = new GUIStyle();
            btnStyle.stretchHeight = true;
            btnStyle.stretchWidth = true;
        }
        //*********************************************************************************
        void OnGUI()
        {
            maxX = 0;
            maxY = 0;
            //	Debug.Log("scrollPosition ["+scrollPosition.x+", "+scrollPosition.y+"]");
            scrollPosition = GUI.BeginScrollView(
                new Rect(0, 0, position.width, position.height),
                scrollPosition,
                virtualWindow
                );

            if (Application.isPlaying)
                GUI.Label(new Rect(position.width / 2 - 100, 5, 210, 20), "State Machine In Operation.");
            else
                GUI.Label(new Rect(position.width / 2 - 100, 5, 210, 20), "Double Click to Create a New State.");

            BeginWindows();
            int count = 0;

            foreach (StatePanel panel in statesPanels)
            {
                panel.screenRect = GUILayout.Window(count, panel.screenRect, OnCurrentState, "State " + count + ": " + panel.stateName);
                if (panel.screenRect.x + panel.screenRect.width > maxX)
                    maxX = panel.screenRect.x + panel.screenRect.width + 10;

                if (panel.screenRect.y + panel.screenRect.height > maxY)
                    maxY = panel.screenRect.y + panel.screenRect.height + 10;

                panel.id = count;
                panel.Show();
                ++count;

                if (panel.markedForDeath) dirty = true;

                if (panel.selected == true) panel.ShowHighlight();

                if (Application.isPlaying)
                {
                    if (currentState == panel.stateName)
                        panel.ShowHighlight(HelperConstants.lightOrange);
                    if (previousState == panel.stateName)
                        panel.ShowHighlight(HelperConstants.darkRiceYellow);
                }
            }

            virtualWindow.width = maxX;
            virtualWindow.height = maxY;
            EndWindows();

            DrawLinks();

            GUI.EndScrollView();

            DrawLogoPanel();
            DrawControlPanels();

            if (!Application.isPlaying)
            {
                OnMouseEvent();
            }
            if (dirty) Clear();
            ClearEvents();
        }
        //---------------------------------------------------------------------------------



        //----------------------------------------------------------------------------
        float GetChunk(Rect frame, int l, float size)
        {
            return frame.y + 15 + size * l;
        }
        //----------------------------------------------------------------------------



        //----------------------------------------------------------------------------
        float GetChunk(Rect frame, int l)
        {
            float lChunk = 50;
            return frame.y + 15 + lChunk * l;
        }
        //----------------------------------------------------------------------------




        //----------------------------------------------------------------------------
        public void Update()
        {
            if (justRecompiled)
            {
                justRecompiled = false;
                LoadFSM(HelperFile.getTextFileFromResource(filename));

                if (needToAddControllerScript)
                {
                    needToAddControllerScript = false;
                    GameObject o = (GameObject)target;
                    o.AddComponent(controllerName);
                }
            }

            if (target != null && Application.isPlaying)
            {
                GameObject sco = (GameObject)target;
                if (sco != null)
                {
                    StateController sc = sco.GetComponent<StateController>();
                    if (sc.myStateMachine != null)
                    {
                        if (sc.myStateMachine.state != null)
                        {
                            previousState = sc.previousState;
                            currentState = sc.currentState;

                            currentStateController = sc;
                            Repaint();
                        }
                    }
                }
            }

        }
        //----------------------------------------------------------------------------



        //----------------------------------------------------------------------------
//         public static void OnCompileScripts()
//         {
//             Debug.Log("OnCompileScripts");
//         }
        //----------------------------------------------------------------------------



        //----------------------------------------------------------------------------
        void DrawAttributesPanel(Rect frame, float boarder)
        {
            HelperEditor.DrawColorBox(new Rect(frame.x - boarder, frame.y - boarder, frame.width + boarder * 2, frame.height + boarder * 2), Color.gray, "");
            HelperEditor.DrawColorBox(frame, new Color(.15f, .15f, .15f), "Attributes");
            float lChunk = 25;
            float rightSide = frame.width - boarder * 2;
            float l1 = frame.y + 15;
            float width = 70;

            //atttributScroll = EditorGUILayout.BeginScrollView(atttributScroll, true, true,  GUILayout.Width(frame.width),  GUILayout.Height(frame.height-30));  
            atttributScroll = GUI.BeginScrollView(new Rect(frame.x + 5, l1 + 10, frame.width - 10, 200),
                                                   atttributScroll, new Rect(frame.x + 5, 0, 0, lChunk * attributes.Count), false, false);
            for (int i = 0; i < attributes.Count; ++i)
            {
                AttributePair attr = (AttributePair)attributes[i];

                attr.label = GUI.TextField(new Rect(15, i * lChunk, width, 20), attr.label);

                GUI.Label(new Rect(18 + width, i * lChunk, 20, 20), (Texture2D)Resources.Load("Editor/equal"), btnStyle);

                if (!Application.isPlaying)
                {
                    if (GUI.Button(new Rect(45 + width + width / 1.5f, i * lChunk, 20, 20), (Texture2D)Resources.Load("Editor/close"), btnStyle))
                    {
                        attributes.Remove(attr);
                    }
                    attr.value = GUI.TextField(new Rect(15 + width + 25, i * lChunk, width / 1.5f, 20), attr.value);
                }
                else
                {
                    if (currentStateController != null)
                        GUI.TextField(new Rect(15 + width + 25, i * lChunk, width / 1.5f, 20), currentStateController.myStateMachine.getAtributeValue(attr.label));
                }
            }
            GUI.EndScrollView();


            GUILayout.BeginArea(new Rect(frame.x + 70, frame.y + frame.height - 20, 60, 20));
            if (GUILayout.Button("Add"))
            {
                attributes.Add(new AttributePair("", ""));
            }
            GUILayout.EndArea();
        }
        //----------------------------------------------------------------------------



        //----------------------------------------------------------------------------	
        private void AddStatePanel()
        {
            StatePanel newStatePanel = new StatePanel(new Rect(startPos.x - HelperConstants.StateWidth / 2, startPos.y - HelperConstants.StateHeight / 2,
                                                      HelperConstants.StateWidth,
                                                      HelperConstants.StateHeight), stateName);
            newStatePanel.stateDiscription = stateDiscription;
            newStatePanel.id = statesPanels.Count;
            statesPanels.Add(newStatePanel);
        }
        //----------------------------------------------------------------------------

        /// <summary>
        /// Save the FSM to a text file
        /// </summary>
        private void SaveFSM()
        {
            //Debug.Log(("SAVE FSM " + Application.dataPath + resourcesDirectory));
            save();
            HelperFile.saveToFile(Application.dataPath + resourcesDirectory + "/" + filename + ".txt", GetFSMString());

            //AssetDatabase.ImportAsset(Application.dataPath + resourcesDirectory+"/"+filename+".txt");
            //loadFSM(HelperFile.getTextFileFromResource(filename));
            Repaint();
        }

        /// <summary>
        /// Load the FSM from a text file
        /// </summary>
        private void LoadFSM()
        {
            //Debug.Log(("LOAD FSM " + Application.dataPath + resourcesDirectory));
            save();
            //HelperFile.saveToFile(Application.dataPath + resourcesDirectory+"/"+filename+".txt",getFSMString());

            string filestring = HelperFile.getTextFileFromResource(filename);
            //filestring = EditorGUILayout.ObjectField(filestring,typeof(object), true);


            if (filestring.Length > 3)
                LoadFSM(filestring);
        }

        //----------------------------------------------------------------------------
        /// <summary>
        /// Save the editor window fields
        /// </summary>
        private void save()
        {
            PlayerPrefs.SetString("resourceFilename", filename);
            PlayerPrefs.SetString("controllerName", controllerName);
            if (target != null)
                PlayerPrefs.SetString("target", target.name);

            PlayerPrefs.Save();
        }
        //----------------------------------------------------------------------------

        void DrawLogoPanel()
        {
            Vector2 point = new Vector2(10, 20);

            GUIStyle style = new GUIStyle();
            style.normal.textColor = HelperConstants.cyan;
            style.fontSize = 12;

            HelperEditor.DrawColorBox(new Rect(point.x - 7, point.y - 7, 204, 86), Color.gray);
            HelperEditor.DrawColorBox(new Rect(point.x - 5, point.y - 5, 200, 82), new Color(.15f, .15f, .15f));

            GUILayout.BeginArea(new Rect(point.x, point.y, 200, 84));
            GUI.Label(new Rect(0, 0, 70, 70), (Texture2D)Resources.Load("Editor/tcd"), style);
            GUI.Label(new Rect(55, 0, 200, 30), "Finite State Machine", style);
            GUI.Label(new Rect(85, 15, 200, 30), "Designer", style);
            GUI.Label(new Rect(55, 40, 200, 30), "- Huanxiang Wang", style);
            GUI.Label(new Rect(80, 55, 200, 30), "14333168", style); 
            GUILayout.EndArea();
        }


        //----------------------------------------------------------------------------
        void DrawFilePanel(Rect rect)
        {
            Vector2 point = new Vector2(rect.x, rect.y);
            HelperEditor.DrawColorBox(new Rect(point.x - 7, point.y - 7, rect.width + 4, rect.height + 4), Color.gray);
            HelperEditor.DrawColorBox(new Rect(point.x - 5, point.y - 5, rect.width, rect.height), new Color(.15f, .15f, .15f));

//             GUI.Label(new Rect(point.x, point.y, 150, 20), "Resource Directory");
//             resourcesDirectory = GUI.TextField(new Rect(point.x, point.y + 20, 180, 20), resourcesDirectory);

//             GUI.Label(new Rect(point.x, point.y, 200, 20), "Resource Filename");
//             filename = GUI.TextField(new Rect(point.x, point.y + 20, 150, 20), filename);


//             GUILayout.BeginArea(new Rect(point.x, point.y + 45, 80, 50));
//             if (GUILayout.Button("SAVE FSM"))
//             {
//                 saveFSM();
//             }
//             GUILayout.EndArea();
// 
//             GUILayout.BeginArea(new Rect(point.x + 100, point.y + 45, 80, 50));
//             if (GUILayout.Button("LOAD FSM"))
//             {
//                 loadFSM();
//             }
//             GUILayout.EndArea();


            string beforeSource = "";
            if (target != null)
            {
                beforeSource = target.name;
            }
            GUI.Label(new Rect(point.x, point.y + 10, 155, 20), "Target");
            GUILayout.BeginArea(new Rect(point.x + 50, point.y + 10, 135, 50));
            target = EditorGUILayout.ObjectField(target, typeof(GameObject), true);
            GUILayout.EndArea();
            if (target != null && target.name != beforeSource)
            {
                PlayerPrefs.SetString("target", target.name);
                PlayerPrefs.Save();
                GameObject o = (GameObject)target;
                StateController tempSC = o.GetComponent<StateController>();
                if (tempSC != null)
                {
                    controllerName = GetShortName(tempSC.ToString());
                    filename = controllerName + "FSM";
                    LoadFSM();
                    PlayerPrefs.SetString("controllerName", controllerName);
                    PlayerPrefs.Save();
                }
                else
                {
                    controllerName = "";
                    filename = "";
                    Reset();
                }
            }


            GUI.Label(new Rect(point.x, point.y + 35, 200, 20), "Controller Name:");

            controllerName = GUI.TextField(new Rect(point.x, point.y + 55, 180, 20), controllerName);

            GUILayout.BeginArea(new Rect(point.x, point.y + 90, 80, 50));
            if (GUILayout.Button("Build"))
            {
                if (controllerName == "StateController") controllerName = controllerName + "1";
                if (controllerName.Length == 0 && target != null)
                {
                    controllerName = target.name + "Controller";
                }
                filename = controllerName + "FSM";
                SaveFSM();

                //Debug.Log("Make Controller: " + Application.dataPath + "/" + controllerName);
                save();
                HelperFile.saveToFile(Application.dataPath + "/CS7056_AIToolKit/FiniteStateMachine/Controllers/" + controllerName + ".cs", HelperFormater.makeFileUsing(controllerName, filename, statesPanels));

                //AssetDatabase.ImportAsset(Application.dataPath+"/"+controllerName+".cs");
                //loadFSM(HelperFile.getTextFileFromResource(filename));

                //if(currentStateController == null)
                if (target == null)
                {
                    Debug.Log("No controller selected");
                    GameObject o = new GameObject();
                    o.name = controllerName;
                    target = o;
                    needToAddControllerScript = true;
                    AssetDatabase.Refresh();//
                    //GameObject go = (GameObject)Instantiate(o);
                    //o.AddComponent(controllerName);//
                }
                else
                {
                    GameObject so = (GameObject)target;
                    object testO = so.GetComponent(controllerName);
                    if (testO == null)
                    {
                        needToAddControllerScript = true;
                        AssetDatabase.Refresh();//
                    }
                }

                Repaint();
            }
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(point.x + 100, point.y + 90, 80, 50));
            if (GUILayout.Button("Reload"))
            {
                LoadFSM();
            }
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(point.x, point.y + 120, 180, 50));
            if (GUILayout.Button("Clear"))
            {
                Reset();
            }
            GUILayout.EndArea();

        }
        //-----------------------------------------------------------------------------

        private string GetShortName(string name)
        {
            string[] s = name.Split('(');
            return s[1].Replace(")", "");

        }

        //-----------------------------------------------------------------------------
        void DrawControlPanels()
        {
            DrawFilePanel(new Rect(10, 110, 200, 150));

            //stateInputBox(new Rect(10,10, 200, 180),2);
            //eventInputBox(new Rect(10,200, 200, 300),2);

            DrawAttributesPanel(new Rect(5, 263, 200, 280), 2);
        }
        //----------------------------------------------------------------------------



        //----------------------------------------------------------------------------
        void ClearEvents()
        {
            for (int i = events.Count - 1; i >= 0; i--)
            {
                if (events[i].markedForDeath)
                {
                    events[i].selected = false;
                    events.Remove(events[i]);
                }

            }
            int count = 1;
            foreach (EventConnection ec in events)
            {
                ec.fromToCount = GetEventsCount(ec.from, ec.to, count);
                ec.id = count - 1;
                count++;
            }
        }
        //----------------------------------------------------------------------------



        //----------------------------------------------------------------------------
        int GetEventsCount(StatePanel from, StatePanel to, int startIndex)
        {
            int sum = 0;
            for (int i = startIndex; i < events.Count; i++)
            {
                EventConnection ec = (EventConnection)events[i];
                if (ec.from == from && ec.to == to)
                {
                    sum++;
                }
            }
            return sum;
        }
        //----------------------------------------------------------------------------



        //----------------------------------------------------------------------------
        void Clear()
        {//
            dirty = false;
            for (int i = statesPanels.Count - 1; i >= 0; i--)
            {
                if (statesPanels[i].markedForDeath)
                {
                    statesPanels[i].selected = false;
                    DeleteState(statesPanels[i]);
                }
            }
        }
        //---------------------------------------------------------------------------------



        //---------------------------------------------------------------------------------
        bool mouseUp
        {
            get
            {
                Event currentEvent = Event.current;

                EventType currentEventType = currentEvent.type;
                //Debug.Log("mouse:"+currentEventType);
                return currentEventType == EventType.mouseUp;
            }

        }
        //---------------------------------------------------------------------------------

        //---------------------------------------------------------------------------------
        bool mouseDown
        {
            get
            {
                Event currentEvent = Event.current;

                EventType currentEventType = currentEvent.type;
                return currentEventType == EventType.mouseDown;
            }

        }
        //---------------------------------------------------------------------------------


        //---------------------------------------------------------------------------------
        bool mouseDrag
        {
            get
            {
                Event currentEvent = Event.current;

                EventType currentEventType = currentEvent.type;
                return currentEventType == EventType.mouseDrag;
            }

        }
        //---------------------------------------------------------------------------------

        //*********************************************************************************
        //---------------------------------------------------------------------------------

        /*
            //----------------------------------------------------------------------------
            private EventConnection getConnection(Vector2 point)
                {
                    //Debug.Log("scrollPosition ["+scrollPosition.x+", "+scrollPosition.y+"]  "+point.x+",  "+point.y);
                    Vector2 pmod=new Vector2(point.x+scrollPosition.x,point.y+scrollPosition.y);
                    foreach (EventConnection ec in events)
                    {
                        if(ec.holdsPoint(pmod))return ec;
                    }
                return null;
                }
            //----------------------------------------------------------------------------
            */

        //----------------------------------------------------------------------------
        private void OnMouseEvent()
        {//...................................................................
            if (mouseUp)
            {

                Vector2 stopMousePos = Event.current.mousePosition;
                //currentPos = Event.current.mousePosition;

                bool clicked = Vector2.Distance(stopMousePos, startPos) < .5f;
                if (clicked)
                {
                    clickCounter++;
                    SelectEvent(startPos);
                    //Debug.Log("Clicked" + clickCounter + "  " + Vector2.Distance(stopMousePos, lastClick));
                    if (selectedEvent == null && clickCounter > 1)
                    {
                        clickCounter = 1;
                        if (Vector2.Distance(stopMousePos, lastClick) < .9f)
                        {
                            clickCounter = 0;
                            AddStatePanel();
                            Repaint();
                            return;
                        }
                    }
                    if (clickCounter > 1) clickCounter = 1;
                    lastClick = new Vector2(stopMousePos.x, stopMousePos.y);
                    //if(Vector2.Distance(stopMousePos,lastClick)<.9f)

                }
                else
                    clickCounter = 0;

                StatePanel sp = GetStateForPoint(stopMousePos);

                if (sp != null && startStateSelected != null && eventConDrag != null)
                {
                    endStateSelected = sp;
                    //Debug.Log("mouseUP  "+sp.stateName);
                    EventConnection newEvent = new EventConnection(startStateSelected, endStateSelected);
                    //newState.stateDiscription = stateDiscription;
                    newEvent.eventName = eventName;
                    newEvent.id = events.Count;
                    newEvent.conditions = eventCondition;
                    newEvent.actions = eventAction;
                    events.Add(newEvent);



                }
                eventConDrag = null;

                //getConnection(stopMousePos);

                SelectStateReset();
                Repaint();
            }
            //...................................................................
            if (mouseDown)
            {
                startPos = Event.current.mousePosition;
                Vector2 startMousePos = Event.current.mousePosition;

                StatePanel sp = GetStateHandleForPoint(startMousePos);
                if (sp != null)
                {
                    startStateSelected = sp;
                    eventConDrag = new EventConnection(sp);

                }
                Repaint();
            }
            //...................................................................
            if (mouseDrag && startStateSelected != null)
            {

                Vector2 mousePos = Event.current.mousePosition;

                Vector2 pmod = new Vector2(mousePos.x + scrollPosition.x, mousePos.y + scrollPosition.y);
                mousePos = pmod;
                //if(startStateSelected!=null && eventConDrag!=null)
                if (eventConDrag != null)
                {
                    if (eventConDrag.to == null)
                        eventConDrag.to = new StatePanel(new Rect(mousePos.x, mousePos.y, 5, 5), "temp");
                    else
                    {
                        eventConDrag.to.screenRect.x = mousePos.x;
                        eventConDrag.to.screenRect.y = mousePos.y;

                        SelectState(mousePos);

                        Repaint();
                    }
                }
            }
        }
        //----------------------------------------------------------------------------

        //----------------------------------------------------------------------------
        private void SelectState(Vector2 point)
        {
            foreach (StatePanel sp in statesPanels)
            {
                if (sp.IsHolding(point))
                {
                    sp.selected = true;
                }
                else sp.selected = false;
            }
        }
        //----------------------------------------------------------------------------

        //----------------------------------------------------------------------------
        private void SelectEvent(Vector2 point)
        {
            selectedEvent = null;
            Vector2 pmod = new Vector2(point.x + scrollPosition.x, point.y + scrollPosition.y);
            foreach (EventConnection ec in events)
            {
                if (ec.IsHoldingPoint(pmod))
                {
                    ec.selected = true;
                    selectedEvent = ec;
                    eventName = ec.eventName;
                    eventCondition = ec.conditions;
                    eventAction = ec.actions;
                }
                else
                {
                    ec.selected = false;
                }
            }
        }
        //----------------------------------------------------------------------------


        //----------------------------------------------------------------------------
        private void ResetEvent(Vector2 point)
        {
            foreach (EventConnection ec in events)
            {
                ec.selected = false;
            }
            selectedEvent = null;
        }
        //----------------------------------------------------------------------------

        //----------------------------------------------------------------------------
        private void SelectStateReset()
        {
            foreach (StatePanel sp in statesPanels)
            {
                sp.selected = false;
            }
            selectedState = null;
        }
        //----------------------------------------------------------------------------

        //----------------------------------------------------------------------------
        private StatePanel GetStateHandleForPoint(Vector2 point)
        {
            Vector2 pmod = new Vector2(point.x + scrollPosition.x, point.y + scrollPosition.y);

            foreach (StatePanel sp in statesPanels)
            {
                if (sp.IsHandlerHolding(pmod)) return sp;
            }
            return null;
        }
        //----------------------------------------------------------------------------

        //----------------------------------------------------------------------------
        private StatePanel GetStateForPoint(Vector2 point)
        {
            Vector2 pmod = new Vector2(point.x + scrollPosition.x, point.y + scrollPosition.y);

            foreach (StatePanel sp in statesPanels)
            {
                if (sp.IsHolding(pmod)) return sp;
            }
            return null;
        }
        //----------------------------------------------------------------------------


        //----------------------------------------------------------------------------
        void DrawLinks()
        {
            //Debug.Log("test 1: "+events.Count);

            foreach (EventConnection ec in events)
            {

                Handles.BeginGUI();
                if (ec != null)
                    DrawEventCurve(ec.from.screenRect, ec.to.screenRect, ec);

                Handles.EndGUI();


                //  BeginWindows ();

                //windowRect2 = GUILayout.Window (2, windowRect2, DoWindow, "State2");
                //windowRect = GUILayout.Window(1,
                //EndWindows();
            }

            if (eventConDrag != null)
            {
                if (eventConDrag.to != null)
                {
                    Handles.BeginGUI();
                    DrawEventCurve(eventConDrag.from.screenRect, eventConDrag.to.screenRect, eventConDrag);
                    Handles.EndGUI();
                }
            }

            foreach (EventConnection ec in events)
            {
                ec.ShowNotSelected();
                ec.ShowSelected();
            }

        }
        //----------------------------------------------------------------------------

        //----------------------------------------------------------------------------
        NodeLocation GetNodeLocation(float angle)
        {
            if (angle >= -45 && angle < 45) return NodeLocation.right;
            if (angle >= 45 && angle < 135) return NodeLocation.top;
            if (angle >= 135 || angle < -135) return NodeLocation.left;
            if (angle >= -135 && angle < -45) return NodeLocation.bottom;

            return NodeLocation.right;
        }
        //----------------------------------------------------------------------------


        //----------------------------------------------------------------------------
        NodeLocation GetNodeLocation(Rect start, Rect end)
        {
            return GetNodeLocation(HelperGraphics.angle(start.center, end.center));
        }
        //----------------------------------------------------------------------------


        //----------------------------------------------------------------------------	
        Vector3 GetStartPos(Rect start, Rect end, NodeLocation currentNodeLocation, int repeatCount)
        {
            float adj = 0;
            if (start == end) adj = -60.0f;
            float boxAdjust = 2;

            if (currentNodeLocation == NodeLocation.right)
                return new Vector3(start.x + start.width + boxAdjust, start.y + start.height / 3 + repeatCount * 10 + adj, 0);
            if (currentNodeLocation == NodeLocation.left)
                return new Vector3(start.x - boxAdjust, start.y + start.height / 3 + repeatCount * 10 + adj, 0);
            if (currentNodeLocation == NodeLocation.top)
                return new Vector3(start.x + start.width / 3 + repeatCount * 10 + adj, start.y - boxAdjust, 0);
            if (currentNodeLocation == NodeLocation.bottom)
                return new Vector3(start.x + start.width / 3 + repeatCount * 10 + adj, start.y + start.height + boxAdjust, 0);
            
            return new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        }
        //----------------------------------------------------------------------------


        //----------------------------------------------------------------------------
        //new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 GetEndPos(Rect start, Rect end, NodeLocation currentNodeLocation, int repeatCount)
        {
            float adj = 10;
            float adj2 = 0;
            if (start == end) adj2 = 60.0f;

            if (currentNodeLocation == NodeLocation.right)
                return new Vector3(end.x - adj, end.y + end.height / 1.5f + repeatCount * 10 + adj2, 0);
            if (currentNodeLocation == NodeLocation.left)
                return new Vector3(end.x + end.width + adj, end.y + end.height / 1.5f + repeatCount * 10 + adj2, 0);
            if (currentNodeLocation == NodeLocation.top)
                return new Vector3(end.x + end.width / 1.5f + repeatCount * 10 + adj2, end.y + end.height + adj, 0);
            if (currentNodeLocation == NodeLocation.bottom)
                return new Vector3(end.x + end.width / 1.5f + repeatCount * 10 + adj2, end.y - adj, 0);
            
            return new Vector3(end.x, end.y + end.height / 1.5f, 0);
        }
        //----------------------------------------------------------------------------



        //----------------------------------------------------------------------------
        Vector3 GetStartTan(NodeLocation currentNodeLocation, float chunk, Vector3 startPos)
        {
            if (currentNodeLocation == NodeLocation.right)
                return startPos + chunk * Vector3.right;
            if (currentNodeLocation == NodeLocation.left)
                return startPos + chunk * Vector3.left;
            if (currentNodeLocation == NodeLocation.top)
                return startPos + chunk * Vector3.down;
            if (currentNodeLocation == NodeLocation.bottom)
                return startPos + chunk * Vector3.up;

            return startPos + chunk * Vector3.right;
        }
        //----------------------------------------------------------------------------



        //----------------------------------------------------------------------------
        Vector3 GetEndTan(NodeLocation currentNodeLocation, float chunk, Vector3 endPos)
        {
            if (currentNodeLocation == NodeLocation.right)
                return endPos + chunk * Vector3.left;
            if (currentNodeLocation == NodeLocation.left)
                return endPos + chunk * Vector3.right;
            if (currentNodeLocation == NodeLocation.top)
                return endPos + chunk * Vector3.up;
            if (currentNodeLocation == NodeLocation.bottom)
                return endPos + chunk * Vector3.down;

            return endPos + chunk * Vector3.right;
        }
        //----------------------------------------------------------------------------


        //----------------------------------------------------------------------------
        void DrawHandler(Vector3 point)
        {
            float boarder = 1;
            float size = 4;
            Rect box = new Rect(point.x - size / 2, point.y - size / 2, size, size);
            Rect box2 = new Rect(box.x - boarder, box.y - boarder, box.width + boarder * 2, box.height + boarder * 2);

            HelperEditor.DrawColorBox(box2, new Color(.35f, .35f, .35f));
            HelperEditor.DrawColorBox(box, new Color(.15f, .15f, .15f));
        }
        //----------------------------------------------------------------------------



        //----------------------------------------------------------------------------
        void DrawEventCurve(Rect start, Rect end, EventConnection eventCon)
        {
            NodeLocation currentNodeLocation = GetNodeLocation(start, end);

            Vector3 startPos = GetStartPos(start, end, currentNodeLocation, eventCon.fromToCount);
            Vector3 endPos = GetEndPos(start, end, currentNodeLocation, eventCon.fromToCount);

            eventCon.fromPT = startPos;
            eventCon.toPT = endPos;

            float chunk = Vector3.Distance(startPos, endPos) / 2.5f;

            Vector3 startTan = GetStartTan(currentNodeLocation, chunk, startPos);
            Vector3 endTan = GetEndTan(currentNodeLocation, chunk, endPos);//endPos +  chunk*Vector3.left;;

            Color shadowCol = new Color(0, 0, 0, 0.06f);
            Vector3 end3=new Vector3(endPos.x-(endPos.x- startPos.x)/45,endPos.y-(endPos.y- startPos.y)/45,0);

            // Draw a shadow
            for (int i = 0; i < 3; ++i)
            {
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 3) * 5);
            }
            
            float arrowSize = 18;
            Rect arrowRect = new Rect(endPos.x - arrowSize / 2, endPos.y - arrowSize / 2, arrowSize, arrowSize);
            if (currentNodeLocation == NodeLocation.bottom)
                arrow = (Texture2D)Resources.Load("Editor/arrowDown");
            else if (currentNodeLocation == NodeLocation.top)
                arrow = (Texture2D)Resources.Load("Editor/arrowUp");
            else if (currentNodeLocation == NodeLocation.left)
                 arrow = (Texture2D)Resources.Load("Editor/arrowLeft");
            else
                 arrow = (Texture2D)Resources.Load("Editor/arrowRight");


            //Handles.color = Color.white;

            // Check the current state
            Color cureveColor = Color.white;
            if (Application.isPlaying && eventCon.from.stateName == previousState && eventCon.to.stateName == currentState)
            {
                cureveColor = HelperConstants.lightOrange;
            }

            Handles.DrawBezier(startPos, endPos, startTan, endTan, cureveColor, null, 2);
            DrawHandler(startPos);
            
            GUI.Label(arrowRect, arrow);
        }
        //----------------------------------------------------------------------------


        //---------------------------------------------------------------------------------
        StatePanel GetPanelAtClick(Vector2 clickedAt)
        {
            foreach (StatePanel sp in statesPanels)
            {
                if (sp.screenRect.Contains(clickedAt)) return sp;
            }
            return null;
        }
        //---------------------------------------------------------------------------------


        //----------------------------------------------------------------------------
        void DeleteState(StatePanel sp)
        {
            statesPanels.Remove(sp);

            for (int i = events.Count - 1; i >= 0; i--)
            {
                if (events[i].from == sp || events[i].to == sp)
                {
                    EventConnection ec = (EventConnection)events[i];
                    events.Remove(ec);
                }
            }
            for (int i = 0; i < events.Count; i++)
            {
                events[i].id = i;
            }
        }
        //----------------------------------------------------------------------------


        //----------------------------------------------------------------------------
        private string GetLinkedEvents(StatePanel state)
        {
            List<string> links = new List<string>();
            foreach (EventConnection ec in events)
            {
                if (ec.from == state) links.Add(ec.id.ToString());
            }

            string outString = "";
            for (int i = 0; i < links.Count; i++)
            {
                if (i > 0) outString = outString + ":" + links[i];
                else outString = links[i];
            }

            return outString;
        }
        //----------------------------------------------------------------------------


        //----------------------------------------------------------------------------
        private string GetAttributeString()
        {
            string line = "";
            for (int i = 0; i < attributes.Count; i++)
            {
                if (i < attributes.Count - 1)
                    line = line + attributes[i].label + "=" + attributes[i].value + ";\n";
                else
                    line = line + attributes[i].label + "=" + attributes[i].value + "\n";
            }
            return line + "|\n\n";
        }
        //----------------------------------------------------------------------------


        //----------------------------------------------------------------------------
        private string GetStateString()
        {
            string line1 = "//STATES   ID,   EVENTS\n";
            int i = 0;
            foreach (StatePanel sp in statesPanels)
            {
                if (i < statesPanels.Count - 1)
                    line1 = line1 + sp.stateName + "," + sp.id + "," + GetLinkedEvents(sp) + "," + sp.screenRect.x + "," + sp.screenRect.y + "," + sp.stateDiscription + ";\n";
                else
                    line1 = line1 + sp.stateName + "," + sp.id + "," + GetLinkedEvents(sp) + "," + sp.screenRect.x + "," + sp.screenRect.y + "," + sp.stateDiscription + "\n";
                i++;
            }

            return line1 + "|\n\n";
        }
        //----------------------------------------------------------------------------


        //----------------------------------------------------------------------------
        private string GetFSMString()
        {
            return GetStateString() + GetAttributeString() + GetEventsString();
        }
        //----------------------------------------------------------------------------


        //---------------------------------------------------------------------------
        private string GetEventsString()
        {
            string line = "";
            for (int i = 0; i < events.Count; i++)
            {
                if (i < events.Count - 1)
                    line = line + events[i].eventName + "," + events[i].id + "," + events[i].to.id + "," + events[i].conditions + "," + events[i].actions + ";\n";
                else
                    line = line + events[i].eventName + "," + events[i].id + "," + events[i].to.id + "," + events[i].conditions + "," + events[i].actions + "\n";
            }
            return line;
        }
        //----------------------------------------------------------------------------


        //----------------------------------------------------------------------------
         void OnCurrentState(int windowID)
        {
             // Draw Close button
            if (!Application.isPlaying)
            {
                if (GUI.Button(new Rect(180, 0, 20, 15), (Texture2D)Resources.Load("Editor/close"), btnStyle))
                {
                    //Debug.Log("Delete "+windowID);
                    statesPanels[windowID].markedForDeath = true;
                    dirty = true;
                }
            }
            
            statesPanels[windowID].stateName = GUILayout.TextField(statesPanels[windowID].stateName);
            statesPanels[windowID].stateName = statesPanels[windowID].stateName.Replace(" ", "");
            statesPanels[windowID].stateName = statesPanels[windowID].stateName.Replace(".", "");
            statesPanels[windowID].stateName = statesPanels[windowID].stateName.Replace(",", "");
            statesPanels[windowID].stateName = statesPanels[windowID].stateName.Replace("/", "");
            statesPanels[windowID].stateName = statesPanels[windowID].stateName.Replace("\"", "");
            statesPanels[windowID].stateName = statesPanels[windowID].stateName.Replace(";", "");
            statesPanels[windowID].stateName = statesPanels[windowID].stateName.Replace(":", "");
            statesPanels[windowID].stateDiscription = GUILayout.TextArea(statesPanels[windowID].stateDiscription);

            // Draw the indicator
            if (Application.isPlaying)
            {
                if (currentState == statesPanels[windowID].stateName)
                {
                    ++runCounter;
                    int adjRC = runCounter % ((int)HelperConstants.StateWidth - 10);
                    int adjX = runCounter % ((int)HelperConstants.StateWidth - 25);
                    int adjY = runCounter % ((int)45);
                    HelperEditor.DrawColorBox(new Rect(5, HelperConstants.StateHeight - 15, 5 + adjRC, 10), HelperConstants.darkOrange);
                }
            }
            else
            {
                runCounter=0;
                //HelperEditor.DrawColorBox(new Rect(5,HelperConstants.StateHeight-10,HelperConstants.StateWidth-10,5),Color.gray);
            }
            //GUI.Box(,)

            GUI.DragWindow();
            //return null;		
        }
        //----------------------------------------------------------------------------

        private StatePanel GetStateFrom(int id)
        {
            foreach (StatePanel sp in statesPanels)
            {
                if (sp.eventsList.Contains(id)) return sp;
            }
            return null;
        }

        private void LoadFSM(string FSM)
        {
            Reset();
            //string line = FSM.Replace(" ","");
            string line = HelperFormater.stripComments(FSM.Split('\n'));
            if (line.Length == 0) return;

            string[] parts = line.Split('|');

            string[] stateParts = parts[0].Split(';');
            string[] attributeParts = parts[1].Split(';');
            string[] eventParts = parts[2].Split(';');

            for (int i = 0; i < stateParts.Length; i++)
            {
                statesPanels.Add(new StatePanel(stateParts[i]));
            }


            for (int i = 0; i < attributeParts.Length; i++)
            {
                string[] s = attributeParts[i].Split('=');
                if (s.Length > 1)
                    attributes.Add(new AttributePair(s[0], s[1]));

            }
            //name,id,to,cond,action		
            for (int i = 0; i < eventParts.Length; i++)
            {

                string[] s = eventParts[i].Split(',');
                if (s.Length > 3)
                {
                    //Debug.Log("EVENT="+eventParts[i]);
                    int id_ = int.Parse(s[1]);
                    string name_ = s[0];
                    StatePanel from_ = GetStateFrom(id_);
                    StatePanel to_ = statesPanels[int.Parse(s[2])];

                    EventConnection ec = new EventConnection(from_, to_);
                    ec.eventName = name_;
                    ec.id = id_;
                    ec.conditions = s[3];
                    ec.actions = s[4];
                    events.Add(ec);
                }

            }

        }

        private void Reset()
        {
            //source = null;
            statesPanels.Clear();
            events.Clear();
            attributes.Clear();
            clickCounter = 0;
            startStateSelected = null;
            endStateSelected = null;

            Repaint();
        }
    }
}
