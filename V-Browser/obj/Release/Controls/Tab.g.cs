﻿

#pragma checksum "C:\Users\ITALO\Documents\Visual Studio 2013\Projects\V-Browser\V-Browser\Controls\Tab.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D464296C99E18B494455E02EAFF16E14"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace V_Browser.Controls
{
    partial class Tab : global::Windows.UI.Xaml.Controls.UserControl, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 99 "..\..\Controls\Tab.xaml"
                ((global::V_Browser.Controls.Navigator)(target)).NavigationCompleted += this.webview_NavigationCompleted;
                 #line default
                 #line hidden
                #line 99 "..\..\Controls\Tab.xaml"
                ((global::V_Browser.Controls.Navigator)(target)).NavigationStarting += this.webview_FrameNavigationStarting;
                 #line default
                 #line hidden
                #line 99 "..\..\Controls\Tab.xaml"
                ((global::V_Browser.Controls.Navigator)(target)).UnviewableContentIdentified += this.webview_UnviewableContentIdentified;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 27 "..\..\Controls\Tab.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.AppBarButton_Click_1;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 31 "..\..\Controls\Tab.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).KeyDown += this.TextBox_KeyDown;
                 #line default
                 #line hidden
                #line 31 "..\..\Controls\Tab.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).GotFocus += this.txtAdress_GotFocus;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 33 "..\..\Controls\Tab.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.AppBarButton_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 34 "..\..\Controls\Tab.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.AppBarButton_Click_3;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 35 "..\..\Controls\Tab.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.AppBarButton_Click_2;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 90 "..\..\Controls\Tab.xaml"
                ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target)).Click += this.MenuFlyoutItem_Click;
                 #line default
                 #line hidden
                break;
            case 8:
                #line 91 "..\..\Controls\Tab.xaml"
                ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target)).Click += this.MenuFlyoutItem_Click_1;
                 #line default
                 #line hidden
                break;
            case 9:
                #line 92 "..\..\Controls\Tab.xaml"
                ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target)).Click += this.MenuFlyoutItem_Click_2;
                 #line default
                 #line hidden
                break;
            case 10:
                #line 61 "..\..\Controls\Tab.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.urls_ItemClick;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

