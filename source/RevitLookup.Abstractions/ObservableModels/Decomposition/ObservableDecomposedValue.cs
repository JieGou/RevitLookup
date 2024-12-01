﻿using CommunityToolkit.Mvvm.ComponentModel;
using LookupEngine.Abstractions.ComponentModel;

namespace RevitLookup.Abstractions.ObservableModels.Decomposition;

public sealed class ObservableDecomposedValue : ObservableObject
{
    public required object? RawValue { get; set; }
    public required string Name { get; set; }
    public required string TypeName { get; set; }
    public required string TypeFullName { get; set; }
    public Descriptor? Descriptor { get; init; }
}