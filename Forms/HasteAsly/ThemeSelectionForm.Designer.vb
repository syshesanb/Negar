Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices

Namespace Sys_Hes_Anb.Forms
    <DesignerGenerated()>
    Partial Class ThemeSelectionForm
        Inherits Form

        Private components As IContainer
        Friend WithEvents dgvThemes As DataGridView
        Friend WithEvents btnSave As Button
        Friend WithEvents colRow As DataGridViewTextBoxColumn
        Friend WithEvents colSelect As DataGridViewCheckBoxColumn
        Friend WithEvents colName As DataGridViewTextBoxColumn
        Friend WithEvents colShowImage As DataGridViewButtonColumn
        Friend WithEvents colPreview As DataGridViewImageColumn
        Friend WithEvents colColorHex As DataGridViewTextBoxColumn

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New Container()
            Me.dgvThemes = New DataGridView()
            Me.btnSave = New Button()
            Me.colRow = New DataGridViewTextBoxColumn()
            Me.colSelect = New DataGridViewCheckBoxColumn()
            Me.colName = New DataGridViewTextBoxColumn()
            Me.colShowImage = New DataGridViewButtonColumn()
            Me.colPreview = New DataGridViewImageColumn()
            Me.colColorHex = New DataGridViewTextBoxColumn()
            CType(Me.dgvThemes, ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            
            Me.dgvThemes.AllowUserToAddRows = False
            Me.dgvThemes.AllowUserToDeleteRows = False
            Me.dgvThemes.AllowUserToResizeRows = False
            Me.dgvThemes.BackgroundColor = Color.White
            Me.dgvThemes.ColumnHeadersHeight = 35
            Me.dgvThemes.Columns.AddRange(New DataGridViewColumn() {Me.colRow, Me.colSelect, Me.colName, Me.colShowImage, Me.colPreview, Me.colColorHex})
            Me.dgvThemes.Dock = DockStyle.Top
            Me.dgvThemes.Location = New Point(0, 0)
            Me.dgvThemes.MultiSelect = False
            Me.dgvThemes.Name = "dgvThemes"
            Me.dgvThemes.RowHeadersVisible = False
            Me.dgvThemes.RowTemplate.Height = 100
            Me.dgvThemes.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            Me.dgvThemes.Size = New Size(620, 300)
            Me.dgvThemes.TabIndex = 0
            
            Me.colRow.HeaderText = "ردیف"
            Me.colRow.Name = "colRow"
            Me.colRow.ReadOnly = True
            Me.colRow.Width = 50
            
            Me.colSelect.HeaderText = "انتخاب"
            Me.colSelect.Name = "colSelect"
            Me.colSelect.Width = 60
            
            Me.colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            Me.colName.HeaderText = "نام تم"
            Me.colName.Name = "colName"
            Me.colName.ReadOnly = True
            
            Me.colShowImage.HeaderText = "تصویر"
            Me.colShowImage.Name = "colShowImage"
            Me.colShowImage.Text = "نمایش تصویر"
            Me.colShowImage.UseColumnTextForButtonValue = True
            Me.colShowImage.Width = 100
            
            Me.colPreview.HeaderText = "تصویر نمونه تم"
            Me.colPreview.ImageLayout = DataGridViewImageCellLayout.Zoom
            Me.colPreview.Name = "colPreview"
            Me.colPreview.ReadOnly = True
            Me.colPreview.Width = 250
            
            Me.colColorHex.HeaderText = "کد رنگ"
            Me.colColorHex.Name = "colColorHex"
            Me.colColorHex.ReadOnly = True
            Me.colColorHex.Visible = False
            
            Me.btnSave.Location = New Point(215, 315)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New Size(150, 40)
            Me.btnSave.TabIndex = 1
            Me.btnSave.Text = "ذخیره تم انتخاب شده"
            Me.btnSave.UseVisualStyleBackColor = True
            
            Me.AutoScaleDimensions = New SizeF(7.0!, 14.0!)
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.ClientSize = New Size(580, 370)
            Me.Controls.Add(Me.btnSave)
            Me.Controls.Add(Me.dgvThemes)
            Me.Font = New Font("Tahoma", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType(178, Byte))
            Me.Name = "ThemeSelectionForm"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "گالری تم‌های فرم‌ها"
            CType(Me.dgvThemes, ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
