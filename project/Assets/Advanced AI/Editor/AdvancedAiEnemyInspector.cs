using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof (AdvancedAiEnemy))]
public class AdvancedAiEnemyInspector : Editor
{
    private static AdvancedAiEnemy script;
    private readonly Texture mine = Resources.Load("Preview") as Texture;
    private string dispName;
    private new string name;
    private SerializedProperty[] properties;
    private SerializedObject so;
    private Rect tooltipRect;
    private List<InspectorPlusVar> vars;

    private void OnSceneGUI()
    {
        if (GameObject.Find(script.waypointsGroup) == null) return;

        Transform wpg = GameObject.Find(script.waypointsGroup).transform;
        if (script.showHide)
        {
            for (int i = 0; i < wpg.childCount; i++)
            {
                wpg.GetChild(i).position = Handles.PositionHandle(wpg.GetChild(i).position,
                                                                  Quaternion.identity);
            }
        }
        if (GUI.changed)
            EditorUtility.SetDirty(script);
    }

    private void RefreshVars()
    {
        for (int i = 0; i < vars.Count; i += 1) properties[i] = so.FindProperty(vars[i].name);
    }

    private void OnEnable()
    {
        script = (AdvancedAiEnemy) target;
        vars = new List<InspectorPlusVar>();
        so = serializedObject;
        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "general",
                                      "generalParameters", "General Parameters", InspectorPlusVar.VectorDrawType.None,
                                      false, false, 0, new[] {false, false, false, false}, new[] {"", "", "", ""},
                                      new[] {false, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false,
                                              false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 0, "AdvancedAiEnemy",
                                      new Vector3(0.5f, 0.5f, 0f), false, true, "Tooltip", false, false, 0, 0, false, 70,
                                      "", false));
        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "navmov",
                                      "navigationRanges", "Navigation & Ranges", InspectorPlusVar.VectorDrawType.None,
                                      false, false, 0, new[] {false, false, false, false}, new[] {"", "", "", ""},
                                      new[] {false, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false,
                                              false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 0, "AdvancedAiEnemy",
                                      new Vector3(0.5f, 0.5f, 0f), false, true, "Tooltip", false, false, 0, 0, false, 70,
                                      "", false));
        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "layergroup", "layers",
                                      "Layers", InspectorPlusVar.VectorDrawType.None, false, false, 0,
                                      new[] {false, false, false, false}, new[] {"", "", "", ""},
                                      new[] {false, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false,
                                              false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 0, "AdvancedAiEnemy",
                                      new Vector3(0.5f, 0.5f, 0f), false, true, "Tooltip", false, false, 0, 0, false, 70,
                                      "", false));
        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "attackM", "attackMode",
                                      "Attack Mode", InspectorPlusVar.VectorDrawType.None, false, false, 0,
                                      new[] {false, false, false, false}, new[] {"", "", "", ""},
                                      new[] {false, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false,
                                              false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 0, "AdvancedAiEnemy",
                                      new Vector3(0.5f, 0.5f, 0f), false, true, "Tooltip", false, false, 0, 0, false, 70,
                                      "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "intelMode",
                                      "intelligenceMode",
                                      "Intelligence Mode", InspectorPlusVar.VectorDrawType.None, false, false, 0,
                                      new[] {false, false, false, false}, new[] {"", "", "", ""},
                                      new[] {false, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false,
                                              false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 0, "AdvancedAiEnemy",
                                      new Vector3(0.5f, 0.5f, 0f), false, true, "Tooltip", false, false, 0, 0, false, 70,
                                      "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "patmode", "patrolMode",
                                      "Patrol Mode", InspectorPlusVar.VectorDrawType.None, false, false, 0,
                                      new[] {false, false, false, false}, new[] {"", "", "", ""},
                                      new[] {false, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false,
                                              false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 0, "AdvancedAiEnemy",
                                      new Vector3(0.5f, 0.5f, 0f), false, true, "Tooltip", false, false, 0, 0, false, 70,
                                      "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "animgroup",
                                      "animations", "Animations", InspectorPlusVar.VectorDrawType.None, false, false, 0,
                                      new[] {false, false, false, false}, new[] {"", "", "", ""},
                                      new[] {false, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false,
                                              false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 0, "AdvancedAiEnemy",
                                      new Vector3(0.5f, 0.5f, 0f), false, true, "Tooltip", false, false, 0, 0, false, 70,
                                      "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "animgroup",
                                      "animationParameters", "Animation Parameters",
                                      InspectorPlusVar.VectorDrawType.None, false, false, 0,
                                      new[] {false, false, false, false}, new[] {"", "", "", ""},
                                      new[] {false, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false,
                                              false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 0, "AdvancedAiEnemy",
                                      new Vector3(0.5f, 0.5f, 0f), false, true, "Tooltip", false, false, 0, 0, false, 70,
                                      "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "meleeA", "meleeAttack",
                                      "Melee Attack", InspectorPlusVar.VectorDrawType.None, false, false, 0,
                                      new[] {false, false, false, false}, new[] {"", "", "", ""},
                                      new[] {false, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false,
                                              false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 0, "AdvancedAiEnemy",
                                      new Vector3(0.5f, 0.5f, 0f), false, true, "Tooltip", false, false, 0, 0, false, 70,
                                      "", false));
        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "rangedA",
                                      "rangedAttack", "Ranged Attack", InspectorPlusVar.VectorDrawType.None, false,
                                      false, 0, new[] {false, false, false, false}, new[] {"", "", "", ""},
                                      new[] {false, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false,
                                              false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 0, "AdvancedAiEnemy",
                                      new Vector3(0.5f, 0.5f, 0f), false, true, "Tooltip", false, false, 0, 0, false, 70,
                                      "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "soundgroup",
                                      "sounds", "Sounds", InspectorPlusVar.VectorDrawType.None, false,
                                      false, 0, new[] {false, false, false, false}, new[] {"", "", "", ""},
                                      new[] {false, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false,
                                              false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 0, "AdvancedAiEnemy",
                                      new Vector3(0.5f, 0.5f, 0f), false, true, "Tooltip", false, false, 0, 0, false, 70,
                                      "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "soundpara",
                                      "soundsVolume",
                                      "Sounds Volume", InspectorPlusVar.VectorDrawType.None, false, false, 100,
                                      new[] {true, false, false, false}, new[] {"<--Waypoint Editor-->", "", "", ""},
                                      new[] {true, false, false, false}, new[] {false, false, false, false},
                                      new[] {1, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, true, false, false, false, true, false, false,
                                              false, true, true, false, false
                                          },
                                      new[]
                                          {
                                              "", "", "", "", "Add Waypoint", "", "", "", "Delete Last", "", "", "",
                                              "Delete All", "Show/Hide GUI", "", ""
                                          },
                                      new[]
                                          {
                                              "", "", "", "", "AddWay", "", "", "", "DeleteWP", "", "", "", "IniWP",
                                              "ShowHide", "", ""
                                          }, new[] {false, false, false, false}, 4,
                                      "AdvancedAiEnemy", new Vector3(0.5f, 0.5f, 0f), false, true, "Tooltip", false,
                                      false, 0, 0, false, 70, "", false));
        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "Color", "linesColor",
                                      "Lines Color", InspectorPlusVar.VectorDrawType.None, false, false, 0,
                                      new[] {false, false, false, false}, new[] {"", "", "", ""},
                                      new[] {false, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false,
                                              false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 0, "AdvancedAiEnemy",
                                      new Vector3(0.5f, 0.5f, 0f), false, true, "Tooltip", false, false, 0, 0, false, 70,
                                      "", false));
        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "String",
                                      "waypointsGroup", "Waypoints Group", InspectorPlusVar.VectorDrawType.None, false,
                                      false, 48, new[] {false, false, false, false}, new[] {"", "", "", ""},
                                      new[] {false, false, false, false}, new[] {false, false, false, false},
                                      new[] {1, 0, 0, 0},
                                      new[]
                                          {
                                              true, false, false, false, false, false, false, false, false, false, false
                                              ,
                                              false, false, false, false, false
                                          },
                                      new[] {"Change Name", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"ChangeName", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 1, "AdvancedAiEnemy",
                                      new Vector3(0.5f, 0.5f, 0f), false, true, "Tooltip", false, false, 0, 0, false, 70,
                                      "", false));
        int count = vars.Count;
        properties = new SerializedProperty[count];
    }

    private void PropertyField(SerializedProperty sp, string name)
    {
        if (sp.hasChildren)
        {
            GUILayout.BeginVertical();
            while (true)
            {
                if (sp.propertyPath != name && !sp.propertyPath.StartsWith(name + "."))
                    break;

                EditorGUI.indentLevel = sp.depth;
                bool child = false;

                if (sp.depth == 0)
                    child = EditorGUILayout.PropertyField(sp, new GUIContent(dispName));
                else
                    child = EditorGUILayout.PropertyField(sp);

                if (!sp.NextVisible(child))
                    break;
            }
            EditorGUI.indentLevel = 0;
            GUILayout.EndVertical();
        }
        else EditorGUILayout.PropertyField(sp, new GUIContent(dispName));
    }

    public override void OnInspectorGUI()
    {
        SceneView.RepaintAll();
        so.Update();
        RefreshVars();
        GUILayout.Label(mine);
        EditorGUIUtility.LookLikeControls(135.0f, 50.0f);

        for (int i = 0; i < properties.Length; i += 1)
        {
            InspectorPlusVar v = vars[i];

            if (v.active && properties[i] != null)
            {
                SerializedProperty sp = properties[i];
              //  string s = v.type;
                //bool skip = false;
                name = v.name;
                dispName = v.dispName;

                GUI.enabled = v.canWrite;

                GUILayout.BeginHorizontal();

                if (v.toggleLevel != 0)
                    GUILayout.Space(v.toggleLevel*10.0f);

                PropertyField(sp, name);
                GUILayout.EndHorizontal();
                GUI.enabled = true;
            }
            if (v.space == 0.0f)
                continue;
            float usedSpace = 0.0f;
            for (int j = 0; j < v.numSpace; j += 1)
            {
                if (v.labelEnabled[j] || v.buttonEnabled[j])
                    usedSpace += 18.0f;
            }
            if (v.space == 0.0f)
                continue;
            float space = Mathf.Max(0.0f, (v.space - usedSpace)/2.0f);
            GUILayout.Space(space);
            for (int j = 0; j < v.numSpace; j += 1)
            {
                bool buttonLine = false;
                for (int k = 0; k < 4; k += 1) if (v.buttonEnabled[j*4 + k]) buttonLine = true;
                if (!v.labelEnabled[j] && !buttonLine)
                    continue;


                GUILayout.BeginHorizontal();
                if (v.labelEnabled[j])
                {
                    var boldItalic = new GUIStyle();
                    boldItalic.margin = new RectOffset(5, 5, 5, 5);

                    if (v.labelAlign[j] == 0)
                        boldItalic.alignment = TextAnchor.MiddleLeft;
                    else if (v.labelAlign[j] == 1)
                        boldItalic.alignment = TextAnchor.MiddleCenter;
                    else if (v.labelAlign[j] == 2)
                        boldItalic.alignment = TextAnchor.MiddleRight;

                    if (v.labelBold[j] && v.labelItalic[j])
                        boldItalic.fontStyle = FontStyle.BoldAndItalic;
                    else if (v.labelBold[j])
                        boldItalic.fontStyle = FontStyle.Bold;
                    else if (v.labelItalic[j])
                        boldItalic.fontStyle = FontStyle.Italic;

                    GUILayout.Label(v.label[j], boldItalic);
                    boldItalic.alignment = TextAnchor.MiddleLeft;
                }
                bool alignRight = (v.labelEnabled[j] && buttonLine);

                if (!alignRight)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }

                GUILayout.FlexibleSpace();
                for (int k = 0; k < 4; k += 1)
                {
                    if (v.buttonEnabled[j*4 + k])
                    {
                        if (!v.buttonCondense[j] && !alignRight)
                            GUILayout.FlexibleSpace();

                        string style = "Button";
                        if (v.buttonCondense[j])
                        {
                            bool hasLeft = false;
                            bool hasRight = false;
                            for (int p = k - 1; p >= 0; p -= 1)
                                if (v.buttonEnabled[j*4 + p])
                                    hasLeft = true;
                            for (int p = k + 1; p < 4; p += 1)
                                if (v.buttonEnabled[j*4 + p])
                                    hasRight = true;

                            if (!hasLeft && hasRight)
                                style = "ButtonLeft";
                            else if (hasLeft && hasRight)
                                style = "ButtonMid";
                            else if (hasLeft && !hasRight)
                                style = "ButtonRight";
                            else if (!hasLeft && !hasRight)
                                style = "Button";
                        }

                        if (GUILayout.Button(v.buttonText[j*4 + k], style, GUILayout.MinWidth(60.0f)))
                        {
                            foreach (object t in targets)
                            {
                                MethodInfo m = t.GetType()
                                                .GetMethod(v.buttonCallback[j*4 + k],
                                                           BindingFlags.Public | BindingFlags.DeclaredOnly |
                                                           BindingFlags.Instance | BindingFlags.NonPublic);
                                if (m != null)
                                    m.Invoke(target, null);
                            }
                        }

                        if (!v.buttonCondense[j] && !alignRight)
                            GUILayout.FlexibleSpace();
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            GUILayout.Space(space);
        }
        so.ApplyModifiedProperties();
    }

    public class InspectorPlusVar
    {
        public enum LimitType
        {
            None,
            Max,
            Min,
            Range
        };

        public enum VectorDrawType
        {
            None,
            Direction,
            Point,
            PositionHandle,
            Scale,
            Rotation
        };

        public bool QuaternionHandle;

        public bool active = true;
        public string[] buttonCallback = new string[16];
        public bool[] buttonCondense = new bool[4];
        public bool[] buttonEnabled = new bool[16];
        public string[] buttonText = new string[16];
        public bool canWrite = true;
        public string classType;
        public string dispName;
        public bool hasTooltip = false;
        public int iMax = -0;
        public int iMin = -0;
        public string[] label = new string[4];
        public int[] labelAlign = new int[4];
        public bool[] labelBold = new bool[4];
        public bool[] labelEnabled = new bool[4];
        public bool[] labelItalic = new bool[4];
        public bool largeTexture;
        public LimitType limitType = LimitType.None;
        public float max = -0.0f;
        public float min = -0.0f;
        public string name;

        public int numSpace = 0;
        public Vector3 offset = new Vector3(0.5f, 0.5f);
        public bool progressBar;
        public bool relative = false;
        public bool scale = false;
        public float space = 0.0f;
        public bool textArea;
        public string textFieldDefault;
        public float textureSize;
        public int toggleLevel = 0;
        public int toggleSize = 0;
        public bool toggleStart = false;
        public string tooltip;
        public string type;
        public VectorDrawType vectorDrawType;

        public InspectorPlusVar(LimitType _limitType, float _min, float _max, bool _progressBar, int _iMin, int _iMax,
                                bool _active, string _type, string _name, string _dispName,
                                VectorDrawType _vectorDrawType, bool _relative, bool _scale, float _space,
                                bool[] _labelEnabled, string[] _label, bool[] _labelBold, bool[] _labelItalic,
                                int[] _labelAlign, bool[] _buttonEnabled, string[] _buttonText,
                                string[] _buttonCallback, bool[] buttonCondense, int _numSpace, string _classType,
                                Vector3 _offset, bool _QuaternionHandle, bool _canWrite, string _tooltip,
                                bool _hasTooltip,
                                bool _toggleStart, int _toggleSize, int _toggleLevel, bool _largeTexture,
                                float _textureSize, string _textFieldDefault, bool _textArea)
        {
            limitType = _limitType;
            min = _min;
            max = _max;
            progressBar = _progressBar;
            iMax = _iMax;
            iMin = _iMin;
            active = _active;
            type = _type;
            name = _name;
            dispName = _dispName;
            vectorDrawType = _vectorDrawType;
            relative = _relative;
            scale = _scale;
            space = _space;
            labelEnabled = _labelEnabled;
            label = _label;
            labelBold = _labelBold;
            labelItalic = _labelItalic;
            labelAlign = _labelAlign;
            buttonEnabled = _buttonEnabled;
            buttonText = _buttonText;
            buttonCallback = _buttonCallback;
            numSpace = _numSpace;
            classType = _classType;
            offset = _offset;
            QuaternionHandle = _QuaternionHandle;
            canWrite = _canWrite;
            tooltip = _tooltip;
            hasTooltip = _hasTooltip;
            toggleStart = _toggleStart;
            toggleSize = _toggleSize;
            toggleLevel = _toggleLevel;
            largeTexture = _largeTexture;
            textureSize = _textureSize;
            textFieldDefault = _textFieldDefault;
            textArea = _textArea;
        }
    }
}