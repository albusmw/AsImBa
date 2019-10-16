<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.msMain = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LoadImageFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmRecentFiles = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiRecent1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiRecent2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiRecent3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiRecent4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiRecent5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiRecent6 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiRecent7 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiRecent8 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiRecent9 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiRecent10 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.OpenEXEPathToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewProcessorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DEBUGToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OnlyUseCSharpFitsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TestLookupTableToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiGenerateTestImage = New System.Windows.Forms.ToolStripMenuItem()
        Me.GenerateTestImagesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GrayscaleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowLogToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ofdMain = New System.Windows.Forms.OpenFileDialog()
        Me.tsMain = New System.Windows.Forms.ToolStrip()
        Me.tstbFileToLoad = New System.Windows.Forms.ToolStripTextBox()
        Me.dpMain = New WeifenLuo.WinFormsUI.Docking.DockPanel()
        Me.ssMain = New System.Windows.Forms.StatusStrip()
        Me.tsslMain = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tUpdateStatus = New System.Windows.Forms.Timer(Me.components)
        Me.msMain.SuspendLayout()
        Me.tsMain.SuspendLayout()
        Me.ssMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'msMain
        '
        Me.msMain.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.msMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ImageToolStripMenuItem, Me.DEBUGToolStripMenuItem})
        Me.msMain.Location = New System.Drawing.Point(0, 0)
        Me.msMain.Name = "msMain"
        Me.msMain.Size = New System.Drawing.Size(901, 24)
        Me.msMain.TabIndex = 1
        Me.msMain.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LoadImageFileToolStripMenuItem, Me.tsmRecentFiles, Me.ToolStripMenuItem2, Me.OpenEXEPathToolStripMenuItem, Me.ToolStripMenuItem1, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'LoadImageFileToolStripMenuItem
        '
        Me.LoadImageFileToolStripMenuItem.Name = "LoadImageFileToolStripMenuItem"
        Me.LoadImageFileToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.L), System.Windows.Forms.Keys)
        Me.LoadImageFileToolStripMenuItem.Size = New System.Drawing.Size(195, 22)
        Me.LoadImageFileToolStripMenuItem.Text = "Load image file"
        '
        'tsmRecentFiles
        '
        Me.tsmRecentFiles.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiRecent1, Me.tsmiRecent2, Me.tsmiRecent3, Me.tsmiRecent4, Me.tsmiRecent5, Me.tsmiRecent6, Me.tsmiRecent7, Me.tsmiRecent8, Me.tsmiRecent9, Me.tsmiRecent10})
        Me.tsmRecentFiles.Name = "tsmRecentFiles"
        Me.tsmRecentFiles.Size = New System.Drawing.Size(195, 22)
        Me.tsmRecentFiles.Text = "Load recent file"
        '
        'tsmiRecent1
        '
        Me.tsmiRecent1.Name = "tsmiRecent1"
        Me.tsmiRecent1.Size = New System.Drawing.Size(152, 22)
        Me.tsmiRecent1.Text = "Item1"
        '
        'tsmiRecent2
        '
        Me.tsmiRecent2.Name = "tsmiRecent2"
        Me.tsmiRecent2.Size = New System.Drawing.Size(152, 22)
        Me.tsmiRecent2.Text = "Item2"
        '
        'tsmiRecent3
        '
        Me.tsmiRecent3.Name = "tsmiRecent3"
        Me.tsmiRecent3.Size = New System.Drawing.Size(152, 22)
        Me.tsmiRecent3.Text = "Item3"
        '
        'tsmiRecent4
        '
        Me.tsmiRecent4.Name = "tsmiRecent4"
        Me.tsmiRecent4.Size = New System.Drawing.Size(152, 22)
        Me.tsmiRecent4.Text = "Item4"
        '
        'tsmiRecent5
        '
        Me.tsmiRecent5.Name = "tsmiRecent5"
        Me.tsmiRecent5.Size = New System.Drawing.Size(152, 22)
        Me.tsmiRecent5.Text = "Item5"
        '
        'tsmiRecent6
        '
        Me.tsmiRecent6.Name = "tsmiRecent6"
        Me.tsmiRecent6.Size = New System.Drawing.Size(152, 22)
        Me.tsmiRecent6.Text = "Item6"
        '
        'tsmiRecent7
        '
        Me.tsmiRecent7.Name = "tsmiRecent7"
        Me.tsmiRecent7.Size = New System.Drawing.Size(152, 22)
        Me.tsmiRecent7.Text = "Item7"
        '
        'tsmiRecent8
        '
        Me.tsmiRecent8.Name = "tsmiRecent8"
        Me.tsmiRecent8.Size = New System.Drawing.Size(152, 22)
        Me.tsmiRecent8.Text = "Item8"
        '
        'tsmiRecent9
        '
        Me.tsmiRecent9.Name = "tsmiRecent9"
        Me.tsmiRecent9.Size = New System.Drawing.Size(152, 22)
        Me.tsmiRecent9.Text = "Item9"
        '
        'tsmiRecent10
        '
        Me.tsmiRecent10.Name = "tsmiRecent10"
        Me.tsmiRecent10.Size = New System.Drawing.Size(152, 22)
        Me.tsmiRecent10.Text = "Item10"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(192, 6)
        '
        'OpenEXEPathToolStripMenuItem
        '
        Me.OpenEXEPathToolStripMenuItem.Name = "OpenEXEPathToolStripMenuItem"
        Me.OpenEXEPathToolStripMenuItem.Size = New System.Drawing.Size(195, 22)
        Me.OpenEXEPathToolStripMenuItem.Text = "Open EXE path"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(192, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(195, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'ImageToolStripMenuItem
        '
        Me.ImageToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewProcessorToolStripMenuItem})
        Me.ImageToolStripMenuItem.Name = "ImageToolStripMenuItem"
        Me.ImageToolStripMenuItem.Size = New System.Drawing.Size(52, 20)
        Me.ImageToolStripMenuItem.Text = "Image"
        '
        'NewProcessorToolStripMenuItem
        '
        Me.NewProcessorToolStripMenuItem.Name = "NewProcessorToolStripMenuItem"
        Me.NewProcessorToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.NewProcessorToolStripMenuItem.Size = New System.Drawing.Size(193, 22)
        Me.NewProcessorToolStripMenuItem.Text = "New processor"
        '
        'DEBUGToolStripMenuItem
        '
        Me.DEBUGToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OnlyUseCSharpFitsToolStripMenuItem, Me.TestLookupTableToolStripMenuItem, Me.tsmiGenerateTestImage, Me.GenerateTestImagesToolStripMenuItem, Me.ShowLogToolStripMenuItem})
        Me.DEBUGToolStripMenuItem.Name = "DEBUGToolStripMenuItem"
        Me.DEBUGToolStripMenuItem.Size = New System.Drawing.Size(68, 20)
        Me.DEBUGToolStripMenuItem.Text = "!!DEBUG!!"
        '
        'OnlyUseCSharpFitsToolStripMenuItem
        '
        Me.OnlyUseCSharpFitsToolStripMenuItem.Name = "OnlyUseCSharpFitsToolStripMenuItem"
        Me.OnlyUseCSharpFitsToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.OnlyUseCSharpFitsToolStripMenuItem.Text = "Only use CSharpFits"
        '
        'TestLookupTableToolStripMenuItem
        '
        Me.TestLookupTableToolStripMenuItem.Name = "TestLookupTableToolStripMenuItem"
        Me.TestLookupTableToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.TestLookupTableToolStripMenuItem.Text = "Test lookup table"
        '
        'tsmiGenerateTestImage
        '
        Me.tsmiGenerateTestImage.Name = "tsmiGenerateTestImage"
        Me.tsmiGenerateTestImage.Size = New System.Drawing.Size(184, 22)
        Me.tsmiGenerateTestImage.Text = "Generate test image"
        '
        'GenerateTestImagesToolStripMenuItem
        '
        Me.GenerateTestImagesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GrayscaleToolStripMenuItem})
        Me.GenerateTestImagesToolStripMenuItem.Name = "GenerateTestImagesToolStripMenuItem"
        Me.GenerateTestImagesToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.GenerateTestImagesToolStripMenuItem.Text = "Generate test images"
        '
        'GrayscaleToolStripMenuItem
        '
        Me.GrayscaleToolStripMenuItem.Name = "GrayscaleToolStripMenuItem"
        Me.GrayscaleToolStripMenuItem.Size = New System.Drawing.Size(124, 22)
        Me.GrayscaleToolStripMenuItem.Text = "Grayscale"
        '
        'ShowLogToolStripMenuItem
        '
        Me.ShowLogToolStripMenuItem.Name = "ShowLogToolStripMenuItem"
        Me.ShowLogToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ShowLogToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.ShowLogToolStripMenuItem.Text = "Show log"
        '
        'ofdMain
        '
        Me.ofdMain.FileName = "OpenFileDialog1"
        '
        'tsMain
        '
        Me.tsMain.AllowDrop = True
        Me.tsMain.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.tsMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tstbFileToLoad})
        Me.tsMain.Location = New System.Drawing.Point(0, 24)
        Me.tsMain.Name = "tsMain"
        Me.tsMain.Size = New System.Drawing.Size(901, 25)
        Me.tsMain.TabIndex = 6
        Me.tsMain.Text = "ToolStrip1"
        '
        'tstbFileToLoad
        '
        Me.tstbFileToLoad.Name = "tstbFileToLoad"
        Me.tstbFileToLoad.Size = New System.Drawing.Size(100, 25)
        '
        'dpMain
        '
        Me.dpMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dpMain.BackColor = System.Drawing.Color.Silver
        Me.dpMain.Location = New System.Drawing.Point(0, 52)
        Me.dpMain.Name = "dpMain"
        Me.dpMain.Size = New System.Drawing.Size(901, 634)
        Me.dpMain.TabIndex = 11
        '
        'ssMain
        '
        Me.ssMain.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ssMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsslMain})
        Me.ssMain.Location = New System.Drawing.Point(0, 689)
        Me.ssMain.Name = "ssMain"
        Me.ssMain.Size = New System.Drawing.Size(901, 22)
        Me.ssMain.TabIndex = 14
        Me.ssMain.Text = "StatusStrip1"
        '
        'tsslMain
        '
        Me.tsslMain.Name = "tsslMain"
        Me.tsslMain.Size = New System.Drawing.Size(886, 17)
        Me.tsslMain.Spring = True
        Me.tsslMain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tUpdateStatus
        '
        Me.tUpdateStatus.Enabled = True
        '
        'MainForm
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(901, 711)
        Me.Controls.Add(Me.ssMain)
        Me.Controls.Add(Me.dpMain)
        Me.Controls.Add(Me.tsMain)
        Me.Controls.Add(Me.msMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.msMain
        Me.Name = "MainForm"
        Me.Text = "AstroImageBatch Version 0.1"
        Me.msMain.ResumeLayout(false)
        Me.msMain.PerformLayout
        Me.tsMain.ResumeLayout(false)
        Me.tsMain.PerformLayout
        Me.ssMain.ResumeLayout(false)
        Me.ssMain.PerformLayout
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
    Friend WithEvents msMain As System.Windows.Forms.MenuStrip
    Friend WithEvents ImageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ofdMain As System.Windows.Forms.OpenFileDialog
    Friend WithEvents tsMain As System.Windows.Forms.ToolStrip
    Friend WithEvents tstbFileToLoad As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents dpMain As WeifenLuo.WinFormsUI.Docking.DockPanel
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LoadImageFileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents OpenEXEPathToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents NewProcessorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DEBUGToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ssMain As System.Windows.Forms.StatusStrip
    Friend WithEvents tsslMain As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tUpdateStatus As System.Windows.Forms.Timer
    Friend WithEvents OnlyUseCSharpFitsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TestLookupTableToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiGenerateTestImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmRecentFiles As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiRecent1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiRecent2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiRecent3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiRecent4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiRecent5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiRecent6 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiRecent7 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiRecent8 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiRecent9 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiRecent10 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GenerateTestImagesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GrayscaleToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowLogToolStripMenuItem As ToolStripMenuItem
End Class
