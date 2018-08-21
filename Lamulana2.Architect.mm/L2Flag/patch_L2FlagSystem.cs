using System;
using System.Collections.Generic;
using System.IO;
using L2Base;
using MonoMod.ModInterop;
using UnityEngine;

namespace L2Flag
{
    
     public class patch_L2FlagSystem : L2FlagSystem
    {
        [NonSerialized] public List<string> flagWatch = new List<string>();

        [NonSerialized] public Font currentFont = null;
        
        public patch_L2FlagSystem(L2System l2sys) : base(l2sys)
        {
            typeof(patch_L2FlagSystem).ModInterop();
        }

        public extern bool orig_setFlagData(int sheet_no, string name, short data);
        public extern bool orig_setFlagData(int sheet_no, int flag_no, short data);
        
        public bool setFlagData(int sheet_no, string name, short data)
        {
            AddFlagToWatch(sheet_no, name, data);

            return orig_setFlagData(sheet_no, name, data);
        }

        public bool setFlagData(int sheet_no, int flag_no, short data)
        {
            AddFlagToWatch(sheet_no, flag.cellData[sheet_no][flag_no + 1][0][0], data);
            
            return orig_setFlagData(sheet_no, flag_no, data);
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

            flagWatch.Add($"[{time}] {flag.seetName[sheet_no]}.{name} = {data} (diff:{difference})");
            File.AppendAllText("flags.log", flagWatch[flagWatch.Count-1] + "\r\n");
        }

        public string GetCurrentTimestamp()
        {
            short h = 0, m = 0, s = 0;
            if (!getFlag(0, 35, ref h)) return "";
            if (!getFlag(0, 36, ref m)) return "";
            if (!getFlag(0, 37, ref s)) return "";

            return $"{h:D}:{m:D2}:{s:D2}";
        }

        public List<string> GetFlagWatches()
        {
            return flagWatch;
        }
    }
}