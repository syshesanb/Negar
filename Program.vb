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

            Dim exeName As String = Path.GetFileName(Application.ExecutablePath)
            If exeName.StartsWith("Setup", StringComparison.OrdinalIgnoreCase) Then
                Application.Run(New Forms.SetupInstallerForm())
                Return
            ElseIf exeName.StartsWith("Update", StringComparison.OrdinalIgnoreCase) Then
                Application.Run(New Forms.UpdateInstallerForm())
                Return
            End If

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
End Namespace
