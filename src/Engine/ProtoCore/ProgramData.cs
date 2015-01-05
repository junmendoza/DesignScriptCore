using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoCore.CompileAndExecutePass
{
    /// <summary>
    /// Compile time data is determined at the following stages:
    ///     1. Compile time - during the compilation
    ///     2. First run pass - a first pass run on DSVM to determine information about the program
    /// </summary>
    /// 

    /// <summary>
    /// Program data gathered at runtime
    /// </summary>
    public struct Runtime
    {
        public Dictionary<string, ProtoCore.CallSite> CallsiteCache;
    }

    /// <summary>
    /// Program data gathered during compilation
    /// </summary>
    public struct CompileTime
    {
    }

    public class ProgramData
    {
        public Runtime runtime;
        public CompileTime compileTime;
    }

}
