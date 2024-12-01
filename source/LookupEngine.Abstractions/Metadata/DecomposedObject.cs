﻿using System.Diagnostics;
using JetBrains.Annotations;
using LookupEngine.Abstractions.ComponentModel;

// ReSharper disable once CheckNamespace
namespace LookupEngine.Abstractions;

[PublicAPI]
[DebuggerDisplay("Name = {Name} Value = {RawValue}")]
public sealed class DecomposedObject
{
    public required object? RawValue { get; init; }
    public required string Name { get; init; }
    public required string TypeName { get; set; }
    public required string TypeFullName { get; set; }
    public required List<DecomposedMember> Members { get; init; }
    public Descriptor? Descriptor { get; init; }
}