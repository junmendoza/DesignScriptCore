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
        public ModuleNode(string moduleName, bool isBuiltIn = false)
        {
            this.Name = moduleName;
            IsTopModule = false;

            OutputFile = null;
            if (!isBuiltIn)
            {
                // Setup output vhdl file
                // A vhd file is generated for user defined components
                string path = @"..\..\..\DSAccelerate\" + this.Name + ".vhd";
                OutputFile = new StreamWriter(File.Open(path, FileMode.Create));
            }

            this.LibraryList = new List<LibraryNode>();
            this.UseNodeList = new List<UseNode>();
            this.Entity = null;
            this.SignalDeclarationList = new List<SignalDeclarationNode>();
            this.ComponentList = new List<ComponentNode>();
            this.PortMapList = new List<PortMapNode>();
            this.ProcessList = new List<ProcessNode>();
            this.ExecutionBody = new List<VHDLNode>();
            this.ReturnSignalName = string.Empty;
            this.IsBuiltIn = isBuiltIn;
            this.DefinedArrayTypes = new Dictionary<string, ArrayTypeDefinitionNode>();
        }

        public void Emit()
        {
            if (OutputFile != null)
            {
                OutputFile.Write(ToString());
                OutputFile.Flush();
            }
        }

        private List<VHDLNode> GetCurrentExecutionBody()
        {
            IfNode resetSync = null;
            IfNode clockSync = null;
            IfNode executionStartFlagIf = null;

            // The execution body in the top level module is at the elsif body of the ClockSync
            ProcessNode procNode = ProcessList[ProcessList.Count - 1];
            bool isFirstProcessOnTopModule = IsTopModule == true && ProcessList.Count == 1;
            if (isFirstProcessOnTopModule)
            {
                //proc : process(clock)
                //begin
                //    ResetSync : if reset = '1' then
                //        execution_started <= '0';
                //    elsif reset = '0' then
                //        ClockSync : if rising_edge(clock) then
                //            if execution_started = '0' then       <-- Process body
                resetSync = procNode.Body[0] as IfNode;
                Validity.Assert(resetSync != null);

                clockSync = resetSync.ElsifBodyList[0][0] as IfNode;
                Validity.Assert(clockSync != null);

                executionStartFlagIf = clockSync.IfBody[0] as IfNode;
                Validity.Assert(executionStartFlagIf != null);

                return executionStartFlagIf.IfBody;
            }

            // The execution body of a process is the elsif body of the ResetSync

            //proc : process(a)
            //begin
            //    ResetSync : if reset = '1' then
            //
            //    elsif reset = '0' then
            //            x <= a                     <-- Process body
            resetSync = procNode.Body[0] as IfNode;
            Validity.Assert(resetSync != null);

            return resetSync.ElsifBodyList[0];
        }

        /// <summary>
        /// The current process instantiates its own copy and contents of the execution body 
        /// This function must be called before creating a new process
        /// The execution body is cleared
        /// </summary>
        public void CloseCurrentProcess()
        {
            Validity.Assert(ProcessList != null);
            Validity.Assert(ProcessList.Count > 0);
          
            // Create a copy of the current process body
            List<VHDLNode> execBody = GetCurrentExecutionBody();
            execBody = new List<VHDLNode>(execBody);
            ExecutionBody = new List<VHDLNode>();
        }

        public int GetProcessCount()
        {
            return ProcessList.Count;
        }

        public ProcessNode GetCurrentProcess()
        {
            Validity.Assert(ProcessList != null);
            Validity.Assert(ProcessList.Count > 0);
            return ProcessList[ProcessList.Count-1];
        }

        /// <summary>
        /// Appends a compiled DS script statment to the execution body
        /// </summary>
        /// <param name="executionStmt"></param>
        public void AppendExecutionStatement(VHDLNode executionStmt)
        {
            ExecutionBody.Add(executionStmt);

            // If the stmt is a signal assignment, update the lhs signal
            AssignmentNode assignNode = executionStmt as AssignmentNode;
            if (assignNode != null)
            {
                ProcessNode process = GetCurrentProcess();
                Validity.Assert(assignNode.LHS is IdentifierNode);
                process.AssignedSignals.Add((assignNode.LHS as IdentifierNode).Value);
            }
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
            sbArchitectureDecl.Append(ProtoCore.VHDL.Constants.Keyword.Architecture);
            sbArchitectureDecl.Append(" ");
            sbArchitectureDecl.Append("Behavioral");
            sbArchitectureDecl.Append(" ");
            sbArchitectureDecl.Append(ProtoCore.VHDL.Constants.Keyword.Of);
            sbArchitectureDecl.Append(" ");
            sbArchitectureDecl.Append(Name);
            sbArchitectureDecl.Append(" ");
            sbArchitectureDecl.Append(ProtoCore.VHDL.Constants.Keyword.Is);

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
                    sbComponents.Append("\n\n");
                }
            }

            //==============================
            // Architecture begin
            //==============================
            StringBuilder sbArchitectureBegin = new StringBuilder();
            sbArchitectureBegin.Append(ProtoCore.VHDL.Constants.Keyword.Begin);

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
            sbArchitectureEnd.Append(ProtoCore.VHDL.Constants.Keyword.End);
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

            // TODO Jun: Temp vars in DS are assigned with '%' at codegen
            // Handle this here for now.
            sbModule = sbModule.Replace("%tSSA_", "tSSA_");

            return sbModule.ToString();
        }

        public string Name { get; private set; }
        public bool IsTopModule { get; set; }

        public List<LibraryNode> LibraryList { get; set; }
        public List<UseNode> UseNodeList { get; set; }
        public EntityNode Entity { get; set; }
        public List<SignalDeclarationNode> SignalDeclarationList { get; set; }
        public List<ComponentNode> ComponentList { get; set; }
        public List<PortMapNode> PortMapList { get; set; }
        public List<ProcessNode> ProcessList { get; set; }
        public string ReturnSignalName { get; set; }
        public Dictionary<string, ArrayTypeDefinitionNode> DefinedArrayTypes { get; set; }

        public TextWriter OutputFile { get; private set; }
        public bool IsBuiltIn { get; private set; }

        // This list contains the current execution logic of the current process
        // This will be stored within the process 
        public List<VHDLNode> ExecutionBody { get; private set; }
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

            library.Append(ProtoCore.VHDL.Constants.Keyword.Library);
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

            use.Append(ProtoCore.VHDL.Constants.Keyword.Use);
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
                
            entity.Append(ProtoCore.VHDL.Constants.Keyword.Entity);
            entity.Append(" ");
            entity.Append(Name);
            entity.Append(" ");
            entity.Append(ProtoCore.VHDL.Constants.Keyword.Is);
            entity.Append("\n");

            entity.Append(ProtoCore.VHDL.Utils.GeneratePortList(PortEntryList));

            entity.Append(ProtoCore.VHDL.Constants.Keyword.End);
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

            entity.Append(ProtoCore.VHDL.Constants.Keyword.Component);
            entity.Append(" ");
            entity.Append(Name);
            entity.Append(" ");
            entity.Append(ProtoCore.VHDL.Constants.Keyword.Is);
            entity.Append("\n");

            entity.Append(ProtoCore.VHDL.Utils.GeneratePortList(PortEntryList));

            entity.Append(ProtoCore.VHDL.Constants.Keyword.End);
            entity.Append(" ");
            entity.Append(ProtoCore.VHDL.Constants.Keyword.Component);
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

        public PortEntryNode(string signal, Direction direction, int bits, bool setDefault = false)
        {
            this.SignalName = signal;
            this.EntryDirection = direction;
            this.BitCount = bits;
            this.generateDefault = setDefault;
        }

        public override string ToString()
        {
            string dir = string.Empty;
            if (EntryDirection == Direction.In)
            {
                dir = ProtoCore.VHDL.Constants.Keyword.In;
            }
            else if (EntryDirection == Direction.Out)
            {
                dir = ProtoCore.VHDL.Constants.Keyword.Out;
            }
            else
            {
                dir = ProtoCore.VHDL.Constants.Keyword.Inout;
            }

            string type = string.Empty;
            if (BitCount == 1)
            {
                type = ProtoCore.VHDL.Constants.Keyword.Std_logic;
            }
            else
            {
                type =
                    ProtoCore.VHDL.Constants.Keyword.Std_logic_vector
                    + "("
                    + (BitCount - 1).ToString()
                    + " "
                    + ProtoCore.VHDL.Constants.Keyword.Downto
                    + " "
                    + "0"
                    + ")";
                if (generateDefault)
                {
                    // := (others => '0')
                    type +=
                        " "
                        + ProtoCore.VHDL.Constants.Naming.kVariableAssignSymbol
                        + " "
                        + "("
                        + ProtoCore.VHDL.Constants.Keyword.Others
                        + " "
                        + ProtoCore.VHDL.Constants.Naming.kDefaultAssignSymbol
                        + " "
                        + "'0'"
                        + ")";
                }
            }

            string entry = SignalName + " : " + dir + " " + type;
            return entry;
        }

        public string SignalName { get; private set; }
        public Direction EntryDirection { get; private set; }
        public int BitCount { get; private set; }
        private bool generateDefault;
    }

    /// <summary>
    /// This node represents an array type definition
    /// It needs to be extended to support multiple types
    /// It currently supports and array of 32bit signals
    /// </summary>
    public class ArrayTypeDefinitionNode
    {
        public ArrayTypeDefinitionNode(List<int> dimensionList)
        {
            int elems = dimensionList[0];
            ArrayTypeName = ProtoCore.VHDL.Utils.GenerateArrayTypeName(elems, ProtoCore.VHDL.Constants.Numeric.SignalBitCount);
            this.DimensionList = new List<int>(dimensionList);
        }

        public override string ToString()
        {
	        // type t_array_3_32 is array (0 to 2) of STD_LOGIC_VECTOR(31 downto 0);
            StringBuilder sbArrayType = new StringBuilder();
            sbArrayType.Append(ProtoCore.VHDL.Constants.Keyword.Type);
            sbArrayType.Append(" ");
            sbArrayType.Append(ArrayTypeName);
            sbArrayType.Append(" ");
            sbArrayType.Append(ProtoCore.VHDL.Constants.Keyword.Is);


            StringBuilder sbDimension = new StringBuilder();
            sbDimension.Append(ProtoCore.VHDL.Constants.Keyword.Array);
            sbDimension.Append(" (0 ");
            sbDimension.Append(ProtoCore.VHDL.Constants.Keyword.To);
            sbDimension.Append(" ");
            sbDimension.Append(DimensionList[0].ToString());
            sbDimension.Append(")");

            StringBuilder sbElementType = new StringBuilder();
            sbElementType.Append(ProtoCore.VHDL.Constants.Keyword.Std_logic_vector);
            sbElementType.Append("(");
            sbElementType.Append(ProtoCore.VHDL.Constants.Numeric.SignalBitCount - 1);
            sbElementType.Append(" ");
            sbElementType.Append(ProtoCore.VHDL.Constants.Keyword.Downto);
            sbElementType.Append(" 0)");

            StringBuilder sbFormat = new StringBuilder();
            sbFormat.Append(sbArrayType);
            sbFormat.Append(" ");
            sbFormat.Append(sbDimension);
            sbFormat.Append(" ");
            sbFormat.Append(ProtoCore.VHDL.Constants.Keyword.Of);
            sbFormat.Append(" ");
            sbFormat.Append(sbElementType);
            sbFormat.Append(";");
            return sbFormat.ToString();
        }
        public string ArrayTypeName { get; private set; }
        public List<int> DimensionList { get; private set; }
    }

    public class VariableDeclarationNode : VHDLNode
    {
        public VariableDeclarationNode(string name, string type)
        {
            this.Name = name;
            this.Type = type;
        }

        public override string ToString()
        {
            return ProtoCore.VHDL.Constants.Keyword.Variable + " " + Name + ":" + " " + Type + ";"; 
        }

        public string Name { get; private set; }
        public string Type { get; private set; }
    }

    public class SignalDeclarationNode : VHDLNode
    {
        public SignalDeclarationNode(string signal, ArrayTypeDefinitionNode arrayType = null, string arrayTypeName = null, int bits = ProtoCore.VHDL.Constants.Numeric.SignalBitCount)
        {
            SignalName = signal;
            BitCount = bits;
            ArrayType = arrayType;
            ArrayTypeName = arrayTypeName;
        }

        public SignalDeclarationNode(ProtoCore.DSASM.SymbolNode symbol, ArrayTypeDefinitionNode arrayType = null, string arrayTypeName = null, int bits = ProtoCore.VHDL.Constants.Numeric.SignalBitCount)
        {
           DSSymbol = symbol;
           SignalName = DSSymbol.name;
           BitCount = ProtoCore.VHDL.Constants.Numeric.SignalBitCount;
           ArrayTypeName = arrayTypeName;

           int elements = DSSymbol.size;
           ArrayType = arrayType;

           if (DSSymbol.staticType.UID == (int)ProtoCore.PrimitiveType.kTypeBool)
           {
               BitCount = 1;
           }
           else
           {
               // Handle other data type size here
               BitCount = ProtoCore.VHDL.Constants.Numeric.SignalBitCount;
           }
        }

        private string GetSignalDeclaration()
        {
            string type = string.Empty;
            if (BitCount == 1)
            {
                type = ProtoCore.VHDL.Constants.Keyword.Std_logic;
            }
            else
            {
                type =
                    ProtoCore.VHDL.Constants.Keyword.Std_logic_vector
                    + "("
                    + (BitCount - 1).ToString()
                    + " "
                    + ProtoCore.VHDL.Constants.Keyword.Downto
                    + " "
                    + "0"
                    + ")";
            }

            string signalDecl =
                ProtoCore.VHDL.Constants.Keyword.Signal
                + " "
                + SignalName
                + " : "
                + type
                + ";";
            return signalDecl;
        }

        private string GetArraySignalDeclaration()
        {
            StringBuilder sbArrayType = new StringBuilder();
            if (ArrayType != null)
            {
                sbArrayType.Append(ArrayType.ToString());
            }

            StringBuilder sbSignalDecl = new StringBuilder();
            sbSignalDecl.Append(ProtoCore.VHDL.Constants.Keyword.Signal);
            sbSignalDecl.Append(" ");
            sbSignalDecl.Append(SignalName);
            sbSignalDecl.Append(" : ");
            sbSignalDecl.Append(ArrayTypeName);
            sbSignalDecl.Append(";");

            StringBuilder sbFormat = new StringBuilder();
            sbFormat.Append(ArrayType);
            sbFormat.Append("\n");
            sbFormat.Append(sbSignalDecl);
            sbFormat.Append("\n");
            return sbFormat.ToString();
        }

        public override string ToString()
        {
            string signalDecl = string.Empty;
            bool isArrayDecl = ArrayTypeName != null;
            if (isArrayDecl)
            {
                Validity.Assert(DSSymbol.size > 1);
                signalDecl = GetArraySignalDeclaration();
            }
            else
            {
                signalDecl = GetSignalDeclaration();
            }
            return signalDecl;
        }

        public ProtoCore.DSASM.SymbolNode DSSymbol { get; private set; }
        public string SignalName { get; private set; }
        public int BitCount { get; private set; }
        public ArrayTypeDefinitionNode ArrayType { get; private set; }
        public string ArrayTypeName { get; private set; }

    }

    public class PortMapNode : VHDLNode
    {
        public PortMapNode(string instanceName, ComponentNode component, List<string> signalMap)
        {
            this.Name = instanceName;
            this.Component = component;
            this.SignalMap = new List<string>(signalMap);
        }

        public override string ToString()
        {
            //InstanceName : ComponentName port map
            //(
            //    reset => reset,
            //    a => local_signal,
            //    return_val => return_val_local_signal
            //);
		
            StringBuilder portmapDecl = new StringBuilder();

            // Declaration
            portmapDecl.Append(Name);
            portmapDecl.Append(" : ");
            portmapDecl.Append(Component.Name);
            portmapDecl.Append(" ");
            portmapDecl.Append(ProtoCore.VHDL.Constants.Keyword.Port);
            portmapDecl.Append(" ");
            portmapDecl.Append(ProtoCore.VHDL.Constants.Keyword.Map);

            // Signal map
            StringBuilder portmapSignals = new StringBuilder();
            portmapSignals.Append("(");
            portmapSignals.Append("\n");

            Validity.Assert(Component.PortEntryList.Count == SignalMap.Count);
            for (int n = 0; n < SignalMap.Count; ++n)
            {
                portmapSignals.Append(Component.PortEntryList[n].SignalName);
                portmapSignals.Append(" ");
                portmapSignals.Append("=>");
                portmapSignals.Append(" ");
                portmapSignals.Append(SignalMap[n]);
                if (n < (SignalMap.Count - 1))
                {
                    portmapSignals.Append(",");
                }
                portmapSignals.Append("\n");
            }
            portmapSignals.Append(");");
            
            StringBuilder sbFormat = new StringBuilder();
            sbFormat.Append(portmapDecl);
            sbFormat.Append("\n");
            sbFormat.Append(portmapSignals);

            return sbFormat.ToString();
        }

        public string Name { get; private set; }
        public ComponentNode Component { get; private set; }
        public List<string> SignalMap { get; private set; }
    }

    public class ProcessNode : VHDLNode
    {
        public ProcessNode(string name, List<string> sensitivityList, List<VariableDeclarationNode> varDeclaration, List<VHDLNode> body)
        {
            this.ProcessName = name;  
            this.SensitivityList = new List<string>(sensitivityList);
            this.Body = new List<VHDLNode>(body);
            this.VariableDeclarations = new List<VariableDeclarationNode>(varDeclaration);
            AssignedSignals = new HashSet<string>();
        }
        
        public override string ToString()
        {
            StringBuilder processStart = new StringBuilder();
            processStart.Append(ProcessName);
            processStart.Append(" : ");
            processStart.Append(ProtoCore.VHDL.Constants.Keyword.Process);

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
            processBody.Append(ProtoCore.VHDL.Constants.Keyword.Begin);
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
            processEnd.Append(ProtoCore.VHDL.Constants.Keyword.End);
            processEnd.Append(" ");
            processEnd.Append(ProtoCore.VHDL.Constants.Keyword.Process);
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
        public HashSet<string> AssignedSignals { get; private set; }
        public List<VariableDeclarationNode> VariableDeclarations { get; private set; }
        public List<VHDLNode> Body { get; set; }
    }
    
    public class AssignmentNode : VHDLNode
    {
        public AssignmentNode(VHDLNode lhs, VHDLNode rhs, bool isSignal = true)
        {
            LHS = lhs;
            RHS = rhs;
            IsSignalAssignment = isSignal;
        }

        public override string ToString()
        {
            StringBuilder sbFormat = new StringBuilder();
            sbFormat.Append(LHS.ToString());
            sbFormat.Append(" ");
            if (IsSignalAssignment)
            {
                sbFormat.Append(ProtoCore.VHDL.Constants.Naming.kSignalAssignSymbol);
            }
            else
            {
                sbFormat.Append(ProtoCore.VHDL.Constants.Naming.kVariableAssignSymbol);
            }
            sbFormat.Append(" ");
            sbFormat.Append(RHS.ToString());
            sbFormat.Append(";");
            return sbFormat.ToString();
        }

        public VHDLNode LHS { get; private set; }
        public VHDLNode RHS { get; private set; }
        public bool IsSignalAssignment { get; private set; }
    }

    public class BinaryExpressionNode : VHDLNode
    {
        public enum Operator
        {
            Or, Nor, Xnor, And, Not, Eq, Add, Sub, Mul, Div
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

            ElsifExprList = new List<VHDLNode>();
            ElsifBodyList = new List<List<VHDLNode>>();

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
            sbIfExpr.Append(ProtoCore.VHDL.Constants.Keyword.If);
            sbIfExpr.Append(" ");
            sbIfExpr.Append(IfExpr.ToString());
            sbIfExpr.Append(" ");
            sbIfExpr.Append(ProtoCore.VHDL.Constants.Keyword.Then);
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
            StringBuilder sbElsifStmt = new StringBuilder();
            if (ElsifExprList.Count > 0)
            {
                for (int i = 0; i < ElsifExprList.Count; ++i)
                {
                    VHDLNode exprNode = ElsifExprList[i];
                    sbElsifStmt.Append(ProtoCore.VHDL.Constants.Keyword.Elsif);
                    sbElsifStmt.Append(" ");
                    sbElsifStmt.Append(exprNode.ToString());
                    sbElsifStmt.Append(" ");
                    sbElsifStmt.Append(ProtoCore.VHDL.Constants.Keyword.Then);
                    sbElsifStmt.Append("\n");

                    // Elsif body
                    foreach (VHDLNode node in ElsifBodyList[i])
                    {
                        sbElsifStmt.Append(node.ToString());
                        sbElsifStmt.Append("\n");
                    }
                    sbElsifStmt.Append("\n");
                }
            }

            // Else 
            StringBuilder sbElse = new StringBuilder();
            StringBuilder sbElseBody = new StringBuilder();
            if (ElseBody.Count > 0)
            {
                sbElse.Append(ProtoCore.VHDL.Constants.Keyword.Else);
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
            sbEndIf.Append(ProtoCore.VHDL.Constants.Keyword.End);
            sbEndIf.Append(" ");
            sbEndIf.Append(ProtoCore.VHDL.Constants.Keyword.If);
            sbEndIf.Append(" ");
            sbEndIf.Append(Name);
            sbEndIf.Append(";");


            StringBuilder sbFormat = new StringBuilder();
            sbFormat.Append(sbIfExpr);
            sbFormat.Append(sbIfBody);
            sbFormat.Append(sbElsifStmt);
            sbFormat.Append(sbElse);
            sbFormat.Append(sbElseBody);
            sbFormat.Append(sbEndIf);
            return sbFormat.ToString();
        }

        public string Name { get; private set; }

        public VHDLNode IfExpr { get; set; }
        public List<VHDLNode> IfBody { get; set; }

        public List<VHDLNode> ElsifExprList { get; set; }
        public List<List<VHDLNode>> ElsifBodyList { get; set; }

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
        public HexStringNode(int value, int bits)
        {
            this.Value = value;
            this.bitCount = bits;
        }

        public override string ToString()
        {
            StringBuilder sbFormat = new StringBuilder();
            int nibbles = (bitCount / 4) + (bitCount % 4);
            string hexString = Value.ToString("X" + nibbles.ToString());
            sbFormat.Append("X" + '"' + hexString + '"');
            return sbFormat.ToString();
        }
        public int Value { get; private set; }
        private int bitCount;
        public string StringFormat { get; private set; }
    }
}
