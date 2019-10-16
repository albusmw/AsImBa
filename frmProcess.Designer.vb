<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProcess
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
        Me.cbChannels = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.tsslStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ttMain = New System.Windows.Forms.ToolTip(Me.components)
        Me.cbRescale = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cbPowers = New System.Windows.Forms.ComboBox()
        Me.cbFlipRotate = New System.Windows.Forms.ComboBox()
        Me.cbComplexOperations = New System.Windows.Forms.ComboBox()
        Me.btnHisto = New System.Windows.Forms.Button()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cbChannels
        '
        Me.cbChannels.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbChannels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbChannels.FormattingEnabled = True
        Me.cbChannels.Location = New System.Drawing.Point(64, 12)
        Me.cbChannels.Name = "cbChannels"
        Me.cbChannels.Size = New System.Drawing.Size(225, 21)
        Me.cbChannels.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(46, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Channel"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsslStatus})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 328)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(295, 22)
        Me.StatusStrip1.TabIndex = 4
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'tsslStatus
        '
        Me.tsslStatus.Name = "tsslStatus"
        Me.tsslStatus.Size = New System.Drawing.Size(30, 17)
        Me.tsslStatus.Text = "IDLE"
        '
        'cbRescale
        '
        Me.cbRescale.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbRescale.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.cbRescale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbRescale.FormattingEnabled = True
        Me.cbRescale.Items.AddRange(New Object() {"Rescale ...", "  Range 0 ... 255", "  Range 0 ... 65535", "  Range 0 ... 1", "  Range x ... y"})
        Me.cbRescale.Location = New System.Drawing.Point(79, 47)
        Me.cbRescale.Name = "cbRescale"
        Me.cbRescale.Size = New System.Drawing.Size(209, 21)
        Me.cbRescale.TabIndex = 10
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 50)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(61, 13)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "Operations:"
        '
        'cbPowers
        '
        Me.cbPowers.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbPowers.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.cbPowers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPowers.FormattingEnabled = True
        Me.cbPowers.Items.AddRange(New Object() {"Powers ...", "  (..)^2", "  (..)^3", "  (..)^0.5"})
        Me.cbPowers.Location = New System.Drawing.Point(79, 74)
        Me.cbPowers.Name = "cbPowers"
        Me.cbPowers.Size = New System.Drawing.Size(209, 21)
        Me.cbPowers.TabIndex = 12
        '
        'cbFlipRotate
        '
        Me.cbFlipRotate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbFlipRotate.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.cbFlipRotate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbFlipRotate.FormattingEnabled = True
        Me.cbFlipRotate.Items.AddRange(New Object() {"Flip / rotate ...", "  Flip left-right", "  Flip top-buttom", "  Rotate clockwise", "  Rotate counter-clock wise"})
        Me.cbFlipRotate.Location = New System.Drawing.Point(79, 101)
        Me.cbFlipRotate.Name = "cbFlipRotate"
        Me.cbFlipRotate.Size = New System.Drawing.Size(209, 21)
        Me.cbFlipRotate.TabIndex = 13
        '
        'cbComplexOperations
        '
        Me.cbComplexOperations.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbComplexOperations.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.cbComplexOperations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbComplexOperations.FormattingEnabled = True
        Me.cbComplexOperations.Items.AddRange(New Object() {"Operation", "  Invert within present range", "  Sin", "  Stretch with equal distribution", "  Expand high and low intensities", "  Remove stars", "  Set highest amplitude to 0", "  Center-of-Mass", "  Grid bright stars"})
        Me.cbComplexOperations.Location = New System.Drawing.Point(79, 128)
        Me.cbComplexOperations.Name = "cbComplexOperations"
        Me.cbComplexOperations.Size = New System.Drawing.Size(210, 21)
        Me.cbComplexOperations.TabIndex = 14
        '
        'btnHisto
        '
        Me.btnHisto.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnHisto.Location = New System.Drawing.Point(79, 155)
        Me.btnHisto.Name = "btnHisto"
        Me.btnHisto.Size = New System.Drawing.Size(209, 25)
        Me.btnHisto.TabIndex = 15
        Me.btnHisto.Text = "Show histogram"
        Me.btnHisto.UseVisualStyleBackColor = True
        '
        'frmProcess
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(295, 350)
        Me.Controls.Add(Me.btnHisto)
        Me.Controls.Add(Me.cbComplexOperations)
        Me.Controls.Add(Me.cbFlipRotate)
        Me.Controls.Add(Me.cbPowers)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cbRescale)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cbChannels)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmProcess"
        Me.Text = "Processing window"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cbChannels As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents tsslStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ttMain As System.Windows.Forms.ToolTip
    Friend WithEvents cbRescale As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cbPowers As System.Windows.Forms.ComboBox
    Friend WithEvents cbFlipRotate As System.Windows.Forms.ComboBox
    Friend WithEvents cbComplexOperations As System.Windows.Forms.ComboBox
    Friend WithEvents btnHisto As System.Windows.Forms.Button
End Class
