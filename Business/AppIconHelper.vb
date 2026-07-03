Option Strict Off
Option Explicit On

Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Namespace Sys_Hes_Anb.Business
    Public Module AppIconHelper
        Private _cachedIcon As Icon = Nothing

        Public Function GetAppIcon() As Icon
            If _cachedIcon IsNot Nothing Then Return _cachedIcon
            Try
                Using bmp As New Bitmap(64, 64)
                    Using g As Graphics = Graphics.FromImage(bmp)
                        g.SmoothingMode = SmoothingMode.AntiAlias
                        ' رسم آیکون حرفه‌ای مالی و حسابداری
                        Using b As New SolidBrush(Color.FromArgb(39, 174, 96))
                            g.FillRectangle(b, 8, 4, 48, 56)
                        End Using
                        Using b As New SolidBrush(Color.White)
                            g.FillRectangle(b, 14, 10, 36, 10)
                            g.FillRectangle(b, 14, 24, 10, 10)
                            g.FillRectangle(b, 27, 24, 10, 10)
                            g.FillRectangle(b, 40, 24, 10, 10)
                            g.FillRectangle(b, 14, 38, 10, 10)
                            g.FillRectangle(b, 27, 38, 10, 10)
                            g.FillRectangle(b, 40, 38, 10, 10)
                            g.FillRectangle(b, 14, 50, 23, 6)
                        End Using
                    End Using
                    _cachedIcon = Icon.FromHandle(bmp.GetHicon())
                End Using
            Catch
            End Try
            Return _cachedIcon
        End Function

        Public Sub ApplyAppIcon(targetForm As Form)
            If targetForm Is Nothing Then Return
            Try
                Dim ico = GetAppIcon()
                If ico IsNot Nothing Then
                    targetForm.Icon = ico
                End If
            Catch
            End Try
        End Sub
    End Module
End Namespace
