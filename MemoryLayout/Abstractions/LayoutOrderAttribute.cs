namespace MemoryLayout.Abstractions
{
    /// <summary>
    /// Marca un struct para la generación automática de disposición de memoria binaria.
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct)]
    public class MemoryLayoutContractAttribute : Attribute { }

    /// <summary>
    /// Define el orden explícito de un campo en el layout binario.
    /// Es vital para mantener la compatibilidad entre versiones.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class LayoutOrderAttribute(int order) : Attribute
    {
        public int Order { get; } = order;
    }
}
