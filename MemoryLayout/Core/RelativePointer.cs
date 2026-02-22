using System.Runtime.InteropServices;

namespace MemoryLayout.Core
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RelativePointer(int offset, int length)
    {
        public int Offset = offset;
        public int Length = length;
    }
}
