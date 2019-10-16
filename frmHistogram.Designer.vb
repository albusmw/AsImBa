<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHistogram
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
        Me.ttMain = New System.Windows.Forms.ToolTip(Me.components)
        Me.pGraphPlaceholder = New System.Windows.Forms.Panel()
        Me.SuspendLayout()
        '
        'pGraphPlaceholder
        '
        Me.pGraphPlaceholder.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pGraphPlaceholder.Location = New System.Drawing.Point(12, 12)
        Me.pGraphPlaceholder.Name = "pGraphPlaceholder"
        Me.pGraphPlaceholder.Size = New System.Drawing.Size(548, 530)
        Me.pGraphPlaceholder.TabIndex = 0
        '
        'frmHistogram
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(572, 554)
        Me.Controls.Add(Me.pGraphPlaceholder)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmHistogram"
        Me.Text = "Histogram"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ttMain As System.Windows.Forms.ToolTip
    Friend WithEvents pGraphPlaceholder As System.Windows.Forms.Panel
End Class
