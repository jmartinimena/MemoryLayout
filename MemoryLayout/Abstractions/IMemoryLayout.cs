namespace MemoryLayout.Abstractions
{
    public interface IMemoryLayout<T>
    {
        void Encode(ref byte dest, out int totalBytes);
        static abstract void Decode(ReadOnlySpan<byte> buffer, out T result);
        static abstract ushort TypeId { get; }
    }
}
