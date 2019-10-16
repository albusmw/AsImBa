Option Explicit On
Option Strict On

Public Class ImageManipulation

    '''<summary>Mask the radius around the center with the given MaskValue.</summary>
    Public Shared Sub HardMask(ByRef ImageData(,) As Double, ByVal Center As PointF, ByVal Radius As Double, ByVal MaskValue As Double)

        Dim RR As Double = Radius * Radius

        If Double.IsNaN(Center.X) = True Then Exit Sub
        If Double.IsNaN(Center.Y) = True Then Exit Sub

        'Scan a rectangular region and remove the pixel within the radius specified
        For IdxX As Integer = CInt(Math.Floor(Center.X - Radius)) To CInt(Math.Ceiling(Center.X + Radius))
            If IdxX >= 0 And IdxX <= ImageData.GetUpperBound(0) Then
                Dim DeltaX As Double = ((IdxX - Center.X) * (IdxX - Center.X))
                For IdxY As Integer = CInt(Math.Floor(Center.Y - Radius)) To CInt(Math.Ceiling(Center.Y + Radius))
                    If IdxY >= 0 And IdxY <= ImageData.GetUpperBound(1) Then
                        Dim CurrentRadius As Double = DeltaX + ((IdxY - Center.Y) * (IdxY - Center.Y))
                        If CurrentRadius <= RR Then
                            ImageData(IdxX, IdxY) = MaskValue
                        End If
                    End If
                Next IdxY
            End If
        Next IdxX

    End Sub

End Class
