Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.QC
    Public Class QcEditDialog
        Inherits Form

        Private cboType As ComboBox
        Private txtBatchNumber As TextBox
        Private txtItemName As TextBox
        Private txtSampleQty As TextBox
        Private txtPassedQty As TextBox
        Private txtRejectedQty As TextBox
        Private txtInspector As TextBox
        Private txtNotes As TextBox
        Private btnSave As Button
        Private btnCancel As Button

        Private _qcSvc As QcService
        Private _id As Integer
        Private _companyID As Integer

        Public Sub New(companyID As Integer, Optional id As Integer = 0)
            _companyID = companyID
            _id = id
            _qcSvc = New QcService()
            InitializeUI()
        End Sub

        Private Sub InitializeUI()
            Me.Text = "🔬 ثبت جدید برگه بازرسی کیفی (QC Inspection)"
            Me.Size = New Size(520, 360)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(248, 249, 250)

            Dim lblType As New Label With {.Text = "نوع بازرسی کیفی:", .Location = New Point(370, 25), .AutoSize = True}
            cboType = New ComboBox With {.Location = New Point(170, 22), .Size = New Size(180, 26), .DropDownStyle = ComboBoxStyle.DropDownList}
            cboType.Items.AddRange(New Object() {"بازرسی ورودی IQC", "بازرسی حین تولید IPQC", "محصول نهایی OQC"})
            cboType.SelectedIndex = 1

            Dim lblBatch As New Label With {.Text = "شماره محموله/بچ (Batch):", .Location = New Point(370, 65), .AutoSize = True}
            txtBatchNumber = New TextBox With {.Location = New Point(170, 62), .Size = New Size(180, 26), .Text = "BATCH-" & (Environment.TickCount Mod 10000).ToString()}

            Dim lblItem As New Label With {.Text = "نام کالا / محصول:", .Location = New Point(370, 105), .AutoSize = True}
            txtItemName = New TextBox With {.Location = New Point(30, 102), .Size = New Size(320, 26), .Text = "پروفیل آلومینیوم آنودایز شده"}

            Dim lblSample As New Label With {.Text = "تعداد نمونه آزمایشی:", .Location = New Point(370, 145), .AutoSize = True}
            txtSampleQty = New TextBox With {.Location = New Point(170, 142), .Size = New Size(180, 26), .Text = "200"}

            Dim lblPassed As New Label With {.Text = "تعداد سالم (Pass):", .Location = New Point(370, 185), .AutoSize = True}
            txtPassedQty = New TextBox With {.Location = New Point(170, 182), .Size = New Size(180, 26), .Text = "192"}

            Dim lblInspector As New Label With {.Text = "بازرس مسئول QC:", .Location = New Point(370, 225), .AutoSize = True}
            txtInspector = New TextBox With {.Location = New Point(30, 222), .Size = New Size(320, 26), .Text = "مهندس محمدی (کارشناس کیفیت)"}

            txtRejectedQty = New TextBox With {.Location = New Point(30, 182), .Size = New Size(120, 26), .Visible = False}
            txtNotes = New TextBox With {.Location = New Point(30, 222), .Size = New Size(120, 26), .Visible = False}

            btnSave = New Button With {
                .Text = "💾 ثبت برگه بازرسی کیفی",
                .Size = New Size(170, 36),
                .Location = New Point(180, 270),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSave.Click, AddressOf BtnSave_Click

            btnCancel = New Button With {
                .Text = "انصراف",
                .Size = New Size(100, 36),
                .Location = New Point(70, 270),
                .BackColor = Color.FromArgb(198, 40, 40),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnCancel.Click, Sub() Me.Close()

            Me.Controls.AddRange(New Control() {
                lblType, cboType, lblBatch, txtBatchNumber, lblItem, txtItemName,
                lblSample, txtSampleQty, lblPassed, txtPassedQty,
                lblInspector, txtInspector, btnSave, btnCancel
            })
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(txtItemName.Text) Then
                MessageBox.Show("لطفاً نام کالا/محصول را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim sampleQty As Double = 100
            Dim passQty As Double = 95
            Double.TryParse(txtSampleQty.Text.Replace(",", ""), sampleQty)
            Double.TryParse(txtPassedQty.Text.Replace(",", ""), passQty)
            Dim rejectQty As Double = Math.Max(0, sampleQty - passQty)

            _qcSvc.SaveInspection(
                _id, _companyID, cboType.SelectedItem.ToString(), txtBatchNumber.Text,
                txtItemName.Text, sampleQty, passQty, rejectQty,
                txtInspector.Text, "تست کنترل کیفیت در ایستگاه تولید"
            )

            MessageBox.Show("برگه بازرسی کیفی با موفقیت ثبت گردید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub
    End Class
End Namespace
