Option Explicit On
Option Strict On

Public Class Config

    '''<summary>Default zoom size [pixel].</summary>
    Public Shared Property DefaultZoomSize As Integer = 31

    Public Shared Property HardMaskSize As Integer = 20

    Public Shared Property HardMaskFillValue As Double = 0

    '''<summary>Color value indicating a wrong value range for RGB.</summary>
    Public Shared Property InvalidColor As Color = Color.Red

    '''<summary>Maximum radius for the radial histogram statistics [pixel].</summary>
    Public Shared Property HistMaxRadius As Double = 10

    '''<summary>Width of the radial histogram statistics [pixel].</summary>
    Public Shared Property HistBinWidth As Double = 1 / 10

End Class
