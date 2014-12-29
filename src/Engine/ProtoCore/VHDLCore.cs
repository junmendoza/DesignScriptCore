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

    namespace AST
    {
        public abstract class VHDLNode
        {
            public abstract string Emit();
        }

        public class PortEntry : VHDLNode
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

            public override string Emit()
            {
                string dir = string.Empty;
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

        public class SignalDeclaration : VHDLNode
        {
            public SignalDeclaration(string signal, int bits)
            {
                this.SignalName = signal;
                this.BitCount = bits;
            }

            public override string Emit()
            {
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
                        + (BitCount - 1).ToString()
                        + " "
                        + ProtoCore.VHDL.Keyword.Downto
                        + " "
                        + "0"
                        + ")";
                }

                string signalDecl =
                    ProtoCore.VHDL.Keyword.Signal
                    + " "
                    + SignalName
                    + " : "
                    + type
                    + ";";
                return signalDecl;
            }

            public string SignalName { get; private set; }
            public int BitCount { get; private set; }
        }

        public class Process : VHDLNode
        {
            public Process(string description, int processCount, List<string> sensitivityList, List<VHDLNode> varDeclaration, List<VHDLNode> body)
            {
                this.ProcessName = ProtoCore.VHDL.Constants.ProcessName.Prefix + "_" + processCount.ToString() + " " + description;
                this.SensitivityList = new List<string>(sensitivityList);
                this.Body = new List<VHDLNode>(body);
                this.VariableDeclarations = new List<VHDLNode>(varDeclaration);
            }

            public override string Emit()
            {
                StringBuilder proc = new StringBuilder();
                proc.Append(ProcessName);
                proc.Append(" : ");
                proc.Append(ProtoCore.VHDL.Keyword.Process);

                // Generate sensitivity list
                if (SensitivityList.Count > 0)
                {
                    proc.Append("(");
                    for (int i = 0; i < SensitivityList.Count; ++i)
                    {
                        proc.Append(SensitivityList[i]);
                        if (i < (SensitivityList.Count - 1))
                        {
                            proc.Append(", ");
                        }
                    }
                    proc.Append(")");
                }
                proc.Append("\n");

                // Generate variable declaration 
                if (VariableDeclarations.Count > 0)
                {
                    for (int i = 0; i < VariableDeclarations.Count; ++i)
                    {
                        proc.Append(VariableDeclarations[i]);
                        proc.Append("\n");
                    }
                }
                proc.Append("\n");

                // Generate body
                proc.Append(ProtoCore.VHDL.Keyword.Begin);
                proc.Append("\n");

                if (Body.Count > 0)
                {
                    for (int i = 0; i < Body.Count; ++i)
                    {
                        proc.Append(Body[i].Emit());
                        proc.Append("\n");
                    }
                }
                proc.Append("\n");

                proc.Append("\n");
                proc.Append(ProtoCore.VHDL.Keyword.End);
                proc.Append(" ");
                proc.Append(ProtoCore.VHDL.Keyword.Process);
                proc.Append(ProcessName);
                proc.Append(ProtoCore.VHDL.Keyword.Process);
                proc.Append(";");
                return string.Empty;
            }

            public string ProcessName { get; private set; }
            public List<string> SensitivityList { get; private set; }
            public List<VHDLNode> VariableDeclarations { get; private set; }
            public List<VHDLNode> Body { get; private set; }
        }
    }
}
