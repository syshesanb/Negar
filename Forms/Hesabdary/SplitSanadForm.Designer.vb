Option Strict Off
Option Explicit On

Namespace Negar.Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class SplitSanadForm
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.pnlLeft = New System.Windows.Forms.GroupBox()
            Me.lblSourceInfo = New System.Windows.Forms.Label()
            Me.lblPrevInfo = New System.Windows.Forms.Label()
            Me.lblNextInfo = New System.Windows.Forms.Label()
            Me.lblSuggestions = New System.Windows.Forms.Label()
            Me.pnlTopControls = New System.Windows.Forms.Panel()
            Me.lblNewDocsCount = New System.Windows.Forms.Label()
            Me.numNewDocs = New System.Windows.Forms.NumericUpDown()
            Me.tabDocs = New System.Windows.Forms.TabControl()
            Me.btnConfirm = New System.Windows.Forms.Button()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.pnlLeft.SuspendLayout()
            Me.pnlTopControls.SuspendLayout()
            CType(Me.numNewDocs, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'pnlLeft
            '
            Me.pnlLeft.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.pnlLeft.Controls.Add(Me.lblSourceInfo)
            Me.pnlLeft.Controls.Add(Me.lblPrevInfo)
            Me.pnlLeft.Controls.Add(Me.lblNextInfo)
            Me.pnlLeft.Controls.Add(Me.lblSuggestions)
            Me.pnlLeft.Location = New System.Drawing.Point(12, 12)
            Me.pnlLeft.Name = "pnlLeft"
            Me.pnlLeft.Size = New System.Drawing.Size(220, 480)
            Me.pnlLeft.TabIndex = 0
            Me.pnlLeft.TabStop = False
            Me.pnlLeft.Text = "اطلاعات سند مبدا و همسایه"
            '
            'lblSourceInfo
            '
            Me.lblSourceInfo.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblSourceInfo.ForeColor = System.Drawing.Color.Navy
            Me.lblSourceInfo.Location = New System.Drawing.Point(10, 30)
            Me.lblSourceInfo.Name = "lblSourceInfo"
            Me.lblSourceInfo.Size = New System.Drawing.Size(200, 100)
            Me.lblSourceInfo.TabIndex = 0
            Me.lblSourceInfo.Text = "در حال بارگذاری..."
            '
            'lblPrevInfo
            '
            Me.lblPrevInfo.Location = New System.Drawing.Point(10, 150)
            Me.lblPrevInfo.Name = "lblPrevInfo"
            Me.lblPrevInfo.Size = New System.Drawing.Size(200, 100)
            Me.lblPrevInfo.TabIndex = 1
            Me.lblPrevInfo.Text = "در حال بارگذاری..."
            '
            'lblNextInfo
            '
            Me.lblNextInfo.Location = New System.Drawing.Point(10, 270)
            Me.lblNextInfo.Name = "lblNextInfo"
            Me.lblNextInfo.Size = New System.Drawing.Size(200, 100)
            Me.lblNextInfo.TabIndex = 2
            Me.lblNextInfo.Text = "در حال بارگذاری..."
            '
            'lblSuggestions
            '
            Me.lblSuggestions.Location = New System.Drawing.Point(10, 380)
            Me.lblSuggestions.Name = "lblSuggestions"
            Me.lblSuggestions.Size = New System.Drawing.Size(200, 90)
            Me.lblSuggestions.TabIndex = 3
            Me.lblSuggestions.ForeColor = System.Drawing.Color.DarkGreen
            Me.lblSuggestions.Text = "در حال بارگذاری پیشنهادات..."
            '
            'pnlTopControls
            '
            Me.pnlTopControls.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.pnlTopControls.Controls.Add(Me.lblNewDocsCount)
            Me.pnlTopControls.Controls.Add(Me.numNewDocs)
            Me.pnlTopControls.Location = New System.Drawing.Point(245, 12)
            Me.pnlTopControls.Name = "pnlTopControls"
            Me.pnlTopControls.Size = New System.Drawing.Size(677, 45)
            Me.pnlTopControls.TabIndex = 1
            '
            'lblNewDocsCount
            '
            Me.lblNewDocsCount.AutoSize = True
            Me.lblNewDocsCount.Location = New System.Drawing.Point(10, 12)
            Me.lblNewDocsCount.Name = "lblNewDocsCount"
            Me.lblNewDocsCount.Size = New System.Drawing.Size(126, 14)
            Me.lblNewDocsCount.TabIndex = 0
            Me.lblNewDocsCount.Text = "تعداد اسناد مقصد جدید:"
            '
            'numNewDocs
            '
            Me.numNewDocs.Location = New System.Drawing.Point(160, 10)
            Me.numNewDocs.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
            Me.numNewDocs.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.numNewDocs.Name = "numNewDocs"
            Me.numNewDocs.Size = New System.Drawing.Size(80, 22)
            Me.numNewDocs.TabIndex = 1
            Me.numNewDocs.Value = New Decimal(New Integer() {1, 0, 0, 0})
            '
            'tabDocs
            '
            Me.tabDocs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tabDocs.Location = New System.Drawing.Point(245, 65)
            Me.tabDocs.Name = "tabDocs"
            Me.tabDocs.Size = New System.Drawing.Size(677, 427)
            Me.tabDocs.TabIndex = 2
            '
            'btnConfirm
            '
            Me.btnConfirm.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnConfirm.Location = New System.Drawing.Point(682, 510)
            Me.btnConfirm.Name = "btnConfirm"
            Me.btnConfirm.Size = New System.Drawing.Size(115, 35)
            Me.btnConfirm.TabIndex = 3
            Me.btnConfirm.Text = "تایید و تجزیه سند"
            Me.btnConfirm.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(807, 510)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(115, 35)
            Me.btnCancel.TabIndex = 4
            Me.btnCancel.Text = "انصراف"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'SplitSanadForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(934, 561)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnConfirm)
            Me.Controls.Add(Me.tabDocs)
            Me.Controls.Add(Me.pnlTopControls)
            Me.Controls.Add(Me.pnlLeft)
            Me.Font = New System.Drawing.Font("Tahoma", 9.0!)
            Me.Name = "SplitSanadForm"
            Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "تجزیه سند حسابداری"
            Me.pnlLeft.ResumeLayout(False)
            Me.pnlTopControls.ResumeLayout(False)
            Me.pnlTopControls.PerformLayout()
            CType(Me.numNewDocs, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents pnlLeft As System.Windows.Forms.GroupBox
        Friend WithEvents lblSourceInfo As System.Windows.Forms.Label
        Friend WithEvents lblPrevInfo As System.Windows.Forms.Label
        Friend WithEvents lblNextInfo As System.Windows.Forms.Label
        Friend WithEvents lblSuggestions As System.Windows.Forms.Label
        Friend WithEvents pnlTopControls As System.Windows.Forms.Panel
        Friend WithEvents lblNewDocsCount As System.Windows.Forms.Label
        Friend WithEvents numNewDocs As System.Windows.Forms.NumericUpDown
        Friend WithEvents tabDocs As System.Windows.Forms.TabControl
        Friend WithEvents btnConfirm As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
    End Class
End Namespace
