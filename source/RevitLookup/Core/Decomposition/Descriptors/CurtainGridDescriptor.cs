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

public sealed class CurtainGridDescriptor(CurtainGrid curtainGrid) : Descriptor, IDescriptorResolver
{
    public Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(CurtainGrid.GetCell) => ResolveCells,
            nameof(CurtainGrid.GetPanel) => ResolvePanels,
            _ => null
        };

        IVariant ResolveCells()
        {
            var uLinesIds = (List<ElementId>) curtainGrid.GetUGridLineIds();
            var vLinesIds = (List<ElementId>) curtainGrid.GetVGridLineIds();
            uLinesIds.Add(ElementId.InvalidElementId);
            vLinesIds.Add(ElementId.InvalidElementId);
            var capacity = uLinesIds.Count * vLinesIds.Count;

            var variants = Variants.Values<CurtainCell>(capacity);
            foreach (var uLineId in uLinesIds)
            {
                foreach (var vLineId in vLinesIds)
                {
                    var cell = curtainGrid.GetCell(uLineId, vLineId);
                    variants.Add(cell, $"U {uLineId}, V {vLineId}");
                }
            }

            return variants.Consume();
        }

        IVariant ResolvePanels()
        {
            var uLinesIds = (List<ElementId>) curtainGrid.GetUGridLineIds();
            var vLinesIds = (List<ElementId>) curtainGrid.GetVGridLineIds();
            uLinesIds.Add(ElementId.InvalidElementId);
            vLinesIds.Add(ElementId.InvalidElementId);
            var capacity = uLinesIds.Count * vLinesIds.Count;

            var variants = Variants.Values<Panel>(capacity);
            foreach (var uLineId in uLinesIds)
            {
                foreach (var vLineId in vLinesIds)
                {
                    var panel = curtainGrid.GetPanel(uLineId, vLineId);
                    variants.Add(panel, $"U {uLineId}, V {vLineId} - {panel.Name}, ID{panel.Id}");
                }
            }

            return variants.Consume();
        }
    }
}