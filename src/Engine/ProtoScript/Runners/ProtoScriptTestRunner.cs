using System;
using System.Text;
using System.Collections.Generic;
using ProtoCore.DSASM.Mirror;
using ProtoCore.Utils;
using ProtoCore.DSASM;

namespace ProtoScript.Runners
{

    public class ProtoScriptTestRunner
    {
        public ProtoCore.DebugServices.EventSink EventSink = new ProtoCore.DebugServices.ConsoleEventSink();


        public bool Compile(ProtoCore.CompileTime.Context context, ProtoCore.Core core, out int blockId)
        {
            bool buildSucceeded = false;

            core.ExecMode = ProtoCore.DSASM.InterpreterMode.kNormal;

            blockId = ProtoCore.DSASM.Constants.kInvalidIndex;
            try
            {
                // No More HashAngleReplace for unified parser (Fuqiang)
                //String strSource = ProtoCore.Utils.LexerUtils.HashAngleReplace(code);    

                //defining the global Assoc block that wraps the entire .ds source file
                ProtoCore.LanguageCodeBlock globalBlock = new ProtoCore.LanguageCodeBlock();
                globalBlock.language = ProtoCore.Language.kAssociative;

                Validity.Assert(null != context.SourceCode && String.Empty != context.SourceCode);
                globalBlock.body = context.SourceCode;
                //the wrapper block can be given a unique id to identify it as the global scope
                globalBlock.id = ProtoCore.LanguageCodeBlock.OUTERMOST_BLOCK_ID;


                //passing the global Assoc wrapper block to the compiler
                ProtoCore.Language id = globalBlock.language;
                core.Executives[id].Compile(out blockId, null, globalBlock, context, EventSink);

                core.BuildStatus.ReportBuildResult();
                buildSucceeded = core.BuildStatus.BuildSucceeded;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return buildSucceeded;
        }

        public bool Compile(string code, ProtoCore.Core core, out int blockId)
        {
            bool buildSucceeded = false;

            core.ExecMode = ProtoCore.DSASM.InterpreterMode.kNormal;

            blockId = ProtoCore.DSASM.Constants.kInvalidIndex;
            try
            {
                // No More HashAngleReplace for unified parser (Fuqiang)
                //String strSource = ProtoCore.Utils.LexerUtils.HashAngleReplace(code);    

                //defining the global Assoc block that wraps the entire .ds source file
                ProtoCore.LanguageCodeBlock globalBlock = new ProtoCore.LanguageCodeBlock();
                globalBlock.language = ProtoCore.Language.kAssociative;
                globalBlock.body = code;
                //the wrapper block can be given a unique id to identify it as the global scope
                globalBlock.id = ProtoCore.LanguageCodeBlock.OUTERMOST_BLOCK_ID;


                //passing the global Assoc wrapper block to the compiler
                ProtoCore.CompileTime.Context context = new ProtoCore.CompileTime.Context();
                ProtoCore.Language id = globalBlock.language;
                core.Executives[id].Compile(out blockId, null, globalBlock, context, EventSink);

                core.BuildStatus.ReportBuildResult();
                buildSucceeded = core.BuildStatus.BuildSucceeded;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return buildSucceeded;
        }


        public bool Compile(List<ProtoCore.AST.AssociativeAST.AssociativeNode> astList, ProtoCore.Core core, out int blockId)
        {
            bool buildSucceeded = false;

            core.ExecMode = ProtoCore.DSASM.InterpreterMode.kNormal;

            blockId = ProtoCore.DSASM.Constants.kInvalidIndex;
            try
            {
                //defining the global Assoc block that wraps the entire .ds source file
                ProtoCore.LanguageCodeBlock globalBlock = new ProtoCore.LanguageCodeBlock();
                globalBlock.language = ProtoCore.Language.kAssociative;
                globalBlock.body = string.Empty;
                //the wrapper block can be given a unique id to identify it as the global scope
                globalBlock.id = ProtoCore.LanguageCodeBlock.OUTERMOST_BLOCK_ID;


                //passing the global Assoc wrapper block to the compiler
                ProtoCore.CompileTime.Context context = new ProtoCore.CompileTime.Context();
                context.SetData(string.Empty, new Dictionary<string, object>(), null);
                ProtoCore.Language id = globalBlock.language;

                
		        ProtoCore.AST.AssociativeAST.CodeBlockNode codeblock = new ProtoCore.AST.AssociativeAST.CodeBlockNode();
                codeblock.Body.AddRange(astList);

                core.Executives[id].Compile(out blockId, null, globalBlock, context, EventSink, codeblock);

                core.BuildStatus.ReportBuildResult();

                buildSucceeded = core.BuildStatus.BuildSucceeded;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return buildSucceeded;
        }

        public void Execute(ProtoCore.Core core, ProtoCore.Runtime.Context context)
        {
            try
            {
                core.NotifyExecutionEvent(ProtoCore.ExecutionStateEventArgs.State.kExecutionBegin);
                foreach (ProtoCore.DSASM.CodeBlock codeblock in core.CodeBlockList)
                {
                    //ProtoCore.Runtime.Context context = new ProtoCore.Runtime.Context();

                    int locals = 0;


                    // Comment Jun:
                    // On first bounce, the stackframe depth is initialized to -1 in the Stackfame constructor.
                    // Passing it to bounce() increments it so the first depth is always 0
                    ProtoCore.DSASM.StackFrame stackFrame = new ProtoCore.DSASM.StackFrame(core.GlobOffset);
                    stackFrame.FramePointer = core.Rmem.FramePointer;
                    
                    // Comment Jun: Tell the new bounce stackframe that this is an implicit bounce
                    // Register TX is used for this.
                    StackValue svCallConvention = StackValue.BuildCallingConversion((int)ProtoCore.DSASM.CallingConvention.BounceType.kImplicit);
                    stackFrame.TX = svCallConvention;

                    core.Bounce(codeblock.codeBlockId, codeblock.instrStream.entrypoint, context, stackFrame, locals, EventSink);
                }
                core.NotifyExecutionEvent(ProtoCore.ExecutionStateEventArgs.State.kExecutionEnd);
            }
            catch 
            {
                core.NotifyExecutionEvent(ProtoCore.ExecutionStateEventArgs.State.kExecutionEnd);
                throw;
            }
        }

        public ExecutionMirror Execute(string code, ProtoCore.Core core, Dictionary<string, Object> values, bool isTest = true)
        {
            //Inject the context data values from external source.
            core.AddContextData(values);
            int blockId = ProtoCore.DSASM.Constants.kInvalidIndex;
            bool succeeded = Compile(code, core, out blockId);
            if (succeeded)
            {
                core.GenerateExecutable();
                core.Rmem.PushFrameForGlobals(core.GlobOffset);
                core.RunningBlock = blockId;

                Execute(core, new ProtoCore.Runtime.Context());

                if (!isTest) { core.Heap.Free(); }
            }
            else
            {
                throw new ProtoCore.Exceptions.CompileErrorsOccured();
            }

            if (isTest && !core.Options.CompileToLib)
            {
                return new ExecutionMirror(core.CurrentExecutive.CurrentDSASMExec, core);
            }

            return null;
        }

        public ExecutionMirror Execute(ProtoCore.CompileTime.Context staticContext, ProtoCore.Runtime.Context runtimeContext, ProtoCore.Core core, bool isTest = true)
        {
            Validity.Assert(null != staticContext.SourceCode && String.Empty != staticContext.SourceCode);
            
            int blockId = ProtoCore.DSASM.Constants.kInvalidIndex;
            bool succeeded = Compile(staticContext, core, out blockId);
            if (succeeded)
            {
                core.GenerateExecutable();
                core.Rmem.PushFrameForGlobals(core.GlobOffset);
                core.RunningBlock = blockId;
                core.InitializeContextGlobals(staticContext.GlobalVarList);

                Validity.Assert(null != runtimeContext);
                Execute(core, runtimeContext);
                if (!isTest)
                {
                    core.Heap.Free();
                }
            }
            else
            {
                throw new ProtoCore.Exceptions.CompileErrorsOccured();
            }

            if (isTest && !core.Options.CompileToLib)
            {
                return new ExecutionMirror(core.CurrentExecutive.CurrentDSASMExec, core);
            }

            return null;
        }

        public ExecutionMirror Execute(List<ProtoCore.AST.AssociativeAST.AssociativeNode> astList, ProtoCore.Core core, bool isTest = true)
        {
            int blockId = ProtoCore.DSASM.Constants.kInvalidIndex;
            bool succeeded = Compile(astList, core, out blockId);
            if (succeeded)
            {
                core.GenerateExecutable();
                core.Rmem.PushFrameForGlobals(core.GlobOffset);
                core.RunningBlock = blockId;

                Execute(core, new ProtoCore.Runtime.Context());
                if (!isTest) 
                { 
                    core.Heap.Free(); 
                }
            }
            else
            {
                throw new ProtoCore.Exceptions.CompileErrorsOccured();
            }

            if (isTest && !core.Options.CompileToLib)
            {
                return new ExecutionMirror(core.CurrentExecutive.CurrentDSASMExec, core);
            }

            return null;
        }

        public ExecutionMirror Execute(string code, ProtoCore.Core core, bool isTest = true)
        {
            int blockId = ProtoCore.DSASM.Constants.kInvalidIndex;
            bool succeeded = Compile(code, core, out blockId);
            if (succeeded)
            {
                core.GenerateExecutable();
                core.Rmem.PushFrameForGlobals(core.GlobOffset);
                core.RunningBlock = blockId;

                try
                {
                    Execute(core, new ProtoCore.Runtime.Context());
                }
                catch (ProtoCore.Exceptions.ExecutionCancelledException e)
                {
                    Console.WriteLine("The execution has been cancelled!");             
                }
                
                if (!isTest)
                {
                    core.Heap.Free();
                }
            }
            else
            {
                throw new ProtoCore.Exceptions.CompileErrorsOccured();
            }

            if (isTest && !core.Options.CompileToLib)
            {
                return new ExecutionMirror(core.CurrentExecutive.CurrentDSASMExec, core);
            }

            return null;
        }

        public ExecutionMirror LoadAndExecute(string filename, ProtoCore.Core core, bool isTest = true)
        {
            System.IO.StreamReader reader = null;
            try
            {
                reader = new System.IO.StreamReader(filename, Encoding.UTF8, true);
            }
            catch (System.IO.IOException)
            {
                throw new Exception("Cannot open file " + filename);
            }

            string strSource = reader.ReadToEnd();
            reader.Dispose();
            //Start the timer       
            core.StartTimer();

            core.Options.RootModulePathName = ProtoCore.Utils.FileUtils.GetFullPathName(filename);
            core.CurrentDSFileName = core.Options.RootModulePathName;
            Execute(strSource, core);

            if (isTest && !core.Options.CompileToLib)
                return new ExecutionMirror(core.CurrentExecutive.CurrentDSASMExec, core);
            else
                return null;
        }

        /// <summary>
        /// Execute to gather information about the program
        /// Data is stored in ProtoCore.CompileAndExecutePass.ProgramData
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="programData"></param>
        /// <returns></returns>
        private bool CompileAndExecutePass(string filename, out ProtoCore.CompileAndExecutePass.ProgramData programData)
        {
            ProtoCore.Options options = new ProtoCore.Options();
            ProtoCore.Core core = new ProtoCore.Core(options);
            options.ExecutionMode = ProtoCore.ExecutionMode.Serial;
            core.Executives.Add(ProtoCore.Language.kAssociative, new ProtoAssociative.Executive(core));
            core.Executives.Add(ProtoCore.Language.kImperative, new ProtoImperative.Executive(core));
            core.Options.DumpByteCode = true;
            core.Options.Verbose = true;

            ProtoFFI.DLLFFIHandler.Register(ProtoFFI.FFILanguage.CSharp, new ProtoFFI.CSModuleHelper());
            ExecutionMirror mirror = LoadAndExecute(filename, core);

            programData = core.GetProgramData();

            return mirror == null ? false : true;
        }

        public bool CompileToVHDL(string topLevelModule, string filename)
        {
            // Execute and gather program data 
            ProtoCore.CompileAndExecutePass.ProgramData programData = null;
            bool compileAndExecuteSuceeded = CompileAndExecutePass(filename, out programData);
            Validity.Assert(compileAndExecuteSuceeded == true);
            Validity.Assert(programData != null);

            ProtoCore.Options options = new ProtoCore.Options();
            options.CompilationTarget = ProtoCore.DSDefinitions.CompileTarget.VHDL;

            // Generate a new core and pass it the data gathered from the previous pass
            ProtoCore.Core core = new ProtoCore.Core(options);
            core.SetProgramData(programData);

            options.ExecutionMode = ProtoCore.ExecutionMode.Serial;
            core.Executives.Add(ProtoCore.Language.kAssociative, new ProtoAssociative.Executive(core));
            core.Executives.Add(ProtoCore.Language.kImperative, new ProtoImperative.Executive(core));
            core.Executives.Add(ProtoCore.Language.kVHDL, new ProtoVHDL.Executive(core));
            core.Options.DumpByteCode = true;
            core.Options.Verbose = true;

            core.VhdlCore = new ProtoCore.VHDL.VHDLCore(topLevelModule);

            System.IO.StreamReader reader = null;
            try
            {
                reader = new System.IO.StreamReader(filename, Encoding.UTF8, true);
            }
            catch (System.IO.IOException)
            {
                throw new Exception("Cannot open file " + filename);
            }

            string strSource = reader.ReadToEnd();
            reader.Dispose();

            core.Options.RootModulePathName = ProtoCore.Utils.FileUtils.GetFullPathName(filename);
            core.CurrentDSFileName = core.Options.RootModulePathName;
            core.ExecMode = ProtoCore.DSASM.InterpreterMode.kNormal;

            bool buildSucceeded = false;
            try
            {
                //defining the global Assoc block that wraps the entire .ds source file
                ProtoCore.LanguageCodeBlock globalBlock = new ProtoCore.LanguageCodeBlock();
                globalBlock.language = ProtoCore.Language.kVHDL;
                globalBlock.body = strSource;

                //the wrapper block can be given a unique id to identify it as the global scope
                globalBlock.id = ProtoCore.LanguageCodeBlock.OUTERMOST_BLOCK_ID;

                //passing the global Assoc wrapper block to the compiler
                ProtoCore.CompileTime.Context context = new ProtoCore.CompileTime.Context();
                ProtoCore.Language id = globalBlock.language;
                int blockID = 0;
                core.Executives[id].Compile(out blockID, null, globalBlock, context);

                core.BuildStatus.ReportBuildResult();
                buildSucceeded = core.BuildStatus.BuildSucceeded;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return buildSucceeded;
        }
    }
}
