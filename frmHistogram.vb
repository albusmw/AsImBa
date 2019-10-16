Option Explicit On
Option Strict On

Public Class frmHistogram

    'This elements are self-coded and will not work in 64-bit from the toolbox ...
    Private WithEvents zedHisto As ZedGraph.ZedGraphControl

    Private Sub frmHistogram_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Load custom controls - graph (must be done due to 64-bit IDE limitation)
        zedHisto = New ZedGraph.ZedGraphControl
        pGraphPlaceholder.Controls.Add(zedHisto)
        zedHisto.Dock = DockStyle.Fill

    End Sub

    Public Sub LoadData(ByRef Histo As Dictionary(Of Double, Integer), ByVal X_Min As Double, ByVal X_Max As Double)
        Functions.ShowDictionaryHistogram(Histo, zedHisto, X_Min, X_Max)
    End Sub

End Class