using System.Runtime.InteropServices;

namespace MemoryLayout.Messaging
{
    // Bloques pequeños: Ideales para eventos, coordenadas o estados simples
    [StructLayout(LayoutKind.Sequential, Size = 60)] public struct Size64 { }
    [StructLayout(LayoutKind.Sequential, Size = 124)] public struct Size128 { }
    [StructLayout(LayoutKind.Sequential, Size = 252)] public struct Size256 { }

    // Bloques medianos: Para structs con un par de strings cortos o colecciones pequeñas
    [StructLayout(LayoutKind.Sequential, Size = 508)] public struct Size512 { }
    [StructLayout(LayoutKind.Sequential, Size = 1020)] public struct Size1024 { }
    [StructLayout(LayoutKind.Sequential, Size = 2044)] public struct Size2048 { }

    // Bloques grandes: Para payloads pesados o strings muy largos (BIO, descripciones, JSONs)
    [StructLayout(LayoutKind.Sequential, Size = 4092)] public struct Size4096 { }
    [StructLayout(LayoutKind.Sequential, Size = 8188)] public struct Size8192 { }
}
