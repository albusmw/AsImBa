Option Explicit On
Option Strict On

Public Class MathEx

    '''<summary>Calculate Min and Max based on the given data, ignoring NAN's.</summary>
    '''<remarks>Calculation is done if eight MyOriginal_RangeMin or MyOriginal_RangeMax is NAN.</remarks>
    Public Shared Sub MaxMinIgnoreNAN(ByRef ImageData(,) As Double, ByRef MyOriginal_RangeMin As Double, ByRef MyOriginal_RangeMax As Double)

        'Special processing on NAN in the content
        If Double.IsNaN(MyOriginal_RangeMin) Or Double.IsNaN(MyOriginal_RangeMax) Then
            MyOriginal_RangeMin = Double.MaxValue
            MyOriginal_RangeMax = Double.MinValue
            For Idx1 As Integer = 0 To ImageData.GetUpperBound(0)
                For Idx2 As Integer = 0 To ImageData.GetUpperBound(1)
                    If ImageData(Idx1, Idx2) > MyOriginal_RangeMax Then MyOriginal_RangeMax = ImageData(Idx1, Idx2)
                    If ImageData(Idx1, Idx2) < MyOriginal_RangeMin Then MyOriginal_RangeMin = ImageData(Idx1, Idx2)
                Next Idx2
            Next Idx1
        End If

    End Sub

End Class
