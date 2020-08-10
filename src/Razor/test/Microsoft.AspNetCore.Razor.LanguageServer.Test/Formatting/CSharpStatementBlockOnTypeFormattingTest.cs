﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Xunit;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting
{
    public class CSharpStatementBlockOnTypeFormattingTest : FormattingTestBase
    {
        [Fact]
        public async Task CloseCurly_IfBlock_SingleLine()
        {
            await RunOnTypeFormattingTestAsync(
input: @"
@{
 if(true){|}|
}
",
expected: @"
@{
    if (true) { }
}
",
triggerCharacter: "}");
        }

        [Fact]
        public async Task CloseCurly_IfBlock_MultiLine()
        {
            await RunOnTypeFormattingTestAsync(
input: @"
@{
 if(true)
{
 |}|
}
",
expected: @"
@{
    if (true)
    {
    }
}
",
triggerCharacter: "}");
        }

        [Fact]
        public async Task Semicolon_Variable_SingleLine()
        {
            await RunOnTypeFormattingTestAsync(
input: @"
@{
 var x = 'foo'|;|
}
",
expected: @"
@{
    var x = 'foo';
}
",
triggerCharacter: ";");
        }

        [Fact]
        public async Task Semicolon_Variable_MultiLine()
        {
            await RunOnTypeFormattingTestAsync(
input: @"
@{
 var x = @""
foo""|;|
}
",
expected: @"
@{
    var x = @""
foo"";
}
",
triggerCharacter: ";");
        }
    }
}