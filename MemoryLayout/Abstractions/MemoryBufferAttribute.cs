namespace MemoryLayout.Abstractions
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class MemoryBufferAttribute(int size) : Attribute
    {
        public int Size { get; } = size;
    }
}
