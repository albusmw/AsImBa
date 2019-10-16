Option Explicit On
Option Strict On

'''<summary>Processor for monochromatic images.</summary>
Public Class cImageMonochrome

    Public Enum eBayerChannel
        Gray
        Green0
        Green1
        Red
        Blue
    End Enum

    '''<summary>Position of this image within the DB ChannelList.</summary>
    Public Property ChannelListIdx As Integer
        Get
            Return MyChannelListIdx
        End Get
        Set(value As Integer)
            MyChannelListIdx = value
        End Set
    End Property
    Private MyChannelListIdx As Integer = -1

    '''<summary>File the data where loaded from.</summary>
    Public Property SourceFileName As String
        Get
            Return MySourceFileName
        End Get
        Set(value As String)
            MySourceFileName = value
        End Set
    End Property
    Private MySourceFileName As String = String.Empty

    Private Sub ProcessingMessage(ByVal Level As Integer, ByVal Content As String)
        DB.FireEvent(ChannelListIdx, Level, Content)
    End Sub

    '''<summary>Image data that you can do write / read access to.</summary>
    Public ImageData(,) As Double

    '''<summary>Histographic data.</summary>
    Public HistCalc As New cStatistics

    '''<summary>Width [pixel] in left-right dimension.</summary>
    Public ReadOnly Property Width() As Integer
        Get
            If IsNothing(ImageData) = True Then Return -1
            Return ImageData.GetUpperBound(0) + 1
        End Get
    End Property

    '''<summary>Height [pixel] in top-down dimension.</summary>
    Public ReadOnly Property Height() As Integer
        Get
            If IsNothing(ImageData) = True Then Return 0
            Return ImageData.GetUpperBound(1) + 1
        End Get
    End Property

    '================================================================================
    ' Constructor using only width and hight
    '================================================================================

    Public Sub New()
        ReDim ImageData(0, 0)
    End Sub

    Public Sub New(ByVal Width As Integer, ByVal Height As Integer)
        ReDim ImageData(Width - 1, Height - 1)
    End Sub

    '================================================================================
    ' Constructor using a FITS image
    '================================================================================

    Public Sub LoadFITS_BuildIn(ByVal FITSFileName As String)

        ProcessingMessage(0, "Loading FITS file <" & FITSFileName & ">")

        'Load data and store source file name
        Dim Loader As New cFITSReader
        Loader.ReadIn(FITSFileName, ImageData)
        SourceFileName = FITSFileName

        'Calculate statistics
        CalculateStatistics()

        ProcessingMessage(0, "Loading FITS file done.")

    End Sub

    '================================================================================
    ' Constructor using a PGM image (output for RAW files from DCRaw)
    '================================================================================

    Public Sub LoadJPEGData(ByVal FileName As String)

        ProcessingMessage(0, "Loading JPEG file <" & FileName & ">")

        Dim LoadedImage As Bitmap = CType(System.Drawing.Bitmap.FromFile(FileName), Bitmap)
        Dim LoadedLockedImage As New cLockBitmap(LoadedImage.Width, LoadedImage.Height)
        LoadedLockedImage.BitmapToProcess = LoadedImage
        LoadedLockedImage.LockBits()

        ReDim ImageData(LoadedImage.Width - 1, LoadedImage.Height - 1)

        'Extract the "brightness" channel
        For Idx1 As Integer = 0 To LoadedImage.Width - 1
            For Idx2 As Integer = 0 To LoadedImage.Height - 1
                ImageData(Idx1, Idx2) = LoadedLockedImage.GetPixel(Idx1, Idx2).GetBrightness
            Next Idx2
        Next Idx1

        MySourceFileName = FileName
        CalculateStatistics()

        ProcessingMessage(0, "Loading JPEG file done.")

    End Sub

    Public Sub LoadTIFFData(ByVal FileName As String)

        ProcessingMessage(0, "Loading TIFF file <" & FileName & ">")

        'Special TIFF loader
        Dim TifDec As New System.Windows.Media.Imaging.TiffBitmapDecoder(New Uri(FileName), Windows.Media.Imaging.BitmapCreateOptions.PreservePixelFormat, Windows.Media.Imaging.BitmapCacheOption.OnLoad)
        Dim BmpFrame As System.Windows.Media.Imaging.BitmapFrame = TifDec.Frames(0)
        Dim Data(BmpFrame.PixelWidth * BmpFrame.PixelHeight - 1) As UShort
        BmpFrame.CopyPixels(Data, 2 * BmpFrame.PixelWidth, 0)

        'Classical old routine
        'Dim LoadedImage As Bitmap = CType(System.Drawing.Bitmap.FromFile(FileName), Bitmap)
        'Dim LoadedLockedImage As New cLockBitmap(LoadedImage.Width, LoadedImage.Height)
        'LoadedLockedImage.BitmapToProcess = LoadedImage
        'LoadedLockedImage.LockBits()

        ReDim ImageData(BmpFrame.PixelWidth - 1, BmpFrame.PixelHeight - 1)

        'Extract the "brightness" channel
        Dim ReadPtr As Integer = 0
        For Idx1 As Integer = 0 To BmpFrame.PixelHeight - 1
            For Idx2 As Integer = 0 To BmpFrame.PixelWidth - 1
                ImageData(Idx2, Idx1) = Data(ReadPtr)
                ReadPtr += 1
            Next Idx2
        Next Idx1

        SourceFileName = FileName
        CalculateStatistics()

        ProcessingMessage(0, "Loading TIFF file done.")

    End Sub

    Public Sub LoadPortableAnymap(ByVal FileName As String)
        LoadPortableAnymap(FileName, eBayerChannel.Gray)
    End Sub

    ''' <summary>Load a PGM / PBM / PPM file.</summary>
    ''' <param name="FileName">File name.</param>
    ''' <param name="BayerChannel">Color channel to load (or gray for a simple gray-scale image).</param>
    Public Sub LoadPortableAnymap(ByVal FileName As String, ByVal BayerChannel As eBayerChannel)

        ProcessingMessage(0, "Loading PGM file <" & FileName & ">")

        'Decode information
        LoadPortableAnyMapNew(FileName)

        'Read the header data
        'Dim Header As New IO.StreamReader(FileName)
        'Dim FirstLine As String = Header.ReadLine
        'Dim SecondLine As String = Header.ReadLine
        'Dim ThirdLine As String = Header.ReadLine

        ''Calculate image size and create inner storage
        'Dim Size As String() = Split(SecondLine, " ")
        'Dim ArrayWidth As Integer = CInt(Size(0))
        'Dim ArrayHeight As Integer = CInt(Size(1))
        'ReDim ImageData(ArrayWidth \ 2, ArrayHeight \ 2)

        'Dim AllData As Byte() = System.IO.File.ReadAllBytes(FileName)
        'Dim StartOffset As Long = AllData.Length - (ArrayWidth * ArrayHeight)

        Select Case BayerChannel

            Case eBayerChannel.Gray



            Case eBayerChannel.Green0
                'For BayerPosY As Integer = 0 To ArrayHeight - 1 Step 2
                '    For BayerPosX As Integer = 0 To ArrayWidth - 1 Step 2
                '        Dim BayerOffset As Integer = CInt((BayerPosY * ArrayWidth) + BayerPosX)
                '        Dim OffsetInFileData As Integer = CInt(StartOffset + BayerOffset)
                '        'Calculate color of the Bayer matrix
                '        SetPixel(BayerPosX \ 2, BayerPosY \ 2, AllData(OffsetInFileData))
                '    Next BayerPosX
                'Next BayerPosY
        End Select

        'Header.Close()

        SourceFileName = FileName
        CalculateStatistics()

        ProcessingMessage(0, "Loading PGM file done.")

    End Sub

    Private Function LoadPortableAnyMapNew(ByRef FileName As String) As Boolean

        Dim File As New IO.FileStream(FileName, IO.FileMode.Open, IO.FileAccess.Read)
        Dim Header As New IO.BinaryReader(File)
        Dim Whitespaces() As Byte = {&H20, &H9, &HD, &HA}

        Dim ASCIIMode As Boolean = False
        Dim BitPerValue As Integer = 0

        'First byte must be a "P"
        If Header.ReadChar <> "P" Then Return False

        'The next ASCII number indicates the format
        Dim MagicNumber As Integer = CInt(Header.ReadChar.ToString)
        Select Case MagicNumber
            Case 1
                'Portable Bitmap  ASCII
                ASCIIMode = True : BitPerValue = 1
            Case 2
                'Portable Graymap ASCII
                ASCIIMode = True : BitPerValue = 8      '-> may get 16 if indicated with values > 255
            Case 3
                'Portable Pixmap  ASCII
                ASCIIMode = True : BitPerValue = 24      '-> may get 48 if indicated with values > 255
            Case 4
                'Portable Bitmap  Binary
                ASCIIMode = False : BitPerValue = 1
            Case 5
                'Portable Graymap Binary
                ASCIIMode = False : BitPerValue = 8      '-> may get 16 if indicated with values > 255
            Case 6
                'Portable Pixmap  Binary
                ASCIIMode = False : BitPerValue = 24      '-> may get 48 if indicated with values > 255
            Case 7
                'Portable Anymap
                ASCIIMode = False
            Case Else
                'Invalid
                Return False
        End Select

        'Read width (ASCII coded)
        Dim Width As String = String.Empty
        Do
            Dim ReadIn As Byte = Header.ReadByte
            If Whitespaces.Contains(ReadIn) = True Then
                If Width.Length > 0 Then Exit Do
            Else
                Width &= System.Text.ASCIIEncoding.ASCII.GetString(New Byte() {ReadIn})
            End If
        Loop Until File.Position = File.Length - 1
        Dim ArrayWidth As Integer = CInt(Width)

        'Read height (ASCII coded)
        Dim Height As String = String.Empty
        Do
            Dim ReadIn As Byte = Header.ReadByte
            If Whitespaces.Contains(ReadIn) = True Then
                If Height.Length > 0 Then Exit Do
            Else
                Height &= System.Text.ASCIIEncoding.ASCII.GetString(New Byte() {ReadIn})
            End If
        Loop Until File.Position = File.Length - 1
        Dim ArrayHeight As Integer = CInt(Height)

        'Read maximum value
        Select Case MagicNumber
            Case 1, 4
                'Not available
            Case 2, 3, 5, 6
                Dim MaxValueText As String = String.Empty
                Do
                    Dim ReadIn As Byte = Header.ReadByte
                    If Whitespaces.Contains(ReadIn) = True Then
                        If MaxValueText.Length > 0 Then Exit Do
                    Else
                        MaxValueText &= System.Text.ASCIIEncoding.ASCII.GetString(New Byte() {ReadIn})
                    End If
                Loop Until File.Position = File.Length - 1
                If CInt(MaxValueText) > 255 Then BitPerValue *= 2
        End Select

        'Move to file start
        Dim ImageBytes As Long = (ArrayWidth * ArrayWidth) * (BitPerValue \ 8)
        File.Seek(File.Length - ImageBytes, IO.SeekOrigin.Begin)

        'Do
        '    Dim ReadIn As Byte = Header.ReadByte
        '    If Whitespaces.Contains(ReadIn) = False Then Exit Do
        'Loop Until File.Position = File.Length - 1

        'Read
        ReDim ImageData(ArrayWidth - 1, ArrayWidth - 1)
        Select Case MagicNumber
            Case 5
                Select Case BitPerValue
                    Case 8
                        For BayerPosY As Integer = 0 To ArrayHeight - 1
                            For BayerPosX As Integer = 0 To ArrayWidth - 1
                                SetPixel(BayerPosX, BayerPosY, Header.ReadByte)
                            Next BayerPosX
                        Next BayerPosY
                    Case 16
                        For BayerPosY As Integer = 0 To ArrayHeight - 1
                            For BayerPosX As Integer = 0 To ArrayWidth - 1
                                'Reading UInt16 does not read in the correct byte order ...
                                Dim Byte1 As Byte = Header.ReadByte
                                Dim Byte2 As Byte = Header.ReadByte
                                SetPixel(BayerPosX, BayerPosY, BitConverter.ToUInt16(New Byte() {Byte2, Byte1}, 0))
                            Next BayerPosX
                        Next BayerPosY
                End Select
        End Select

        Header.Close()

        Return True

    End Function

    Public Sub Save(ByVal Filename As String)
        Dim BMP As Drawing.Bitmap = GetLockBitmap(cColorMaps.eMaps.None).BitmapToProcess
        BMP.Save(Filename, System.Drawing.Imaging.ImageFormat.Bmp)
    End Sub

    Public Function GetPeak(ByRef Peak_X As Integer, ByRef Peak_Y As Integer) As Double
        Dim Peak As Double = Double.MinValue
        Peak_X = -1
        Peak_Y = -1
        For X As Integer = 0 To ImageData.GetUpperBound(0)
            For Y As Integer = 0 To ImageData.GetUpperBound(1)
                If ImageData(X, Y) > Peak Then
                    Peak = ImageData(X, Y)
                    Peak_X = X
                    Peak_Y = Y
                End If
            Next Y
        Next X
        Return Peak
    End Function

    '================================================================================
    ' Update statistics
    '================================================================================

    Public Sub CalculateStatistics()

        'MyOriginal_RangeMin = Double.NaN
        'MyOriginal_RangeMax = Double.NaN
        'IntelIPP.MinMax(ImageData, MyOriginal_RangeMin, MyOriginal_RangeMax)
        'MathEx.MaxMinIgnoreNAN(ImageData, MyOriginal_RangeMin, MyOriginal_RangeMax)

        'Calculate histogram
        HistCalc.Calculate(ImageData)

        ProcessingMessage(1, "Load done, image is " & (ImageData.GetUpperBound(0) + 1).ToString.Trim & " x " & (ImageData.GetUpperBound(1) + 1).ToString.Trim & " pixel.")
        ProcessingMessage(2, "Range: " & HistCalc.Minimum.ToString.Trim & " ... " & HistCalc.Maximum.ToString.Trim)
        ProcessingMessage(2, "Individual values: " & HistCalc.DifferentValues.ToString.Trim)

    End Sub

    

    '================================================================================
    ' Init with defined color
    '================================================================================

    '''<summary>Init the picture data with the given value for all pixel.</summary>
    Public Sub Init(ByVal Value As Double)
        For IdxX As Integer = 0 To Me.Width - 1
            For IdxY As Integer = 0 To Me.Height - 1
                ImageData(IdxX, IdxY) = Value
            Next IdxY
        Next IdxX
    End Sub

    '================================================================================
    ' Set a certain pixel (area) to the given value
    '================================================================================

    '''<summary>Set a given pixel position to the given value.</summary>
    Public Sub SetPixel(ByVal X As Integer, ByVal Y As Integer, ByVal Value As Double)
        If X >= 0 And X <= ImageData.GetUpperBound(0) Then
            If Y >= 0 And Y <= ImageData.GetUpperBound(1) Then
                ImageData(X, Y) = Value
            End If
        End If
    End Sub

    '================================================================================
    ' Convert to cLockBitmap
    '================================================================================

    Public Function GetFixed() As Byte(,)

        'Calculate y = a*x + b in double precision to scale data to full short range for conversion
        Dim A As Double : Dim B As Double
        Dim ScaledDouble(,) As Double = ExpandAndCopy(Short.MinValue, Short.MaxValue, A, B)
        Dim Orig_Min As Double : Dim Orig_Max As Double : DB.IPP.MinMax(ImageData, Orig_Min, Orig_Max)

        'Convert the data to 16-bit fixed point
        Dim RetVal_short(,) As Short = {}
        DB.IPP.Convert(ScaledDouble, RetVal_short, cIntelIPP.IppRoundMode.ippRndNear, 0)

        'Create LUT
        Dim LUT(65535) As Byte
        For Idx As Integer = 0 To LUT.GetUpperBound(0)
            Dim DoubleVal As Double = Orig_Min + ((Idx / LUT.GetUpperBound(0)) * Idx)
            If DoubleVal > 0 And DoubleVal < 60 Then
                LUT(Idx) = CByte(Math.Floor(Idx / 256))
            Else
                LUT(Idx) = 0
            End If
        Next Idx

        'Apply LUT
        Dim ZeroOffset As Integer = -1 * Short.MinValue
        Dim RetVal(Width - 1, Height - 1) As Byte
        For X As Integer = 0 To Width - 1
            For Y As Integer = 0 To Height - 1
                RetVal(X, Y) = LUT(RetVal_short(X, Y) + ZeroOffset)
            Next Y
        Next X

        Return RetVal

    End Function

    '================================================================================
    ' Convert to cLockBitmap
    '================================================================================

    '''<summary>Get an color-mapped bitmap image from the given image data.</summary>
    Public Function CalculateImageFromData() As cLockBitmap

        Dim RunWithNewMethod As Boolean = False

        'Generate output image
        Dim OutputImage As New cLockBitmap(Width, Height)

        Dim Invalid_R As Byte = Config.InvalidColor.R
        Dim Invalid_G As Byte = Config.InvalidColor.G
        Dim Invalid_B As Byte = Config.InvalidColor.B

        'Auto-strech the LUT to the min and max in the image
        ' Color maps take values between 0 and 255
        Dim LinOffset As Double = -HistCalc.Minimum
        Dim LinScale As Double = 255 / (HistCalc.Maximum - HistCalc.Minimum)

        'Build a LUT for all colors present in the picture
        DB.Log.Tic("Generating LUT for each pixel value in the image ...")
        Dim LUT As New Dictionary(Of Double, Color)
        For Each Entry As Double In HistCalc.Histogram.Keys
            Dim Scaled As Double = (Entry + LinOffset) * LinScale
            LUT.Add(Entry, cColorMaps.None(Scaled))
        Next Entry
        DB.Log.Toc()

        'Create the image
        DB.Log.Tic("Locking image ...")
        OutputImage.LockBits()
        DB.Log.Toc()

        DB.Log.Tic("Applying LUT ...")
        Dim Stride As Integer = OutputImage.BitmapData.Stride
        Dim BytePerPixel As Integer = OutputImage.ColorBytesPerPixel
        Dim YOffset As Integer = 0
        For Y As Integer = 0 To Height - 1
            Dim BaseOffset As Integer = YOffset
            For X As Integer = 0 To Width - 1
                Dim Coloring As Color = LUT(ImageData(X, Y))
                OutputImage.Pixels(BaseOffset) = Coloring.R
                OutputImage.Pixels(BaseOffset + 1) = Coloring.G
                OutputImage.Pixels(BaseOffset + 2) = Coloring.B
                BaseOffset += BytePerPixel
            Next X
            YOffset += Stride
        Next Y
        DB.Log.Toc()

            DB.Log.Tic("Unlocking image ...")
            OutputImage.UnlockBits()
            DB.Log.Toc()

            Return OutputImage

    End Function

    '''<summary>Get an grayscale bitmap image from the given image data.</summary>
    Public Function GetLockBitmap(ByVal ColorMap As cColorMaps.eMaps) As cLockBitmap

        Dim Coloring As Color

        DB.Log.Tic("GetLockBitmap")

        'Generate output image
        Dim OutputImage As New cLockBitmap(Width, Height)
        OutputImage.LockBits()
        Dim BytePerPixel As Integer = OutputImage.ColorBytesPerPixel
        Dim Strides As Integer = OutputImage.BitmapData.Stride
        Select Case ColorMap
            Case cColorMaps.eMaps.None
                Dim YOffset As Integer = 0
                For Y As Integer = 0 To Height - 1
                    Dim BaseOffset As Integer = YOffset
                    For X As Integer = 0 To Width - 1
                        Coloring = cColorMaps.None(ImageData(X, Y))
                        OutputImage.Pixels(BaseOffset) = Coloring.R
                        OutputImage.Pixels(BaseOffset + 1) = Coloring.G
                        OutputImage.Pixels(BaseOffset + 2) = Coloring.B
                        BaseOffset += BytePerPixel
                    Next X
                    YOffset += Strides
                Next Y
            Case cColorMaps.eMaps.Jet
                Dim YOffset As Integer = 0
                For Y As Integer = 0 To Height - 1
                    Dim BaseOffset As Integer = YOffset
                    For X As Integer = 0 To Width - 1
                        Coloring = cColorMaps.Jet(ImageData(X, Y))
                        OutputImage.Pixels(BaseOffset) = Coloring.R
                        OutputImage.Pixels(BaseOffset + 1) = Coloring.G
                        OutputImage.Pixels(BaseOffset + 2) = Coloring.B
                        BaseOffset += BytePerPixel
                    Next X
                    YOffset += Strides
                Next Y
            Case cColorMaps.eMaps.Hot
                Dim YOffset As Integer = 0
                For Y As Integer = 0 To Height - 1
                    Dim BaseOffset As Integer = YOffset
                    For X As Integer = 0 To Width - 1
                        Coloring = cColorMaps.Hot(ImageData(X, Y))
                        OutputImage.Pixels(BaseOffset) = Coloring.R
                        OutputImage.Pixels(BaseOffset + 1) = Coloring.G
                        OutputImage.Pixels(BaseOffset + 2) = Coloring.B
                        BaseOffset += BytePerPixel
                    Next X
                    YOffset += Strides
                Next Y
            Case cColorMaps.eMaps.Bone
                Dim YOffset As Integer = 0
                For Y As Integer = 0 To Height - 1
                    Dim BaseOffset As Integer = YOffset
                    For X As Integer = 0 To Width - 1
                        Coloring = cColorMaps.Bone(ImageData(X, Y))
                        OutputImage.Pixels(BaseOffset) = Coloring.R
                        OutputImage.Pixels(BaseOffset + 1) = Coloring.G
                        OutputImage.Pixels(BaseOffset + 2) = Coloring.B
                        BaseOffset += BytePerPixel
                    Next X
                    YOffset += Strides
                Next Y
        End Select
        OutputImage.UnlockBits()

        DB.Log.Toc()

        Return OutputImage

    End Function

    Public Shared Function GetLockBitmap(ByRef R As cImageMonochrome, ByRef G As cImageMonochrome, ByRef B As cImageMonochrome) As cLockBitmap

        'Generate output image
        Dim OutputImage As New cLockBitmap(R.Width, R.Height)
        OutputImage.LockBits()
        For X As Integer = 0 To R.Width - 1
            For Y As Integer = 0 To R.Height - 1
                OutputImage.SetPixel(X, Y, R.ImageData(X, Y), G.ImageData(X, Y), B.ImageData(X, Y))
            Next Y
        Next X
        OutputImage.UnlockBits()

        Return OutputImage

    End Function

    '================================================================================
    ' Operator +
    '================================================================================

    Public Shared Operator +(ByVal ImageToManipulate As cImageMonochrome, ByVal Offset As Double) As cImageMonochrome
        Dim RetVal As New cImageMonochrome(ImageToManipulate.Width, ImageToManipulate.Height)
        For IdxX As Integer = 0 To RetVal.Width - 1
            For IdxY As Integer = 0 To RetVal.Height - 1
                RetVal.ImageData(IdxX, IdxY) = ImageToManipulate.ImageData(IdxX, IdxY) + Offset
            Next IdxY
        Next IdxX
        Return RetVal
    End Operator

    Public Shared Operator +(ByVal ImageA As cImageMonochrome, ByVal ImageB As cImageMonochrome) As cImageMonochrome
        Dim RetVal As New cImageMonochrome(ImageA.Width, ImageA.Height)
        For IdxX As Integer = 0 To RetVal.Width - 1
            For IdxY As Integer = 0 To RetVal.Height - 1
                RetVal.ImageData(IdxX, IdxY) = ImageA.ImageData(IdxX, IdxY) + ImageB.ImageData(IdxX, IdxY)
            Next IdxY
        Next IdxX
        Return RetVal
    End Operator

    '================================================================================
    ' Operator -
    '================================================================================

    Public Shared Operator -(ByVal Offset As Double, ByVal ImageToManipulate As cImageMonochrome) As cImageMonochrome
        Dim RetVal As New cImageMonochrome(ImageToManipulate.Width, ImageToManipulate.Height)
        For IdxX As Integer = 0 To RetVal.Width - 1
            For IdxY As Integer = 0 To RetVal.Height - 1
                RetVal.ImageData(IdxX, IdxY) = ImageToManipulate.ImageData(IdxX, IdxY) - Offset
            Next IdxY
        Next IdxX
        Return RetVal
    End Operator

    Public Shared Operator -(ByVal ImageA As cImageMonochrome, ByVal ImageB As cImageMonochrome) As cImageMonochrome
        Dim RetVal As New cImageMonochrome(ImageA.Width, ImageA.Height)
        For IdxX As Integer = 0 To RetVal.Width - 1
            For IdxY As Integer = 0 To RetVal.Height - 1
                RetVal.ImageData(IdxX, IdxY) = ImageA.ImageData(IdxX, IdxY) - ImageB.ImageData(IdxX, IdxY)
            Next IdxY
        Next IdxX
        Return RetVal
    End Operator

    '================================================================================
    ' Operator *
    '================================================================================

    Public Shared Operator *(ByVal ImageA As cImageMonochrome, ByVal ImageB As cImageMonochrome) As cImageMonochrome
        Dim RetVal As New cImageMonochrome(ImageA.Width, ImageA.Height)
        For IdxX As Integer = 0 To RetVal.Width - 1
            For IdxY As Integer = 0 To RetVal.Height - 1
                RetVal.ImageData(IdxX, IdxY) = ImageA.ImageData(IdxX, IdxY) * ImageB.ImageData(IdxX, IdxY)
            Next IdxY
        Next IdxX
        Return RetVal
    End Operator

    Public Shared Operator *(ByVal Offset As Double, ByVal ImageToManipulate As cImageMonochrome) As cImageMonochrome
        Dim RetVal As New cImageMonochrome(ImageToManipulate.Width, ImageToManipulate.Height)
        For IdxX As Integer = 0 To RetVal.Width - 1
            For IdxY As Integer = 0 To RetVal.Height - 1
                RetVal.ImageData(IdxX, IdxY) = ImageToManipulate.ImageData(IdxX, IdxY) * Offset
            Next IdxY
        Next IdxX
        Return RetVal
    End Operator

    '================================================================================
    ' Operator ^
    '================================================================================

    Public Shared Operator ^(ByVal ImageA As cImageMonochrome, ByVal Exponent As Double) As cImageMonochrome
        Dim RetVal As New cImageMonochrome(ImageA.Width, ImageA.Height)
        For IdxX As Integer = 0 To RetVal.Width - 1
            For IdxY As Integer = 0 To RetVal.Height - 1
                RetVal.ImageData(IdxX, IdxY) = ImageA.ImageData(IdxX, IdxY) ^ Exponent
            Next IdxY
        Next IdxX
        Return RetVal
    End Operator

    '================================================================================
    ' Combined operation A*x + B
    '================================================================================

    '''<summary>Calculate A*x + B in-place.</summary>
    Public Sub Scale(ByVal A As Double, ByVal B As Double)
        For X As Integer = 0 To ImageData.GetUpperBound(0)
            For Y As Integer = 0 To ImageData.GetUpperBound(1)
                ImageData(X, Y) = (A * ImageData(X, Y)) + B
            Next Y
        Next X
    End Sub

    '================================================================================
    ' Expand
    '================================================================================

    '''<summary>Get the values of A and B so that y=a*x+b results in NewMin and NewMax.</summary>
    '''<param name="NewMin">New minimum (included).</param>
    '''<param name="NewMax">New maximum (included).</param>
    Public Sub GetAB(ByVal NewMin As Double, ByVal NewMax As Double, ByRef A As Double, ByRef B As Double)
        Dim OldMin As Double : Dim OldMax As Double
        DB.IPP.MinMax(ImageData, OldMin, OldMax)
        'MathEx.MaxMinIgnoreNAN(ImageData, OldMin, OldMax)
        A = (NewMin - NewMax) / (OldMin - OldMax)
        B = NewMin - (A * OldMin)
    End Sub


    '''<summary>Expand to the given maximum and minimum values.</summary>
    '''<param name="NewMin">New minimum (included).</param>
    '''<param name="NewMax">New maximum (included).</param>
    Public Sub Expand(ByRef NewMin As Double, ByRef NewMax As Double)
        Dim A As Double : Dim B As Double
        GetAB(NewMin, NewMax, A, B)
        ApplyAB(ImageData, A, B)
    End Sub

    '''<summary>Apply y=a*x+b..</summary>
    Public Sub ApplyAB(ByRef Data(,) As Double, ByRef A As Double, ByRef B As Double)
        DB.IPP.MulC(Data, A)
        DB.IPP.AddC(Data, B)
    End Sub

    '''<summary>Expand by y=a*x+b and return the new calculated matrix.</summary>
    Public Function ApplyABAndCopy(ByVal A As Double, ByVal B As Double) As Double(,)
        Dim RetVal(,) As Double = DB.IPP.Copy(ImageData)
        ApplyAB(RetVal, A, B)
        Return RetVal
    End Function

    '''<summary>Expand to the given maximum and minimum values and return the new calculated matrix.</summary>
    '''<param name="NewMin">New minimum (included).</param>
    '''<param name="NewMax">New maximum (included).</param>
    Public Function ExpandAndCopy(ByRef NewMin As Double, ByRef NewMax As Double, ByRef A As Double, ByRef B As Double) As Double(,)
        Dim RetVal(,) As Double = DB.IPP.Copy(ImageData)
        Dim OldMin As Double : Dim OldMax As Double
        DB.IPP.MinMax(RetVal, OldMin, OldMax)
        MathEx.MaxMinIgnoreNAN(ImageData, OldMin, OldMax)
        A = (NewMin - NewMax) / (OldMin - OldMax)
        B = NewMin - (A * OldMin)
        ApplyAB(RetVal, A, B)
        Return RetVal
    End Function

    '================================================================================
    ' Invert
    '================================================================================

    '''<summary>Invert the data within the present range.</summary>
    Public Sub Invert()
        Dim OldMin As Double : Dim OldMax As Double
        DB.IPP.MinMax(ImageData, OldMin, OldMax)
        MathEx.MaxMinIgnoreNAN(ImageData, OldMin, OldMax)
        DB.IPP.MulC(ImageData, -1)
        DB.IPP.AddC(ImageData, OldMax + OldMin)
    End Sub

    '================================================================================
    ' Clip
    '================================================================================

    '''<summary>Clip the image data to the given lower and upper max value.</summary>
    '''<param name="NewMin">If image data is below NewMin NewMin is set.</param>
    '''<param name="NewMax">If image data is above NewMax NewMax is set.</param>
    Public Sub Clip(ByRef NewMin As Double, ByRef NewMax As Double)
        For IdxX As Integer = 0 To ImageData.GetUpperBound(0)
            For IdxY As Integer = 0 To ImageData.GetUpperBound(1)
                If ImageData(IdxX, IdxY) < NewMin Then ImageData(IdxX, IdxY) = NewMin
                If ImageData(IdxX, IdxY) > NewMax Then ImageData(IdxX, IdxY) = NewMax
            Next IdxY
        Next IdxX
    End Sub

    '================================================================================
    ' Clip
    '================================================================================

    '''<summary>Clip the image data to the given SetTo value if the data are below the given limit.</summary>
    Public Sub SetBelowTo(ByRef Limit As Double, ByRef SetTo As Double)
        For IdxX As Integer = 0 To ImageData.GetUpperBound(0)
            For IdxY As Integer = 0 To ImageData.GetUpperBound(1)
                If ImageData(IdxX, IdxY) < Limit Then ImageData(IdxX, IdxY) = SetTo
            Next IdxY
        Next IdxX
    End Sub

    '================================================================================
    ' Rotate & flip
    '================================================================================

    Public Sub RotateCW()
        Dim NewImage(ImageData.GetUpperBound(1), ImageData.GetUpperBound(0)) As Double
        For IdxX As Integer = 0 To ImageData.GetUpperBound(0)
            For IdxY As Integer = 0 To ImageData.GetUpperBound(1)
                NewImage(ImageData.GetUpperBound(1) - IdxY, IdxX) = ImageData(IdxX, IdxY)
            Next IdxY
        Next IdxX
        ImageData = NewImage
    End Sub

    Public Sub RotateCCW()
        Dim NewImage(ImageData.GetUpperBound(1), ImageData.GetUpperBound(0)) As Double
        For IdxX As Integer = 0 To ImageData.GetUpperBound(0)
            For IdxY As Integer = 0 To ImageData.GetUpperBound(1)
                NewImage(IdxY, ImageData.GetUpperBound(0) - IdxX) = ImageData(IdxX, IdxY)
            Next IdxY
        Next IdxX
        ImageData = NewImage
    End Sub

    Public Sub FlipLR()
        Dim NewImage(ImageData.GetUpperBound(0), ImageData.GetUpperBound(1)) As Double
        For IdxX As Integer = 0 To ImageData.GetUpperBound(0)
            For IdxY As Integer = 0 To ImageData.GetUpperBound(1)
                NewImage(IdxX, IdxY) = ImageData(ImageData.GetUpperBound(0) - IdxX, IdxY)
            Next IdxY
        Next IdxX
        ImageData = NewImage
    End Sub

    Public Sub FlipTB()
        Dim NewImage(ImageData.GetUpperBound(0), ImageData.GetUpperBound(1)) As Double
        For IdxX As Integer = 0 To ImageData.GetUpperBound(0)
            For IdxY As Integer = 0 To ImageData.GetUpperBound(1)
                NewImage(IdxX, IdxY) = ImageData(IdxX, ImageData.GetUpperBound(1) - IdxY)
            Next IdxY
        Next IdxX
        ImageData = NewImage
    End Sub

    Public Function GetImagePart(ByVal X_left As Integer, ByVal X_right As Integer, ByVal Y_bottom As Integer, ByVal Y_top As Integer) As cImageMonochrome
        If X_left < 0 Then X_left = 0
        If X_right > Width - 1 Then X_right = Width - 1
        If Y_bottom < 0 Then Y_bottom = 0
        If Y_top > Height - 1 Then Y_top = Height - 1
        Dim PartWidth As Integer = (X_right - X_left + 1)
        Dim PartHeight As Integer = (Y_top - Y_bottom + 1)
        If PartWidth < 1 Or PartHeight < 1 Then
            Return New cImageMonochrome(1, 1)
        Else
            Dim ImagePart As New cImageMonochrome(PartWidth, PartHeight)
            Dim CurrentX As Integer = 0
            For Idx1 As Integer = X_left To X_right
                Dim CurrentY As Integer = 0
                For Idx2 As Integer = Y_bottom To Y_top
                    ImagePart.SetPixel(CurrentX, CurrentY, ImageData(Idx1, Idx2))
                    CurrentY += 1
                Next Idx2
                CurrentX += 1
            Next Idx1
            Return ImagePart
        End If
    End Function

    Public Function GetImagePartZoomed(ByVal PixelPerRealPixel As Integer, ByVal ExpandDynamicsMode As cImageStat.eExpandDynamicsMode, ByVal HistExpand_Min As Double, ByVal HistExpand_Max As Double) As cImageMonochrome
        Dim ZoomInAsDisplayed As New cImageMonochrome(Width * PixelPerRealPixel, Height * PixelPerRealPixel)
        Dim X_exp_base As Integer = 0
        For X As Integer = 0 To ImageData.GetUpperBound(0)
            Dim Y_exp_base As Integer = 0
            For Y As Integer = 0 To ImageData.GetUpperBound(1)
                Dim Value As Double = ImageData(X, Y)
                'Get a magnified version for e.g. showing the center-of-mass in the image
                If ExpandDynamicsMode <> cImageStat.eExpandDynamicsMode.DoNotExpand Then Value = 255 * ((Value - HistExpand_Min) / (HistExpand_Max - HistExpand_Min))
                For X_exp As Integer = 0 To PixelPerRealPixel - 1
                    For Y_exp As Integer = 0 To PixelPerRealPixel - 1
                        ZoomInAsDisplayed.SetPixel(X_exp_base + X_exp, Y_exp_base + Y_exp, Value)
                    Next Y_exp
                Next X_exp
                Y_exp_base += PixelPerRealPixel
            Next Y
            X_exp_base += PixelPerRealPixel
        Next X
        Return ZoomInAsDisplayed
    End Function

    '================================================================================
    ' Shift the image by the given values
    '================================================================================

    '''<summary>Shift the pixel.</summary>
    '''<param name="DeltaX">X axis shift: positiv values move the channel to the right, negative values move the channel to the left.</param>
    '''<param name="DeltaY">Y axis shift: positiv values move the channel to the bottom, negative values move the channel to the top.</param>
    Public Sub Shift(ByVal DeltaX As Integer, ByVal DeltaY As Integer)

        Dim Temp As Integer = DeltaX
        DeltaX = DeltaY : DeltaY = Temp

        'TODO:
        ' - Beim Shiften werden die Randbereiche nicht bearbeitet (also wenn nach rechts geschiftet wird bleibt links was stehen, ...)

        Dim NewData(ImageData.GetUpperBound(0), ImageData.GetUpperBound(1)) As Double

        For X As Integer = 0 To NewData.GetUpperBound(0)
            Dim NewX As Integer = X + DeltaX
            If (NewX < 0) Or (NewX > NewData.GetUpperBound(0)) Then
                'Invalid copy position
            Else
                For Y As Integer = 0 To NewData.GetUpperBound(1)
                    Dim NewY As Integer = Y + DeltaY
                    If (NewY < 0) Or (NewY > NewData.GetUpperBound(1)) Then
                        'Invalid copy position
                    Else
                        NewData(NewX, NewY) = ImageData(X, Y)
                    End If
                Next Y
            End If
        Next X

        ImageData = NewData

    End Sub

    '================================================================================
    ' Help function to process the FITS information
    '================================================================================

    'Private Sub GetFITSInformation(ByRef HDU As nom.tam.fits.BasicHDU)
    '    ProcessingMessage(2, "FITS header:")
    '    ProcessingMessage(3, "Author      : " & HDU.Author)
    '    ProcessingMessage(3, "Instrument  : " & HDU.Instrument)
    '    ProcessingMessage(3, "Object      : " & HDU.Object)
    '    ProcessingMessage(3, "BITPIX      : " & HDU.Header.FindCard("BITPIX").Value)
    '    Dim NAxis As Integer = CInt(GetString(HDU, "NAXIS"))
    '    ProcessingMessage(3, "NAXIS       : " & NAxis.ToString.Trim)
    '    ProcessingMessage(3, "NAXIS1      : " & GetString(HDU, "NAXIS1"))
    '    ProcessingMessage(3, "NAXIS2      : " & GetString(HDU, "NAXIS2"))
    '    ProcessingMessage(3, "NAXIS3      : " & GetString(HDU, "NAXIS3"))
    '    ProcessingMessage(2, "------------------------------------------------------------")
    '    'Uncomment to get all header data
    '    'PrintHeader(HDU)
    'End Sub

    'Private Function GetString(ByRef Head As nom.tam.fits.BasicHDU, ByVal KeyWords As String) As String
    '    Dim Card As nom.tam.fits.HeaderCard = Head.Header.FindCard(KeyWords)
    '    If IsNothing(Card) Then
    '        Return "---"
    '    Else
    '        Return Card.Value
    '    End If
    'End Function

    ' '''<summary>Print all header cards.</summary>
    'Private Sub PrintHeader(ByRef Head As nom.tam.fits.BasicHDU)
    '    For Idx As Integer = 0 To Head.Header.NumberOfCards - 1
    '        Debug.Print(" >> " & Idx.ToString.Trim & " -> " & Head.Header.GetCard(Idx))
    '    Next Idx
    'End Sub

End Class
