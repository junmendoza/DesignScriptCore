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
            ProtoCore.VHDL.Utils.FunctionCallToComponentMap.Add(ProtoCore.DSASM.Op.GetOpFunction(ProtoCore.DSASM.Operator.sub), ProtoCore.VHDL.ComponentName.ALU.Sub);

            // ALU mul
            module = CreateModule(ProtoCore.VHDL.ComponentName.ALU.Mul, true);
            ProtoCore.VHDL.Utils.FunctionCallToComponentMap.Add(ProtoCore.DSASM.Op.GetOpFunction(ProtoCore.DSASM.Operator.mul), ProtoCore.VHDL.ComponentName.ALU.Mul);

            // ALU div
            module = CreateModule(ProtoCore.VHDL.ComponentName.ALU.Div, true);
            ProtoCore.VHDL.Utils.FunctionCallToComponentMap.Add(ProtoCore.DSASM.Op.GetOpFunction(ProtoCore.DSASM.Operator.div), ProtoCore.VHDL.ComponentName.ALU.Div);
        }
        

        public ProtoCore.VHDL.AST.ModuleNode CreateModule(string componentName, bool isBuiltIn = false)
        {
            ModuleName = componentName;
            if (!ModuleMap.ContainsKey(ModuleName))
            {
                ModuleMap[ModuleName] = new AST.ModuleNode(ModuleName, isBuiltIn);
                return ModuleMap[ModuleName];
            }
            return null;
        }

        public ProtoCore.VHDL.AST.ModuleNode CreateTopModule()
        {
            return CreateModule(TopLevelModuleName);
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

        /// <summary>
        /// Tracks the number of times a component is instantiated
        /// </summary>
        public Dictionary<string, int> ComponentInstanceCountMap { get; private set; }
    }
}
