<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmImage
    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

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
        Me.ssMain = New System.Windows.Forms.StatusStrip()
        Me.scMain = New System.Windows.Forms.SplitContainer()
        Me.scRight = New System.Windows.Forms.SplitContainer()
        Me.scGraphics = New System.Windows.Forms.SplitContainer()
        Me.tbAutoExpand = New System.Windows.Forms.TextBox()
        Me.tbPositionLock = New System.Windows.Forms.TextBox()
        Me.gbDetails = New System.Windows.Forms.GroupBox()
        Me.rtbMain = New System.Windows.Forms.RichTextBox()
        Me.tUpdate = New System.Windows.Forms.Timer(Me.components)
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        CType(Me.scRight, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scRight.Panel1.SuspendLayout()
        Me.scRight.Panel2.SuspendLayout()
        Me.scRight.SuspendLayout()
        CType(Me.scGraphics, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scGraphics.SuspendLayout()
        Me.gbDetails.SuspendLayout()
        Me.SuspendLayout()
        '
        'ssMain
        '
        Me.ssMain.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ssMain.Location = New System.Drawing.Point(0, 864)
        Me.ssMain.Name = "ssMain"
        Me.ssMain.Size = New System.Drawing.Size(1392, 22)
        Me.ssMain.TabIndex = 4
        Me.ssMain.Text = "StatusStrip1"
        '
        'scMain
        '
        Me.scMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.scMain.Location = New System.Drawing.Point(0, 0)
        Me.scMain.Name = "scMain"
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.scRight)
        Me.scMain.Size = New System.Drawing.Size(1392, 861)
        Me.scMain.SplitterDistance = 1055
        Me.scMain.TabIndex = 6
        '
        'scRight
        '
        Me.scRight.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scRight.Location = New System.Drawing.Point(2, 3)
        Me.scRight.Name = "scRight"
        Me.scRight.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scRight.Panel1
        '
        Me.scRight.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.scRight.Panel1.Controls.Add(Me.scGraphics)
        '
        'scRight.Panel2
        '
        Me.scRight.Panel2.Controls.Add(Me.tbAutoExpand)
        Me.scRight.Panel2.Controls.Add(Me.tbPositionLock)
        Me.scRight.Panel2.Controls.Add(Me.gbDetails)
        Me.scRight.Size = New System.Drawing.Size(331, 858)
        Me.scRight.SplitterDistance = 424
        Me.scRight.TabIndex = 5
        '
        'scGraphics
        '
        Me.scGraphics.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scGraphics.Location = New System.Drawing.Point(0, 0)
        Me.scGraphics.Name = "scGraphics"
        Me.scGraphics.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.scGraphics.Size = New System.Drawing.Size(331, 424)
        Me.scGraphics.SplitterDistance = 214
        Me.scGraphics.TabIndex = 7
        '
        'tbAutoExpand
        '
        Me.tbAutoExpand.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbAutoExpand.Location = New System.Drawing.Point(71, 3)
        Me.tbAutoExpand.Name = "tbAutoExpand"
        Me.tbAutoExpand.ReadOnly = True
        Me.tbAutoExpand.Size = New System.Drawing.Size(56, 26)
        Me.tbAutoExpand.TabIndex = 7
        Me.tbAutoExpand.Text = "Dyn Exp"
        Me.tbAutoExpand.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tbPositionLock
        '
        Me.tbPositionLock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbPositionLock.Location = New System.Drawing.Point(9, 3)
        Me.tbPositionLock.Name = "tbPositionLock"
        Me.tbPositionLock.ReadOnly = True
        Me.tbPositionLock.Size = New System.Drawing.Size(56, 26)
        Me.tbPositionLock.TabIndex = 6
        Me.tbPositionLock.Text = "Lock Pos"
        Me.tbPositionLock.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'gbDetails
        '
        Me.gbDetails.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbDetails.Controls.Add(Me.rtbMain)
        Me.gbDetails.Location = New System.Drawing.Point(3, 29)
        Me.gbDetails.Name = "gbDetails"
        Me.gbDetails.Size = New System.Drawing.Size(325, 398)
        Me.gbDetails.TabIndex = 5
        Me.gbDetails.TabStop = False
        Me.gbDetails.Text = "Details"
        '
        'rtbMain
        '
        Me.rtbMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbMain.Location = New System.Drawing.Point(6, 19)
        Me.rtbMain.Name = "rtbMain"
        Me.rtbMain.Size = New System.Drawing.Size(313, 373)
        Me.rtbMain.TabIndex = 7
        Me.rtbMain.Text = ""
        Me.rtbMain.WordWrap = False
        '
        'tUpdate
        '
        Me.tUpdate.Enabled = True
        '
        'frmImage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1392, 886)
        Me.Controls.Add(Me.scMain)
        Me.Controls.Add(Me.ssMain)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmImage"
        Me.Text = "Image"
        Me.scMain.Panel2.ResumeLayout(False)
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.ResumeLayout(False)
        Me.scRight.Panel1.ResumeLayout(False)
        Me.scRight.Panel2.ResumeLayout(False)
        Me.scRight.Panel2.PerformLayout()
        CType(Me.scRight, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scRight.ResumeLayout(False)
        CType(Me.scGraphics, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scGraphics.ResumeLayout(False)
        Me.gbDetails.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ssMain As System.Windows.Forms.StatusStrip
    Friend WithEvents scMain As System.Windows.Forms.SplitContainer
    Friend WithEvents tUpdate As System.Windows.Forms.Timer
    Friend WithEvents scRight As System.Windows.Forms.SplitContainer
    Friend WithEvents gbDetails As System.Windows.Forms.GroupBox
    Friend WithEvents rtbMain As System.Windows.Forms.RichTextBox
    Friend WithEvents tbPositionLock As System.Windows.Forms.TextBox
    Friend WithEvents tbAutoExpand As System.Windows.Forms.TextBox
    Friend WithEvents scGraphics As System.Windows.Forms.SplitContainer
End Class
