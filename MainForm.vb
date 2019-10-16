Option Explicit On
Option Strict On

Public Class MainForm

    Public MyPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)
    Public INIFile As String = MyPath & "\Config.INI"

    Public FileFormats As New Dictionary(Of String(), String)

    Public RecentFiles As New List(Of String)

    Private Sub LoadImage(ByVal FileNames As String())

        Dim LastAddedGUID As String = String.Empty

        For Each FileName As String In FileNames

            If System.IO.File.Exists(FileName) Then

                'Get file extension and remove leading "."
                Dim Extension As String = System.IO.Path.GetExtension(FileName).ToUpper
                If Extension.StartsWith(".") Then Extension = Extension.Substring(1)

                Select Case Extension

                    Case "CR2", "NEF"

                        'Create a local copy
                        Dim LocalFileNameRoot As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly.Location) & "\" & CInt(Rnd() * 1000000)
                        Dim LocalFile As String = LocalFileNameRoot & System.IO.Path.GetExtension(FileName)
                        If System.IO.File.Exists(LocalFile) Then System.IO.File.Delete(LocalFile)
                        System.IO.File.Copy(FileName, LocalFile)

                        Dim StartInfo As New ProcessStartInfo
                        StartInfo.FileName = "dcraw-9.26-ms-64-bit.exe"
                        StartInfo.Arguments = "-v -W -g 1 1 -D " & Chr(34) & LocalFile & Chr(34)
                        StartInfo.UseShellExecute = False
                        StartInfo.CreateNoWindow = True
                        StartInfo.RedirectStandardOutput = True
                        StartInfo.RedirectStandardError = True
                        Dim DCRaw As New Process
                        DCRaw.StartInfo = StartInfo
                        DCRaw.Start()
                        DCRaw.WaitForExit()
                        Dim Output As String() = DCRaw.StandardError.ReadToEnd.Trim.Split(Chr(10))
                        Dim DecodeFile As String = LocalFileNameRoot & ".pgm"

                        LastAddedGUID = DB.AddChannel(New cImageMonochrome)
                        DB.Channels(LastAddedGUID).LoadPortableAnymap(DecodeFile, cImageMonochrome.eBayerChannel.Green0)

                        'Delete temporary file
                        System.IO.File.Delete(LocalFile)
                        System.IO.File.Delete(DecodeFile)

                    Case "JPG"

                        LastAddedGUID = DB.AddChannel(New cImageMonochrome)
                        DB.Channels(LastAddedGUID).LoadJPEGData(FileName)

                    Case "TIFF", "TIF"

                        LastAddedGUID = DB.AddChannel(New cImageMonochrome)
                        DB.Channels(LastAddedGUID).LoadTIFFData(FileName)

                    Case "FIT", "FITS"

                        'Direct load image
                        Dim NewChannel As New cImageMonochrome
                        NewChannel.LoadFITS_BuildIn(FileName)
                        LastAddedGUID = DB.AddChannel(NewChannel)

                    Case "PBM", "PGM", "PPM"

                        'Direct load image
                        Dim NewChannel As New cImageMonochrome
                        NewChannel.LoadPortableAnymap(FileName)
                        LastAddedGUID = DB.AddChannel(NewChannel)

                    Case Else

                        MsgBox("Extension <" & Extension & "> not recognized!")

                End Select

                'Display image window
                Dim NewImage As New frmImage
                NewImage.Show(dpMain)
                Dim TabCaption As String = String.Empty
                TabCaption &= System.IO.Path.GetFileName(FileName)
                TabCaption &= " <" & System.IO.Path.GetDirectoryName(FileName) & ">"
                NewImage.Text = TabCaption
                NewImage.LoadChannel(LastAddedGUID)

            End If

        Next FileName

    End Sub

    Private Sub MainForm_DragDrop(sender As Object, e As DragEventArgs) Handles tsMain.DragDrop, dpMain.DragDrop

        Dim FilePaths() As String

        ' Read paths of all files contained in the drop
        FilePaths = CType(e.Data.GetData(DataFormats.FileDrop), String())

        If FilePaths.Length = 1 Then

            Dim FilePath As String

            ' Read first filename from the filepaths-vector
            FilePath = FilePaths(0).ToUpper

            ' Check if the dropped filetype is allowed
            If SupportedFileFormat(FilePath) Then

                ' ------------ Single Drop ------------
                LoadImage(New String() {FilePaths(0)})

                ' Directory doesn't exist
                Return

            End If

        Else
            ' Unknown file format --> Forbid filedrop
            Return
        End If

    End Sub

    Private Sub MainForm_DragEnter(sender As Object, e As DragEventArgs) Handles tsMain.DragEnter, dpMain.DragEnter

        ' Checks if the detected Drop-Event is a File-Drop
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then

            Dim FilePath As String
            Dim FilePaths() As String

            ' Read paths of all files contained in the drop
            FilePaths = CType(e.Data.GetData(DataFormats.FileDrop), String())

            ' Check number of files contained in the drop

            ' -------------- Single File Drop ----------------
            If FilePaths.Length = 1 Then

                ' Read first filename from the filepaths-vector
                FilePath = FilePaths(0).ToUpper

                ' Check if the dropped filetype is allowed
                If SupportedFileFormat(FilePath) Then
                    ' Config File or Settings File --> Allow filedrop
                    e.Effect = DragDropEffects.Copy
                    Return
                End If
            End If

        End If

    End Sub

    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        RunFinalCode()
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Add recent files handler
        AddHandler tsmiRecent1.Click, AddressOf tsmiRecent_Click
        AddHandler tsmiRecent2.Click, AddressOf tsmiRecent_Click
        AddHandler tsmiRecent3.Click, AddressOf tsmiRecent_Click
        AddHandler tsmiRecent4.Click, AddressOf tsmiRecent_Click
        AddHandler tsmiRecent5.Click, AddressOf tsmiRecent_Click
        AddHandler tsmiRecent6.Click, AddressOf tsmiRecent_Click
        AddHandler tsmiRecent7.Click, AddressOf tsmiRecent_Click
        AddHandler tsmiRecent8.Click, AddressOf tsmiRecent_Click
        AddHandler tsmiRecent9.Click, AddressOf tsmiRecent_Click
        AddHandler tsmiRecent10.Click, AddressOf tsmiRecent_Click

        'Set supported file types
        FileFormats = New Dictionary(Of String(), String)
        With FileFormats
            .Add(New String() {"fit", "fit"}, "Flexible Image Transport System")
            .Add(New String() {"cr2"}, "Canon RAW file")
            .Add(New String() {"nef"}, "Nikon RAW file")
            .Add(New String() {"jpg", "jpeg"}, "JPEG files")
            .Add(New String() {"pbm", "pgm", "ppm"}, "Portable Anymap")
            .Add(New String() {"tif", "tiff"}, "TIF files")
        End With

        'Set title
        Me.Text = "AStro IMage TOol Version from " & (New System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly.Location)).LastWriteTime

        'Init IPP
        IPPStarter.SearchIPP(Reflection.ProcessorArchitecture.IA64, New String() {"ipps", "ippvm"})

        LoadRecentFiles()
        UpdateRecentFiles()

    End Sub

    Private Sub LoadImageFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoadImageFileToolStripMenuItem.Click

        With ofdMain
            .InitialDirectory = "D:\Privat\to_home\21a193ad-5a59-4e2c-8d46-e319f5422a8d"
            .FileName = String.Empty
            .Filter = GetFilterList()
            .FilterIndex = 7
            .Multiselect = True
            If .ShowDialog() <> Windows.Forms.DialogResult.OK Then Exit Sub
        End With

        LoadImage(ofdMain.FileNames)
        AddToRecentFiles(ofdMain.FileNames)

    End Sub

    Private Sub AddToRecentFiles(ByVal FileNames() As String)
        For Each FileName As String In FileNames
            If RecentFiles.Contains(FileName) = False Then
                RecentFiles.Insert(0, FileName)
            End If
        Next FileName
        UpdateRecentFiles()
    End Sub

    Private Sub UpdateRecentFiles()
        'Remove all entries
        For Each Entry As ToolStripItem In tsmRecentFiles.DropDownItems
            Entry.Visible = False
        Next Entry
        'Fill with queued entries
        Dim PeakIdx As Integer = -1
        Dim AllFiles As String() = RecentFiles.ToArray
        For Each Entry As ToolStripItem In tsmRecentFiles.DropDownItems
            PeakIdx += 1
            If PeakIdx <= AllFiles.GetUpperBound(0) Then
                Entry.Text = AllFiles(PeakIdx)
                Entry.Visible = True
            End If
        Next Entry
    End Sub

    '================================================================================
    ' GUI related helper functions
    '================================================================================

    '''<summary>Returns TRUE if the given file format is supported.</summary>
    Private Function SupportedFileFormat(ByVal FileName As String) As Boolean
        Dim Extension As String = System.IO.Path.GetExtension(FileName).ToUpper
        For Each Key As String() In FileFormats.Keys
            For Each Entry As String In Key
                If Extension.ToUpper = "." & Entry.ToUpper Then Return True
            Next Entry
        Next Key
        Return False
    End Function

    '''<summary>Return the file filter list to be used.</summary>
    Private Function GetFilterList() As String
        Dim RetVal As New List(Of String)
        Dim AllExtensions As New List(Of String)
        For Each Entry As String() In FileFormats.Keys
            Dim FilterPattern As New List(Of String)
            For Each Extension As String In Entry
                FilterPattern.Add("*." & Extension)
            Next Extension
            Dim FilterReady As String = Join(FilterPattern.ToArray, ";")
            RetVal.Add(FileFormats(Entry) & " (" & FilterReady & ")|" & FilterReady)
            AllExtensions.AddRange(FilterPattern)
        Next Entry
            RetVal.Add("All supporter formats|" & Join(AllExtensions.ToArray, ";"))
        RetVal.Add("All files(*.*)|*.")
        Return Join(RetVal.ToArray, "|")
    End Function

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        RunFinalCode()
        End
    End Sub

    Private Sub OpenEXEPathToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenEXEPathToolStripMenuItem.Click
        System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location))
    End Sub

    Private Sub NewProcessorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewProcessorToolStripMenuItem.Click
        Dim Process As New frmProcess
        Process.Show(dpMain, WeifenLuo.WinFormsUI.Docking.DockState.DockLeft)
    End Sub

    Private Sub tUpdateStatus_Tick(sender As Object, e As EventArgs) Handles tUpdateStatus.Tick
        tsslMain.Text = DB.Log.LastEntry
    End Sub

    Private Sub tsslMain_DoubleClick(sender As Object, e As EventArgs) Handles tsslMain.DoubleClick, ssMain.DoubleClick
        DB.Log.ShowLog()
    End Sub

    Private Sub OnlyUseCSharpFitsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OnlyUseCSharpFitsToolStripMenuItem.Click

        'Dim ImageData(,) As Double = {}
        'Dim TestFits_001 As String = "F:\Astro\Test-Bildmaterial\5b37113b-34be-4488-81e7-5b27736ebfce\sii 10 x 1800.fit"

        'For Each FileName As String In New String() {TestFits_001}
        '    'Load passed FITS file
        '    Dim Fits As New nom.tam.fits.Fits(FileName)
        '    Dim MainHDU As nom.tam.fits.BasicHDU = Fits.GetHDU(0)

        '    'Pass data matrix to the image data
        '    Dim PixelFormat As Integer = CInt(MainHDU.Header.FindCard("BITPIX").Value)
        '    'Dim Dim1 As Integer = CInt(MainHDU.Header.FindCard("NAXIS1").Value)
        '    'Dim Dim2 As Integer = CInt(MainHDU.Header.FindCard("NAXIS2").Value)

        '    'Single-precision
        '    Dim KernelIn As Object = MainHDU.Kernel
        '    Dim RawData As Array() = CType(KernelIn, Array())
        '    Dim Ptr As Integer = 0
        '    For Dim2Idx As Integer = 0 To RawData.GetUpperBound(0)
        '        Dim OneLine As Single() = CType(RawData(Dim2Idx), Single())
        '        If Dim2Idx = 0 Then ReDim ImageData(OneLine.GetUpperBound(0), RawData.GetUpperBound(0))
        '        For Dim1Idx As Integer = 0 To OneLine.GetUpperBound(0)
        '            ImageData(Dim1Idx, Dim2Idx) = OneLine(Dim1Idx)
        '            Ptr += 1
        '        Next Dim1Idx
        '    Next Dim2Idx
        'Next FileName

        'MsgBox("Loaded.")

    End Sub

    Private Sub TestLookupTableToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TestLookupTableToolStripMenuItem.Click

        Dim Width As Integer = 6000
        Dim Height As Integer = 4000
        Dim DoubleRange As Double = 1000000000.0
        Dim Data_Double(,) As Double : ReDim Data_Double(Width, Height)
        Dim Data_UInt16(,) As UInt16 : ReDim Data_UInt16(Width, Height)
        Dim DataOut1(,) As Byte : ReDim DataOut1(Width, Height)
        Dim DataOut2(,) As Byte : ReDim DataOut2(Width, Height)
        Dim RndGen As New Random

        Dim Time1 As Long
        Dim Time2 As Long
        Dim Time3 As Long

        'Build test data
        For Idx1 As Integer = 0 To Data_UInt16.GetUpperBound(0)
            For Idx2 As Integer = 0 To Data_UInt16.GetUpperBound(1)
                Data_Double(Idx1, Idx2) = (RndGen.NextDouble() * 2 * DoubleRange) - DoubleRange
                Data_UInt16(Idx1, Idx2) = CUShort(RndGen.Next(0, UInt16.MaxValue - 1))
            Next Idx2
        Next Idx1

        'Build LUT
        DB.Log.Tic("Building LUT ...")
        Dim LUT As New SortedDictionary(Of UInt32, Byte)
        For Idx As UInt32 = 0 To UInt16.MaxValue
            LUT.Add(Idx, CByte(Rnd() * 254))
        Next Idx
        DB.Log.Toc()

        'Build display data
        DB.Log.Tic("Calculating LUT-based image ...")
        For Idx1 As Integer = 0 To Data_UInt16.GetUpperBound(0)
            For Idx2 As Integer = 0 To Data_UInt16.GetUpperBound(1)
                DataOut1(Idx1, Idx2) = LUT(Data_UInt16(Idx1, Idx2))
            Next Idx2
        Next Idx1
        DB.Log.Toc()

        '================================================================================
        'Mode2: Multi-thread
        DB.Log.Tic("Calculating LUT-based image multi-threaded...")
        Dim LUTRun As New cLUT_MultiThread
        LUTRun.DataIn = Data_UInt16
        LUTRun.DataOut = DataOut2
        LUTRun.Process(LUT)
        DB.Log.Toc()

        '================================================================================
        'Mode3: Intel IPP and array access
        DB.Log.Tic("Running IPP-based calculation")
        Dim Data_short(,) As Short : ReDim Data_short(Width, Height)
        DB.IPP.Convert(Data_Double, Data_short, cIntelIPP.IppRoundMode.ippRndNear, CInt(1))
        DB.Log.Toc()

        '================================================================================

        'Check
        Dim OK As Boolean = True
        For Idx1 As Integer = 0 To Data_UInt16.GetUpperBound(0)
            For Idx2 As Integer = 0 To Data_UInt16.GetUpperBound(1)
                If DataOut1(Idx1, Idx2) <> DataOut2(Idx1, Idx2) Then
                    OK = False
                    Exit For
                End If
            Next Idx2
            If OK = False Then Exit For
        Next Idx1

        Debug.Print("Sorted dictionary: " & Time1.ToString.Trim)
        Debug.Print("Multi-thread: " & Time2.ToString.Trim)
        Debug.Print("Intel IPP and array access: " & Time3.ToString.Trim)

        MsgBox(OK)

    End Sub

    Private Sub tsmiGenerateTestImage_Click(sender As Object, e As EventArgs) Handles tsmiGenerateTestImage.Click

        Dim TestImageWidth As Integer = 1050
        Dim TestImageHeight As Integer = 700

        Dim RedChannel As New cImageMonochrome(TestImageWidth, TestImageHeight)

        'Set pixel
        For Idx As Integer = 0 To (TestImageHeight \ 2) - 1
            RedChannel.ImageData(Idx, Idx) = 255
            RedChannel.ImageData(TestImageWidth - 1 - Idx, Idx) = 120
        Next Idx

        'Display image
        Dim NewImage As New frmImage
        NewImage.Show(dpMain)
        NewImage.Text = "Test image"
        NewImage.LoadChannel(DB.AddChannel(RedChannel))

    End Sub

    Private Sub RunFinalCode()
        'Store recent files
        If System.IO.File.Exists(INIFile) Then System.IO.File.Delete(INIFile)
        Dim INIContent As New List(Of String)
        INIContent.Add("[Recent]")
        For Each Value As String In RecentFiles.ToArray
            INIContent.Add(Value)
        Next Value
        System.IO.File.WriteAllLines(INIFile, INIContent.ToArray)
    End Sub

    Private Sub LoadRecentFiles()
        If System.IO.File.Exists(INIFile) = False Then Exit Sub
        Dim Content As String() = System.IO.File.ReadAllLines(INIFile)
        For Idx As Integer = Content.GetUpperBound(0) To 1 Step -1
            RecentFiles.Insert(0, Content(Idx))
        Next Idx
    End Sub

    Private Sub tsmiRecent_Click(sender As Object, e As EventArgs)
        Dim FileName As String = CType(sender, ToolStripItem).Text
        LoadImage(New String() {FileName})
    End Sub

    Private Sub GrayscaleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GrayscaleToolStripMenuItem.Click

        Dim EqualPixel As Integer = 4
        Dim TestImageWidth As Integer = 256 * EqualPixel
        Dim TestImageHeight As Integer = 256

        Dim RedChannel As New cImageMonochrome(TestImageWidth, TestImageHeight)

        'Set pixel
        Dim CurrentGray As Integer = 0
        For Idx As Integer = 0 To TestImageWidth - 1 Step EqualPixel
            For Idx1 As Integer = 0 To TestImageHeight - 1
                For Repeat As Integer = 0 To EqualPixel - 1
                    RedChannel.ImageData(Idx + Repeat, Idx1) = CByte(CurrentGray)
                Next Repeat
            Next Idx1
            CurrentGray += 1
        Next Idx

        'Display image
        Dim NewImage As New frmImage
        NewImage.Show(dpMain)
        NewImage.Text = "Test image"

        Dim NewImageGUID As String = DB.AddChannel(RedChannel)
        NewImage.LoadChannel(NewImageGUID)

        'Save image
        Dim CodecInfo As System.Drawing.Imaging.ImageCodecInfo = GetEncoderInfo(System.Drawing.Imaging.ImageFormat.Png)
        Dim myEncoder As System.Drawing.Imaging.Encoder = System.Drawing.Imaging.Encoder.ColorDepth

        Dim PathToStore As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
        DB.Channels(NewImageGUID).CalculateImageFromData().BitmapToProcess.Save(PathToStore & "\Grayscale.tiff", System.Drawing.Imaging.ImageFormat.Tiff)

    End Sub

    Private Shared Function GetEncoderInfo(ByVal format As System.Drawing.Imaging.ImageFormat) As System.Drawing.Imaging.ImageCodecInfo
        Dim j As Integer = 0
        Dim encoders() As System.Drawing.Imaging.ImageCodecInfo
        encoders = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders()
        While j < encoders.Length
            If encoders(j).FormatID = format.Guid Then
                Return encoders(j)
            End If
            j += 1
        End While
        Return Nothing
    End Function

    Private Sub ShowLogToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowLogToolStripMenuItem.Click
        DB.Log.ShowLog()
    End Sub

End Class