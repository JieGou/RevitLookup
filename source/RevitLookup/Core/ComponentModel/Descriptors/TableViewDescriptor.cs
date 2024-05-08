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
using Autodesk.Revit.DB.Electrical;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class TableViewDescriptor : Descriptor, IDescriptorResolver
{
    private readonly TableView _tableView;
    
    public TableViewDescriptor(TableView tableView)
    {
        _tableView = tableView;
        Name = ElementDescriptor.CreateName(tableView);
    }
    
    public ResolveSet Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            // nameof(TableView.GetAvailableParameterCategories) => ResolveAvailableParameterCategories(), disabled, long computation time
            nameof(TableView.GetAvailableParameters) => ResolveAvailableParameters(),
            nameof(TableView.GetCalculatedValueName) => ResolveCalculatedValueName(),
            nameof(TableView.GetCalculatedValueText) => ResolveCalculatedValueText(),
            nameof(TableView.IsValidSectionType) => ResolveIsValidSectionType(),
            nameof(TableView.GetCellText) => ResolveCellText(),
            _ => null
        };
        
        ResolveSet ResolveAvailableParameters()
        {
            var categories = context.Settings.Categories;
            var resolveSummary = new ResolveSet(categories.Size);
            foreach (Category category in categories)
            {
                var result = TableView.GetAvailableParameters(context, category.Id);
                resolveSummary.AppendVariant(result, $"{category.Name}");
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCalculatedValueName()
        {
            var tableData = _tableView switch
            {
                ViewSchedule viewSchedule => viewSchedule.GetTableData(),
                PanelScheduleView panelScheduleView => panelScheduleView.GetTableData(),
                _ => throw new NotSupportedException($"{_tableView.GetType().FullName} is not supported in the current API version")
            };
            
            var sectionTypes = Enum.GetValues(typeof(SectionType));
            var resolveSummary = new ResolveSet();
            foreach (SectionType sectionType in sectionTypes)
            {
                var tableSectionData = tableData!.GetSectionData(sectionType);
                if (tableSectionData is null) continue;
                
                for (var i = tableSectionData.FirstRowNumber; i < tableSectionData.LastRowNumber; i++)
                for (var j = tableSectionData.FirstColumnNumber; j < tableSectionData.LastColumnNumber; j++)
                {
                    var result = _tableView.GetCalculatedValueName(sectionType, i, j);
                    resolveSummary.AppendVariant(result, $"{sectionType}, row {i}, column {j}: {result}");
                }
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCalculatedValueText()
        {
            var tableData = _tableView switch
            {
                ViewSchedule viewSchedule => viewSchedule.GetTableData(),
                PanelScheduleView panelScheduleView => panelScheduleView.GetTableData(),
                _ => throw new NotSupportedException($"{_tableView.GetType().FullName} is not supported in the current API version")
            };
            
            var sectionTypes = Enum.GetValues(typeof(SectionType));
            var resolveSummary = new ResolveSet();
            foreach (SectionType sectionType in sectionTypes)
            {
                var tableSectionData = tableData!.GetSectionData(sectionType);
                if (tableSectionData is null) continue;
                
                for (var i = tableSectionData.FirstRowNumber; i < tableSectionData.LastRowNumber; i++)
                for (var j = tableSectionData.FirstColumnNumber; j < tableSectionData.LastColumnNumber; j++)
                {
                    var result = _tableView.GetCalculatedValueText(sectionType, i, j);
                    resolveSummary.AppendVariant(result, $"{sectionType}, row {i}, column {j}: {result}");
                }
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveCellText()
        {
            var tableData = _tableView switch
            {
                ViewSchedule viewSchedule => viewSchedule.GetTableData(),
                PanelScheduleView panelScheduleView => panelScheduleView.GetTableData(),
                _ => throw new NotSupportedException($"{_tableView.GetType().FullName} is not supported in the current API version")
            };
            
            var sectionTypes = Enum.GetValues(typeof(SectionType));
            var resolveSummary = new ResolveSet();
            foreach (SectionType sectionType in sectionTypes)
            {
                var tableSectionData = tableData!.GetSectionData(sectionType);
                if (tableSectionData is null) continue;
                for (var i = tableSectionData.FirstRowNumber; i < tableSectionData.LastRowNumber; i++)
                for (var j = tableSectionData.FirstColumnNumber; j < tableSectionData.LastColumnNumber; j++)
                {
                    var result = _tableView.GetCellText(sectionType, i, j);
                    resolveSummary.AppendVariant(result, $"{sectionType}, row {i}, column {j}: {result}");
                }
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveIsValidSectionType()
        {
            var sectionTypes = Enum.GetValues(typeof(SectionType));
            var resolveSummary = new ResolveSet();
            foreach (SectionType sectionType in sectionTypes)
            {
                var result = _tableView.IsValidSectionType(sectionType);
                resolveSummary.AppendVariant(result, $"{sectionType}: {result}");
            }
            
            return resolveSummary;
        }
    }
}