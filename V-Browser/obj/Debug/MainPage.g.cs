﻿

#pragma checksum "C:\Users\ITALO\Documents\Visual Studio 2013\Projects\V-Browser\V-Browser\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E6CB8375BF0CC85B44AE9796FFF4C883"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace V_Browser
{
    partial class MainPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 32 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.tabs_ItemClick;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 51 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Button_Click_1;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 22 "..\..\MainPage.xaml"
                ((global::Microsoft.Advertising.WinRT.UI.AdControl)(target)).ErrorOccurred += this.AdControl_ErrorOccurred;
                 #line default
                 #line hidden
                #line 22 "..\..\MainPage.xaml"
                ((global::Microsoft.Advertising.WinRT.UI.AdControl)(target)).AdRefreshed += this.AdControl_AdRefreshed;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 23 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Button_Click_2;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


