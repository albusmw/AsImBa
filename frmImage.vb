Option Explicit On
Option Strict On

Public Class frmImage

    Private HistExpand_Min As Double = Double.NaN
    Private HistExpand_Max As Double = Double.NaN

    'This elements are self-coded and will not work in 64-bit from the toolbox ...
    Private WithEvents pbMain As GUIElements.PictureBoxEx
    Private WithEvents pbZoom As GUIElements.PictureBoxEx
    Private WithEvents zedHisto As ZedGraph.ZedGraphControl

    '''<summary>Size of the zooming area - can be adjusted with the mouse wheel.</summary>
    Private ZoomSize As Integer = Config.DefaultZoomSize
    '''<summary>Statistics of the zoomed area.</summary>
    Private ZoomedStatistics As New cStatistics

    '''<summary>Image statistics.</summary>
    Private WithEvents Statistics As New cImageStat

    '''<summary>Calculated center of mass.</summary>
    Private StarCenter As PointF

    '''<summary>Calculated star radius [pixel].</summary>
    Private StarRadius As Double

    '''<summary>Mean energy outside the star center [value].</summary>
    Private OuterEnergy As Double

    '''<summary>GUId of current displayed channel.</summary>
    Public DisplayedChannel As String = String.Empty

    'RTF-specifics
    Private RTFGen As New GUIElements.cRTFGenerator
    Private RTFHeaderColor As Color = Color.Red

    '''<summary>Load the displayed channel.</summary>
    Public Sub LoadChannel()
        LoadChannel(DisplayedChannel)
    End Sub

    '''<summary>Display the selected channel from the list of channels.</summary>
    Public Sub LoadChannel(ByVal GUID As String)
        'Set the displayed channel and update the complete statistics
        DisplayedChannel = GUID
        'Display the bitmap
        UpdateImage()
    End Sub

    '================================================================================
    ' Image drawing
    '================================================================================

    Public Sub UpdateImage()
        If String.IsNullOrEmpty(DisplayedChannel) = False Then
            Dim ImageToDisplay As cLockBitmap = DB.Channels(DisplayedChannel).CalculateImageFromData
            pbMain.Image = ImageToDisplay.BitmapToProcess
        End If
    End Sub

    '================================================================================
    ' Picture box mouse handling
    '================================================================================

    Private Sub pbMain_MouseWheel(sender As Object, e As MouseEventArgs) Handles pbMain.MouseWheel
        If e.Delta < 0 Then
            ZoomSize += 2
        Else
            If ZoomSize > 7 Then ZoomSize -= 2
        End If
        UpdateDetailedView(ZoomSize)
    End Sub

    '''<summary>This is required to enable the _MouseWheel event on the picture box.</summary>
    Private Sub pbMain_MouseEnter(sender As Object, e As EventArgs) Handles pbMain.MouseEnter
        pbMain.Focus()
    End Sub

    '''<summary>Moving the mouse changed the point to zoom in.</summary>
    Private Sub pbMain_MouseMove(sender As Object, e As MouseEventArgs) Handles pbMain.MouseMove
        If Not Statistics.LockLocation Then
            UpdateDetailedView(ZoomSize)
        End If
    End Sub

    '================================================================================
    ' Detailed view generation
    '================================================================================

    Private Sub UpdateDetailedView(ByVal ZoomSize As Integer)

        'Exit on no data
        If IsNothing(DB.Channels) Then
            Exit Sub
        Else
            ShowDetails(pbMain.ScreenCoordinatesToImageCoordinates)
        End If

    End Sub

    Private Sub ShowDetails()
        ShowDetails(ZoomedStatistics.Center)
    End Sub

    Public Sub ShowDetails(ByVal Coordinates As PointF)

        Dim PixelPerRealPixel As Integer = 4

        'Exit on ill conditions
        If String.IsNullOrEmpty(DisplayedChannel) = True Then Exit Sub

        'Calculate the zoom area
        Dim X_left As Integer : Dim X_right As Integer : Dim Y_top As Integer : Dim Y_bottom As Integer
        Dim RealCenter As Point = GUIElements.PictureBoxEx.CenterSizeToXY(Coordinates, ZoomSize, X_left, X_right, Y_top, Y_bottom)

        'Adjust left-right
        If X_left > X_right Then
            Dim Temp As Integer = X_left : X_left = X_right : X_right = Temp
        End If
        If Y_bottom > Y_top Then
            Dim Temp As Integer = Y_top : Y_top = Y_bottom : Y_bottom = Temp
        End If

        'Limit displayed values to image limits
        Dim ImageWidth As Integer = DB.Channels(DisplayedChannel).Width
        Dim ImageHeight As Integer = DB.Channels(DisplayedChannel).Height
        If X_left < 0 Then X_left = 0
        If X_right > ImageWidth - 1 Then X_right = ImageWidth - 1
        If Y_bottom < 0 Then Y_bottom = 0
        If Y_top > ImageHeight - 1 Then Y_top = ImageHeight - 1

        'Construct the holder of the zoomed image
        DB.Log.Tic("Getting the zoom-in part")
        Dim ZoomedInPart As cImageMonochrome = DB.Channels(DisplayedChannel).GetImagePart(X_left, X_right, Y_bottom, Y_top)
        DB.Log.Toc()

        'Fill the zoomed image, get MIN / MAX / pixel value count / histogram ("how many different values")
        ZoomedStatistics.Calculate(DB.Channels(DisplayedChannel).ImageData, X_left, X_right, Y_bottom, Y_top)

        'Store the new min and max values for Expand
        If Statistics.ExpandDynamicsMode = cImageStat.eExpandDynamicsMode.AutoExpand Then
            HistExpand_Min = ZoomedStatistics.Minimum
            HistExpand_Max = ZoomedStatistics.Maximum
        End If

        'Get the magnified version
        DB.Log.Tic("Getting the magnified zoom-in part")
        Dim ZoomInAsDisplayed As cImageMonochrome = ZoomedInPart.GetImagePartZoomed(PixelPerRealPixel, Statistics.ExpandDynamicsMode, HistExpand_Min, HistExpand_Max)
        DB.Log.Toc()

        '================================================================================
        'Calculate "center of mass" and mean values and display

        'Calculate center of mass
        'StartCenter is (0/0) if the star is in the middle of the detail
        DB.Log.Tic("Calculate center-of-mass")
        StarCenter = ImageAnalysis.CenterOfMass(ZoomedInPart)
        DB.Log.Toc()

        'Display center of mass in magnified image
        MarkCenter(ZoomInAsDisplayed, New PointF(StarCenter.X * PixelPerRealPixel, StarCenter.Y * PixelPerRealPixel), 4)
        pbZoom.Image = ZoomInAsDisplayed.GetLockBitmap(Statistics.ColorMap).BitmapToProcess

        '================================================================================
        'Show histogram
        Dim ShowRadialHisto As Boolean = False
        If ShowRadialHisto = True Then
            Dim HistRadius As Double = Config.HistMaxRadius
            HistRadius = (ZoomSize * Math.Sqrt(2)) / 2
            ShowHistoAroundCenter(HistRadius, Config.HistBinWidth)
        Else
            Functions.ShowDictionaryHistogram(ZoomedStatistics.Histogram, zedHisto, DB.Channels(DisplayedChannel).HistCalc.Minimum, DB.Channels(DisplayedChannel).HistCalc.Maximum)
        End If

        '================================================================================
        'Display details
        DisplayDetailedInformation()

    End Sub

    Private Sub MarkCenter(ByRef Image As cImageMonochrome, ByRef Center As PointF, ByVal IndicatorLength As Integer)

        If Single.IsNaN(Center.X) Then Exit Sub
        If Single.IsNaN(Center.Y) Then Exit Sub
        Dim ZoomCenter_X As Integer = CInt(Center.X)
        Dim ZoomCenter_Y As Integer = CInt(Center.Y)
        Dim MarkerColor As Double = Double.NaN

        Image.SetPixel(ZoomCenter_X, ZoomCenter_Y, MarkerColor)
        For Idx As Integer = 1 To IndicatorLength
            If ZoomCenter_X + Idx <= Image.Width - 1 Then Image.SetPixel(ZoomCenter_X + Idx, ZoomCenter_Y, MarkerColor)
            If ZoomCenter_X - Idx >= 0 Then Image.SetPixel(ZoomCenter_X - Idx, ZoomCenter_Y, MarkerColor)
            If ZoomCenter_Y + Idx <= Image.Height - 1 Then Image.SetPixel(ZoomCenter_X, ZoomCenter_Y + Idx, MarkerColor)
            If ZoomCenter_Y - Idx >= 0 Then Image.SetPixel(ZoomCenter_X, ZoomCenter_Y - Idx, MarkerColor)
        Next Idx

    End Sub

    

    Private Sub tUpdate_Tick(sender As Object, e As EventArgs) Handles tUpdate.Tick

        With tbPositionLock
            If Statistics.LockLocation = True Then
                .BackColor = Color.Red : .ForeColor = Color.Black
            Else
                .BackColor = Color.Gray : .ForeColor = Color.DarkGray
            End If
        End With

        With tbAutoExpand
            Select Case Statistics.ExpandDynamicsMode
                Case cImageStat.eExpandDynamicsMode.AutoExpand
                    .Text = "Expand" : .BackColor = Color.Green : .ForeColor = Color.Black
                Case cImageStat.eExpandDynamicsMode.FreezeExpand
                    .Text = "Expand" : .BackColor = Color.Blue : .ForeColor = Color.Black
                Case cImageStat.eExpandDynamicsMode.DoNotExpand
                    .Text = "Original" : .BackColor = Color.Gray : .ForeColor = Color.DarkGray
            End Select
        End With

    End Sub

    Private Sub DisplayDetailedInformation()

        With RTFGen
            .Clear()
            AddToRTF("Original data")
            AddToRTF("  Size", DB.Channels(DisplayedChannel).Width.ToString.Trim & " x " & DB.Channels(DisplayedChannel).Height.ToString.Trim)
            AddToRTF("  Pixel", DB.Channels(DisplayedChannel).HistCalc.Pixel.ToString.Trim)
            AddToRTF("  Different values", DB.Channels(DisplayedChannel).HistCalc.DifferentValues.ToString.Trim)
            AddToRTF("  Range MIN", FormatNum(DB.Channels(DisplayedChannel).HistCalc.Minimum, "0.000"))
            AddToRTF("        MAX", FormatNum(DB.Channels(DisplayedChannel).HistCalc.Maximum, "0.000"))
            AddToRTF("Scaled data")
            AddToRTF("  Median", Format(DB.Channels(DisplayedChannel).HistCalc.Median, "0.000").Replace(",", "."))
            AddToRTF("  25-/75-percentil", Format(DB.Channels(DisplayedChannel).HistCalc.Pct25, "0.000").Replace(",", ".") & " / " & Format(DB.Channels(DisplayedChannel).HistCalc.Pct75, "0.000").Replace(",", "."))
            AddToRTF("  Histo peak level", Format(DB.Channels(DisplayedChannel).HistCalc.HistoPeakPos, "0.000").Replace(",", "."))
            AddToRTF("Detail")
            AddToRTF("  Center", ZoomedStatistics.CenterFormated)
            AddToRTF("  Size", ZoomedStatistics.DetailSizeFormated)
            AddToRTF("  Used values", ZoomedStatistics.DifferentValues.ToString.Trim)
            AddToRTF("  Range", ZoomedStatistics.DetailRangeFormated)
            AddToRTF("  Mean", FormatNum(ZoomedStatistics.Mean, "0.00"))
            AddToRTF("  Median", FormatNum(ZoomedStatistics.Median, "0.00"))
            AddToRTF("  StdDev", FormatNum(ZoomedStatistics.StdDev, "0.00"))
            AddToRTF("Aperture")
            AddToRTF("  Middle X", FormatNum(ZoomedStatistics.Center.X + StarCenter.X, "0.000"))
            AddToRTF("  Middle Y", FormatNum(ZoomedStatistics.Center.Y + StarCenter.Y, "0.000"))
            AddToRTF("  Radius", FormatNum(StarRadius, "0.00"))
            AddToRTF("  Outer Energy", FormatNum(OuterEnergy, "0.00"))
            .ForceRefresh()
        End With
    End Sub

    Private Function FormatNum(ByVal Number As Double, ByVal FormatString As String) As String
        Return Format(Number, FormatString).Replace(",", ".")
    End Function

    Private Sub AddToRTF(ByVal Header As String)
        AddToRTF(Header, String.Empty)
    End Sub

    Private Sub AddToRTF(ByVal Header As String, ByVal Value As String)
        If String.IsNullOrEmpty(Value) Then
            'Header
            RTFGen.AddEntry(Header, RTFHeaderColor, True, True)
        Else
            'Entry
            RTFGen.AddEntry(Header.PadRight(20), Color.Black, False)
            RTFGen.AddEntry(Value.PadLeft(20), Color.Black, True)
        End If
    End Sub

    Private Sub pbMain_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles pbMain.PreviewKeyDown

        'KEY HANDLER

        Dim OldCenter As PointF = ZoomedStatistics.Center

        Select Case e.KeyCode
            Case Keys.Left
                OldCenter.X -= 1 : ShowDetails(OldCenter)
            Case Keys.Right
                OldCenter.X += 1 : ShowDetails(OldCenter)
            Case Keys.Up
                OldCenter.Y -= 1 : ShowDetails(OldCenter)
            Case Keys.Down
                OldCenter.Y += 1 : ShowDetails(OldCenter)
            Case Keys.PageUp
                If e.Shift = True Then
                    ZoomSize = 11 : UpdateDetailedView(ZoomSize)    'Default zom size
                Else
                    ZoomSize -= 2 : UpdateDetailedView(ZoomSize)    'Increase zoom (move in)
                End If
            Case Keys.PageDown
                ZoomSize += 2 : UpdateDetailedView(ZoomSize)        'Decease zoom (move out)
            Case Keys.L
                'Lock location
                Statistics.LockLocation = Not Statistics.LockLocation
            Case Keys.F
                'Freeze Expand dynamics in zoom display
                Statistics.ExpandDynamicsMode = cImageStat.eExpandDynamicsMode.FreezeExpand
            Case Keys.M
                'Change color map in zoom window
                Select Case Statistics.ColorMap
                    Case CType(3, cColorMaps.eMaps) : Statistics.ColorMap = CType(0, cColorMaps.eMaps)
                    Case Else : Statistics.ColorMap = CType(Statistics.ColorMap + 1, cColorMaps.eMaps)
                End Select
        End Select

        'Expand dynamics
        If e.KeyCode.ToString = "E" Then
            Select Case Statistics.ExpandDynamicsMode
                Case cImageStat.eExpandDynamicsMode.DoNotExpand
                    Statistics.ExpandDynamicsMode = cImageStat.eExpandDynamicsMode.AutoExpand
                Case cImageStat.eExpandDynamicsMode.AutoExpand
                    Statistics.ExpandDynamicsMode = cImageStat.eExpandDynamicsMode.DoNotExpand
                Case cImageStat.eExpandDynamicsMode.FreezeExpand
                    Statistics.ExpandDynamicsMode = cImageStat.eExpandDynamicsMode.AutoExpand
            End Select
        End If

        'Jump to peak
        If e.KeyCode.ToString = "P" Then
            'Search the peak value based on the image data and translate the 1D-peak to 2D-data index
            Dim MaxValue As Double : Dim MaxIdx As Integer
            DB.IPP.MaxIndx(DB.Channels(DisplayedChannel).ImageData, MaxValue, MaxIdx)
            Dim ImageHeight As Integer = DB.Channels(DisplayedChannel).ImageData.GetUpperBound(1) + 1
            Dim Y As Integer = MaxIdx Mod ImageHeight
            Dim X As Integer = (MaxIdx - Y) \ ImageHeight
            'Show details
            ShowDetails(New Point(X, Y))
        End If

        'Mask star
        If e.KeyCode.ToString = "X" Then
            pbMain_MouseUp(Nothing, Nothing)
        End If

        pbMain.Select()

    End Sub

    Private Sub Statistics_SettingChanged() Handles Statistics.DetailSettingChanged
        UpdateDetailedView(ZoomSize)
    End Sub

    Private Sub Statistics_ImageDataChanged() Handles Statistics.ImageDataChanged
        UpdateImage()
    End Sub

    Private Sub frmImage_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Load custom controls - main image (must be done due to 64-bit IDE limitation)
        pbMain = New GUIElements.PictureBoxEx
        scMain.Panel1.Controls.Add(pbMain)
        pbMain.Dock = DockStyle.Fill
        pbMain.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        pbMain.SizeMode = PictureBoxSizeMode.Zoom

        'Load custom controls - zoom image (must be done due to 64-bit IDE limitation)
        pbZoom = New GUIElements.PictureBoxEx
        scGraphics.Panel1.Controls.Add(pbZoom)
        pbZoom.Dock = DockStyle.Fill
        pbZoom.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        pbZoom.SizeMode = PictureBoxSizeMode.Zoom
        'Load custom controls - graph (must be done due to 64-bit IDE limitation)
        zedHisto = New ZedGraph.ZedGraphControl
        scGraphics.Panel2.Controls.Add(zedHisto)
        zedHisto.Dock = DockStyle.Fill


        'Set RTF
        RTFGen.AttachToControl(rtbMain)
        RTFGen.AutoScroll = False
        RTFGen.AutoRefresh = False
        RTFGen.RTFInit(8)

        'Set splitter width
        scMain.Panel2MinSize = 300

    End Sub

    Private Sub pbMain_MouseUp(sender As Object, e As MouseEventArgs)

        'Exit on no data
        If IsNothing(DB.Channels) Then Exit Sub

        'Replace with star center
        'Dim Center_X As Double = Statistics.CalcStat.DetailCenter.X + StarCenter.X - (Statistics.CalcStat.DetailSize_X / 2)
        'Dim Center_Y As Double = Statistics.CalcStat.DetailCenter.Y + StarCenter.Y - (Statistics.CalcStat.DetailSize_Y / 2)
        'Dim Coordinates As New PointF(CSng(Center_X), CSng(Center_Y))

        'ImageManipulation.HardMask(DB.Channels(DisplayedChannel).ImageData, Coordinates, Math.Ceiling(StarRadius), OuterEnergy)

        'Update image
        ShowDetails()
        UpdateImage()

    End Sub

    '''<summary>Calculate the histogram around the center (to get star radius, ...).</summary>
    Private Sub ShowHistoAroundCenter(ByVal Radius As Double, ByVal BinWidth As Double)

        Dim Center_X As Double = ZoomedStatistics.Center.X + StarCenter.X - (ZoomedStatistics.Width / 2)
        Dim Center_Y As Double = ZoomedStatistics.Center.Y + StarCenter.Y - (ZoomedStatistics.Height / 2)
        Dim Center As New PointF(CSng(Center_X), CSng(Center_Y))

        Dim HistData As New cHistData
        Dim ValuesAdded As New List(Of Integer)

        If Double.IsNaN(Center.X) = True Then Exit Sub
        If Double.IsNaN(Center.Y) = True Then Exit Sub

        'Scan a rectangular region and remove the pixel within the radius specified
        For IdxX As Integer = CInt(Math.Floor(Center.X - Radius)) To CInt(Math.Ceiling(Center.X + Radius))
            If IdxX >= 0 And IdxX <= DB.Channels(DisplayedChannel).ImageData.GetUpperBound(0) Then
                Dim DeltaX As Double = ((IdxX - Center.X) * (IdxX - Center.X))
                For IdxY As Integer = CInt(Math.Floor(Center.Y - Radius)) To CInt(Math.Ceiling(Center.Y + Radius))
                    If IdxY >= 0 And IdxY <= DB.Channels(DisplayedChannel).ImageData.GetUpperBound(1) Then
                        Dim CurrentRadius As Integer = CInt(Math.Sqrt(Math.Floor(DeltaX + ((IdxY - Center.Y) * (IdxY - Center.Y)))) / BinWidth)
                        HistData.Update(CurrentRadius, DB.Channels(DisplayedChannel).ImageData(IdxX, IdxY))
                    End If
                Next IdxY
            End If
        Next IdxX

        Dim Plot_X As Double() = {}
        Dim Plot_Y As Double() = {}

        HistData.GetXY(Plot_X, Plot_Y, BinWidth, StarRadius, OuterEnergy)

        ZEDGraphUtil.PlotXvsY(zedHisto, "Power over center distance", Plot_X, Plot_Y, New ZEDGraphUtil.sGraphStyle(Color.Red, ZEDGraphUtil.sGraphStyle.eCurveMode.Dots), Double.NaN, Double.NaN)

    End Sub

End Class