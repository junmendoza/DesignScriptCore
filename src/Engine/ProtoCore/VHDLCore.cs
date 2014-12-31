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
            ProtoCore.VHDL.Utils.InitTables();
            ComponentInstanceCountMap = new Dictionary<string, int>();
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

        public AST.ModuleNode GetModule(string name)
        {
            Validity.Assert(ModuleMap.Count > 0);
            Validity.Assert(ModuleMap[name] != null);
            return ModuleMap[name];
        }

        public AST.ModuleNode GetCurrentModule()
        {
            Validity.Assert(ModuleMap.Count > 0);
            Validity.Assert(ModuleMap[ModuleName] != null);
            return ModuleMap[ModuleName];
        }

        public void SetTopModule()
        {
            ModuleName = TopLevelModuleName;
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

        public void UpdateComponentInstanceCount(string componentName)
        {
            if (ComponentInstanceCountMap.ContainsKey(componentName))
            {
                // Increment the instance count
                ComponentInstanceCountMap[componentName]++;
            }
            else
            {
                // The component is instantiated for the first time
                ComponentInstanceCountMap.Add(componentName, 1);
            }
        }


        public Dictionary<string, AST.ModuleNode> ModuleMap { get; private set; }
        public string TopLevelModuleName { get; private set; }
        public string ModuleName { get; set; }

        /// <summary>
        /// Tracks the number of times a component is instantiated
        /// </summary>
        public Dictionary<string, int> ComponentInstanceCountMap { get; private set; }
    }
}
