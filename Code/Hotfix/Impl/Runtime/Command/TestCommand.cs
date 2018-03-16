using System.Collections;
using System.Collections.Generic;
using Hotfix.framework;

namespace Hotfix.runtime
{
    [Command(0x01, 0x01)]
    public class TestCommand : CommandBase<GxTest.gx_data>
    {
    }
}
