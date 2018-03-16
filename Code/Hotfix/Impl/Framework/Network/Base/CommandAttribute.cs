using System;

namespace Hotfix.framework
{
    public class CommandAttribute : Attribute
    {
        public ushort Opcode { get; set; }

        public CommandAttribute(byte first, byte second)
        {
            Opcode = first;
            Opcode <<= 8;
            Opcode |= second;
        }
    }
}
