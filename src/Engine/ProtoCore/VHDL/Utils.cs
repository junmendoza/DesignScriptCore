using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using ProtoCore.Utils;

namespace ProtoCore.VHDL
{
    public static class Utils
    {
        public static string GeneratePortList(List<AST.PortEntryNode> portEntryList)
        {
            StringBuilder portList = new StringBuilder();
            portList.Append(ProtoCore.VHDL.Keyword.Port);
            portList.Append("(");
            portList.Append(" ");

            portList.Append("\n\t");
            for (int i = 0; i < portEntryList.Count; ++i)
            {
                ProtoCore.VHDL.AST.PortEntryNode portEntry = portEntryList[i];
                portList.Append(portEntry.ToString());
                if (i < (portEntryList.Count - 1))
                {
                    portList.Append(";\n\t");
                }
            }
            portList.Append("\n" + ");" + "\n");
            return portList.ToString();
        }

        public static void InitTables()
        {
            BinaryExprOperatorTable = new Dictionary<AST.BinaryExpressionNode.Operator, string>();
            BinaryExprOperatorTable.Add(AST.BinaryExpressionNode.Operator.Or, ProtoCore.VHDL.Keyword.Or);
            BinaryExprOperatorTable.Add(AST.BinaryExpressionNode.Operator.Nor, ProtoCore.VHDL.Keyword.Nor);
            BinaryExprOperatorTable.Add(AST.BinaryExpressionNode.Operator.Xnor, ProtoCore.VHDL.Keyword.Xnor);
            BinaryExprOperatorTable.Add(AST.BinaryExpressionNode.Operator.And, ProtoCore.VHDL.Keyword.And);
            BinaryExprOperatorTable.Add(AST.BinaryExpressionNode.Operator.Not, ProtoCore.VHDL.Keyword.Nor);
            BinaryExprOperatorTable.Add(AST.BinaryExpressionNode.Operator.Eq, "=");
        }

        public static string GeneratePortMapName(string componentName, int instanceCount)
        {
            return "call" + "_" + instanceCount.ToString() + "_" + componentName;
        }

        public static string GenerateComponentReturnSignal(string componentName, int instanceCount)
        {
            return "call" + "_"+ instanceCount.ToString() + "_" + componentName + "_" + "return_val";
        }

        public static string GenerateProcessName(string description, int processCount)
        {
            return ProtoCore.VHDL.Constants.PrefixProcess + "_" + processCount.ToString() + "_" + description;
        }

        public static string GetBinaryOperatorString(VHDL.AST.BinaryExpressionNode.Operator optr)
        {
            return BinaryExprOperatorTable[optr];
        }
        
        public static string GenerateReturnSignalName(string funcName)
        {
            return "return_" + funcName;
        }

        /// <summary>
        /// Get the VHDL component name if the function to be called is mapped to a component
        /// This is required when DS code is calling a hardware component directly such as an ALU
        /// </summary>
        /// <param name="functionCallName"></param>
        /// <returns></returns>
        public static string GetComponentMappedFunctionCallName(string functionCallName)
        {
            if (FunctionCallToComponentMap.ContainsKey(functionCallName))
            {
                return FunctionCallToComponentMap[functionCallName];
            }
            return functionCallName;
        }

#region AST_UTILS

        public static List<AST.LibraryNode> GenerateLibraryNodeList(List<string> libraryNameList)
        {
            List<AST.LibraryNode> libraryNodes = new List<AST.LibraryNode>();
            foreach (string libraryName in libraryNameList)
            {
                libraryNodes.Add(new AST.LibraryNode(libraryName));
            }
            return libraryNodes;
        }

        public static List<AST.UseNode> GenerateUseNodeList(List<string> moduleNameList)
        {
            List<AST.UseNode> useNodes = new List<AST.UseNode>();
            foreach (string moduleName in moduleNameList)
            {
                useNodes.Add(new AST.UseNode(moduleName));
            }
            return useNodes;
        }

        public static List<ProtoCore.VHDL.AST.SignalDeclarationNode> GenerateSignalsDeclarationList(ProtoCore.DSASM.SymbolTable symbolTable)
        {
            List<ProtoCore.VHDL.AST.SignalDeclarationNode> signalDeclList = new List<ProtoCore.VHDL.AST.SignalDeclarationNode>();
            foreach (KeyValuePair<int, ProtoCore.DSASM.SymbolNode> kvp in symbolTable.symbolList)
            {
                ProtoCore.VHDL.AST.SignalDeclarationNode signalDecl = new ProtoCore.VHDL.AST.SignalDeclarationNode(kvp.Value.name, 32);
                signalDeclList.Add(signalDecl);
            }
            return signalDeclList;
        }

        public static ProtoCore.VHDL.AST.IfNode GenerateResetSyncTemplate()
        {            
            // Reset sync ifstmt
            ProtoCore.VHDL.AST.IfNode resetSyncIf = new ProtoCore.VHDL.AST.IfNode(ProtoCore.VHDL.Constants.ResetSync);
            resetSyncIf.IfExpr = new ProtoCore.VHDL.AST.BinaryExpressionNode(
                new ProtoCore.VHDL.AST.IdentifierNode(ProtoCore.VHDL.Constants.ResetSignalName),
                new ProtoCore.VHDL.AST.BitStringNode(1),
                ProtoCore.VHDL.AST.BinaryExpressionNode.Operator.Eq
                );

            resetSyncIf.ElsifExpr = new ProtoCore.VHDL.AST.BinaryExpressionNode(
                new ProtoCore.VHDL.AST.IdentifierNode(ProtoCore.VHDL.Constants.ResetSignalName),
                new ProtoCore.VHDL.AST.BitStringNode(0),
                ProtoCore.VHDL.AST.BinaryExpressionNode.Operator.Eq
                );

            return resetSyncIf;
        }

        public static ProtoCore.VHDL.AST.IfNode GenerateClockSyncTemplate()
        {
            // rising_edge call
            List<ProtoCore.VHDL.AST.VHDLNode> argList = new List<ProtoCore.VHDL.AST.VHDLNode>();
            argList.Add(new ProtoCore.VHDL.AST.IdentifierNode(ProtoCore.VHDL.Constants.ClockSignalName));
            ProtoCore.VHDL.AST.FunctionCallNode clockedgeCall = new ProtoCore.VHDL.AST.FunctionCallNode(
                ProtoCore.VHDL.Constants.RisingEdge,
                argList
                );

            // clock sync ifstmt
            ProtoCore.VHDL.AST.IfNode clockIf = new ProtoCore.VHDL.AST.IfNode(ProtoCore.VHDL.Constants.ClockSync);
            clockIf.IfExpr = clockedgeCall;

            return clockIf;
        }

#endregion

        public static Dictionary<VHDL.AST.BinaryExpressionNode.Operator, string> BinaryExprOperatorTable = null;

        /// <summary>
        /// Map of DS function calls to VHDL components
        /// </summary>
        public static Dictionary<string, string> FunctionCallToComponentMap = null;
    }
}
