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
        }

        public ProtoCore.VHDL.AST.ModuleNode CreateModule(string componentName)
        {
            ModuleName = componentName;
            if (!ModuleMap.ContainsKey(ModuleName))
            {
                ModuleMap[ModuleName] = new AST.ModuleNode(ModuleName);
                return ModuleMap[ModuleName];
            }
            return null;
        }

        public ProtoCore.VHDL.AST.ModuleNode CreateTopModule()
        {
            return CreateModule(TopLevelModuleName);
        }

        /// <summary>
        /// Creates the target output vhdl file stream
        /// Sets the module name
        /// </summary>
        /// <param name="componentName"></param>
        //public void SetupTargetComponent(string componentName)
        //{
        //    ModuleName = componentName;
        //    if (!ModuleMap.ContainsKey(ModuleName))
        //    {
        //        ModuleMap[ModuleName] = new AST.ModuleNode(ModuleName);
        //    }
        //}

        //public void SetupTargetComponentTopLevel()
        //{
        //    SetupTargetComponent(TopLevelModuleName);
        //}

        //public TextWriter GetOutputStream()
        //{
        //    Validity.Assert(ModuleMap.Count > 0);
        //    Validity.Assert(ModuleMap[ModuleName] != null);
        //    return ModuleMap[ModuleName].OutputFile;
        //}

        public AST.ModuleNode GetCurrentModule()
        {
            Validity.Assert(ModuleMap.Count > 0);
            Validity.Assert(ModuleMap[ModuleName] != null);
            return ModuleMap[ModuleName];
        }

        /// <summary>
        /// Emits module to stream and saves to file
        /// </summary>
        public void EmitModulesToFile()
        {
            foreach (var kvp in ModuleMap)
            {
                kvp.Value.Emit();
                kvp.Value.OutputFile.Flush();
            }
        }

        public Dictionary<string, AST.ModuleNode> ModuleMap { get; private set; }
        public string TopLevelModuleName { get; private set; }

        public string ModuleName { get; set; }
    }
}
