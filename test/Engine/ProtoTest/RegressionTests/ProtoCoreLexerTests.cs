using System;
using NUnit.Framework;
using ProtoCore.DSASM.Mirror;
using ProtoCore.Lang;
namespace ProtoTest.RegressionTests
{
    [TestFixture]
    class LexerRegressionTests : ProtoTestBase
    {
        [Test]
        public void PreClarifyPreParseBracket001()
        {
            String code =
@"x;
            ProtoScript.Runners.ProtoScriptTestRunner fsr = new ProtoScript.Runners.ProtoScriptTestRunner();
            ExecutionMirror mirror = fsr.Execute(code, core);
            Obj o = mirror.GetValue("x");
            Assert.IsTrue((Int64)o.Payload == 1001);
        }
    }
}