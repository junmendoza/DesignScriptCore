using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using ProtoCore.Utils;

namespace ProtoCore.VHDL
{
    public class VHDLCore
    {
        public VHDLCore(string topLevelModule)
        {
            MapOutputFile = new Dictionary<string, TextWriter>();

            // Setup output vhdl file
            string path = @"..\..\ControlUnit.vhd";
            TopLevelModuleName = ModuleName = topLevelModule;
            MapOutputFile[ModuleName] = new StreamWriter(File.Open(path, FileMode.Create));
        }

        /// <summary>
        /// Creates the target output vhdl file stream
        /// Sets the module name
        /// </summary>
        /// <param name="componentName"></param>
        public void SetupTargetComponent(string componentName)
        {
            ModuleName = componentName;
            if (!MapOutputFile.ContainsKey(ModuleName))
            {
                string path = @"..\..\" + componentName + ".vhd";
                MapOutputFile[ModuleName] = new StreamWriter(File.Open(path, FileMode.Create));
            }
        }

        public void SetupTargetComponentTopLevel()
        {
            SetupTargetComponent(TopLevelModuleName);
        }

        public TextWriter GetOutputStream()
        {
            Validity.Assert(MapOutputFile.Count > 0);
            Validity.Assert(MapOutputFile[ModuleName] != null);
            return MapOutputFile[ModuleName];
        }

        /// <summary>
        /// Saves the destination vhdl files by flushing the output streams
        /// </summary>
        public void CommitOutputStream()
        {
            foreach (var kvp in MapOutputFile)
            {
                kvp.Value.Flush();
            }
        }

        public Dictionary<string, TextWriter> MapOutputFile { get; private set; }
        public string TopLevelModuleName { get; private set; }

        public string ModuleName { get; set; }
    }

    public class PortEntry
    {
        public enum Direction
        {
            In,
            Out,
            InOut
        }

        public PortEntry(string signal, Direction direction, int bits)
        {
            this.SignalName = signal;
            this.EntryDirection = direction;
            this.BitCount = bits;
        }

        public string Emit()
        {
            string dir =  string.Empty;
            if (EntryDirection == Direction.In)
            {
                dir = ProtoCore.VHDL.Keyword.In;
            }
            else if (EntryDirection == Direction.In)
            {
                dir = ProtoCore.VHDL.Keyword.Out;
            }
            else
            {
                dir = ProtoCore.VHDL.Keyword.Inout;
            }

            string type = string.Empty;
            if (BitCount == 1)
            {
                type = ProtoCore.VHDL.Keyword.Std_logic;
            }
            else
            {
                type = 
                    ProtoCore.VHDL.Keyword.Std_logic_vector
                    + "("
                    + " "
                    + (BitCount - 1).ToString()
                    + " "
                    + ProtoCore.VHDL.Keyword.Downto
                    + " "
                    + "0"
                    + ")";
            }

            string entry = SignalName + " : " + dir + " " + type;
            return entry;        
        }

        public string SignalName { get; private set; }
        public Direction EntryDirection { get; private set; }
        public int BitCount { get; private set; }
    }

}
