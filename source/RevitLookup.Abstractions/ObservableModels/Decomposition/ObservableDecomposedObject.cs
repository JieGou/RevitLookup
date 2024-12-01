﻿using CommunityToolkit.Mvvm.ComponentModel;
using LookupEngine.Abstractions.ComponentModel;

namespace RevitLookup.Abstractions.ObservableModels.Decomposition;

public sealed class ObservableDecomposedObject : ObservableObject
{
    public required object? RawValue { get; init; }
    public required string Name { get; init; }
    public required string TypeName { get; set; }
    public required string TypeFullName { get; set; }
    public required List<ObservableDecomposedMember> Members { get; init; }
    public string? Description { get; init; }
    public Descriptor? Descriptor { get; init; }
}