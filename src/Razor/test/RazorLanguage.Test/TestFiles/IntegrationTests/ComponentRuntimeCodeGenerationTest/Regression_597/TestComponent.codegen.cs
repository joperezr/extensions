// <auto-generated/>
#pragma warning disable 1591
namespace Test
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    public class TestComponent : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenComponent<Test.Counter>(0);
            __builder.AddAttribute(1, "v", 
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
                  y

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(2, "vChanged", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => y = __value, y));
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
       
    string y = null;

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
