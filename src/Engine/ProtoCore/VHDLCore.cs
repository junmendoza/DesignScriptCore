using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ProtoCore.VHDL
{
    public class VHDLCore
    {
        public VHDLCore(string topLevelModule)
        {
            // Setup output vhdl file
            string path = @"..\..\ControlUnit.vhd";
            OutputFile = new StreamWriter(File.Open(path, FileMode.Create));

            TopLevelModuleName = ModuleName = topLevelModule;
        }

        public TextWriter OutputFile { get; private set; }
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
