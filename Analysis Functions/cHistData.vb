Option Explicit On
Option Strict On

'''<summary>Histogramm class with sort function used for e.g. star diameter calculation.</summary>
Public Class cHistData
    Private Structure sSortHelper
        '''<summary>X axis value.</summary>
        Public XValue As Integer
        '''<summary>Collected Y axis values.</summary>
        Public Samples As List(Of Double)
        Public Sub New(ByVal NewXValue As Integer, ByVal NewSample As List(Of Double))
            Me.XValue = NewXValue
            Me.Samples = NewSample
        End Sub
        Public Shared Function Sorter(ByVal A As sSortHelper, ByVal B As sSortHelper) As Integer
            Return A.XValue.CompareTo(B.XValue)
        End Function
    End Structure

    Private XValues As New List(Of Integer)
    Private Samples As New List(Of List(Of Double))

    Public Sub Update(ByVal XValue As Integer, ByVal NewSample As Double)
        If XValues.Contains(XValue) = False Then
            'New X axis value
            XValues.Add(XValue)
            Samples.Add(New List(Of Double)({NewSample}))
        Else
            Samples(XValues.IndexOf(XValue)).Add(NewSample)
        End If
    End Sub

    Public Sub GetXY(ByRef X As Double(), ByRef Y As Double(), ByVal XScalingMultiplier As Double, ByRef Radius As Double, ByRef OuterEnergy As Double)

        'Put pushed values to a list of structure elements
        Dim SortHelper As New List(Of sSortHelper)
        For Idx As Integer = 0 To XValues.Count - 1
            SortHelper.Add(New sSortHelper(XValues(Idx), Samples(Idx)))
        Next Idx

        'Sort list
        SortHelper.Sort(AddressOf sSortHelper.Sorter)

        'Generate X and Y
        Dim TotalSum As Double = 0              'total sum of all pixel values in the area
        ReDim X(SortHelper.Count - 1)
        ReDim Y(SortHelper.Count - 1)
        For Idx As Integer = 0 To SortHelper.Count - 1
            X(Idx) = SortHelper(Idx).XValue * XScalingMultiplier
            Dim SumOfSamples As Double = 0
            For Each Sample As Double In SortHelper(Idx).Samples
                SumOfSamples += Sample
            Next Sample
            TotalSum += SumOfSamples
            Y(Idx) = SumOfSamples / SortHelper(Idx).Samples.Count
        Next Idx

        'Get star radius
        Dim CenterEnergy As Double = 0
        Dim NoiseEnergy As Double = 0
        Dim NoiseEnergyCount As Integer = 0
        Dim PixelRadiusIdx As Integer = -1
        For Idx As Integer = 0 To SortHelper.Count - 1
            CenterEnergy += Y(Idx) * SortHelper(Idx).Samples.Count
            If PixelRadiusIdx <> -1 Then
                NoiseEnergy += Y(PixelRadiusIdx) * SortHelper(PixelRadiusIdx).Samples.Count
                NoiseEnergyCount += SortHelper(PixelRadiusIdx).Samples.Count
            End If
            If CenterEnergy > TotalSum * 0.9 Then
                If PixelRadiusIdx = -1 Then PixelRadiusIdx = Idx 'set pixel radius once
                If Idx >= 2 * PixelRadiusIdx Then Exit For
            End If
        Next Idx

        If PixelRadiusIdx >= 0 And PixelRadiusIdx <= X.GetUpperBound(0) Then
            Radius = X(PixelRadiusIdx)
        Else
            Radius = Double.NaN
        End If

        If NoiseEnergyCount > 0 Then
            OuterEnergy = NoiseEnergy / NoiseEnergyCount
        Else
            OuterEnergy = Double.NaN
        End If


    End Sub

End Class