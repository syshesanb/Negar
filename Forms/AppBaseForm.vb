Option Strict Off
Option Explicit On

Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms

    ''' <summary>
    ''' فرم پایه مشترک پروژه.
    ''' هر فرم جدیدی که در پروژه ساخته می‌شود باید از این کلاس ارث ببرد
    ''' به جای اینکه مستقیماً از System.Windows.Forms.Form ارث ببرد.
    '''
    ''' ویژگی‌های خودکار:
    '''  - اعمال تم رنگی فرم (ApplyFormTheme)
    '''  - افزودن نوار وضعیت مشترک (AppendStatusBar)
    '''    شامل: نام کاربر / شرکت جاری / سال مالی / تاریخ و ساعت زنده
    '''
    ''' نحوه استفاده:
    '''   Public Class MyNewForm
    '''       Inherits AppBaseForm
    '''   ...
    ''' </summary>
    Public Class AppBaseForm
        Inherits Form

        ''' <summary>
        ''' اگر True باشد، نوار وضعیت مشترک به این فرم اضافه نمی‌شود.
        ''' برای فرم‌های کوچک مثل Dialog، Popup یا Print می‌توان True کرد.
        ''' </summary>
        Protected Overridable ReadOnly Property SkipSharedStatusBar As Boolean
            Get
                Return False
            End Get
        End Property

        Protected Overrides Sub OnLoad(e As EventArgs)
            MyBase.OnLoad(e)
            Try
                ThemeHelper.ApplyFormTheme(Me)
            Catch
            End Try
            If Not SkipSharedStatusBar Then
                Try
                    ThemeHelper.AppendStatusBar(Me)
                Catch
                End Try
            End If
        End Sub
    End Class

End Namespace
