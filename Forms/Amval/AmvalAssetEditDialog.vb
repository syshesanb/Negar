Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Negar.Business

Namespace Negar.Forms.Amval
    Public Class AmvalAssetEditDialog
        Inherits Form

        Private txtAssetCode As TextBox
        Private txtPlakNo As TextBox
        Private txtAssetName As TextBox
        Private cmbCategory As ComboBox
        Private txtPurchaseDate As TextBox
        Private txtPurchasePrice As TextBox
        Private txtSalvageValue As TextBox
        Private txtLocation As TextBox
        Private cmbPersonnel As ComboBox
        Private txtNotes As TextBox
        Private btnSave As Button
        Private btnCancel As Button

        Private _amvalSvc As AmvalService
        Private _payrollSvc As PayrollService
        Private _assetID As Integer
        Private _companyID As Integer

        Public Sub New(companyID As Integer, Optional assetID As Integer = 0)
            _companyID = companyID
            _assetID = assetID
            _amvalSvc = New AmvalService()
            _payrollSvc = New PayrollService()
            InitializeUI()
            LoadData()
        End Sub

        Private Sub InitializeUI()
            Me.Text = If(_assetID <= 0, "🏛️ ثبت دارایی ثابت جدید", "🏛️ ویرایش شناسنامه دارایی ثابت")
            Me.Size = New Size(540, 520)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(248, 249, 250)

            Dim lblCode As New Label With {.Text = "کد دارایی:", .Location = New Point(380, 25), .AutoSize = True}
            txtAssetCode = New TextBox With {.Location = New Point(180, 22), .Size = New Size(180, 26)}

            Dim lblPlak As New Label With {.Text = "پلاک اموال (بارکد):", .Location = New Point(380, 65), .AutoSize = True}
            txtPlakNo = New TextBox With {.Location = New Point(180, 62), .Size = New Size(180, 26)}

            Dim lblName As New Label With {.Text = "نام / شرح دارایی:", .Location = New Point(380, 105), .AutoSize = True}
            txtAssetName = New TextBox With {.Location = New Point(30, 102), .Size = New Size(330, 26)}

            Dim lblCat As New Label With {.Text = "گروه دارایی:", .Location = New Point(380, 145), .AutoSize = True}
            cmbCategory = New ComboBox With {.Location = New Point(30, 142), .Size = New Size(330, 26), .DropDownStyle = ComboBoxStyle.DropDownList}

            Dim lblPDate As New Label With {.Text = "تاریخ خرید (شمسی):", .Location = New Point(380, 185), .AutoSize = True}
            txtPurchaseDate = New TextBox With {.Location = New Point(180, 182), .Size = New Size(180, 26), .Text = PersianDateHelper.ToPersian(DateTime.Now)}

            Dim lblPPrice As New Label With {.Text = "بهای تمام‌شده (ریال):", .Location = New Point(380, 225), .AutoSize = True}
            txtPurchasePrice = New TextBox With {.Location = New Point(180, 222), .Size = New Size(180, 26), .Text = "0"}

            Dim lblSalVal As New Label With {.Text = "ارزش اسقاط (ریال):", .Location = New Point(380, 265), .AutoSize = True}
            txtSalvageValue = New TextBox With {.Location = New Point(180, 262), .Size = New Size(180, 26), .Text = "0"}

            Dim lblLoc As New Label With {.Text = "محل استقرار فیزیکی:", .Location = New Point(380, 305), .AutoSize = True}
            txtLocation = New TextBox With {.Location = New Point(30, 302), .Size = New Size(330, 26)}

            Dim lblCust As New Label With {.Text = "امین اموال / تحویل‌گیرنده:", .Location = New Point(380, 345), .AutoSize = True}
            cmbPersonnel = New ComboBox With {.Location = New Point(30, 342), .Size = New Size(330, 26), .DropDownStyle = ComboBoxStyle.DropDownList}

            Dim lblNotes As New Label With {.Text = "توضیحات تکمیلی:", .Location = New Point(380, 385), .AutoSize = True}
            txtNotes = New TextBox With {.Location = New Point(30, 382), .Size = New Size(330, 26)}

            btnSave = New Button With {
                .Text = "💾 ثبت و ذخیره",
                .Size = New Size(120, 36),
                .Location = New Point(240, 430),
                .BackColor = Color.FromArgb(46, 125, 50),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnSave.Click, AddressOf BtnSave_Click

            btnCancel = New Button With {
                .Text = "انصراف",
                .Size = New Size(100, 36),
                .Location = New Point(130, 430),
                .BackColor = Color.FromArgb(198, 40, 40),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat
            }
            AddHandler btnCancel.Click, Sub() Me.Close()

            Me.Controls.AddRange(New Control() {
                lblCode, txtAssetCode, lblPlak, txtPlakNo, lblName, txtAssetName,
                lblCat, cmbCategory, lblPDate, txtPurchaseDate, lblPPrice, txtPurchasePrice,
                lblSalVal, txtSalvageValue, lblLoc, txtLocation, lblCust, cmbPersonnel,
                lblNotes, txtNotes, btnSave, btnCancel
            })
        End Sub

        Private Sub LoadData()
            ' Load Categories
            cmbCategory.Items.Clear()
            Dim dtCat = _amvalSvc.GetCategories()
            If dtCat IsNot Nothing Then
                For Each r As DataRow In dtCat.Rows
                    cmbCategory.Items.Add(New KeyValuePair(Of Integer, String)(Convert.ToInt32(r("CategoryID")), Convert.ToString(r("CategoryName"))))
                Next
            End If
            If cmbCategory.Items.Count > 0 Then cmbCategory.SelectedIndex = 0

            ' Load Personnel
            cmbPersonnel.Items.Clear()
            cmbPersonnel.Items.Add(New KeyValuePair(Of Integer, String)(0, "-- بدون امین اموال --"))
            Dim dtPers = _payrollSvc.GetPersonnelList()
            If dtPers IsNot Nothing Then
                For Each r As DataRow In dtPers.Rows
                    cmbPersonnel.Items.Add(New KeyValuePair(Of Integer, String)(Convert.ToInt32(r("PersonnelID")), Convert.ToString(r("FullName"))))
                Next
            End If
            cmbPersonnel.SelectedIndex = 0

            If _assetID > 0 Then
                Dim row = _amvalSvc.GetAssetById(_assetID)
                If row IsNot Nothing Then
                    txtAssetCode.Text = Convert.ToString(row("AssetCode"))
                    txtPlakNo.Text = Convert.ToString(row("PlakNo"))
                    txtAssetName.Text = Convert.ToString(row("AssetName"))
                    txtPurchaseDate.Text = Convert.ToString(row("PurchaseDate"))
                    txtPurchasePrice.Text = Convert.ToString(row("PurchasePrice"))
                    txtSalvageValue.Text = Convert.ToString(row("SalvageValue"))
                    txtLocation.Text = Convert.ToString(row("Location"))
                    txtNotes.Text = Convert.ToString(row("Notes"))

                    Dim catId = Convert.ToInt32(If(IsDBNull(row("CategoryID")), 0, row("CategoryID")))
                    For i As Integer = 0 To cmbCategory.Items.Count - 1
                        Dim kvp = CType(cmbCategory.Items(i), KeyValuePair(Of Integer, String))
                        If kvp.Key = catId Then
                            cmbCategory.SelectedIndex = i
                            Exit For
                        End If
                    Next

                    Dim persId = Convert.ToInt32(If(IsDBNull(row("PersonnelID")), 0, row("PersonnelID")))
                    For i As Integer = 0 To cmbPersonnel.Items.Count - 1
                        Dim kvp = CType(cmbPersonnel.Items(i), KeyValuePair(Of Integer, String))
                        If kvp.Key = persId Then
                            cmbPersonnel.SelectedIndex = i
                            Exit For
                        End If
                    Next
                End If
            Else
                txtAssetCode.Text = (Environment.TickCount Mod 10000).ToString()
                txtPlakNo.Text = "A-" & (Environment.TickCount Mod 100000).ToString()
            End If
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(txtAssetName.Text) Then
                MessageBox.Show("لطفاً نام / شرح دارایی را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim catId As Integer = 0
            If cmbCategory.SelectedItem IsNot Nothing Then
                catId = CType(cmbCategory.SelectedItem, KeyValuePair(Of Integer, String)).Key
            End If

            Dim persId As Integer = 0
            If cmbPersonnel.SelectedItem IsNot Nothing Then
                persId = CType(cmbPersonnel.SelectedItem, KeyValuePair(Of Integer, String)).Key
            End If

            Dim pPrice As Double = 0
            Double.TryParse(txtPurchasePrice.Text, pPrice)

            Dim sVal As Double = 0
            Double.TryParse(txtSalvageValue.Text, sVal)

            _amvalSvc.SaveAsset(_assetID, _companyID, txtAssetCode.Text, txtPlakNo.Text, txtAssetName.Text, catId, txtPurchaseDate.Text, pPrice, sVal, txtLocation.Text, persId, txtNotes.Text)

            MessageBox.Show("مشخصات دارایی ثابت با موفقیت ثبت گردید.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub
    End Class
End Namespace
