using System;

namespace Hotfix.framework
{
    public class CommandHandlerAttribute : Attribute
    {
    }

    public class CommandHandleAttribute : Attribute
    {
        public ushort Opcode { get; set; }

        public CommandHandleAttribute(byte first, byte second)
        {
            Opcode = first;
            Opcode <<= 8;
            Opcode |= second;
        }
    }
}