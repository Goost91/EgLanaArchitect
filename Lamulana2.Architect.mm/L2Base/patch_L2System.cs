using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using MonoMod;
using L2Base;
using L2IO;
using L2Menu;
using MonoMod.ModInterop;
using UnityEngine;

namespace L2Base
{
    public class patch_L2System : L2System
    {
        public List<string> flagWatch = new List<string>();

        public Font currentFont = null;

        public bool showScore = true;

        public extern void orig_Update();
        public extern void orig_Awake();
        public extern bool orig_setFlagData(int sheet_no, string name, short data);
        public extern bool orig_setFlagData(int sheet_no, int flag_no, short data);

        public void Awake()
        {
            File.Delete("flags.log");
            orig_Awake();
        }
        
        public bool setFlagData(int sheet_no, string name, short data)
        {
            AddFlagToWatch(sheet_no, name, data);

            return orig_setFlagData(sheet_no, name, data);
        }

        public bool setFlagData(int sheet_no, int flag_no, short data)
        {
            AddFlagToWatch(sheet_no, getFlagSys().flag.cellData[sheet_no][flag_no + 1][0][0], data);
            
            return orig_setFlagData(sheet_no, flag_no, data);
        }

        public void Update()
        {
            if (l2sdk == null)
            {
                l2sdk = new L2SystemDebugKeys(this);
            }

            orig_Update();

            SaveAnywhereHandler();
        }

        public void SaveAnywhereHandler()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                float x = getPlayer().transform.position.x;
                float y = getPlayer().transform.position.y;
                int warpPointNo = FindObjectOfType<HolyTabretScript>().warpPointNo;

                l2sal.setSaveBeforSet(x, y, warpPointNo);

                openSaveAndLoadCanvas(SAVELOAD.save, null, "", x, y, warpPointNo);
            }
        }

        public void AddFlagToWatch(int sheet_no, string name, short data)
        {
            if (name.StartsWith("playtime")) return;
            if (name == "Gold" || name == "weight" || name == "Playtime") return;
            
            if (flagWatch == null)
            {
                flagWatch = new List<String>();
            }

            var time = GetCurrentTimestamp();
            short oldData = 0;
            if (!getFlag(sheet_no, name, ref oldData)) return;

            short difference = (short) (data - oldData);

            if (difference == 0) 
                return;

            flagWatch.Add($"[{time}] {getFlagSys().flag.seetName[sheet_no]}.{name} = {data} (diff:{difference})");
            //File.AppendAllText("flags.log", flagWatch[flagWatch.Count-1] + "\r\n");
        }

        public string GetCurrentTimestamp()
        {
            short h = 0, m = 0, s = 0;
            if (!getFlag(0, 35, ref h)) return "";
            if (!getFlag(0, 36, ref m)) return "";
            if (!getFlag(0, 37, ref s)) return "";

            return $"{h:D}:{m:D2}:{s:D2}";
        }

        public void OnGUI()
        {
            if (Input.GetKeyDown(KeyCode.End))
            {
                showScore = !showScore;
            }

            if (currentFont == null)
            {
                currentFont = Font.CreateDynamicFontFromOSFont("Consolas", 14);
            }

            GUIStyle guistyle = new GUIStyle(GUI.skin.label);
            guistyle.normal.textColor = Color.white;
            guistyle.fontStyle = FontStyle.Bold;
            guistyle.font = currentFont;
            guistyle.fontSize = 14;

            short score = 0;
            getFlag(3, 30, ref score);
            GUI.Label(new Rect(0f, 0f, 30f, 30f), string.Concat(score), guistyle);
            
            GUIContent timer = new GUIContent(GetCurrentTimestamp());
            Vector2 size = guistyle.CalcSize(timer);

            GUI.Label(new Rect(Screen.width - size.x, Screen.height - size.y, size.x, size.y), timer, guistyle);
            
            if (flagWatch.Count < 1) 
                return;

            guistyle.fontSize = 10;
            GUIContent flw1 = new GUIContent(flagWatch[flagWatch.Count - 1] + "\r\n" + flagWatch[flagWatch.Count - 2] + "\r\n" + flagWatch[flagWatch.Count - 3]);
            Vector2 flw1Size = guistyle.CalcSize(flw1);
            GUI.Label(new Rect(0, Screen.height - flw1Size.y, flw1Size.x, flw1Size.y), flw1, guistyle);
            
            try
            {
                GUIContent flw2 = new GUIContent(flagWatch[flagWatch.Count - 4] + "\r\n" +
                                                 flagWatch[flagWatch.Count - 5] + "\r\n" +
                                                 flagWatch[flagWatch.Count - 6]);
                Vector2 flw2Size = guistyle.CalcSize(flw2);
                GUI.contentColor = Color.grey;
                GUI.Label(new Rect(flw1Size.x + 20, Screen.height - flw1Size.y, flw2Size.x, flw2Size.y), flw2,
                    guistyle);
            }
            catch (Exception e) {}
        }
    }
}