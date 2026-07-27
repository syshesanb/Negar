Option Strict Off
Option Explicit On

Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports Negar.Forms.Controls

Namespace Negar.Forms.Anbardary
    Public Class AnbardaryVendorsCustomersForm
        Inherits AppBaseForm

        Private partyControl As VendorsCustomersControl

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub InitializeComponent()
            Me.partyControl = New VendorsCustomersControl()
            Me.partyControl.Dock = DockStyle.Fill

            Me.Controls.Add(Me.partyControl)
            Me.Text = "لیست فروشنده و خریدار"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.Font = New Font("B Yekan", 9.0!)
        End Sub
    End Class
End Namespace
