
using System.Collections.Generic;
using System.Diagnostics;
using ProtoCore;
using ProtoCore.DSASM;
using ProtoCore.Utils;

namespace ProtoVHDL
{
	public class Executive : ProtoCore.Executive
	{

		public Executive (Core core) : base(core)
		{
		}

        public override bool Compile(
            out int blockId, 
            ProtoCore.DSASM.CodeBlock parentBlock, 
            ProtoCore.LanguageCodeBlock langBlock, 
            ProtoCore.CompileTime.Context callContext, 
            ProtoCore.DebugServices.EventSink sink = null, 
            ProtoCore.AST.Node codeBlockNode = null, 
            ProtoCore.AssociativeGraph.GraphNode graphNode = null)
        {
            Validity.Assert(langBlock != null);
            bool buildSucceeded = true;
            blockId = 0;

            core.assocCodegen = new ProtoVHDL.CodeGen(core, callContext, parentBlock);

            System.IO.MemoryStream memstream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(langBlock.body));
            ProtoCore.DesignScriptParser.Scanner s = new ProtoCore.DesignScriptParser.Scanner(memstream);
            ProtoCore.DesignScriptParser.Parser p = new ProtoCore.DesignScriptParser.Parser(s, core, core.builtInsLoaded);
            p.Parse();
            core.builtInsLoaded = true;
            codeBlockNode = p.root;

            List<ProtoCore.AST.Node> astNodes = ProtoCore.Utils.ParserUtils.GetAstNodes(codeBlockNode);
            core.AstNodeList = astNodes;

            core.assocCodegen.Emit(codeBlockNode as ProtoCore.AST.AssociativeAST.CodeBlockNode);

            buildSucceeded = core.BuildStatus.BuildSucceeded;
            return buildSucceeded;

        }

      

        public override StackValue Execute(int codeblock, int entry, ProtoCore.Runtime.Context callContext, ProtoCore.DebugServices.EventSink sink)
        {
            if (!core.Options.CompileToLib)
            {
                ProtoCore.DSASM.Interpreter interpreter = new ProtoCore.DSASM.Interpreter(core);
                CurrentDSASMExec = interpreter.runtime;
                StackValue sv = interpreter.Run(codeblock, entry, Language.kAssociative);
                return sv;
            }
            else
            {
                return StackValue.Null;
            }
        }

        public override StackValue Execute(int codeblock, int entry, ProtoCore.Runtime.Context callContext, System.Collections.Generic.List<Instruction> breakpoints, ProtoCore.DebugServices.EventSink sink = null, bool fepRun = false)
        {
            ProtoCore.DSASM.Interpreter interpreter = new ProtoCore.DSASM.Interpreter(core, fepRun);
            CurrentDSASMExec = interpreter.runtime;
            StackValue sv = interpreter.Run(breakpoints, codeblock, entry, Language.kAssociative);
            return sv;
        }

	}
}

