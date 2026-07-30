Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.ImportExport
    Public Class ImportExportEditDialog
        Inherits Form

        Private txtPINumber As TextBox
        Private txtSupplier As TextBox
        Private cboCurrency As ComboBox
        Private txtRate As TextBox
        Private txtCurrAmount As TextBox
        Private cboIncoterms As ComboBox
        Private txtNotes As TextBox
        Private btnSave As Button
        Private btnCancel As Button

        Private _impSvc As ImportExportService
        Private _id As Integer
        Private _companyID As Integer

        Public Sub New(companyID As Integer, Optional id As Integer = 0)
            _companyID = companyID
            _id = id
            _impSvc = New ImportExportService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "🚢 ثبت پرونده خرید خارجی / پروفرما (PI) جدید"
            Me.Size = New Size(520, 390)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(248, 249, 250)

            Dim lblPINumber As New Label With {.Text = "شماره پروفرما (PI):", .Location = New Point(370, 25), .AutoSize = True}
            txtPINumber = New TextBox With {.Location = New Point(170, 22), .Size = New Size(180, 26), .Text = "PI-" & (Environment.TickCount Mod 10000).ToString()}

            Dim lblSupplier As New Label With {.Text = "تامین‌کننده خارجی:", .Location = New Point(370, 65), .AutoSize = True}
            txtSupplier = New TextBox With {.Location = New Point(30, 62), .Size = New Size(320, 26), .Text = "Siemens Germany GMBH"}

            Dim lblCurrency As New Label With {.Text = "کد ارز:", .Location = New Point(370, 105), .AutoSize = True}
            cboCurrency = New ComboBox With {.Location = New Point(170, 102), .Size = New Size(180, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cboCurrency.Items.AddRange(New Object() {"EUR", "USD", "AED", "CNY", "TRY"})
            cboCurrency.SelectedIndex = 0

            Dim lblRate As New Label With {.Text = "نرخ تسعیر ارز (ریال):", .Location = New Point(370, 145), .AutoSize = True}
            txtRate = New TextBox With {.Location = New Point(170, 142), .Size = New Size(180, 26), .Text = "650000"}

            Dim lblCurrAmount As New Label With {.Text = "مبلغ ارزی:", .Location = New Point(370, 185), .AutoSize = True}
            txtCurrAmount = New TextBox With {.Location = New Point(170, 182), .Size = New Size(180, 26), .Text = "50000"}

            Dim lblIncoterms As New Label With {.Text = "شرایط اینکوترمز:", .Location = New Point(370, 225), .AutoSize = True}
            cboIncoterms = New ComboBox With {.Location = New Point(170, 222), .Size = New Size(180, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cboIncoterms.Items.AddRange(New Object() {"FOB", "CFR", "CIF", "EXW", "DDP", "FCA"})
            cboIncoterms.SelectedIndex = 0

            Dim lblNotes As New Label With {.Text = "توضیحات پرونده:", .Location = New Point(370, 265), .AutoSize = True}
            txtNotes = New TextBox With {.Location = New Point(30, 262), .Size = New Size(320, 26)}

            btnSave = New Button With {
                .Text = "💾 ثبت و ایجاد پرونده",
                .Size = New Size(140, 36),
                .Location = New Point(210, 310),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSave.Click, AddressOf BtnSave_Click

            btnCancel = New Button With {
                .Text = "انصراف",
                .Size = New Size(100, 36),
                .Location = New Point(100, 310),
                .BackColor = Color.FromArgb(198, 40, 40),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnCancel.Click, Sub() Me.Close()

            Me.Controls.AddRange(New Control() {
                lblPINumber, txtPINumber, lblSupplier, txtSupplier, lblCurrency, cboCurrency,
                lblRate, txtRate, lblCurrAmount, txtCurrAmount, lblIncoterms, cboIncoterms,
                lblNotes, txtNotes, btnSave, btnCancel
            })
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(txtPINumber.Text) Then
                MessageBox.Show("لطفاً شماره پروفرما را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim rate As Double = 650000
            Double.TryParse(txtRate.Text.Replace(",", ""), rate)

            Dim currAmt As Double = 0
            Double.TryParse(txtCurrAmount.Text.Replace(",", ""), currAmt)

            _impSvc.SaveProforma(
                _id, _companyID, txtPINumber.Text, txtSupplier.Text,
                cboCurrency.SelectedItem.ToString(), rate, currAmt,
                cboIncoterms.SelectedItem.ToString(), txtNotes.Text
            )

            MessageBox.Show("پرونده پروفرما (PI) جدید با موفقیت ثبت گردید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub
    End Class
End Namespace
