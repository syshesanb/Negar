Option Strict Off
Option Explicit On

Imports System
Imports System.IO
Imports System.Windows.Forms

Namespace Negar
    Public Module Program
        <STAThread()>
        Public Sub Main()
            Try
                System.IO.File.WriteAllText("C:\Negar\debug_main.txt", "Main started with args: " & String.Join(", ", Environment.GetCommandLineArgs()))
            Catch ex As Exception
                Try
                    System.IO.File.WriteAllText("C:\Negar\debug_main_error.txt", ex.ToString())
                Catch
                End Try
            End Try
            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            Application.AddMessageFilter(New GlobalCalculatorMessageFilter())

            Dim exeName As String = Path.GetFileName(Application.ExecutablePath)
            If exeName.StartsWith("Setup", StringComparison.OrdinalIgnoreCase) Then
                Application.Run(New Forms.SetupInstallerForm())
                Return
            ElseIf exeName.StartsWith("Update", StringComparison.OrdinalIgnoreCase) Then
                Application.Run(New Forms.UpdateInstallerForm())
                Return
            End If

            ' تعیین نسخه فعال نرم‌افزار نگار (Mini / Medium / Big)
            Try
                Dim configEdition = System.Configuration.ConfigurationManager.AppSettings("AppEdition")
                If String.Equals(configEdition, "Medium", StringComparison.OrdinalIgnoreCase) OrElse exeName.IndexOf("Medium", StringComparison.OrdinalIgnoreCase) >= 0 Then
                    Business.SessionContext.CurrentEdition = Models.AppEdition.Medium
                ElseIf String.Equals(configEdition, "Big", StringComparison.OrdinalIgnoreCase) OrElse exeName.IndexOf("Big", StringComparison.OrdinalIgnoreCase) >= 0 Then
                    Business.SessionContext.CurrentEdition = Models.AppEdition.Big
                Else
                    Business.SessionContext.CurrentEdition = Models.AppEdition.Mini
                End If
            Catch
                Business.SessionContext.CurrentEdition = Models.AppEdition.Mini
            End Try

            Dim dbFolder As String
            Dim devDbFolder = Path.Combine(Application.StartupPath, "..", "..", "Database")
            If Directory.Exists(devDbFolder) AndAlso File.Exists(Path.Combine(Application.StartupPath, "..", "..", "Negar.vbproj")) Then
                dbFolder = Path.GetFullPath(devDbFolder)
            Else
                dbFolder = Path.Combine(Application.StartupPath, "Database")
            End If
            Directory.CreateDirectory(dbFolder)
            AppDomain.CurrentDomain.SetData("DataDirectory", dbFolder)

            AddHandler Application.ApplicationExit, Sub(sender, e) Data.AesDbService.SyncAndLockDatabase()
            AddHandler AppDomain.CurrentDomain.ProcessExit, Sub(sender, e) Data.AesDbService.SyncAndLockDatabase()

            Try
                Data.DbBootstrap.EnsureSeedData()
                Business.MigrationService.ApplyPendingMigrations()
            Catch ex As Exception
                MessageBox.Show("Database initialization warning:" & Environment.NewLine & ex.Message,
                                "Negar",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning)
            End Try

            Dim args As String() = Environment.GetCommandLineArgs()
            If args.Length > 1 AndAlso args(1) = "test" Then
                Business.SessionContext.CurrentCompanyID = 5
                Business.SessionContext.CurrentFiscalYearID = 5
                
                Dim form As New Forms.HesabdaryCodingForm()
                Dim t As New Timer()
                t.Interval = 2000
                AddHandler t.Tick, Sub(snd, ev)
                    t.Stop()
                    form.Close()
                End Sub
                t.Start()
                Application.Run(form)
                Return
            End If

            Using login As New Forms.LoginForm()
                If login.ShowDialog() = DialogResult.OK Then
                    Application.Run(New Forms.MainForm(login.AuthenticatedUser))
                End If
            End Using
        End Sub
    End Module

    Public Class GlobalCalculatorMessageFilter
        Implements IMessageFilter

        Private Const WM_KEYDOWN As Integer = &H100
        Private Const WM_SYSKEYDOWN As Integer = &H104

        Public Function PreFilterMessage(ByRef m As Message) As Boolean Implements IMessageFilter.PreFilterMessage
            If m.Msg = WM_KEYDOWN OrElse m.Msg = WM_SYSKEYDOWN Then
                Dim rawKey As Keys = CType(m.WParam.ToInt32(), Keys)
                Dim keyData As Keys = rawKey Or Control.ModifierKeys

                ' کلید F12 یا F10 یا کلید ترکیبی Ctrl + Shift + C
                If rawKey = Keys.F12 OrElse rawKey = Keys.F10 OrElse keyData = (Keys.Control Or Keys.Shift Or Keys.C) Then
                    OpenCalculator()
                    Return True
                End If
            End If
            Return False
        End Function

        Private Shared Sub OpenCalculator()
            Try
                System.Diagnostics.Process.Start("calc.exe")
            Catch ex As Exception
                Try
                    System.Diagnostics.Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "calc.exe"))
                Catch
                    MessageBox.Show("امکان باز کردن ماشین حساب وجود ندارد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End Try
            End Try
        End Sub
    End Class
End Namespace
