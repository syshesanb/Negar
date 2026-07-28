Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports Negar.Business
Imports Negar.Data

Namespace Negar.Forms
    Public Class HesabdaryInitialSettingsForm
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub HesabdaryInitialSettingsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            LoadSettings()
        End Sub

        Private Function HasInventoryPermission() As Boolean
            Try
                Dim companyId = SessionContext.CurrentCompanyID
                If Not companyId.HasValue Then
                    If SessionContext.CurrentUser IsNot Nothing AndAlso String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                        Return True
                    End If
                End If

                Dim ownerUserId As Integer? = Nothing
                If companyId.HasValue Then
                    Dim ownerObj = Sql.ExecuteScalar("SELECT OwnerUserID FROM Companies WHERE CompanyID = ?", companyId.Value)
                    If ownerObj IsNot Nothing AndAlso Not Convert.IsDBNull(ownerObj) Then
                        ownerUserId = Convert.ToInt32(ownerObj)
                    End If
                End If

                If Not ownerUserId.HasValue AndAlso SessionContext.CurrentUser IsNot Nothing Then
                    ownerUserId = SessionContext.CurrentUser.UserID
                End If

                If Not ownerUserId.HasValue Then Return False

                ' Get Owner UserType
                Dim userTypeObj = Sql.ExecuteScalar("SELECT UserType FROM Users WHERE UserID = ?", ownerUserId.Value)
                Dim uType = If(userTypeObj IsNot Nothing AndAlso Not Convert.IsDBNull(userTypeObj), Convert.ToString(userTypeObj), "")

                ' SuperAdmin always has full access including inventory
                If String.Equals(uType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                    Return True
                End If

                ' Check if user has permission to any inventory module
                Dim invKeys = New String() {
                    PermissionKeys.ManageTradeWarehouse,
                    PermissionKeys.TradeProducts,
                    PermissionKeys.TradeWarehouses,
                    PermissionKeys.TradePurchase,
                    PermissionKeys.TradeSales,
                    PermissionKeys.TradeRemittance,
                    PermissionKeys.TradeReports,
                    PermissionKeys.ManageProducts,
                    PermissionKeys.ManageWarehouses,
                    PermissionKeys.ManagePurchases,
                    PermissionKeys.ManageSales,
                    PermissionKeys.ViewInventory,
                    "AnbarMini", "AnbarMedium", "AnbarBig"
                }

                Dim keyList As New List(Of String)()
                For Each k In invKeys
                    keyList.Add("'" & k & "'")
                Next
                Dim placeholders = String.Join(",", keyList.ToArray())

                Dim sqlCheck = "SELECT COUNT(*) FROM RolePermissions rp " &
                               "INNER JOIN Permissions p ON rp.PermissionID = p.PermissionID " &
                               "WHERE rp.UserID = ? AND (rp.CanView = 1 OR rp.CanCreate = 1 OR rp.CanEdit = 1) " &
                               "AND p.PermissionKey IN (" & placeholders & ")"

                Dim count = Convert.ToInt32(If(Sql.ExecuteScalar(sqlCheck, ownerUserId.Value), 0))
                Return count > 0
            Catch
                Return False
            End Try
        End Function

        Private Sub LoadSettings()
            Try
                Dim companyId = SessionContext.CurrentCompanyID
                Dim dt As DataTable = Sql.ExecuteTable("SELECT EconomicCode, TaxID, AccountLevels, Level1Length, Level2Length, Level3Length, Level4Length, Level5Length, Level6Length, CodingType FROM Companies WHERE CompanyID = ?", companyId)
                
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    Dim row = dt.Rows(0)
                    txtEconomicCode.Text = Convert.ToString(If(row.IsNull("EconomicCode"), "", row("EconomicCode")))
                    txtTaxId.Text = Convert.ToString(If(row.IsNull("TaxID"), "", row("TaxID")))
                    
                    Dim safeGetVal = Function(field As String, defaultVal As Decimal) As Decimal
                                         If row.IsNull(field) OrElse Convert.IsDBNull(row(field)) Then Return defaultVal
                                         Dim v = Convert.ToDecimal(row(field))
                                         Return If(v > 0, v, defaultVal)
                                     End Function

                    numAccountLevels.Value = safeGetVal("AccountLevels", 4D)
                    numLevel1Length.Value = safeGetVal("Level1Length", 2D)
                    numLevel2Length.Value = safeGetVal("Level2Length", 2D)
                    numLevel3Length.Value = safeGetVal("Level3Length", 2D)
                    numLevel4Length.Value = safeGetVal("Level4Length", 2D)
                    numLevel5Length.Value = safeGetVal("Level5Length", 2D)
                    numLevel6Length.Value = safeGetVal("Level6Length", 2D)

                    Dim codingTypeVal As String = Convert.ToString(If(row.IsNull("CodingType"), "", row("CodingType")))
                    If cmbCodingType.Items.Contains(codingTypeVal) Then
                        cmbCodingType.SelectedItem = codingTypeVal
                    Else
                        cmbCodingType.SelectedIndex = 0
                    End If
                Else
                    cmbCodingType.SelectedIndex = 0
                End If

                ' Check inventory module permission rule:
                If HasInventoryPermission() Then
                    cmbCodingType.SelectedItem = "سرفصل های پیش فرض برنامه"
                    cmbCodingType.Enabled = False
                Else
                    cmbCodingType.Enabled = True
                End If
                
                ApplyCodingTypeState()
            Catch ex As Exception
                MessageBox.Show("خطا در بارگذاری تنظیمات: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub CmbCodingType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCodingType.SelectedIndexChanged
            ApplyCodingTypeState()
        End Sub

        Private Sub ApplyCodingTypeState()
            Dim selectedItemStr As String = ""
            If cmbCodingType.SelectedItem IsNot Nothing Then
                selectedItemStr = cmbCodingType.SelectedItem.ToString()
            End If

            If selectedItemStr = "ایجاد سرفصل بصورت دستی" Then
                grpCoding.Enabled = True
                numAccountLevels.Enabled = True
                numLevel1Length.Enabled = True
                numLevel2Length.Enabled = True
                numLevel3Length.Enabled = True
                numLevel4Length.Enabled = True
                numLevel5Length.Enabled = True
                numLevel6Length.Enabled = True
                UpdateLevelsControlsState()
            ElseIf selectedItemStr = "سرفصل های پیش فرض برنامه" Then
                grpCoding.Enabled = False
                numAccountLevels.Value = 5D
                
                Dim grpLen As Decimal = 2D
                Dim kolLen As Decimal = 4D
                Dim moinLen As Decimal = 6D
                Try
                    Dim row = Sql.ExecuteTable("SELECT MAX(LENGTH(AccountCode)) AS MaxLen FROM Cod_Standard WHERE ParentAccountID IS NULL")
                    If row IsNot Nothing AndAlso row.Rows.Count > 0 AndAlso Not row.Rows(0).IsNull("MaxLen") Then
                        grpLen = Convert.ToDecimal(row.Rows(0)("MaxLen"))
                    End If
                    Dim rowK = Sql.ExecuteTable("SELECT MAX(LENGTH(c.AccountCode)) AS MaxLen FROM Cod_Standard c INNER JOIN Cod_Standard p ON c.ParentAccountID = p.AccountID WHERE p.ParentAccountID IS NULL")
                    If rowK IsNot Nothing AndAlso rowK.Rows.Count > 0 AndAlso Not rowK.Rows(0).IsNull("MaxLen") Then
                        kolLen = Convert.ToDecimal(rowK.Rows(0)("MaxLen"))
                    End If
                    Dim rowM = Sql.ExecuteTable("SELECT MAX(LENGTH(c.AccountCode)) AS MaxLen FROM Cod_Standard c INNER JOIN Cod_Standard k ON c.ParentAccountID = k.AccountID INNER JOIN Cod_Standard g ON k.ParentAccountID = g.AccountID")
                    If rowM IsNot Nothing AndAlso rowM.Rows.Count > 0 AndAlso Not rowM.Rows(0).IsNull("MaxLen") Then
                        moinLen = Convert.ToDecimal(rowM.Rows(0)("MaxLen"))
                    End If
                Catch
                End Try

                numLevel1Length.Value = grpLen
                numLevel2Length.Value = kolLen
                numLevel3Length.Value = moinLen
                numLevel4Length.Value = 6D
                numLevel5Length.Value = 6D
                numLevel6Length.Value = 2D

                For Each ctrl As Control In grpCoding.Controls
                    If TypeOf ctrl Is NumericUpDown Then
                        ctrl.Enabled = False
                    End If
                Next
            Else
                grpCoding.Enabled = False
                For Each ctrl As Control In grpCoding.Controls
                    If TypeOf ctrl Is NumericUpDown Then
                        ctrl.Enabled = False
                    End If
                Next
            End If
        End Sub

        Private Sub numAccountLevels_ValueChanged(sender As Object, e As EventArgs) Handles numAccountLevels.ValueChanged
            Dim selectedItemStr As String = ""
            If cmbCodingType.SelectedItem IsNot Nothing Then
                selectedItemStr = cmbCodingType.SelectedItem.ToString()
            End If
            If selectedItemStr = "ایجاد سرفصل بصورت دستی" Then
                UpdateLevelsControlsState()
            End If
        End Sub

        Private Sub UpdateLevelsControlsState()
            Dim lvls = CInt(numAccountLevels.Value)

            numLevel1Length.Enabled = True
            numLevel2Length.Enabled = True

            SetControlState(numLevel3Length, lvls >= 3)
            SetControlState(numLevel4Length, lvls >= 4)
            SetControlState(numLevel5Length, lvls >= 5)
            SetControlState(numLevel6Length, lvls >= 6)
        End Sub

        Private Sub SetControlState(num As NumericUpDown, enabled As Boolean)
            If enabled Then
                num.Enabled = True
                If num.Value < num.Minimum Then
                    num.Value = Math.Max(num.Minimum, 2D)
                End If
            Else
                num.Enabled = False
            End If
        End Sub

        Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            Try
                Dim companyId = SessionContext.CurrentCompanyID
                Dim selectedType As String = ""

                If HasInventoryPermission() Then
                    selectedType = "سرفصل های پیش فرض برنامه"
                ElseIf cmbCodingType.SelectedItem IsNot Nothing Then
                    selectedType = cmbCodingType.SelectedItem.ToString()
                End If

                Sql.ExecuteNonQuery("UPDATE Companies SET EconomicCode = ?, TaxID = ?, AccountLevels = ?, Level1Length = ?, Level2Length = ?, Level3Length = ?, Level4Length = ?, Level5Length = ?, Level6Length = ?, CodingType = ? WHERE CompanyID = ?", 
                                    txtEconomicCode.Text.Trim(), 
                                    txtTaxId.Text.Trim(), 
                                    Convert.ToInt32(numAccountLevels.Value), 
                                    Convert.ToInt32(numLevel1Length.Value), 
                                    Convert.ToInt32(numLevel2Length.Value), 
                                    Convert.ToInt32(numLevel3Length.Value), 
                                    Convert.ToInt32(numLevel4Length.Value), 
                                    Convert.ToInt32(numLevel5Length.Value), 
                                    Convert.ToInt32(numLevel6Length.Value), 
                                    selectedType,
                                    companyId)

                If selectedType = "سرفصل های پیش فرض برنامه" Then
                    PopulateDefaultCodingToSarfaslHesab(companyId.Value)
                End If

                MessageBox.Show("تنظیمات با موفقیت ذخیره شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("خطا در ذخیره‌سازی تنظیمات: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub PopulateDefaultCodingToSarfaslHesab(companyId As Integer)
            Dim dtStd = Sql.ExecuteTable("SELECT AccountID, AccountCode, AccountName, AccountType, ParentAccountID, AccountNature FROM Cod_Standard ORDER BY AccountID")
            If dtStd Is Nothing OrElse dtStd.Rows.Count = 0 Then
                MessageBox.Show("اطلاعات کدینگ استاندارد در دیتابیس یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim dtExist = Sql.ExecuteTable("SELECT AccountID, AccountCode, AccountType, ParentAccountID FROM SarfaslHesab WHERE CompanyID = ?", companyId)
            
            Dim existingMap As New Dictionary(Of String, Integer)()
            If dtExist IsNot Nothing Then
                For Each row As DataRow In dtExist.Rows
                    Dim code = Convert.ToString(row("AccountCode"))
                    Dim id = Convert.ToInt32(row("AccountID"))
                    If Not existingMap.ContainsKey(code) Then
                        existingMap.Add(code, id)
                    End If
                Next
            End If

            ' نقشه برای نگاشت IDهای Cod_Standard به IDهای جدید/موجود در SarfaslHesab
            Dim stdIdToNewIdMap As New Dictionary(Of Integer, Integer)()

            ' مرحله ۱: درج یا بازیابی شناسه تمام رکوردها
            For Each row As DataRow In dtStd.Rows
                Dim stdId = Convert.ToInt32(row("AccountID"))
                Dim code = Convert.ToString(row("AccountCode")).Trim()
                Dim name = Convert.ToString(row("AccountName")).Trim()
                Dim aType = Convert.ToString(row("AccountType")).Trim()
                Dim nature = Convert.ToString(row("AccountNature")).Trim()

                If existingMap.ContainsKey(code) Then
                    stdIdToNewIdMap(stdId) = existingMap(code)
                Else
                    Sql.ExecuteNonQuery(
                        "INSERT INTO SarfaslHesab (CompanyID, AccountCode, AccountName, AccountType, ParentAccountID, IsActive, AccountNature) VALUES (?, ?, ?, ?, NULL, 1, ?)",
                        companyId, code, name, aType, nature)
                    Dim newId = Convert.ToInt32(Sql.ExecuteScalar("SELECT last_insert_rowid()"))
                    stdIdToNewIdMap(stdId) = newId
                    existingMap(code) = newId
                End If
            Next

            ' مرحله ۲: به‌روزرسانی صریح و قطعی فیلد ParentAccountID برای تمامی رکوردها
            For Each row As DataRow In dtStd.Rows
                Dim stdId = Convert.ToInt32(row("AccountID"))
                Dim stdParentId As Integer? = If(row.IsNull("ParentAccountID"), CType(Nothing, Integer?), Convert.ToInt32(row("ParentAccountID")))

                Dim targetAccountId = stdIdToNewIdMap(stdId)

                If stdParentId.HasValue AndAlso stdIdToNewIdMap.ContainsKey(stdParentId.Value) Then
                    Dim pId As Integer = stdIdToNewIdMap(stdParentId.Value)
                    Sql.ExecuteNonQuery(
                        "UPDATE SarfaslHesab SET ParentAccountID = ? WHERE AccountID = ?",
                        pId, targetAccountId)
                Else
                    Sql.ExecuteNonQuery(
                        "UPDATE SarfaslHesab SET ParentAccountID = NULL WHERE AccountID = ?",
                        targetAccountId)
                End If
            Next
        End Sub
    End Class
End Namespace
