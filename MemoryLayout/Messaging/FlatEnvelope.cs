using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace MemoryLayout.Messaging
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FlatEnvelope<TSize> where TSize : unmanaged
    {
        public int PayloadLength;
        public TSize Payload; // Los bytes del struct serializado viven aquí

        /// <summary>
        /// Capacidad total de este sobre (excluyendo el header de longitud).
        /// </summary>
        public static int Capacity => Unsafe.SizeOf<TSize>();
    }
}
