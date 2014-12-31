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

        public static string GetBinaryOperatorString(VHDL.AST.BinaryExpressionNode.Operator optr)
        {
            return BinaryExprOperatorTable[optr];
        }

        public static Dictionary<VHDL.AST.BinaryExpressionNode.Operator, string> BinaryExprOperatorTable = null;
    }
}
