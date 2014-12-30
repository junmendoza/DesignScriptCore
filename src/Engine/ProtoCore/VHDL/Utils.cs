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
        public static string GeneratePortList(List<AST.PortEntry> portEntryList)
        {
            StringBuilder portList = new StringBuilder();
            portList.Append(ProtoCore.VHDL.Keyword.Port);
            portList.Append("(");
            portList.Append(" ");

            portList.Append("\n\t");
            for (int i = 0; i < portEntryList.Count; ++i)
            {
                ProtoCore.VHDL.AST.PortEntry portEntry = portEntryList[i];
                portList.Append(portEntry.ToString());
                if (i < (portEntryList.Count - 1))
                {
                    portList.Append(";\n\t");
                }
            }
            portList.Append("\n" + ")" + "\n");
            return portList.ToString();
        }
    }
}
