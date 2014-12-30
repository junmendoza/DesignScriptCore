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
            TopLevelModuleName = ModuleName = topLevelModule;

            ModuleMap = new Dictionary<string, AST.ModuleNode>();
            ModuleMap[ModuleName] = new AST.ModuleNode(TopLevelModuleName);
        }

        /// <summary>
        /// Creates the target output vhdl file stream
        /// Sets the module name
        /// </summary>
        /// <param name="componentName"></param>
        public void SetupTargetComponent(string componentName)
        {
            ModuleName = componentName;
            if (!ModuleMap.ContainsKey(ModuleName))
            {
                ModuleMap[ModuleName] = new AST.ModuleNode(ModuleName);
            }
        }

        public void SetupTargetComponentTopLevel()
        {
            SetupTargetComponent(TopLevelModuleName);
        }

        public TextWriter GetOutputStream()
        {
            Validity.Assert(ModuleMap.Count > 0);
            Validity.Assert(ModuleMap[ModuleName] != null);
            return ModuleMap[ModuleName].OutputFile;
        }

        /// <summary>
        /// Saves the destination vhdl files by flushing the output streams
        /// </summary>
        public void CommitOutputStream()
        {
            foreach (var kvp in ModuleMap)
            {
                kvp.Value.OutputFile.Flush();
            }
        }

        public Dictionary<string, AST.ModuleNode> ModuleMap { get; private set; }
        public string TopLevelModuleName { get; private set; }

        public string ModuleName { get; set; }
    }

    namespace AST
    {
        public abstract class VHDLNode
        {
        }

        public class ModuleNode : VHDLNode
        {
            public ModuleNode(string moduleName)
            {
                this.Name = moduleName;

                // Setup output vhdl file
                string path = @"..\..\" + this.Name + ".vhd";
                OutputFile = new StreamWriter(File.Open(path, FileMode.Create));
            }

            public override string ToString()
            {
                return string.Empty;
            }

            public string Name { get; private set; }
            public TextWriter OutputFile { get; private set; }
        }

        public class LibraryNode : VHDLNode
        {
            public LibraryNode(string libraryName)
            {
                this.Name = libraryName;
            }

            public override string ToString()
            {
                StringBuilder library = new StringBuilder();

                library.Append(ProtoCore.VHDL.Keyword.Library);
                library.Append(" ");
                library.Append(Name);
                library.Append(";");
                library.Append("\n");

                return library.ToString();
            }
            public string Name { get; private set; }
        }

        public class UseNode : VHDLNode
        {
            public UseNode(string moduleName)
            {
                this.Name = moduleName;
            }

            public override string ToString()
            {
                StringBuilder use = new StringBuilder();

                use.Append(ProtoCore.VHDL.Keyword.Use);
                use.Append(" ");
                use.Append(Name);
                use.Append(";");
                use.Append("\n");

                return use.ToString();
            }
            public string Name { get; private set; }
        }

        public class EntityNode : VHDLNode
        {
            public EntityNode(string name, List<PortEntryNode> portEntryList)
            {
                this.Name = name;
                this.PortEntryList = new List<PortEntryNode>(portEntryList);
            }

            public override string ToString()
            {
                StringBuilder entity = new StringBuilder();
                
                entity.Append(ProtoCore.VHDL.Keyword.Entity);
                entity.Append(" ");
                entity.Append(Name);
                entity.Append(" ");
                entity.Append(ProtoCore.VHDL.Keyword.Is);
                entity.Append("\n");

                entity.Append(ProtoCore.VHDL.Utils.GeneratePortList(PortEntryList));

                entity.Append(ProtoCore.VHDL.Keyword.End);
                entity.Append(" ");
                entity.Append(Name);
                entity.Append(";");
                entity.Append("\n\n");

                return entity.ToString();
            }

            public List<PortEntryNode> PortEntryList { get; private set; }
            public string Name { get; private set; }
        }

        public class PortEntryNode : VHDLNode
        {
            public enum Direction
            {
                In,
                Out,
                InOut
            }

            public PortEntryNode(string signal, Direction direction, int bits)
            {
                this.SignalName = signal;
                this.EntryDirection = direction;
                this.BitCount = bits;
            }

            public override string ToString()
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

        public class SignalDeclarationNode : VHDLNode
        {
            public SignalDeclarationNode(string signal, int bits)
            {
                this.SignalName = signal;
                this.BitCount = bits;
            }

            public override string ToString()
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

        public class ProcessNode : VHDLNode
        {
            public ProcessNode(string description, int processCount, List<string> sensitivityList, List<VHDLNode> varDeclaration, List<VHDLNode> body)
            {
                this.ProcessName = ProtoCore.VHDL.Constants.ProcessName.Prefix + "_" + processCount.ToString() + " " + description;
                this.SensitivityList = new List<string>(sensitivityList);
                this.Body = new List<VHDLNode>(body);
                this.VariableDeclarations = new List<VHDLNode>(varDeclaration);
            }

            public override string ToString()
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
                        proc.Append(Body[i].ToString());
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
