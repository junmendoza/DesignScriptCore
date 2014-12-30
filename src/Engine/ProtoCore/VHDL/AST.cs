using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using ProtoCore.Utils;

namespace ProtoCore.VHDL.AST
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
            string path = @"..\..\..\DSAccelerate\" + this.Name + ".vhd";
            OutputFile = new StreamWriter(File.Open(path, FileMode.Create));

            LibraryList = new List<LibraryNode>();
            UseNodeList = new List<UseNode>();
            Entity = null;
            SignalDeclarationList = new List<SignalDeclarationNode>();
            ComponentList = new List<ComponentNode>();
            PortMapList = new List<PortMapNode>();
            ProcessList = new List<ProcessNode>();
            ExecutionBody = new List<VHDLNode>();
        }

        public void Emit()
        {
            Validity.Assert(OutputFile != null);
            OutputFile.Write(ToString());
        }

        public int GetProcessCount()
        {
            return ProcessList.Count;
        }

        public override string ToString()
        {
            //==============================
            // Library import
            //==============================
            StringBuilder sbLibrary = new StringBuilder();
            if (LibraryList.Count > 0)
            {
                foreach (LibraryNode node in LibraryList)
                {
                    sbLibrary.Append(node.ToString());
                    sbLibrary.Append("\n");
                }
            }

            //==============================
            // Use modules
            //==============================
            StringBuilder sbUseModules = new StringBuilder();
            if (UseNodeList.Count > 0)
            {
                foreach (UseNode node in UseNodeList)
                {
                    sbUseModules.Append(node.ToString());
                    sbUseModules.Append("\n");
                }
            }

            //==============================
            // Entity decl
            //==============================
            StringBuilder sbEntity = new StringBuilder();
            if (Entity != null)
            {
                sbEntity.Append(Entity.ToString());
            }

            //==============================
            // Architecture decl
            //==============================
            StringBuilder sbArchitectureDecl = new StringBuilder();
            sbArchitectureDecl.Append(ProtoCore.VHDL.Keyword.Architecture);
            sbArchitectureDecl.Append(" ");
            sbArchitectureDecl.Append("Behavioral");
            sbArchitectureDecl.Append(" ");
            sbArchitectureDecl.Append(ProtoCore.VHDL.Keyword.Of);
            sbArchitectureDecl.Append(" ");
            sbArchitectureDecl.Append(Name);
            sbArchitectureDecl.Append(" ");
            sbArchitectureDecl.Append(ProtoCore.VHDL.Keyword.Is);

            //==============================
            // Signal list
            //==============================
            StringBuilder sbSignalList = new StringBuilder();
            if (SignalDeclarationList.Count > 0)
            {
                foreach (SignalDeclarationNode node in SignalDeclarationList)
                {
                    sbSignalList.Append(node.ToString());
                    sbSignalList.Append("\n");
                }
            }

            //==============================
            // Component list
            //==============================
            StringBuilder sbComponents = new StringBuilder();
            if (ComponentList.Count > 0)
            {
                foreach (ComponentNode node in ComponentList)
                {
                    sbComponents.Append(node.ToString());
                    sbComponents.Append("\n");
                }
            }

            //==============================
            // Architecture begin
            //==============================
            StringBuilder sbArchitectureBegin = new StringBuilder();
            sbArchitectureBegin.Append(ProtoCore.VHDL.Keyword.Begin);

            //==============================
            // Portmap list
            //==============================
            StringBuilder sbPortMap = new StringBuilder();
            if (PortMapList.Count > 0)
            {
                foreach (PortMapNode node in PortMapList)
                {
                    sbPortMap.Append(node.ToString());
                    sbPortMap.Append("\n");
                }
            }

            //==============================
            // Process list
            //==============================
            StringBuilder sbProcess = new StringBuilder();
            if (ProcessList.Count > 0)
            {
                foreach (ProcessNode node in ProcessList)
                {
                    sbProcess.Append(node.ToString());
                    sbProcess.Append("\n");
                }
            }

            //==============================
            // Architecture end
            //==============================
            StringBuilder sbArchitectureEnd = new StringBuilder();
            sbArchitectureEnd.Append(ProtoCore.VHDL.Keyword.End);
            sbArchitectureEnd.Append(" ");
            sbArchitectureEnd.Append("Behavioral");
            sbArchitectureEnd.Append(";");

                
            StringBuilder sbModule = new StringBuilder();
            sbModule.Append(sbLibrary);
            sbModule.Append('\n');
            sbModule.Append(sbUseModules);
            sbModule.Append('\n');
            sbModule.Append(sbEntity);
            sbModule.Append('\n');
            sbModule.Append('\n');
            sbModule.Append(sbArchitectureDecl);
            sbModule.Append('\n');
            sbModule.Append('\n');
            sbModule.Append(sbSignalList);
            sbModule.Append('\n');
            sbModule.Append(sbComponents);
            sbModule.Append('\n');
            sbModule.Append(sbArchitectureBegin);
            sbModule.Append('\n');
            sbModule.Append(sbPortMap);
            sbModule.Append('\n');
            sbModule.Append(sbProcess);
            sbModule.Append('\n');
            sbModule.Append(sbArchitectureEnd);
            sbModule.Append('\n');

            return sbModule.ToString();
        }

        public string Name { get; private set; }

        public List<LibraryNode> LibraryList { get; set; }
        public List<UseNode> UseNodeList { get; set; }
        public EntityNode Entity { get; set; }
        public List<SignalDeclarationNode> SignalDeclarationList { get; set; }
        public List<ComponentNode> ComponentList { get; set; }
        public List<PortMapNode> PortMapList { get; set; }
        public List<ProcessNode> ProcessList { get; set; }

        public TextWriter OutputFile { get; private set; }

        // This list contains the current execution logic of the current process
        // This will be stored within the process 
        public List<VHDLNode> ExecutionBody { get; set; }
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

            return entity.ToString();
        }

        public List<PortEntryNode> PortEntryList { get; private set; }
        public string Name { get; private set; }
    }

    public class ComponentNode : VHDLNode
    {
        public ComponentNode(string name, List<PortEntryNode> portEntryList)
        {
            this.Name = name;
            this.PortEntryList = new List<PortEntryNode>(portEntryList);
        }

        public override string ToString()
        {
            StringBuilder entity = new StringBuilder();

            entity.Append(ProtoCore.VHDL.Keyword.Component);
            entity.Append(" ");
            entity.Append(Name);
            entity.Append(" ");
            entity.Append(ProtoCore.VHDL.Keyword.Is);
            entity.Append("\n");

            entity.Append(ProtoCore.VHDL.Utils.GeneratePortList(PortEntryList));

            entity.Append(ProtoCore.VHDL.Keyword.End);
            entity.Append(" ");
            entity.Append(ProtoCore.VHDL.Keyword.Component);
            entity.Append(" ");
            entity.Append(Name);
            entity.Append(";");

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

    public class PortMapNode : VHDLNode
    {
        public PortMapNode(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            StringBuilder portmap = new StringBuilder();
            return portmap.ToString();
        }
        public string Name { get; private set; }
    }

    public class ProcessNode : VHDLNode
    {
        public ProcessNode(string description, int processCount, List<string> sensitivityList, List<VHDLNode> varDeclaration, List<VHDLNode> body)
        {
            this.ProcessName = ProtoCore.VHDL.Constants.ProcessPrefix + "_" + processCount.ToString() + "_" + description;
            this.SensitivityList = new List<string>(sensitivityList);
            this.Body = new List<VHDLNode>(body);
            this.VariableDeclarations = new List<VHDLNode>(varDeclaration);
        }

        public override string ToString()
        {
            StringBuilder processStart = new StringBuilder();
            processStart.Append(ProcessName);
            processStart.Append(" : ");
            processStart.Append(ProtoCore.VHDL.Keyword.Process);

            // Generate sensitivity list
            if (SensitivityList.Count > 0)
            {
                processStart.Append("(");
                for (int i = 0; i < SensitivityList.Count; ++i)
                {
                    processStart.Append(SensitivityList[i]);
                    if (i < (SensitivityList.Count - 1))
                    {
                        processStart.Append(", ");
                    }
                }
                processStart.Append(")");
            }

            // Generate variable declaration 
            StringBuilder processVarDecl = new StringBuilder();
            if (VariableDeclarations.Count > 0)
            {
                for (int i = 0; i < VariableDeclarations.Count; ++i)
                {
                    processVarDecl.Append(VariableDeclarations[i]);
                    processVarDecl.Append("\n");
                }
            }


            // Generate body
            StringBuilder processBody = new StringBuilder();
            processBody.Append(ProtoCore.VHDL.Keyword.Begin);
            processBody.Append("\n");

            if (Body.Count > 0)
            {
            
                for (int i = 0; i < Body.Count; ++i)
                {
                    processBody.Append(Body[i].ToString());
                    processBody.Append("\n");
                }
            }

            StringBuilder processEnd = new StringBuilder();
            processEnd.Append("\n");
            processEnd.Append(ProtoCore.VHDL.Keyword.End);
            processEnd.Append(" ");
            processEnd.Append(ProtoCore.VHDL.Keyword.Process);
            processEnd.Append(" ");
            processEnd.Append(ProcessName);
            processEnd.Append(";");


            StringBuilder sbFormat = new StringBuilder();
            sbFormat.Append(processStart);
            sbFormat.Append("\n");
            sbFormat.Append(processVarDecl);
            sbFormat.Append("\n");
            sbFormat.Append(processBody);
            sbFormat.Append("\n");
            sbFormat.Append(processEnd);
            sbFormat.Append("\n");
            return sbFormat.ToString();
        }

        public string ProcessName { get; private set; }
        public List<string> SensitivityList { get; private set; }
        public List<VHDLNode> VariableDeclarations { get; private set; }
        public List<VHDLNode> Body { get; private set; }
    }
    
    public class AssignmentNode : VHDLNode
    {
        public AssignmentNode(VHDLNode lhs, VHDLNode rhs)
        {
            LHS = lhs;
            RHS = rhs;
        }

        public override string ToString()
        {
            StringBuilder sbFormat = new StringBuilder();
            sbFormat.Append(LHS.ToString());
            sbFormat.Append(" ");
            sbFormat.Append(ProtoCore.VHDL.Constants.kSignalAssignSymbol);
            sbFormat.Append(" ");
            sbFormat.Append(RHS.ToString());
            sbFormat.Append(";");
            return sbFormat.ToString();
        }

        public VHDLNode LHS { get; private set; }
        public VHDLNode RHS { get; private set; }
    }

    public class BinaryExpressionNode : VHDLNode
    {
        public enum Operator
        {
            Or, Nor, Xnor, And, Not, Eq
        }

        public BinaryExpressionNode(VHDLNode lhs, VHDLNode rhs, Operator optr)
        {
            this.LHS = lhs;
            this.RHS = rhs;
            this.Optr = optr;
        }

        public override string ToString()
        {
            StringBuilder sbFormat = new StringBuilder();
            sbFormat.Append(LHS.ToString());
            sbFormat.Append(" ");
            sbFormat.Append(ProtoCore.VHDL.Utils.GetBinaryOperatorString(Optr));
            sbFormat.Append(" ");
            sbFormat.Append(RHS.ToString());
            return sbFormat.ToString();
        }

        public Operator Optr { get; private set; }
        public VHDLNode LHS { get; private set; }
        public VHDLNode RHS { get; private set; }
    }

    public class IfNode : VHDLNode
    {
        public IfNode(string name = null)
        {  
            this.Name = name;

            IfExpr = null;
            IfBody = new List<VHDLNode>();

            ElsifExpr = null;
            ElsifBody = new List<VHDLNode>();

            ElseBody = new List<VHDLNode>();
        }

        public override string ToString()
        {
            StringBuilder sbIfExpr = new StringBuilder();

            // Optional name
            if (!string.IsNullOrEmpty(Name))
            {
                sbIfExpr.Append(Name);
                sbIfExpr.Append(" ");
                sbIfExpr.Append(":");
                sbIfExpr.Append(" ");
            }

            // An ifnode must always have an expression
            Validity.Assert(sbIfExpr != null);

            // If expr
            sbIfExpr.Append(ProtoCore.VHDL.Keyword.If);
            sbIfExpr.Append(" ");
            sbIfExpr.Append(IfExpr.ToString());
            sbIfExpr.Append(" ");
            sbIfExpr.Append(ProtoCore.VHDL.Keyword.Then);
            sbIfExpr.Append("\n");

            // If body
            StringBuilder sbIfBody = new StringBuilder();
            foreach (AST.VHDLNode node in IfBody)
            {
                sbIfBody.Append(node.ToString());
                sbIfBody.Append("\n");
            }
            sbIfBody.Append("\n");

            // Elsif expr
            StringBuilder sbElsifExpr = new StringBuilder();
            StringBuilder sbElsifBody = new StringBuilder();
            if (ElsifBody.Count > 0)
            {
                sbElsifExpr.Append(ProtoCore.VHDL.Keyword.Elsif);
                sbElsifExpr.Append(" ");
                sbElsifExpr.Append(ElsifExpr.ToString());
                sbElsifExpr.Append(" ");
                sbElsifExpr.Append(ProtoCore.VHDL.Keyword.Then);
                sbElsifExpr.Append("\n");

                // Elsif body
                foreach (AST.VHDLNode node in ElsifBody)
                {
                    sbElsifBody.Append(node.ToString());
                    sbElsifBody.Append("\n");
                }
            }

            // Else 
            StringBuilder sbElse = new StringBuilder();
            StringBuilder sbElseBody = new StringBuilder();
            if (ElseBody.Count > 0)
            {
                sbElse.Append(ProtoCore.VHDL.Keyword.Else);
                sbElse.Append("\n\t");

                // Else body
                foreach (AST.VHDLNode node in ElseBody)
                {
                    sbElseBody.Append(node.ToString());
                    sbElseBody.Append("\n");
                }
                sbElseBody.Append("\n"); 
            }

            StringBuilder sbEndIf = new StringBuilder();
            sbEndIf.Append(ProtoCore.VHDL.Keyword.End);
            sbEndIf.Append(" ");
            sbEndIf.Append(ProtoCore.VHDL.Keyword.If);
            sbEndIf.Append(" ");
            sbEndIf.Append(Name);
            sbEndIf.Append(";");


            StringBuilder sbFormat = new StringBuilder();
            sbFormat.Append(sbIfExpr);
            sbFormat.Append(sbIfBody);
            sbFormat.Append(sbElsifExpr);
            sbFormat.Append(sbElsifBody);
            sbFormat.Append(sbElse);
            sbFormat.Append(sbElseBody);
            sbFormat.Append(sbEndIf);
            return sbFormat.ToString();
        }

        public string Name { get; private set; }

        public VHDLNode IfExpr { get; set; }
        public List<VHDLNode> IfBody { get; set; }

        public VHDLNode ElsifExpr { get; set; }
        public List<VHDLNode> ElsifBody { get; set; }

        public List<VHDLNode> ElseBody { get; set; }
    }

    public class FunctionCallNode : VHDLNode
    {
        public FunctionCallNode(string functionName, List<VHDLNode> argList)
        {
            this.FunctionName = functionName;
            ArgumentList = new List<VHDLNode>(argList);
        }

        public override string ToString()
        {
            StringBuilder sbFormat = new StringBuilder();
            sbFormat.Append(FunctionName);
            sbFormat.Append("(");
            for (int n = 0; n < ArgumentList.Count; ++n)
            {
                sbFormat.Append(ArgumentList[n].ToString());
                if (n < ArgumentList.Count - 1)
                {
                    sbFormat.Append(", ");
                }
            }
            sbFormat.Append(")");
            return sbFormat.ToString();
        }
        public string FunctionName { get; private set; }
        public List<VHDLNode> ArgumentList { get; private set; }
    }

    public class IdentifierNode : VHDLNode
    {
        public IdentifierNode(string value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
        public string Value { get; private set; }
    }

    public class BitStringNode : VHDLNode
    {
        public BitStringNode(int value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            StringBuilder sbFormat = new StringBuilder();
            if (Value == 0 || Value == 1)
            {
                sbFormat.Append("'");
                sbFormat.Append(Value.ToString());
                sbFormat.Append("'");
                return sbFormat.ToString();
            }

            int[] bits = decimal.GetBits(Value);
            sbFormat.Append(bits.ToString());
            return sbFormat.ToString();
        }
        public int Value { get; private set; }
        public string StringFormat { get; private set; }
    }

    public class HexStringNode : VHDLNode
    {
        public HexStringNode(int value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            StringBuilder sbFormat = new StringBuilder();
            string hexString = Value.ToString("X8");
            sbFormat.Append("X" + '"' + hexString + '"');
            return sbFormat.ToString();
        }
        public int Value { get; private set; }
        public string StringFormat { get; private set; }
    }
}
