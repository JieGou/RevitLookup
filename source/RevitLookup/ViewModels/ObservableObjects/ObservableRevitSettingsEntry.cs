﻿// Copyright 2003-2024 by Autodesk, Inc.
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

using System.ComponentModel.DataAnnotations;

namespace RevitLookup.ViewModels.ObservableObjects;

public sealed partial class ObservableRevitSettingsEntry : ObservableValidator
{
    [ObservableProperty] private bool _isActive;
    [ObservableProperty] [Required] [NotifyDataErrorInfo] private string _category;
    [ObservableProperty] [Required] [NotifyDataErrorInfo] private string _property;
    [ObservableProperty] private string _value;
    
    public string DefaultValue { get; set; }

    public ObservableRevitSettingsEntry Clone()
    {
        return new ObservableRevitSettingsEntry
        {
            Category = Category,
            Property = Property,
            Value = Value
        };
    }
}