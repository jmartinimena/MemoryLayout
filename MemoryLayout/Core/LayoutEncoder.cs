using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace MemoryLayout.Core
{
    public class LayoutEncoder
    {
        // El encabezado mínimo para identificar el formato
        private const uint MagicNumber = 0x59414C4D; // "MLAY" en Little Endian

        /// <summary>
        /// Calcula el tamaño necesario para alojar el objeto en formato binario.
        /// Este método será sobreescrito por el Source Generator para cada tipo.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetByteCount<T>(in T value) where T : unmanaged
        {
            // El generador reemplazará esto con lógica específica
            return Unsafe.SizeOf<T>() + sizeof(uint);
        }

        /// <summary>
        /// Escribe el objeto en un Span de bytes de forma ultra rápida.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode<T>(ref byte destination, in T value) where T : unmanaged
        {
            // 1. Escribir Magic Number
            Unsafe.WriteUnaligned(ref destination, MagicNumber);

            // 2. El Source Generator inyectará aquí las copias de memoria directas (Blit)
            Unsafe.WriteUnaligned(ref Unsafe.Add(ref destination, 4), value);
        }

        /// <summary>
        /// Escribe un string directamente en el buffer binario en formato UTF-8.
        /// </summary>
        /// <param name="destination">Referencia al inicio del espacio reservado para el string.</param>
        /// <param name="value">El string a codificar.</param>
        /// <returns>La cantidad de bytes escritos.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteString(ref byte destination, string? value)
        {
            if (string.IsNullOrEmpty(value)) return 0;

            // Obtenemos un span sobre el destino
            // Usamos un tamaño máximo razonable o el largo del string por 3 (UTF8 max)
            Span<byte> destSpan = MemoryMarshal.CreateSpan(ref destination, value.Length * 3);

            // Codificamos directamente a la memoria del buffer
            int bytesWritten = Encoding.UTF8.GetBytes(value, destSpan);

            return bytesWritten;
        }

        /// <summary>
        /// Lee un string desde un offset específico sin validaciones costosas.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static string ReadString(ReadOnlySpan<byte> buffer, int offset, int length)
        {
            if (length <= 0) return string.Empty;

            // Creamos el string directamente desde el segmento del buffer
            fixed (byte* p = &buffer[offset])
            {
                return Encoding.UTF8.GetString(p, length);
            }
        }
    }
}
