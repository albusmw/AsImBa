Option Explicit On
Option Strict On

Public Class Functions

    '''<summary>Convert the passed (sorted or not) dictionary to 2 vectors, e.g. for plot purpose.</summary>
    Public Shared Sub HistoToXY(ByRef Dict As Dictionary(Of Double, Integer), ByRef X As Double(), ByRef Y As Double())

        Dim Data_X As New List(Of Double)
        Dim Data_Y As New List(Of Double)
        For Each Entry As Double In Dict.Keys
            Data_X.Add(Entry)
            Data_Y.Add(Dict(Entry))
        Next Entry
        X = Data_X.ToArray
        Y = Data_Y.ToArray

    End Sub

    '''<summary>Plot the passed dictionary.</summary>
    Public Shared Sub ShowDictionaryHistogram(ByRef Dict As Dictionary(Of Double, Integer), ByRef zedHisto As ZedGraph.ZedGraphControl, ByVal X_Min As Double, ByVal X_Max As Double)

        Dim Data_X As Double() = {}
        Dim Data_Y As Double() = {}
        Functions.HistoToXY(Dict, Data_X, Data_Y)

        ZEDGraphUtil.PlotXvsY(zedHisto, "Histogram", Data_X.ToArray, Data_Y.ToArray, New ZEDGraphUtil.sGraphStyle(Color.Red, ZEDGraphUtil.sGraphStyle.eCurveMode.LinesAndPoints), X_Min, X_Max)
        ZEDGraphUtil.SetCaptions(zedHisto, "Histogram", "Pixel value", "Pixel value count")
        ZEDGraphUtil.MaximizePlotArea(zedHisto)
        

        zedHisto.GraphPane.XAxis.Type = ZedGraph.AxisType.Linear
        zedHisto.GraphPane.YAxis.Type = ZedGraph.AxisType.Log
        zedHisto.Refresh()

    End Sub



End Class
