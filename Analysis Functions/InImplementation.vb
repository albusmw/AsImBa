Option Explicit On
Option Strict On

Public Class InImplementation

    Public Shared Sub GridBrightStars(ByVal ChannelToEdit As String, ByVal PixelPerDirection As Integer, ByVal ZoomSize As Integer, ByVal OutputFileName As String)

        'Expand to BMP dynamics
        DB.Channels(ChannelToEdit).Expand(0, 255)

        'Calculate the grid sizes
        Dim XGrids As Integer() = IntDefs(DB.Channels(ChannelToEdit).Width)
        Dim YGrids As Integer() = IntDefs(DB.Channels(ChannelToEdit).Height)

        'Decide for a grid size and a zoom size
        Dim XGridSize As Integer = 80 : Dim XGridCount As Integer = DB.Channels(ChannelToEdit).Width \ XGridSize
        Dim YGridSize As Integer = 92 : Dim YGridCount As Integer = DB.Channels(ChannelToEdit).Height \ YGridSize
        Dim TileSize As Integer = ((2 * PixelPerDirection) + 1) * ZoomSize

        Dim GridOut As New cImageMonochrome(XGridCount * TileSize, YGridCount * TileSize)

        For XIdx As Integer = 0 To XGridCount - 1
            For YIdx As Integer = 0 To YGridCount - 1
                'Get the grid part
                Dim Part As cImageMonochrome = DB.Channels(ChannelToEdit).GetImagePart((XIdx * XGridSize), ((XIdx + 1) * XGridSize) - 1, (YIdx * YGridSize), ((YIdx + 1) * YGridSize) - 1)
                'Find the peak
                Dim Peak_X As Integer
                Dim Peak_Y As Integer
                Part.GetPeak(Peak_X, Peak_Y)
                'Convert the peak in the part to the global peak
                Peak_X += (XIdx * XGridSize)
                Peak_Y += (YIdx * YGridSize)
                'Get image around the peak
                Dim PeakPart As cImageMonochrome = DB.Channels(ChannelToEdit).GetImagePart(Peak_X - PixelPerDirection, Peak_X + PixelPerDirection, Peak_Y - PixelPerDirection, Peak_Y + PixelPerDirection)
                Dim Magnified As cImageMonochrome = PeakPart.GetImagePartZoomed(ZoomSize, cImageStat.eExpandDynamicsMode.DoNotExpand, 0, 0)
                'Copy peak part to grid out
                For CopyX As Integer = 0 To Magnified.Width - 1
                    For CopyY As Integer = 0 To Magnified.Height - 1
                        GridOut.SetPixel((XIdx * TileSize) + CopyX, (YIdx * TileSize) + CopyY, Magnified.ImageData(CopyX, CopyY))
                    Next CopyY
                Next CopyX
            Next YIdx
        Next XIdx

        'Draw raster
        For XIdx As Integer = 0 To GridOut.Width - 1 Step TileSize
            For YIdx As Integer = 0 To GridOut.Height - 1
                GridOut.SetPixel(XIdx, YIdx, 255)
            Next
        Next XIdx
        For YIdx As Integer = 0 To GridOut.Height - 1 Step TileSize
            For XIdx As Integer = 0 To GridOut.Width - 1
                GridOut.SetPixel(XIdx, YIdx, 255)
            Next XIdx
        Next YIdx
        For YIdx As Integer = 0 To GridOut.Height - 1
            GridOut.SetPixel(GridOut.Width - 1, YIdx, 255)
        Next
        For XIdx As Integer = 0 To GridOut.Width - 1
            GridOut.SetPixel(XIdx, GridOut.Height - 1, 255)
        Next XIdx

        'Save grid out
        GridOut.Save(OutputFileName)
        Shell(OutputFileName)

    End Sub

    Private Shared Function IntDefs(ByVal Number As Integer) As Integer()
        Dim RetVal As New List(Of Integer)
        For Idx As Integer = 2 To Number \ 2
            If Number / Idx = Number \ Idx Then RetVal.Add(Idx)
        Next Idx
        Return RetVal.ToArray
    End Function

End Class
