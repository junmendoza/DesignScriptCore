﻿

using System;
using ProtoCore;
using ProtoScript.Runners;
using System.IO;

namespace DSConsole
{
    class Program
    {
        static void Run(string filename, bool verbose)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("Cannot find file " + filename);
                return;
            }
            var opts = new Options();
            opts.ExecutionMode = ExecutionMode.Serial;
            ProtoCore.Core core = new Core(opts);
            core.Executives.Add(ProtoCore.Language.kAssociative, new ProtoAssociative.Executive(core));
            core.Executives.Add(ProtoCore.Language.kImperative, new ProtoImperative.Executive(core));
            core.Options.DumpByteCode = verbose;
            core.Options.Verbose = verbose;
            ProtoFFI.DLLFFIHandler.Register(ProtoFFI.FFILanguage.CSharp, new ProtoFFI.CSModuleHelper());

            ProtoScriptTestRunner runner = new ProtoScriptTestRunner();
            runner.LoadAndExecute(filename, core);
        }

        static void DevRun()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();


            ProtoFFI.DLLFFIHandler.Register(ProtoFFI.FFILanguage.CSharp, new ProtoFFI.CSModuleHelper());
            ProtoScriptTestRunner runner = new ProtoScriptTestRunner();

            string vhdlTopLevelModuleName = "ProgramSynthesized";
            runner.CompileToVHDL(vhdlTopLevelModuleName, @"..\..\test.ds");

            long ms = sw.ElapsedMilliseconds;
            sw.Stop();
            Console.WriteLine(ms);
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                Run(args[0], true);
            }
            else
            {
                DevRun();
            }
        }
    }
}
