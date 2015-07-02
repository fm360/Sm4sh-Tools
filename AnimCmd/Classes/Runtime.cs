﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sm4shCommand
{
    public static class Runtime
    {
        public static void GetCommandInfo(string path)
        {
            using (StreamReader stream = new StreamReader(path))
            {
                List<string> raw = stream.ReadToEnd().Split('\n').Select(x => x.Trim('\r')).ToList();
                raw.RemoveAll(x => String.IsNullOrEmpty(x) || String.IsNullOrWhiteSpace(x) || x.Contains("//"));

                for (int i = 0; i < raw.Count; i += 5)
                {
 
                    CommandDefinition h = new CommandDefinition();
                    h.Identifier = uint.Parse(raw[i], System.Globalization.NumberStyles.HexNumber);
                    h.Name = raw[i + 1];
                    string[] paramList = raw[i + 2].Split(',').Where(x => x != "NONE").ToArray();
                    string[] paramSyntax = raw[i + 3].Split(',').Where(x => x != "NONE").ToArray();
                    foreach (string kw in paramSyntax)
                        h.ParamSyntax.Add(kw);
                    foreach (string s in paramList)
                        h.ParamSpecifiers.Add(Int32.Parse(s));
                    if (raw[i + 4] != "NONE")
                        h.EventDescription = raw[i + 4];
                    if (h.Identifier == 0x5766F889 || h.Identifier == 0x89F86657)
                        _endingCommand = h;

                    commandDictionary.Add(h);
                }
            }
        }
        public static List<CommandDefinition> commandDictionary = new List<CommandDefinition>();
        public static CommandDefinition _endingCommand;
    }
}
