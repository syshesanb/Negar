Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports Sys_Hes_Anb.Business
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Forms
    Public Class DataMigrationForm
        Inherits Form

        ' ── Properties ─────────────────────────────────────────────────────
        Public ReadOnly Property SelectedTargetUserID As Integer?
            Get
                If cmbTargetUser.SelectedItem Is Nothing Then Return Nothing
                Dim drv = TryCast(cmbTargetUser.SelectedItem, DataRowView)
                If drv Is Nothing Then Return Nothing
                Dim v = Convert.ToInt32(drv("UserID"))
                If v <= 0 Then Return Nothing
                Return v
            End Get
        End Property

        Public ReadOnly Property SelectedTargetCompanyID As Integer?
            Get
                If Not cmbTargetCompany.Visible Then Return Nothing
                If cmbTargetCompany.SelectedItem Is Nothing Then Return Nothing
                Dim drv = TryCast(cmbTargetCompany.SelectedItem, DataRowView)
                If drv Is Nothing Then Return Nothing
                Dim v = Convert.ToInt32(drv("CompanyID"))
                If v <= 0 Then Return Nothing
                Return v
            End Get
        End Property

        ' ── Load ─────────────────────────────────────────────────────────────
        Private Sub DataMigrationForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            ThemeHelper.ApplyFormTheme(Me)
            LoadUsers()
        End Sub

        Private Sub LoadUsers()
            Try
                Dim dt As DataTable
                If SessionContext.CurrentUser IsNot Nothing AndAlso
                   String.Equals(SessionContext.CurrentUser.UserType, "SuperAdmin", StringComparison.OrdinalIgnoreCase) Then
                    dt = Sql.ExecuteTable("SELECT UserID, FullName || ' (' || Username || ')' AS DisplayName FROM Users WHERE IsActive = 1 ORDER BY FullName")
                Else
                    ' Non-admin: only show self
                    dt = Sql.ExecuteTable("SELECT UserID, FullName || ' (' || Username || ')' AS DisplayName FROM Users WHERE UserID = ?",
                                          SessionContext.CurrentUser.UserID)
                End If

                cmbTargetUser.DataSource = dt
                cmbTargetUser.DisplayMember = "DisplayName"
                cmbTargetUser.ValueMember = "UserID"
                If dt.Rows.Count = 0 Then cmbTargetUser.DataSource = Nothing
            Catch ex As Exception
                ' Silently ignore if users cannot be loaded
            End Try
        End Sub

        Private Sub cmbTargetUser_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTargetUser.SelectedIndexChanged
            LoadUserCompanies()
        End Sub

        Private Sub LoadUserCompanies()
            If Not SelectedTargetUserID.HasValue Then
                cmbTargetCompany.Visible = False
                lblTargetCompany.Visible = False
                pnlUserSelect.Height = 50
                Return
            End If

            Try
                Dim uid = SelectedTargetUserID.Value
                Dim dt = Sql.ExecuteTable(
                    "SELECT CompanyID, CompanyName || ' (' || CompanyCode || ')' AS DisplayName FROM Companies WHERE OwnerUserID = ? AND IsActive = 1 ORDER BY CompanyName",
                    uid)

                If dt.Rows.Count > 1 Then
                    ' Multiple companies: show company selector
                    cmbTargetCompany.DataSource = dt
                    cmbTargetCompany.DisplayMember = "DisplayName"
                    cmbTargetCompany.ValueMember = "CompanyID"
                    cmbTargetCompany.Visible = True
                    lblTargetCompany.Visible = True
                    pnlUserSelect.Height = 72
                ElseIf dt.Rows.Count = 1 Then
                    ' Single company: auto-select, hide combo
                    cmbTargetCompany.DataSource = dt
                    cmbTargetCompany.DisplayMember = "DisplayName"
                    cmbTargetCompany.ValueMember = "CompanyID"
                    cmbTargetCompany.Visible = False
                    lblTargetCompany.Visible = False
                    pnlUserSelect.Height = 50
                Else
                    cmbTargetCompany.DataSource = Nothing
                    cmbTargetCompany.Visible = False
                    lblTargetCompany.Visible = False
                    pnlUserSelect.Height = 50
                End If
            Catch ex As Exception
                ' ignore
            End Try
        End Sub

        ' ── Get effective CompanyID ───────────────────────────────────────
        Public Function GetEffectiveCompanyID() As Integer?
            ' Priority: explicit company combobox selection
            If SelectedTargetCompanyID.HasValue Then Return SelectedTargetCompanyID

            ' Fallback: single company for the selected user
            If Not SelectedTargetUserID.HasValue Then Return Nothing
            Try
                Dim obj = Sql.ExecuteScalar(
                    "SELECT CompanyID FROM Companies WHERE OwnerUserID = ? AND IsActive = 1 ORDER BY CompanyID LIMIT 1",
                    SelectedTargetUserID.Value)
                If obj IsNot Nothing AndAlso Not Convert.IsDBNull(obj) Then Return Convert.ToInt32(obj)
            Catch
            End Try
            Return Nothing
        End Function

        ' ── Verify user+company selected before smart operations ──────────
        Private Function EnsureUserAndCompanySelected() As Boolean
            If Not SelectedTargetUserID.HasValue Then
                MessageBox.Show("لطفاً ابتدا کاربری را از کامبوباکس بالا انتخاب کنید.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If
            If Not GetEffectiveCompanyID().HasValue Then
                Dim msg = "هنوز شرکتی برای این کاربر ایجاد نشده است" & Environment.NewLine &
                          "ابتدا باید یک شرکت جدید ایجاد کنید" & Environment.NewLine &
                          "توجه :" & Environment.NewLine &
                          "در موقع ایجاد شرکت ، به تعداد سطوح و طول کد هر سطح دقت کنید " & Environment.NewLine &
                          "تعداد سطوح کد حداکثر 6 سطح وطول کد هر سطح حداکثر 6 رقم می باشد" & Environment.NewLine &
                          "تعداد سطح و طول کد هر سطح باید منطبق بر اطلاعاتی باشد که می خواهید" & Environment.NewLine &
                          "تبدیل کنید ، در غیر این صورت برخی از اطلاعات خود را از دست خواهید داد" & Environment.NewLine & Environment.NewLine &
                          "آیا می خواهید فرم ایجاد شرکت باز شود؟"
                Dim ans = MessageBox.Show(msg, "ایجاد شرکت لازم است", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                If ans = DialogResult.Yes Then
                    Dim cfForm As New CompanyFiscalYearForm(Nothing, openOnSelectTab:=False, overrideOwnerUserId:=SelectedTargetUserID.Value)
                    cfForm.ShowDialog(Me)
                    ' Refresh companies after form closes
                    LoadUserCompanies()
                    If Not GetEffectiveCompanyID().HasValue Then
                        MessageBox.Show("شرکتی ایجاد نشد. لطفاً ابتدا یک شرکت ایجاد کنید.", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return False
                    End If
                Else
                    Return False
                End If
            End If
            Return True
        End Function

        ' ─── CSV Helpers ─────────────────────────────────────────────────
        Private Sub HandleDownload(type As String, defaultName As String)
            Using sfd As New SaveFileDialog()
                sfd.Filter = "CSV Files (*.csv)|*.csv"
                sfd.FileName = defaultName
                If sfd.ShowDialog() = DialogResult.OK Then
                    Try
                        CsvImportService.CreateTemplate(type, sfd.FileName)
                        MessageBox.Show("فایل نمونه با موفقیت ذخیره شد.", "عملیات موفق", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Catch ex As Exception
                        MessageBox.Show("خطا در ذخیره فایل: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End Using
        End Sub

        Private Sub HandleImport(type As String)
            Using ofd As New OpenFileDialog()
                ofd.Filter = "Excel or CSV files (*.csv;*.xlsx)|*.csv;*.xlsx"
                If ofd.ShowDialog() = DialogResult.OK Then
                    Try
                        Dim result = CsvImportService.ImportData(type, ofd.FileName)
                        MessageBox.Show(result, "نتیجه عملیات", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Catch ex As Exception
                        MessageBox.Show("خطا در پردازش فایل: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End Using
        End Sub

        Private Sub ShowHelp(type As String)
            Dim title = ""
            Dim content = ""

            Select Case type
                Case "CoA"
                    title = "راهنمای انتقال سرفصل حسابها"
                    content = "ستون‌های مورد نیاز در فایل اکسل یا CSV:" & vbCrLf & vbCrLf &
                              "1. AccountCode (کد حساب): کدی یکتا برای حساب." & vbCrLf &
                              "2. AccountName (نام حساب): نام سرفصل حسابداری." & vbCrLf &
                              "3. AccountType (نوع حساب): می‌توانید مقادیر 'کل'، 'معین' یا 'تفصیلی' را وارد کنید." & vbCrLf &
                              "4. ParentAccountCode (کد حساب والد): اگر این حساب زیرمجموعه حساب دیگری است، کد حساب والد را اینجا بنویسید." & vbCrLf &
                              "5. AccountNature (ماهیت حساب): می‌توانید مقادیر 'بدهکار' یا 'بستانکار' را وارد کنید."
                Case "SarfaslShenavar"
                    title = "راهنمای انتقال حسابهای شناور"
                    content = "ستون‌های مورد نیاز در فایل اکسل یا CSV:" & vbCrLf & vbCrLf &
                              "1. AccountCode (کد شناور): کدی یکتا برای شناور." & vbCrLf &
                              "2. AccountName (نام شناور): نام شناور." & vbCrLf &
                              "3. ParentAccountCode (کد شناور والد): اگر این شناور زیرمجموعه شناور دیگری است، کد شناور والد را اینجا بنویسید."
                Case "Docs"
                    title = "راهنمای انتقال اسناد حسابداری"
                    content = "ستون‌های مورد نیاز در فایل اکسل یا CSV:" & vbCrLf & vbCrLf &
                              "1. EntryDate (تاریخ سند): تاریخ سند مثلاً 1403/01/01." & vbCrLf &
                              "2. ReferenceNumber (شماره عطف): شماره سند. تمام ردیف‌های مربوط به یک سند باید شماره عطف یکسانی داشته باشند." & vbCrLf &
                              "3. Description (شرح کل سند): شرح اصلی سند." & vbCrLf &
                              "4. AccountCode (کد سرفصل): کد سرفصلی که مبلغ مربوط به آن است." & vbCrLf &
                              "5. ShenavarCode (کد شناور): کد شناور در صورت وجود." & vbCrLf &
                              "6. Debit (مبلغ بدهکار): مبلغ ریالی بدهکار (بدون ویرگول)." & vbCrLf &
                              "7. Credit (مبلغ بستانکار): مبلغ ریالی بستانکار (بدون ویرگول)." & vbCrLf &
                              "8. SharhRadif (شرح ردیف): شرح اختصاصی برای این ردیف." & vbCrLf & vbCrLf &
                              "نکته مهم: جمع ستون بدهکار و بستانکارِ تمام ردیف‌هایی که شماره عطف یکسان دارند باید برابر (تراز) باشد."
                Case "Products"
                    title = "راهنمای انتقال کالاها"
                    content = "ستون‌های مورد نیاز در فایل اکسل یا CSV:" & vbCrLf & vbCrLf &
                              "1. ProductCode (کد کالا): کدی یکتا برای کالا." & vbCrLf &
                              "2. ProductName (نام کالا): نام کالا." & vbCrLf &
                              "3. Unit (واحد شمارش): مثل عدد، کیلوگرم و..." & vbCrLf &
                              "4. DefaultPrice (قیمت پیش‌فرض): قیمت فروش کالا." & vbCrLf &
                              "5. Category (دسته‌بندی): نام گروه یا دسته بندی کالا."
                Case "Users"
                    title = "راهنمای انتقال اشخاص/کاربران"
                    content = "ستون‌های مورد نیاز در فایل اکسل یا CSV:" & vbCrLf & vbCrLf &
                              "1. Username (نام کاربری): نام کاربری یکتا برای شخص جهت ورود یا شناسایی." & vbCrLf &
                              "2. FullName (نام و نام خانوادگی): نام کامل شخص." & vbCrLf &
                              "3. UserType (نوع شخص): مثلاً کاربر عادی، مدیر، فروشنده." & vbCrLf &
                              "4. Password (کلمه عبور): رمز عبور برای ورود. در صورت خالی بودن، 123 در نظر گرفته می‌شود."
            End Select

            Try
                Dim tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), title & ".txt")
                System.IO.File.WriteAllText(tempPath, content, New System.Text.UTF8Encoding(True))
                System.Diagnostics.Process.Start(tempPath)
            Catch ex As Exception
                MessageBox.Show("خطا در نمایش راهنما: " & ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        ' ─── CoA ─────────────────────────────────────────────────────────
        Private Sub btnDownloadCoATemplate_Click(sender As Object, e As EventArgs) Handles btnDownloadCoATemplate.Click
            HandleDownload("CoA", "SarfaslHesab_Template.csv")
        End Sub
        Private Sub btnImportCoA_Click(sender As Object, e As EventArgs) Handles btnImportCoA.Click
            HandleImport("CoA")
        End Sub
        Private Sub btnHelpCoA_Click(sender As Object, e As EventArgs) Handles btnHelpCoA.Click
            ShowHelp("CoA")
        End Sub
        Private Function ShowSmartConvertDialog(entityName As String) As Boolean?
            Dim confirmMsg = " توجه :" & Environment.NewLine &
                             "در مورد شرکتی که انتخاب کرده اید ، به تعداد سطوح و طول کد هر سطح دقت کنید " & Environment.NewLine &
                             "تعداد سطوح کد حداکثر 6 سطح وطول کد هر سطح حداکثر 6 رقم می باشد" & Environment.NewLine &
                             "تعداد سطح و طول کد هر سطح باید منطبق بر اطلاعاتی باشد که می خواهید" & Environment.NewLine &
                             "تبدیل کنید ، در غیر این صورت برخی از اطلاعات خود را از دست خواهید داد" & Environment.NewLine & Environment.NewLine &
                             $"آیا از شروع تبدیل {entityName} اطمینان دارید؟"

            Dim deleteExisting As Boolean = False
            Using dlg As New Form()
                dlg.Text = "تأیید شروع تبدیل"
                dlg.Size = New Size(500, 310)
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog
                dlg.MaximizeBox = False
                dlg.MinimizeBox = False
                dlg.StartPosition = FormStartPosition.CenterParent
                dlg.RightToLeft = RightToLeft.Yes
                dlg.Font = Me.Font

                Dim lblMsg As New Label()
                lblMsg.Text = confirmMsg
                lblMsg.Location = New Point(12, 12)
                lblMsg.Size = New Size(460, 120)

                Dim rdoAppend As New RadioButton()
                rdoAppend.Text = $"اطلاعات قبلی {entityName} حفظ شود و رکوردهای جدید اضافه شود"
                rdoAppend.Location = New Point(12, 135)
                rdoAppend.Size = New Size(460, 25)
                rdoAppend.Checked = True

                Dim rdoReplace As New RadioButton()
                rdoReplace.Text = $"اطلاعات قبلی {entityName} از دیتابیس حذف شود و مجدداً اطلاعات تبدیل شده وارد شود"
                rdoReplace.Location = New Point(12, 165)
                rdoReplace.Size = New Size(460, 25)

                Dim lblWarn As New Label()
                lblWarn.Text = "⚠ توجه: در صورت حذف، تمام ردیف‌های مرتبط نیز پاک می‌شوند."
                lblWarn.Location = New Point(12, 195)
                lblWarn.Size = New Size(460, 22)
                lblWarn.ForeColor = Drawing.Color.OrangeRed
                lblWarn.Visible = False

                Dim btnOk As New Button()
                btnOk.Text = "تأیید"
                btnOk.DialogResult = DialogResult.OK
                btnOk.Location = New Point(12, 225)
                btnOk.Size = New Size(90, 32)
                btnOk.UseVisualStyleBackColor = True

                Dim btnCancel As New Button()
                btnCancel.Text = "انصراف"
                btnCancel.DialogResult = DialogResult.Cancel
                btnCancel.Location = New Point(112, 225)
                btnCancel.Size = New Size(90, 32)
                btnCancel.UseVisualStyleBackColor = True

                AddHandler rdoReplace.CheckedChanged, Sub(s, ev)
                                                          lblWarn.Visible = rdoReplace.Checked
                                                      End Sub

                dlg.Controls.AddRange(New Control() {lblMsg, rdoAppend, rdoReplace, lblWarn, btnOk, btnCancel})
                dlg.AcceptButton = btnOk
                dlg.CancelButton = btnCancel

                If dlg.ShowDialog(Me) <> DialogResult.OK Then Return Nothing
                deleteExisting = rdoReplace.Checked
            End Using

            Return deleteExisting
        End Function

        Private Sub btnSmartConvertCoA_Click(sender As Object, e As EventArgs) Handles btnSmartConvertCoA.Click
            If Not EnsureUserAndCompanySelected() Then Return
            Dim companyId = GetEffectiveCompanyID()
            If Not companyId.HasValue Then Return

            Dim result = ShowSmartConvertDialog("سرفصل حسابها")
            If Not result.HasValue Then Return

            Dim frm As New HesabdaryTabdilDataSarfaslForm(companyId.Value, result.Value)
            frm.ShowDialog(Me)
        End Sub

        ' ─── Shenavar ────────────────────────────────────────────────────
        Private Sub btnDownloadShenavarTemplate_Click(sender As Object, e As EventArgs) Handles btnDownloadShenavarTemplate.Click
            HandleDownload("SarfaslShenavar", "Shenavar_Template.csv")
        End Sub
        Private Sub btnImportShenavar_Click(sender As Object, e As EventArgs) Handles btnImportShenavar.Click
            HandleImport("SarfaslShenavar")
        End Sub
        Private Sub btnHelpShenavar_Click(sender As Object, e As EventArgs) Handles btnHelpShenavar.Click
            ShowHelp("SarfaslShenavar")
        End Sub

        ' ─── Docs ────────────────────────────────────────────────────────
        Private Sub btnDownloadDocsTemplate_Click(sender As Object, e As EventArgs) Handles btnDownloadDocsTemplate.Click
            HandleDownload("Docs", "Sanad1_Template.csv")
        End Sub
        Private Sub btnImportDocs_Click(sender As Object, e As EventArgs) Handles btnImportDocs.Click
            HandleImport("Docs")
        End Sub
        Private Sub btnHelpDocs_Click(sender As Object, e As EventArgs) Handles btnHelpDocs.Click
            ShowHelp("Docs")
        End Sub
        Private Sub btnSmartConvertDocs_Click(sender As Object, e As EventArgs) Handles btnSmartConvertDocs.Click
            If Not EnsureUserAndCompanySelected() Then Return
            Dim companyId = GetEffectiveCompanyID()
            If Not companyId.HasValue Then Return

            Dim result = ShowSmartConvertDialog("اسناد حسابداری")
            If Not result.HasValue Then Return

            Dim frm As New HesabdaryTabdilDataSanadForm(companyId.Value, result.Value)
            frm.ShowDialog(Me)
        End Sub

        ' ─── Products ────────────────────────────────────────────────────
        Private Sub btnDownloadProductTemplate_Click(sender As Object, e As EventArgs) Handles btnDownloadProductTemplate.Click
            HandleDownload("Products", "Products_Template.csv")
        End Sub
        Private Sub btnImportProducts_Click(sender As Object, e As EventArgs) Handles btnImportProducts.Click
            HandleImport("Products")
        End Sub
        Private Sub btnHelpProducts_Click(sender As Object, e As EventArgs) Handles btnHelpProducts.Click
            ShowHelp("Products")
        End Sub

        ' ─── Users ───────────────────────────────────────────────────────
        Private Sub btnDownloadUserTemplate_Click(sender As Object, e As EventArgs) Handles btnDownloadUserTemplate.Click
            HandleDownload("Users", "Persons_Template.csv")
        End Sub
        Private Sub btnImportUsers_Click(sender As Object, e As EventArgs) Handles btnImportUsers.Click
            HandleImport("Users")
        End Sub
        Private Sub btnHelpUsers_Click(sender As Object, e As EventArgs) Handles btnHelpUsers.Click
            ShowHelp("Users")
        End Sub

    End Class
End Namespace
