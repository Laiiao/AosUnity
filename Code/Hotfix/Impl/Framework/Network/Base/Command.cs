using System;

namespace Hotfix.framework
{
    public class CommandBase<T>
    {
        public readonly T Data = Activator.CreateInstance<T>();
    }
}