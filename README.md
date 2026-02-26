# MemoryLayout
[![Runtime](https://img.shields.io/badge/.NET-10.0-512bd4)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)

**MemoryLayout** is a serialization engine for .NET, specifically engineered for low-latency scenarios where every microsecond matters. It leverages **Source Generation** to transform complex data structures (including variable-length `strings`) into flat, contiguous, and highly optimized binary representations.

Unlike reflection-based or text-based serializers, **MemoryLayout** provides a zero-overhead bridge to encode and decode data between managed objects and binary buffers with zero Heap allocations..


## 🚀 Key Features

* **Zero-Allocation**: Architected to operate entirely with `Span<T>`, `ReadOnlySpan<byte>`, and `ref byte`, effectively bypassing the Garbage Collector (GC) and eliminating memory pressure.
* **Source Generated**: Encoding and decoding logic is injected at compile-time via Roslyn Source Generators. This enables JIT optimizations, such as aggressive inlining, which are impossible to achieve with reflection-based systems.
* **FixedString Management**: Replaces standard .NET strings with high-performance inline buffers. This ensures that text data is part of the struct's contiguous memory layout, maintaining constant offsets and cache locality.
* **Hardware-Optimized Memory**: Utilizes `Unsafe.ReadUnaligned` and `WriteUnaligned` for direct CPU memory access, ensuring maximum throughput and compatibility across different hardware architectures.
* **Transport Agnostic**: Produces raw, deterministic byte streams. Perfect for Shared Memory (MMF), high-speed UDP/TCP sockets, or low-latency disk storage.


## MemoryLayout Ecosystem

The project is focused on a lean, high-performance core designed to eliminate allocation overhead through compile-time logic:

| Module | Description |
| :--- | :--- |
| **MemoryLayout.Abstractions** | The foundational layer. Contains the `IMemoryLayout<T>` interface and the marking attributes used to define high-performance memory contracts. |
| **MemoryLayout.Generator** | The heart of the project. A Roslyn-based Source Generator that injects zero-allocation serialization and memory access logic directly into your structs, including optimized support for **FixedStrings**. |


## 📦 Installation

Install the package via **NuGet Package Manager Console**:

```powershell
Install-Package MemoryLayout
```

## 🛠️ Usage Guide

### 1. Define the Contract
Apply the `[MemoryLayoutContract]` attribute to a `partial struct`. You can control the binary order using `LayoutOrder`.

```csharp
using MemoryLayout.Abstractions;

[MemoryLayoutContract]
public partial struct UserProfile
{
    [LayoutOrder(0)] public int Id;
    [LayoutOrder(1)] public FixedString16 DisplayName;
    [LayoutOrder(2)] public FixedString16 Bio;
}
```

### 2. Serialization (Encode)
The generated method writes directly to the provided memory address.

```csharp
using MemoryLayout.Core;

Span<byte> buffer = stackalloc byte[128];
ref byte dest = ref MemoryMarshal.GetReference(buffer);

var profile = new UserProfile { Id = 1, DisplayName = "Jeremy", Bio = "Developer" };

// Instant serialization without creating new objects
profile.Encode(ref dest, out int totalBytes);
```
### 3. Reconstruction (Decode)
Decoding retrieves the original types, reconstructing strings from the internal heap within the buffer.

```csharp
using MemoryLayout.Core;

UserProfile.Decode(buffer, out var decodedProfile);
```
