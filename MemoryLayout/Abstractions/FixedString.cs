using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace MemoryLayout.Abstractions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FixedString16
    {
        private fixed byte _data[16];
        private int _len;

        public int Length => _len;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FixedString16(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                _len = 0;
                return;
            }

            Span<byte> dest = MemoryMarshal.CreateSpan(ref _data[0], 16);

            _len = Encoding.UTF8.GetBytes(value, dest);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> AsSpan()
            => MemoryMarshal.CreateReadOnlySpan(ref _data[0], _len);

        public override string ToString()
            => _len > 0 ? Encoding.UTF8.GetString(AsSpan()) : string.Empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator string(FixedString16 fs) => fs.ToString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator FixedString16(string s) => new(s);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FixedString32
    {
        private fixed byte _data[32];
        private int _len;

        public int Length => _len;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FixedString32(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                _len = 0;
                return;
            }

            Span<byte> dest = MemoryMarshal.CreateSpan(ref _data[0], 32);

            _len = Encoding.UTF8.GetBytes(value, dest);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> AsSpan()
            => MemoryMarshal.CreateReadOnlySpan(ref _data[0], _len);

        public override string ToString()
            => _len > 0 ? Encoding.UTF8.GetString(AsSpan()) : string.Empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator string(FixedString32 fs) => fs.ToString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator FixedString32(string s) => new(s);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FixedString64
    {
        private fixed byte _data[64];
        private int _len;

        public int Length => _len;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FixedString64(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                _len = 0;
                return;
            }

            Span<byte> dest = MemoryMarshal.CreateSpan(ref _data[0], 64);

            _len = Encoding.UTF8.GetBytes(value, dest);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> AsSpan()
            => MemoryMarshal.CreateReadOnlySpan(ref _data[0], _len);

        public override string ToString()
            => _len > 0 ? Encoding.UTF8.GetString(AsSpan()) : string.Empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator string(FixedString64 fs) => fs.ToString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator FixedString64(string s) => new(s);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FixedString128
    {
        private fixed byte _data[128];
        private int _len;

        public int Length => _len;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FixedString128(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                _len = 0;
                return;
            }

            Span<byte> dest = MemoryMarshal.CreateSpan(ref _data[0], 128);

            _len = Encoding.UTF8.GetBytes(value, dest);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> AsSpan()
            => MemoryMarshal.CreateReadOnlySpan(ref _data[0], _len);

        public override string ToString()
            => _len > 0 ? Encoding.UTF8.GetString(AsSpan()) : string.Empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator string(FixedString128 fs) => fs.ToString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator FixedString128(string s) => new(s);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FixedString256
    {
        private fixed byte _data[256];
        private int _len;

        public int Length => _len;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FixedString256(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                _len = 0;
                return;
            }

            Span<byte> dest = MemoryMarshal.CreateSpan(ref _data[0], 256);

            _len = Encoding.UTF8.GetBytes(value, dest);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> AsSpan()
            => MemoryMarshal.CreateReadOnlySpan(ref _data[0], _len);

        public override string ToString()
            => _len > 0 ? Encoding.UTF8.GetString(AsSpan()) : string.Empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator string(FixedString256 fs) => fs.ToString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator FixedString256(string s) => new(s);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FixedString512
    {
        private fixed byte _data[512];
        private int _len;

        public int Length => _len;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FixedString512(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                _len = 0;
                return;
            }

            Span<byte> dest = MemoryMarshal.CreateSpan(ref _data[0], 512);

            _len = Encoding.UTF8.GetBytes(value, dest);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> AsSpan()
            => MemoryMarshal.CreateReadOnlySpan(ref _data[0], _len);

        public override string ToString()
            => _len > 0 ? Encoding.UTF8.GetString(AsSpan()) : string.Empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator string(FixedString512 fs) => fs.ToString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator FixedString512(string s) => new(s);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FixedString1024
    {
        private fixed byte _data[1024];
        private int _len;

        public int Length => _len;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FixedString1024(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                _len = 0;
                return;
            }

            Span<byte> dest = MemoryMarshal.CreateSpan(ref _data[0], 1024);

            _len = Encoding.UTF8.GetBytes(value, dest);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> AsSpan()
            => MemoryMarshal.CreateReadOnlySpan(ref _data[0], _len);

        public override string ToString()
            => _len > 0 ? Encoding.UTF8.GetString(AsSpan()) : string.Empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator string(FixedString1024 fs) => fs.ToString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator FixedString1024(string s) => new(s);
    }
}
