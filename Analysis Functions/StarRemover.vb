Option Explicit On
Option Strict On

Public Class StarRemover

    '''<summary>Experimental function to remove the stars.</summary>
    Public Shared Sub RemoveStars(ByRef Data(,) As Double, ByVal StarsToRemove As Integer)

        'We define a value that marks a pixel as "removed"
        Dim ReservedValue As Double = CInt(Ato.Statistics.Mean(Data)) + 1

        'We remove a certain number of stars, e.g. 1000
        For Star As Integer = 1 To StarsToRemove

            'First, find the peak within the fiven data
            Dim X As Integer
            Dim Y As Integer
            Ato.Statistics.FindPeak(Data, X, Y)
            Dim Peak As Double = Data(X, Y)
            Dim FixingPossible As Double = StarRegionAnalysis(Data, X, Y)

            Debug.Print(Format(Star, "00000") & ":" & X.ToString.Trim & "/" & Y.ToString.Trim & "<" & FixingPossible & "> -> " & Peak.ToString.Trim)

        Next Star

    End Sub

    Public Shared Function StarRegionAnalysis(ByRef ImageData(,) As Double, ByVal X As Integer, ByVal Y As Integer) As Double

        Dim Delta_X As New List(Of Integer)
        Dim Delta_Y As New List(Of Integer)

        'Create one quadrant steppings list
        Dim Steppings As New List(Of Integer())
        For StepX As Integer = 0 To 40
            For StepY As Integer = 0 To 40
                Steppings.Add(New Integer() {StepX, StepY})
            Next StepY
        Next StepX
        Steppings.Sort(AddressOf DistanceSorter)

        'Create steppings in 1 quadrant and duplicate to 4 quadrants
        Delta_X.Add(0)
        Delta_Y.Add(0)
        For Each Entry As Integer() In Steppings
            Add4Quadrants(Delta_X, Delta_Y, Entry(0), Entry(1))
        Next Entry

        'Scan like a "schnecke" from inside to outside
        Dim FixingPossible As Boolean = False
        Dim LimitForCorrection As Double = 0

        Dim Max As Double = ImageData(X, Y)
        Dim AllSnailPixel As New List(Of Double)
        Dim Sum As Double = Max
        Dim Min As Double = Max
        Dim SearchIdx As Integer = 0
        Dim ClearLog As New List(Of String)
        For SearchIdx = 0 To Delta_X.Count - 1
            'Calculate image pixel position and check if this is a pixel in the image range
            Dim X_abs As Integer = X + Delta_X(SearchIdx)
            Dim Y_abs As Integer = Y + Delta_Y(SearchIdx)
            If (X_abs > 0) And (X_abs <= ImageData.GetUpperBound(0)) And (Y_abs > 0) And (Y_abs <= ImageData.GetUpperBound(1)) Then
                'We access a pixel in the image range
                Dim Pixel As Double = ImageData(X_abs, Y_abs)
                AllSnailPixel.Add(Pixel) : AllSnailPixel.Sort()
                Sum += Pixel
                If Pixel < Min Then Min = Pixel
                'We calculate the difference between the edge and the middle sorted pixel values and calculate the ratio
                If AllSnailPixel.Count >= 9 Then
                    Dim LowRange As Double = AllSnailPixel(AllSnailPixel.Count \ 2) - AllSnailPixel(0)
                    Dim HighRange As Double = AllSnailPixel(AllSnailPixel.Count - 1) - AllSnailPixel((AllSnailPixel.Count \ 2) - 1)
                    Dim LeftRightRatio As Double = LowRange / HighRange
                    ClearLog.Add(LeftRightRatio.ToString)
                    'Stop if half of the pixel intensities fall below a limit value
                    If LeftRightRatio < 1 Then
                        'Fix values
                        LimitForCorrection = 0
                        FixingPossible = True
                        Exit For
                    End If
                End If
            End If
        Next SearchIdx
        Clipboard.SetText(Join(ClearLog.ToArray, System.Environment.NewLine))

        'AllSnailPixel: Index 0 contains lowest level pixel value
        If FixingPossible = True Then
            For CorrectionIdx As Integer = 0 To SearchIdx
                'Calculate image pixel position and check if this is a pixel in the image range
                Dim X_abs As Integer = X + Delta_X(CorrectionIdx)
                Dim Y_abs As Integer = Y + Delta_Y(CorrectionIdx)
                If (X_abs > 0) And (X_abs <= ImageData.GetUpperBound(0)) And (Y_abs > 0) And (Y_abs <= ImageData.GetUpperBound(1)) Then
                    Dim Pixel As Double = ImageData(X_abs, Y_abs)
                    If Pixel > LimitForCorrection Then
                        Dim FillIdx As Integer = CInt(Rnd() * (AllSnailPixel.Count \ 10))
                        ImageData(X_abs, Y_abs) = AllSnailPixel(FillIdx)
                    End If
                End If
            Next CorrectionIdx
            Return Math.Sqrt((Delta_X(SearchIdx) * Delta_X(SearchIdx)) + (Delta_Y(SearchIdx) * Delta_Y(SearchIdx)))
        Else
            Return 0
        End If

    End Function

    Private Shared Function DistanceSorter(ByVal A As Integer(), ByVal B As Integer()) As Integer
        Return ((A(0) * A(0)) + (A(1) * A(1))).CompareTo((B(0) * B(0)) + (B(1) * B(1)))
    End Function

    Private Shared Sub Add4Quadrants(ByRef Delta_X As List(Of Integer), ByRef Delta_Y As List(Of Integer), ByVal dX As Integer, ByVal dY As Integer)
        Delta_X.Add(dX) : Delta_Y.Add(dY)
        Delta_X.Add(dX) : Delta_Y.Add(-dY)
        Delta_X.Add(-dX) : Delta_Y.Add(dY)
        Delta_X.Add(-dX) : Delta_Y.Add(-dY)
    End Sub


End Class
