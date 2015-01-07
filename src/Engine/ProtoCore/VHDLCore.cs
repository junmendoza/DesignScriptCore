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
            ArrayDataSize = 1;
            CurrentDataSize = 1;
            GenerateBuiltInComponents();
        }

        private void GenerateBuiltInComponents()
        {
            ProtoCore.VHDL.AST.ModuleNode module = null;
            ProtoCore.VHDL.Utils.FunctionCallToComponentMap = new Dictionary<string, string>();

            // ALU signals
            
            // Port entries
            ProtoCore.VHDL.AST.PortEntryNode reset = new ProtoCore.VHDL.AST.PortEntryNode("reset", ProtoCore.VHDL.AST.PortEntryNode.Direction.In, 1);
            List<ProtoCore.VHDL.AST.PortEntryNode> listPortEntry = new List<ProtoCore.VHDL.AST.PortEntryNode>();
            listPortEntry.Add(reset);
            listPortEntry.Add(new ProtoCore.VHDL.AST.PortEntryNode(ProtoCore.VHDL.ComponentName.ALU.OpSignal1, ProtoCore.VHDL.AST.PortEntryNode.Direction.In, 32));
            listPortEntry.Add(new ProtoCore.VHDL.AST.PortEntryNode(ProtoCore.VHDL.ComponentName.ALU.OpSignal2, ProtoCore.VHDL.AST.PortEntryNode.Direction.In, 32));
            listPortEntry.Add(new ProtoCore.VHDL.AST.PortEntryNode(ProtoCore.VHDL.ComponentName.ALU.OpSignalResult, ProtoCore.VHDL.AST.PortEntryNode.Direction.Out, 32));
            

            // ALU add
            module = CreateModule(ProtoCore.VHDL.ComponentName.ALU.Add, true);
            module.Entity = new ProtoCore.VHDL.AST.EntityNode(ProtoCore.VHDL.ComponentName.ALU.Add, listPortEntry);
            ProtoCore.VHDL.Utils.FunctionCallToComponentMap.Add(ProtoCore.DSASM.Op.GetOpFunction(ProtoCore.DSASM.Operator.add), ProtoCore.VHDL.ComponentName.ALU.Add);
            
            // ALU sub
            module = CreateModule(ProtoCore.VHDL.ComponentName.ALU.Sub, true);
            module.Entity = new ProtoCore.VHDL.AST.EntityNode(ProtoCore.VHDL.ComponentName.ALU.Add, listPortEntry);
            ProtoCore.VHDL.Utils.FunctionCallToComponentMap.Add(ProtoCore.DSASM.Op.GetOpFunction(ProtoCore.DSASM.Operator.sub), ProtoCore.VHDL.ComponentName.ALU.Sub);

            // ALU mul
            module = CreateModule(ProtoCore.VHDL.ComponentName.ALU.Mul, true);
            module.Entity = new ProtoCore.VHDL.AST.EntityNode(ProtoCore.VHDL.ComponentName.ALU.Add, listPortEntry);
            ProtoCore.VHDL.Utils.FunctionCallToComponentMap.Add(ProtoCore.DSASM.Op.GetOpFunction(ProtoCore.DSASM.Operator.mul), ProtoCore.VHDL.ComponentName.ALU.Mul);

            // ALU div
            module = CreateModule(ProtoCore.VHDL.ComponentName.ALU.Div, true);
            module.Entity = new ProtoCore.VHDL.AST.EntityNode(ProtoCore.VHDL.ComponentName.ALU.Add, listPortEntry);
            ProtoCore.VHDL.Utils.FunctionCallToComponentMap.Add(ProtoCore.DSASM.Op.GetOpFunction(ProtoCore.DSASM.Operator.div), ProtoCore.VHDL.ComponentName.ALU.Div);
        }
        

        private ProtoCore.VHDL.AST.ModuleNode CreateModule(string componentName, bool isBuiltIn = false)
        {
            ModuleName = componentName;
            if (!ModuleMap.ContainsKey(ModuleName))
            {
                ModuleMap[ModuleName] = new AST.ModuleNode(ModuleName, isBuiltIn);
                return ModuleMap[ModuleName];
            }
            return null;
        }

        /// <summary>
        /// A default module contains only the following:
        ///     1. Default library list (IEEE)
        ///     2. Default use module list (IEEE numeric)
        ///     3. Reset signal
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ProtoCore.VHDL.AST.ModuleNode CreateAndAppendDefaultModule(string name, bool isBuiltIn = false)
        {
            ModuleName = name;
            Validity.Assert(!ModuleMap.ContainsKey(ModuleName));

            ProtoCore.VHDL.AST.ModuleNode module = new AST.ModuleNode(ModuleName, isBuiltIn);
            ModuleMap[ModuleName] = module;


            // Library list
            List<string> libaryNameList = new List<string>();
            libaryNameList.Add("IEEE");
            module.LibraryList = ProtoCore.VHDL.Utils.GenerateLibraryNodeList(libaryNameList);

            // Module list
            List<string> moduleNameList = new List<string>();
            moduleNameList.Add("IEEE.STD_LOGIC_1164.ALL");
            moduleNameList.Add("IEEE.NUMERIC_STD.ALL");
            module.UseNodeList = ProtoCore.VHDL.Utils.GenerateUseNodeList(moduleNameList);

            return module;
        }

        public ProtoCore.VHDL.AST.ModuleNode CreateAndAppendTopModule()
        {
            ProtoCore.VHDL.AST.ModuleNode module = CreateAndAppendDefaultModule(TopLevelModuleName);
            module.IsTopModule = true;
            return module;
        }

        public AST.ModuleNode GetTopModule()
        {
            Validity.Assert(ModuleMap.Count > 0);
            Validity.Assert(ModuleMap[TopLevelModuleName] != null);
            return ModuleMap[TopLevelModuleName];
        }

        public AST.ModuleNode GetModule(string name)
        {
            Validity.Assert(ModuleMap.Count > 0);
            Validity.Assert(ModuleMap[name] != null);
            return ModuleMap[name];
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

        public void GenerateBuiltInModules()
        {
        }

        public Dictionary<string, AST.ModuleNode> ModuleMap { get; private set; }
        public string TopLevelModuleName { get; private set; }
        public string ModuleName { get; set; }
        public int ArrayDataSize { get; set; }
        public int CurrentDataSize { get; set; }
        
        /// <summary>
        /// Tracks the number of times a component is instantiated
        /// </summary>
        public Dictionary<string, int> ComponentInstanceCountMap { get; private set; }
    }
}
