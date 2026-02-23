# MemoryLayout
[![Runtime](https://img.shields.io/badge/.NET-10.0-512bd4)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)

**MemoryLayout** is a serialization engine for .NET, specifically engineered for low-latency scenarios where every microsecond matters. It leverages **Source Generation** to transform complex data structures (including variable-length `strings`) into flat, contiguous, and highly optimized binary representations.

Unlike reflection-based or text-based serializers, **MemoryLayout** provides a zero-overhead bridge to encode and decode data between managed objects and binary buffers with zero Heap allocations..



## 🚀 Key Features

* **Zero-Allocation:** Designed to operate entirely with `Span<T>`, `ReadOnlySpan<byte>`, and `ref byte`, removing pressure from the Garbage Collector (GC).
* **Source Generated:** Encoding (`Encode`) and decoding (`Decode`) logic is generated at compile-time, enabling JIT compiler optimizations like aggressive inlining that are impossible with reflection.
* **Variable String Management:** Implements a **Relative Pointer** system that allows multiple strings to be stored in a flat memory block while maintaining constant offsets for other fields.
* **Aligned & Unaligned-Safe Memory:** Optimized for direct CPU access using `Unsafe.ReadUnaligned` and `WriteUnaligned`, ensuring compatibility and speed across any hardware architecture.
* **Transport Agnostic:** Generates raw bytes. It can be used for Shared Memory (MMF), TCP/UDP sockets, disk storage, or any high-speed communication channel.

---

## 🏗️ Memory Architecture

The engine organizes data using a **Fixed Section + Internal Heap** model:

1.  **Fixed Section (Header):** Contains value types (primitives) and metadata for variable types (`RelativePointer`). This ensures that access to non-string fields is $O(1)$ via constant offsets.
2.  **Internal Heap:** Strings are UTF-8 encoded and stored sequentially immediately after the fixed section, maximizing cache locality.

---

## 📦 Project Structure

* **`MemoryLayout.Abstractions`**: Contains the `IMemoryLayout<T>` interface and marking attributes.
* **`MemoryLayout.Core`**: The low-level core (`LayoutEncoder`) responsible for string marshalling and complex type handling.
* **`MemoryLayout.Messaging`**: Provides generic wrappers (`FlatEnvelope<TSize>`) and predefined memory templates (`Size128` to `Size8192`) for easy buffer management.
* **`MemoryLayout.Generator`**: The code generation engine that automatically injects serialization logic into your `structs`.

---

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
    [LayoutOrder(1)] public string DisplayName;
    [LayoutOrder(2)] public string Bio;
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
