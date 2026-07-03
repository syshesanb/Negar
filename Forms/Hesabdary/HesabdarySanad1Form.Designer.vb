Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class HesabdarySanad1Form
        Inherits Form

        Private components As IContainer

        Friend WithEvents pnlTop As Panel
        Friend WithEvents btnNew As Button
        Friend WithEvents btnPrintDocs As Button
        Friend WithEvents btnPrintJournal As Button
        Friend WithEvents btnCopySanad As Button
        Friend WithEvents btnMerge As Button
        Friend WithEvents btnSplit As Button
        Friend WithEvents dgvEntries As DataGridView
        Friend WithEvents pnlSerch As Panel
        Friend WithEvents txtSrcEdit As TextBox
        Friend WithEvents txtSrcDel As TextBox
        Friend WithEvents txtSrcLock As TextBox
        Friend WithEvents txtSrcRef As TextBox
        Friend WithEvents txtSrcDate As TextBox
        Friend WithEvents txtSrcDesc As TextBox
        Friend WithEvents txtSrcBed As TextBox
        Friend WithEvents txtSrcBes As TextBox
        Friend WithEvents txtSrcTaeaz As TextBox
        Friend WithEvents txtSrcVazeiat As TextBox

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.pnlTop = New System.Windows.Forms.Panel()
            Me.btnNew = New System.Windows.Forms.Button()
            Me.btnPrintDocs = New System.Windows.Forms.Button()
            Me.btnPrintJournal = New System.Windows.Forms.Button()
            Me.btnCopySanad = New System.Windows.Forms.Button()
            Me.btnMerge = New System.Windows.Forms.Button()
            Me.btnSplit = New System.Windows.Forms.Button()
            Me.dgvEntries = New System.Windows.Forms.DataGridView()
            Me.pnlSerch = New System.Windows.Forms.Panel()
            Me.txtSrcEdit = New System.Windows.Forms.TextBox()
            Me.txtSrcDel = New System.Windows.Forms.TextBox()
            Me.txtSrcLock = New System.Windows.Forms.TextBox()
            Me.txtSrcRef = New System.Windows.Forms.TextBox()
            Me.txtSrcDate = New System.Windows.Forms.TextBox()
            Me.txtSrcDesc = New System.Windows.Forms.TextBox()
            Me.txtSrcBed = New System.Windows.Forms.TextBox()
            Me.txtSrcBes = New System.Windows.Forms.TextBox()
            Me.txtSrcTaeaz = New System.Windows.Forms.TextBox()
            Me.txtSrcVazeiat = New System.Windows.Forms.TextBox()
            Me.pnlTop.SuspendLayout()
            CType(Me.dgvEntries, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlSerch.SuspendLayout()
            Me.SuspendLayout()
            '
            'pnlTop
            '
            Me.pnlTop.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(251, Byte), Integer))
            Me.pnlTop.Controls.Add(Me.btnNew)
            Me.pnlTop.Controls.Add(Me.btnPrintDocs)
            Me.pnlTop.Controls.Add(Me.btnPrintJournal)
            Me.pnlTop.Controls.Add(Me.btnCopySanad)
            Me.pnlTop.Controls.Add(Me.btnMerge)
            Me.pnlTop.Controls.Add(Me.btnSplit)
            Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlTop.Location = New System.Drawing.Point(0, 0)
            Me.pnlTop.Name = "pnlTop"
            Me.pnlTop.Size = New System.Drawing.Size(1100, 42)
            Me.pnlTop.TabIndex = 0
            '
            'btnNew
            '
            Me.btnNew.Location = New System.Drawing.Point(10, 8)
            Me.btnNew.Name = "btnNew"
            Me.btnNew.Size = New System.Drawing.Size(100, 26)
            Me.btnNew.TabIndex = 0
            Me.btnNew.Text = "جدید"
            '
            'btnPrintDocs
            '
            Me.btnPrintDocs.Location = New System.Drawing.Point(120, 8)
            Me.btnPrintDocs.Name = "btnPrintDocs"
            Me.btnPrintDocs.Size = New System.Drawing.Size(110, 26)
            Me.btnPrintDocs.TabIndex = 1
            Me.btnPrintDocs.Text = "چاپ اسناد"
            Me.btnPrintDocs.UseVisualStyleBackColor = True
            '
            'btnPrintJournal
            '
            Me.btnPrintJournal.Location = New System.Drawing.Point(240, 8)
            Me.btnPrintJournal.Name = "btnPrintJournal"
            Me.btnPrintJournal.Size = New System.Drawing.Size(130, 26)
            Me.btnPrintJournal.TabIndex = 2
            Me.btnPrintJournal.Text = "چاپ دفتر روزنامه"
            Me.btnPrintJournal.UseVisualStyleBackColor = True
            '
            'btnCopySanad
            '
            Me.btnCopySanad.Location = New System.Drawing.Point(380, 8)
            Me.btnCopySanad.Name = "btnCopySanad"
            Me.btnCopySanad.Size = New System.Drawing.Size(100, 26)
            Me.btnCopySanad.TabIndex = 3
            Me.btnCopySanad.Text = "کپی سند"
            Me.btnCopySanad.UseVisualStyleBackColor = True
            '
            'btnMerge
            '
            Me.btnMerge.Location = New System.Drawing.Point(490, 8)
            Me.btnMerge.Name = "btnMerge"
            Me.btnMerge.Size = New System.Drawing.Size(120, 26)
            Me.btnMerge.TabIndex = 4
            Me.btnMerge.Text = "ادغام اسناد"
            Me.btnMerge.UseVisualStyleBackColor = True
            '
            'btnSplit
            '
            Me.btnSplit.Location = New System.Drawing.Point(620, 8)
            Me.btnSplit.Name = "btnSplit"
            Me.btnSplit.Size = New System.Drawing.Size(100, 26)
            Me.btnSplit.TabIndex = 5
            Me.btnSplit.Text = "تجزیه سند"
            Me.btnSplit.UseVisualStyleBackColor = True
            '
            'dgvEntries
            '
            Me.dgvEntries.AllowUserToAddRows = False
            Me.dgvEntries.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvEntries.Location = New System.Drawing.Point(0, 72)
            Me.dgvEntries.MultiSelect = False
            Me.dgvEntries.Name = "dgvEntries"
            Me.dgvEntries.ReadOnly = False
            Me.dgvEntries.RowHeadersVisible = False
            Me.dgvEntries.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvEntries.Size = New System.Drawing.Size(1100, 608)
            Me.dgvEntries.TabIndex = 1
            '
            'pnlSerch
            '
            Me.pnlSerch.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlSerch.Controls.Add(Me.txtSrcEdit)
            Me.pnlSerch.Controls.Add(Me.txtSrcDel)
            Me.pnlSerch.Controls.Add(Me.txtSrcLock)
            Me.pnlSerch.Controls.Add(Me.txtSrcRef)
            Me.pnlSerch.Controls.Add(Me.txtSrcDate)
            Me.pnlSerch.Controls.Add(Me.txtSrcDesc)
            Me.pnlSerch.Controls.Add(Me.txtSrcBed)
            Me.pnlSerch.Controls.Add(Me.txtSrcBes)
            Me.pnlSerch.Controls.Add(Me.txtSrcTaeaz)
            Me.pnlSerch.Controls.Add(Me.txtSrcVazeiat)
            Me.pnlSerch.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlSerch.Location = New System.Drawing.Point(0, 42)
            Me.pnlSerch.Name = "pnlSerch"
            Me.pnlSerch.Size = New System.Drawing.Size(1100, 30)
            Me.pnlSerch.TabIndex = 2
            '
            'txtSrcEdit
            '
            Me.txtSrcEdit.Enabled = False
            Me.txtSrcEdit.ReadOnly = True
            '
            'txtSrcDel
            '
            Me.txtSrcDel.Enabled = False
            Me.txtSrcDel.ReadOnly = True
            '
            'txtSrcLock
            '
            Me.txtSrcLock.Enabled = False
            Me.txtSrcLock.ReadOnly = True
            '
            'HesabdarySanad1Form
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(1100, 680)
            Me.Controls.Add(Me.dgvEntries)
            Me.Controls.Add(Me.pnlSerch)
            Me.Controls.Add(Me.pnlTop)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.Name = "HesabdarySanad1Form"
            Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "اسناد حسابداری"
            Me.pnlTop.ResumeLayout(False)
            CType(Me.dgvEntries, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlSerch.ResumeLayout(False)
            Me.pnlSerch.PerformLayout()
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
