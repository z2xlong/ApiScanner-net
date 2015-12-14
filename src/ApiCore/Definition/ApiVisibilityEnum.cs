namespace ApiScanner.Core
{
    public enum ApiVisibilityEnum
    {
        Default,

        /// <summary>
        /// The member is visible only within its own type.
        /// </summary>
        Private,

        /// <summary>
        /// The member is visible only within the intersection of its family (its own type and any subtypes) and assembly. 
        /// </summary>
        [EnumValue("protected and internal")]
        FamilyAndAssembly,

        /// <summary>
        /// The member is visible only within its own assembly.
        /// </summary>
        [EnumValue("internal")]
        Assembly,

        /// <summary>
        /// The member is visible only within its own type and any subtypes.
        /// </summary>
        [EnumValue("protected")]
        Family,

        /// <summary>
        /// The member is visible only within the union of its family and assembly. 
        /// </summary>
        [EnumValue("protected internal")]
        FamilyOrAssembly,

        /// <summary>
        /// The member is visible everywhere its declaring type is visible.
        /// </summary>
        Public
    }
}
