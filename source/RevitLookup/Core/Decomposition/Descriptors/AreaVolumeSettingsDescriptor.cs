// Copyright 2003-2024 by Autodesk, Inc.
// 
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
// 
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
// 
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

using System.Reflection;
using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;

namespace RevitLookup.Core.Decomposition.Descriptors;

public sealed class AreaVolumeSettingsDescriptor(AreaVolumeSettings settings) : ElementDescriptor(settings)
{
    public override Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(AreaVolumeSettings.GetAreaVolumeSettings) => ResolveGet,
            nameof(AreaVolumeSettings.GetSpatialElementBoundaryLocation) => ResolveGetSpatialElementBoundaryLocation,
            _ => null
        };

        IVariant ResolveGet()
        {
            return Variants.Value(AreaVolumeSettings.GetAreaVolumeSettings(settings.Document));
        }

        IVariant ResolveGetSpatialElementBoundaryLocation()
        {
            var conditions = Enum.GetValues(typeof(SpatialElementType));
            var variants = Variants.Values<SpatialElementBoundaryLocation>(conditions.Length);

            foreach (SpatialElementType condition in conditions)
            {
                var result = settings.GetSpatialElementBoundaryLocation(condition);
                variants.Add(result, $"{condition}: {result}");
            }

            return variants.Consume();
        }
    }

    public override void RegisterExtensions(IExtensionManager manager)
    {
    }
}