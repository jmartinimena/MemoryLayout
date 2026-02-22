using System.Text;
using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MemoryLayout.Generator
{
    [Generator]
    public class MemoryLayoutGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // 1. Buscar structs con el atributo [MemoryLayoutContract]
            var structDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => s is StructDeclarationSyntax { AttributeLists.Count: > 0 },
                    transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
                .Where(static m => m is not null);

            // 2. Combinar con la compilación para generar el código
            IncrementalValueProvider<(Compilation, ImmutableArray<StructDeclarationSyntax?>)> compilationAndStructs
                = context.CompilationProvider.Combine(structDeclarations.Collect());

            context.RegisterSourceOutput(compilationAndStructs,
                static (spc, source) => Execute(source.Item1, source.Item2!, spc));
        }

        private static StructDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            var structDeclaration = (StructDeclarationSyntax)context.Node;
            foreach (var attributeList in structDeclaration.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    if (context.SemanticModel.GetSymbolInfo(attribute).Symbol is IMethodSymbol attributeSymbol &&
                        attributeSymbol.ContainingType.ToDisplayString() == "MemoryLayout.MemoryLayoutContractAttribute")
                    {
                        return structDeclaration;
                    }
                }
            }
            return null;
        }

        private static void Execute(Compilation compilation, ImmutableArray<StructDeclarationSyntax> structs, SourceProductionContext context)
        {
            if (structs.IsDefaultOrEmpty) return;

            foreach (var structDef in structs.Distinct())
            {
                var model = compilation.GetSemanticModel(structDef.SyntaxTree);
                if (model.GetDeclaredSymbol(structDef) is not INamedTypeSymbol structSymbol) continue;

                string namespaceName = structSymbol.ContainingNamespace.ToDisplayString();
                string structName = structSymbol.Name;

                // Generar el código de extensión
                string source = GenerateLayoutCode(namespaceName, structName, structSymbol);
                context.AddSource($"{structName}_LayoutEncoder.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        private static string GenerateLayoutCode(string ns, string name, INamedTypeSymbol symbol)
        {
            var fields = symbol.GetMembers().OfType<IFieldSymbol>()
                .Select(f => new
                {
                    Symbol = f,
                    Order = f.GetAttributes()
                        .FirstOrDefault(a => a.AttributeClass?.Name == "LayoutOrderAttribute")
                        ?.ConstructorArguments[0].Value as int? ?? 999
                })
                .OrderBy(x => x.Order).ToList();

            var encodeLogic = new StringBuilder();
            var decodeLogic = new StringBuilder();

            // 1. IMPORTANTE: Los strings en la parte fija ocupan 8 bytes (RelativePointer)
            // El GetTypeSize debe devolver 8 para strings para que fixedSize sea correcto.
            int fixedSize = fields.Sum(f => f.Symbol.Type.SpecialType == SpecialType.System_String ? 8 : GetTypeSize(f.Symbol.Type));
            int currentFixedOffset = 0;

            encodeLogic.AppendLine($"int currentHeapOffset = {fixedSize};");
            encodeLogic.AppendLine("            ref var value = ref this;");

            foreach (var item in fields)
            {
                var f = item.Symbol;
                string typeName = f.Type.ToDisplayString();

                if (f.Type.SpecialType == SpecialType.System_String)
                {
                    encodeLogic.AppendLine($@"
                    // Codificar String: {f.Name}
                    int bytesWritten_{f.Name} = global::MemoryLayout.LayoutEncoder.WriteString(ref Unsafe.Add(ref dest, currentHeapOffset), value.{f.Name});
                    var ptr_{f.Name} = new global::MemoryLayout.RelativePointer(currentHeapOffset, bytesWritten_{f.Name});
                    Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, {currentFixedOffset}), ptr_{f.Name});
                    currentHeapOffset += bytesWritten_{f.Name};");

                            decodeLogic.AppendLine($@"
                    // Leer String: {f.Name}
                    var ptr_{f.Name} = Unsafe.ReadUnaligned<global::MemoryLayout.RelativePointer>(ref Unsafe.Add(ref src, {currentFixedOffset}));
                    result.{f.Name} = global::MemoryLayout.LayoutEncoder.ReadString(buffer, ptr_{f.Name}.Offset, ptr_{f.Name}.Length);");

                    currentFixedOffset += 8;
                }
                else
                {
                    int size = GetTypeSize(f.Type);
                    encodeLogic.AppendLine($"Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, {currentFixedOffset}), value.{f.Name});");
                    decodeLogic.AppendLine($"result.{f.Name} = Unsafe.ReadUnaligned<{typeName}>(ref Unsafe.Add(ref src, {currentFixedOffset}));");
                    currentFixedOffset += size;
                }
            }

            return $@"
            // <auto-generated/>
            using System;
            using System.Runtime.CompilerServices;
            using System.Runtime.InteropServices;
            using MemoryLayout;

            namespace {ns}
            {{
                public partial struct {name} : global::MemoryLayout.IMemoryLayout<{name}>
                {{
                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    public void Encode(ref byte dest, out int totalBytes)
                    {{
                        {encodeLogic}
                        totalBytes = currentHeapOffset;
                    }}

                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    public static void Decode(ReadOnlySpan<byte> buffer, out {name} result)
                    {{
                        result = default;
                        ref byte src = ref MemoryMarshal.GetReference(buffer);
                        {decodeLogic}
                    }}
                }}
            }}";
        }

        private static int GetTypeSize(ITypeSymbol type)
        {
            return type.SpecialType switch
            {
                SpecialType.System_Byte => 1,
                SpecialType.System_Int16 => 2,
                SpecialType.System_Int32 => 4,
                SpecialType.System_Int64 => 8,
                SpecialType.System_Double => 8,
                _ => 4 // Valor por defecto para enums o tipos simples
            };
        }
    }
}
