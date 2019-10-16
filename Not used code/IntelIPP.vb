Option Explicit On
Option Strict On

Partial Public Class IntelIPP

    'Using late binding:
    ' -> https://www.codeproject.com/Articles/1557/Late-binding-on-native-DLLs-with-C

Private Const IPPRoot As String = "C:\Program Files (x86)\IntelSWTools\compilers_and_libraries_2017.4.210\windows\redist\intel64_win\ipp\"
    Private Const IPP_S_DLL As String = IPPRoot & "ipps.dll"
    Private Const IPP_VM_DLL As String = IPPRoot & "ippvm.dll"

#Region "Enums"

    Public Enum IppStatus
        NotSupportedModeErr = -9999
        CpuNotSupportedErr = -9998
        ConvergeErr = -205
        SizeMatchMatrixErr = -204
        CountMatrixErr = -203
        RoiShiftMatrixErr = -202
        ResizeNoOperationErr = -201
        SrcDataErr = -200
        MaxLenHuffCodeErr = -199
        CodeLenTableErr = -198
        FreqTableErr = -197
        IncompleteContextErr = -196
        SingularErr = -195
        SparseErr = -194
        BitOffsetErr = -193
        QPErr = -192
        VLCErr = -191
        RegExpOptionsErr = -190
        RegExpErr = -189
        RegExpMatchLimitErr = -188
        RegExpQuantifierErr = -187
        RegExpGroupingErr = -186
        RegExpBackRefErr = -185
        RegExpChClassErr = -184
        RegExpMetaChErr = -183
        StrideMatrixErr = -182
        CTRSizeErr = -181
        JPEG2KCodeBlockIsNotAttached = -180
        NotPosDefErr = -179
        EphemeralKeyErr = -178
        MessageErr = -177
        ShareKeyErr = -176
        IvalidPublicKey = -175
        IvalidPrivateKey = -174
        OutOfECErr = -173
        ECCInvalidFlagErr = -172
        MP3FrameHeaderErr = -171
        MP3SideInfoErr = -170
        BlockStepErr = -169
        MBStepErr = -168
        AacPrgNumErr = -167
        AacSectCbErr = -166
        AacSfValErr = -164
        AacCoefValErr = -163
        AacMaxSfbErr = -162
        AacPredSfbErr = -161
        AacPlsDataErr = -160
        AacGainCtrErr = -159
        AacSectErr = -158
        AacTnsNumFiltErr = -157
        AacTnsLenErr = -156
        AacTnsOrderErr = -155
        AacTnsCoefResErr = -154
        AacTnsCoefErr = -153
        AacTnsDirectErr = -152
        AacTnsProfileErr = -151
        AacErr = -150
        AacBitOffsetErr = -149
        AacAdtsSyncWordErr = -148
        AacSmplRateIdxErr = -147
        AacWinLenErr = -146
        AacWinGrpErr = -145
        AacWinSeqErr = -144
        AacComWinErr = -143
        AacStereoMaskErr = -142
        AacChanErr = -141
        AacMonoStereoErr = -140
        AacStereoLayerErr = -139
        AacMonoLayerErr = -138
        AacScalableErr = -137
        AacObjTypeErr = -136
        AacWinShapeErr = -135
        AacPcmModeErr = -134
        VLCUsrTblHeaderErr = -133
        VLCUsrTblUnsupportedFmtErr = -132
        VLCUsrTblEscAlgTypeErr = -131
        VLCUsrTblEscCodeLengthErr = -130
        VLCUsrTblCodeLengthErr = -129
        VLCInternalTblErr = -128
        VLCInputDataErr = -127
        VLCAACEscCodeLengthErr = -126
        NoiseRangeErr = -125
        UnderRunErr = -124
        PaddingErr = -123
        CFBSizeErr = -122
        PaddingSchemeErr = -121
        InvalidCryptoKeyErr = -120
        LengthErr = -119
        BadModulusErr = -118
        LPCCalcErr = -117
        RCCalcErr = -116
        IncorrectLSPErr = -115
        NoRootFoundErr = -114
        JPEG2KBadPassNumber = -113
        JPEG2KDamagedCodeBlock = -112
        H263CBPYCodeErr = -111
        H263MCBPCInterCodeErr = -110
        H263MCBPCIntraCodeErr = -109
        NotEvenStepErr = -108
        HistoNofLevelsErr = -107
        LUTNofLevelsErr = -106
        MP4BitOffsetErr = -105
        MP4QPErr = -104
        MP4BlockIdxErr = -103
        MP4BlockTypeErr = -102
        MP4MVCodeErr = -101
        MP4VLCCodeErr = -100
        MP4DCCodeErr = -99
        MP4FcodeErr = -98
        MP4AlignErr = -97
        MP4TempDiffErr = -96
        MP4BlockSizeErr = -95
        MP4ZeroBABErr = -94
        MP4PredDirErr = -93
        MP4BitsPerPixelErr = -92
        MP4VideoCompModeErr = -91
        MP4LinearModeErr = -90
        H263PredModeErr = -83
        H263BlockStepErr = -82
        H263MBStepErr = -81
        H263FrameWidthErr = -80
        H263FrameHeightErr = -79
        H263ExpandPelsErr = -78
        H263PlaneStepErr = -77
        H263QuantErr = -76
        H263MVCodeErr = -75
        H263VLCCodeErr = -74
        H263DCCodeErr = -73
        H263ZigzagLenErr = -72
        FBankFreqErr = -71
        FBankFlagErr = -70
        FBankErr = -69
        NegOccErr = -67
        CdbkFlagErr = -66
        SVDCnvgErr = -65
        JPEGHuffTableErr = -64
        JPEGDCTRangeErr = -63
        JPEGOutOfBufErr = -62
        DrawTextErr = -61
        ChannelOrderErr = -60
        ZeroMaskValuesErr = -59
        QuadErr = -58
        RectErr = -57
        CoeffErr = -56
        NoiseValErr = -55
        DitherLevelsErr = -54
        NumChannelsErr = -53
        COIErr = -52
        DivisorErr = -51
        AlphaTypeErr = -50
        GammaRangeErr = -49
        GrayCoefSumErr = -48
        ChannelErr = -47
        ToneMagnErr = -46
        ToneFreqErr = -45
        TonePhaseErr = -44
        TrnglMagnErr = -43
        TrnglFreqErr = -42
        TrnglPhaseErr = -41
        TrnglAsymErr = -40
        HugeWinErr = -39
        JaehneErr = -38
        StrideErr = -37
        EpsValErr = -36
        WtOffsetErr = -35
        AnchorErr = -34
        MaskSizeErr = -33
        ShiftErr = -32
        SampleFactorErr = -31
        SamplePhaseErr = -30
        FIRMRFactorErr = -29
        FIRMRPhaseErr = -28
        RelFreqErr = -27
        FIRLenErr = -26
        IIROrderErr = -25
        DlyLineIndexErr = -24
        ResizeFactorErr = -23
        InterpolationErr = -22
        MirrorFlipErr = -21
        Moment00ZeroErr = -20
        ThreshNegLevelErr = -19
        ThresholdErr = -18
        ContextMatchErr = -17
        FftFlagErr = -16
        FftOrderErr = -15
        StepErr = -14
        ScaleRangeErr = -13
        DataTypeErr = -12
        OutOfRangeErr = -11
        DivByZeroErr = -10
        MemAllocErr = -9
        NullPtrErr = -8
        RangeErr = -7
        SizeErr = -6
        BadArgErr = -5
        NoMemErr = -4
        SAReservedErr3 = -3
        Err = -2
        SAReservedErr1 = -1
        NoErr = 0
        NoOperation = 1
        MisalignedBuf = 2
        SqrtNegArg = 3
        InvZero = 4
        EvenMedianMaskSize = 5
        DivByZero = 6
        LnZeroArg = 7
        LnNegArg = 8
        NanArg = 9
        JPEGMarker = 10
        ResFloor = 11
        Overflow = 12
        LSFLow = 13
        LSFHigh = 14
        LSFLowAndHigh = 15
        ZeroOcc = 16
        Underflow = 17
        Singularity = 18
        Domain = 19
        NonIntelCpu = 20
        CpuMismatch = 21
        NoIppFunctionFound = 22
        DllNotFoundBestUsed = 23
        NoOperationInDll = 24
        InsufficientEntropy = 25
        OvermuchStrings = 26
        OverlongString = 27
        AffineQuadChanged = 28
        WrongIntersectROI = 29
        WrongIntersectQuad = 30
        SmallerCodebook = 31
        SrcSizeLessExpected = 32
        DstSizeLessExpected = 33
        StreamEnd = 34
        DoubleSize = 35
        NotSupportedCpu = 36
        UnknownCacheSize = 37
        SymKernelExpected = 38
    End Enum

    Public Enum IppCmpOp
        Less = 0
        LessEq = 1
        Eq = 2
        GreaterEq = 3
        Greater = 4
    End Enum

    Public Enum IppBool
        ippFalse = 0
        ippTrue = 1
    End Enum

    Public Enum IppFFTFlag
        IPP_FFT_DIV_FWD_BY_N = 1
        IPP_FFT_DIV_INV_BY_N = 2
        IPP_FFT_DIV_BY_SQRTN = 4
        IPP_FFT_NODIV_BY_ANY = 8
    End Enum

    Public Enum IppHintAlgorithm
        ippAlgHintNone = 0
        ippAlgHintFast = 1
        ippAlgHintAccurate = 2
    End Enum

    Public Enum IppRoundMode
        ippRndZero = 0
        ippRndNear = 1
    End Enum

    Public Enum IppWinType
        ippWinBartlett = 0
        ippWinBlackman = 1
        ippWinHamming = 2
        ippWinHann = 3
        ippWinRect = 4
    End Enum

    Public Enum IppCpuType
        ippCpuUnknown = 0
        ippCpuPP = 1
        ippCpuPMX = 2
        ippCpuPPR = 3
        ippCpuPII = 4
        ippCpuPIII = 5
        ippCpuP4 = 6
        ippCpuP4HT = 7
        ippCpuP4HT2 = 8
        ippCpuCentrino = 9
        ippCpuDS = 10
        ippCpuITP = 16
        ippCpuITP2 = 17
        ippCpuEM64T = 32
        ippCpuNext = 33
        ippCpuSSE = 64
        ippCpuSSE2 = 65
        ippCpuSSE3 = 66
        ippCpuX8664 = 67
    End Enum

    Public Enum IppDataType
        ipp1u = 0
        ipp8u = 1
        ipp8s = 2
        ipp16u = 3
        ipp16s = 4
        ipp16sc = 5
        ipp32u = 6
        ipp32s = 7
        ipp32sc = 8
        ipp32f = 9
        ipp32fc = 10
        ipp64u = 11
        ipp64s = 12
        ipp64sc = 13
        ipp64f = 14
        ipp64fc = 15
    End Enum

    Public Enum IppiMaskSize
        ippMskSize1x3 = 13
        ippMskSize1x5 = 15
        ippMskSize3x1 = 31
        ippMskSize3x3 = 33
        ippMskSize5x1 = 51
        ippMskSize5x5 = 55
    End Enum

#End Region

#Region "Structures"

    <Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential, CharSet:=Runtime.InteropServices.CharSet.Ansi)> _
    Public Structure stIpp16sc
        Public re As Short
        Public im As Short
        Public Sub New(ByVal re As Short, ByVal im As Short)
            Me.re = re
            Me.im = im
        End Sub
    End Structure

    <Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential, CharSet:=Runtime.InteropServices.CharSet.Ansi)> _
    Public Structure stIpp32fc
        Public re As Single
        Public im As Single
        Public Sub New(ByVal re As Single, ByVal im As Single)
            Me.re = re
            Me.im = im
        End Sub
    End Structure

    <Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential, CharSet:=Runtime.InteropServices.CharSet.Ansi)> _
    Public Structure stIppiPoint_32f
        Public x As Single
        Public y As Single
        Public Sub New(ByVal x As Single, ByVal y As Single)
            Me.x = x
            Me.y = y
        End Sub
    End Structure

    <Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential, CharSet:=Runtime.InteropServices.CharSet.Ansi)> _
    Public Structure stIpp64fc
        Public re As Double
        Public im As Double
        Public Sub New(ByVal re As Double, ByVal im As Double)
            Me.re = re
            Me.im = im
        End Sub
    End Structure

    <Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential, CharSet:=Runtime.InteropServices.CharSet.Ansi)> _
    Public Class cIppLibraryVersion
        Public major As Integer
        Public minor As Integer
        Public majorBuild As Integer
        Public build As Integer
        <Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=4)> _
        Public targetCpu As String
        Public Name As IntPtr
        Public Version As IntPtr
        Public BuildDate As IntPtr
    End Class

    <Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential, CharSet:=Runtime.InteropServices.CharSet.Ansi)> _
    Public Structure stIpp8sc
        Public re As SByte
        Public im As SByte
        Public Sub New(ByVal re As SByte, ByVal im As SByte)
            Me.re = re
            Me.im = im
        End Sub
    End Structure

    <Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential, CharSet:=Runtime.InteropServices.CharSet.Ansi)> _
    Public Structure stIppiPoint
        Public x As Integer
        Public y As Integer
        Public Sub New(ByVal x As Integer, ByVal y As Integer)
            Me.x = x
            Me.y = y
        End Sub
    End Structure

    <Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential, CharSet:=Runtime.InteropServices.CharSet.Ansi)> _
    Public Structure stIpp32sc
        Public re As Integer
        Public im As Integer
        Public Sub New(ByVal re As Integer, ByVal im As Integer)
            Me.re = re
            Me.im = im
        End Sub
    End Structure

    <Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential, CharSet:=Runtime.InteropServices.CharSet.Ansi)> _
    Public Structure stIppiSize
        Public width As Integer
        Public height As Integer
        Public Sub New(ByVal width As Integer, ByVal height As Integer)
            Me.width = width
            Me.height = height
        End Sub
    End Structure

    <Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential, CharSet:=Runtime.InteropServices.CharSet.Ansi)> _
    Public Structure stIpp64sc
        Public re As Long
        Public im As Long
        Public Sub New(ByVal re As Long, ByVal im As Long)
            Me.re = re
            Me.im = im
        End Sub
    End Structure

    <Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential, CharSet:=Runtime.InteropServices.CharSet.Ansi)> _
    Public Structure stIppiRect
        Public x As Integer
        Public y As Integer
        Public width As Integer
        Public height As Integer
        Public Sub New(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
            Me.x = x
            Me.y = y
            Me.width = width
            Me.height = height
        End Sub
    End Structure

#End Region

#Region "Helper functions"

    Friend Shared Function GetPtr(Of T)(ByRef Array() As T) As IntPtr
        Try
            Return System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(Array, 0)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Friend Shared Function GetPtr(Of T)(ByRef Array() As T, ByRef Offset As Integer) As IntPtr
        Try
            Return System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(Array, Offset)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Friend Shared Function GetPtr(Of T)(ByRef Array(,) As T) As IntPtr
        Try
            Return System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(Array, 0)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Friend Shared Sub AdjustSize(Of InT, OutT)(ByRef Source() As InT, ByRef Target() As OutT)
        If Source.Length <> Target.Length Then ReDim Target(0 To Source.Length - 1)
    End Sub
    Friend Shared Sub AdjustSize(Of InT, OutT)(ByRef Source(,) As InT, ByRef Target(,) As OutT)
        Dim Adjust As Boolean = False
        If Source.GetUpperBound(0) <> Target.GetUpperBound(0) Then Adjust = True
        If Source.GetUpperBound(1) <> Target.GetUpperBound(1) Then Adjust = True
        If Adjust Then ReDim Target(0 To Source.GetUpperBound(0), 0 To Source.GetUpperBound(1))
    End Sub
    Friend Shared Sub AdjustSize(Of OutT)(ByRef SourceLength As Integer, ByRef Target() As OutT)
        If SourceLength <> Target.Length Then ReDim Target(0 To SourceLength - 1)
    End Sub
#End Region

    '================================================================================
    'Convert
    <Runtime.InteropServices.DllImportAttribute(IPP_S_DLL)> Private Shared Function ippsConvert_64f32f(ByVal pSrc As IntPtr, ByVal pDst As IntPtr, ByVal len As Integer) As IppStatus
    End Function
    Public Shared Function Convert(ByRef ArrayIn(,) As Double, ByRef ArrayOut(,) As Single) As IppStatus
        AdjustSize(ArrayIn, ArrayOut)
        Return ippsConvert_64f32f(GetPtr(ArrayIn), GetPtr(ArrayOut), ArrayIn.Length)
    End Function

    '================================================================================
    'Convert
    <Runtime.InteropServices.DllImportAttribute(IPP_S_DLL)> Private Shared Function ippsConvert_32f8u_Sfs(ByVal pSrc As IntPtr, ByVal pDst As IntPtr, ByVal len As Integer, ByVal rndMode As IppRoundMode, ByVal scaleFactor As Integer) As IppStatus
    End Function
    Public Shared Function Convert(ByRef ArrayIn(,) As Single, ByRef ArrayOut(,) As Byte, ByVal RoundMode As IppRoundMode, ByVal ScaleFactor As Integer) As IppStatus
        AdjustSize(ArrayIn, ArrayOut)
        Return ippsConvert_32f8u_Sfs(GetPtr(ArrayIn), GetPtr(ArrayOut), ArrayIn.Length, RoundMode, ScaleFactor)
    End Function

    '================================================================================
    'Convert
    <Runtime.InteropServices.DllImportAttribute(IPP_S_DLL)> Private Shared Function ippsConvert_64f16s_Sfs(ByVal pSrc As IntPtr, ByVal pDst As IntPtr, ByVal len As Integer, ByVal rndMode As IppRoundMode, ByVal scaleFactor As Integer) As IppStatus
    End Function
    Public Shared Function Convert(ByRef ArrayIn(,) As Double, ByRef ArrayOut(,) As Short, ByVal RoundMode As IppRoundMode, ByVal ScaleFactor As Integer) As IppStatus
        AdjustSize(ArrayIn, ArrayOut)
        Return ippsConvert_64f16s_Sfs(GetPtr(ArrayIn), GetPtr(ArrayOut), ArrayIn.Length, RoundMode, ScaleFactor)
    End Function

    '================================================================================
    'AddC
    <Runtime.InteropServices.DllImportAttribute(IPP_S_DLL)> Private Shared Function ippsAddC_32f_I(ByVal val As Single, ByVal pSrcDst As IntPtr, ByVal len As Integer) As IppStatus
    End Function
    Public Shared Function AddC(ByRef Array(,) As Single, ByRef ScaleFactor As Single) As IppStatus
        Return ippsAddC_32f_I(ScaleFactor, GetPtr(Array), Array.Length)
    End Function

    '================================================================================
    'AddC
    <Runtime.InteropServices.DllImportAttribute(IPP_S_DLL)> Private Shared Function ippsAddC_64f_I(ByVal val As Double, ByVal pSrcDst As IntPtr, ByVal len As Integer) As IppStatus
    End Function
    Public Shared Function AddC(ByRef Array(,) As Double, ByRef ScaleFactor As Double) As IppStatus
        Return ippsAddC_64f_I(ScaleFactor, GetPtr(Array), Array.Length)
    End Function

    '================================================================================
    'SubC
    <Runtime.InteropServices.DllImportAttribute(IPP_S_DLL)> Private Shared Function ippsSubC_32f_I(ByVal val As Single, ByVal pSrcDst As IntPtr, ByVal len As Integer) As IppStatus
    End Function
    Public Shared Function SubC(ByRef Vector(,) As Single, ByVal SubVal As Single) As IppStatus
        Return ippsSubC_32f_I(SubVal, GetPtr(Vector), Vector.Length)
    End Function

    '================================================================================
    'SubC
    <Runtime.InteropServices.DllImportAttribute(IPP_S_DLL)> Private Shared Function ippsSubC_64f_I(ByVal val As Double, ByVal pSrcDst As IntPtr, ByVal len As Integer) As IppStatus
    End Function
    Public Shared Function SubC(ByRef Vector(,) As Double, ByVal SubVal As Double) As IppStatus
        Return ippsSubC_64f_I(SubVal, GetPtr(Vector), Vector.Length)
    End Function

    '================================================================================
    'MulC
    <Runtime.InteropServices.DllImportAttribute(IPP_S_DLL)> Private Shared Function ippsMulC_32f_I(ByVal val As Single, ByVal pSrcDst As IntPtr, ByVal len As Integer) As IppStatus
    End Function
    Public Shared Function MulC(ByRef Array(,) As Single, ByRef ScaleFactor As Single) As IppStatus
        Return ippsMulC_32f_I(ScaleFactor, GetPtr(Array), Array.Length)
    End Function

    '================================================================================
    'MulC
    <Runtime.InteropServices.DllImportAttribute(IPP_S_DLL)> Private Shared Function ippsMulC_64f_I(ByVal val As Double, ByVal pSrcDst As IntPtr, ByVal len As Integer) As IppStatus
    End Function
    Public Shared Function MulC(ByRef Array(,) As Double, ByRef ScaleFactor As Double) As IppStatus
        Return ippsMulC_64f_I(ScaleFactor, GetPtr(Array), Array.Length)
    End Function

    '================================================================================
    'DivC
    <Runtime.InteropServices.DllImportAttribute(IPP_S_DLL)> Private Shared Function ippsDivC_64f_I(ByVal val As Double, ByVal pSrcDst As IntPtr, ByVal len As Integer) As IppStatus
    End Function
    Public Shared Function DivC(ByRef Array(,) As Double, ByRef ScaleFactor As Double) As IppStatus
        Return ippsDivC_64f_I(ScaleFactor, GetPtr(Array), Array.Length)
    End Function

    <Runtime.InteropServices.DllImportAttribute(IPP_S_DLL)> Private Shared Function ippsMul_64f_I(ByVal pSrc As IntPtr, ByVal pSrcDst As IntPtr, ByVal len As Integer) As IppStatus
    End Function
    Public Shared Function Mul(ByRef ArraySrc(,) As Double, ByRef ArraySrcDst(,) As Double) As IppStatus
        Return ippsMul_64f_I(GetPtr(ArraySrc), GetPtr(ArraySrcDst), ArraySrc.Length)
    End Function

    '================================================================================
    'Sqr
    <Runtime.InteropServices.DllImportAttribute(IPP_S_DLL)> Private Shared Function ippsSqr_64f_I(ByVal pSrcDst As IntPtr, ByVal len As Integer) As IppStatus
    End Function
    Public Shared Function Sqr(ByRef Array(,) As Double) As IppStatus
        Return ippsSqr_64f_I(GetPtr(Array), Array.Length)
    End Function

    '================================================================================
    'Sqr
    <Runtime.InteropServices.DllImportAttribute(IPP_S_DLL)> Private Shared Function ippsSqr_64f(ByVal pSrc As IntPtr, ByVal pDst As IntPtr, ByVal len As Integer) As IppStatus
    End Function
    Public Shared Function Sqr(ByRef ArrayIn(,) As Double, ByRef ArrayOut(,) As Double) As IppStatus
        AdjustSize(ArrayIn, ArrayOut)
        Return ippsSqr_64f(GetPtr(ArrayIn), GetPtr(ArrayOut), ArrayIn.Length)
    End Function

    '================================================================================
    'Sqrt
    <Runtime.InteropServices.DllImportAttribute(IPP_S_DLL)> Private Shared Function ippsSqrt_64f_I(ByVal pSrcDst As IntPtr, ByVal len As Integer) As IppStatus
    End Function
    Public Shared Function Sqrt(ByRef Array(,) As Double) As IppStatus
        Return ippsSqrt_64f_I(GetPtr(Array), Array.Length)
    End Function

    '================================================================================
    'Sqr
    <Runtime.InteropServices.DllImportAttribute(IPP_VM_DLL)> Private Shared Function ippsSin_64f_A53(ByVal pSrc As IntPtr, ByVal pDst As IntPtr, ByVal len As Integer) As IppStatus
    End Function
    Public Shared Function Sin(ByRef ArrayIn(,) As Double) As IppStatus
        Dim InPlace(,) As Double = {}
        AdjustSize(ArrayIn, InPlace)
        Dim RetVal As IppStatus = ippsSin_64f_A53(GetPtr(ArrayIn), GetPtr(InPlace), ArrayIn.Length)
        ArrayIn = Copy(InPlace)
        Return RetVal
    End Function

    '================================================================================
    'MinMax
    <Runtime.InteropServices.DllImportAttribute(IPP_S_DLL)> Private Shared Function ippsMinMax_32f(ByVal pSrc As IntPtr, ByVal len As Integer, ByVal pMin As IntPtr, ByVal pMax As IntPtr) As IppStatus
    End Function
    Public Shared Function MinMax(ByRef Array(,) As Single, ByRef Minimum As Single, ByRef Maximum As Single) As IppStatus
        Dim RetVal As IppStatus : Dim TempVal1(0) As Single : Dim TempVal2(0) As Single
        RetVal = ippsMinMax_32f(GetPtr(Array), Array.Length, GetPtr(TempVal1), GetPtr(TempVal2))
        Minimum = TempVal1(0) : Maximum = TempVal2(0)
        Return RetVal
    End Function

    '================================================================================
    'MaxIndx 
    <Runtime.InteropServices.DllImportAttribute(IPP_S_DLL)> Private Shared Function ippsMaxIndx_64f(ByVal pSrc As IntPtr, ByVal len As Integer, ByVal pMax As IntPtr, ByVal pMaxIndx As IntPtr) As IppStatus
    End Function
    Public Shared Function MaxIndx(ByRef Array(,) As Double, ByRef Maximum As Double, ByRef MaximumIdx As Integer) As IppStatus
        Dim RetVal As IppStatus : Dim TempVal1(0) As Double : Dim TempVal2(0) As Integer
        RetVal = ippsMaxIndx_64f(GetPtr(Array), Array.Length, GetPtr(TempVal1), GetPtr(TempVal2))
        Maximum = TempVal1(0) : MaximumIdx = TempVal2(0)
        Return RetVal
    End Function

    '================================================================================
    'MinMax
    <Runtime.InteropServices.DllImportAttribute(IPP_S_DLL)> Private Shared Function ippsMinMax_64f(ByVal pSrc As IntPtr, ByVal len As Integer, ByVal pMin As IntPtr, ByVal pMax As IntPtr) As IppStatus
    End Function
    Public Shared Function MinMax(ByRef Array() As Double, ByRef Minimum As Double, ByRef Maximum As Double) As IppStatus
        Dim RetVal As IppStatus : Dim TempVal1(0) As Double : Dim TempVal2(0) As Double
        RetVal = ippsMinMax_64f(GetPtr(Array), Array.Length, GetPtr(TempVal1), GetPtr(TempVal2))
        Minimum = TempVal1(0) : Maximum = TempVal2(0)
        Return RetVal
    End Function
    Public Shared Function MinMax(ByRef Array(,) As Double, ByRef Minimum As Double, ByRef Maximum As Double) As IppStatus
        Dim RetVal As IppStatus : Dim TempVal1(0) As Double : Dim TempVal2(0) As Double
        RetVal = ippsMinMax_64f(GetPtr(Array), Array.Length, GetPtr(TempVal1), GetPtr(TempVal2))
        Minimum = TempVal1(0) : Maximum = TempVal2(0)
        Return RetVal
    End Function

    '================================================================================
    'Mean
    <Runtime.InteropServices.DllImportAttribute(IPP_S_DLL)> Private Shared Function ippsMean_64f(ByVal pSrc As IntPtr, ByVal len As Integer, ByVal pMean As IntPtr) As IppStatus
    End Function
    Public Shared Function Mean(ByRef Array(,) As Double, ByRef MeanValue As Double) As IppStatus
        Dim RetVal As IppStatus : Dim TempVal(0) As Double
        RetVal = ippsMean_64f(GetPtr(Array), Array.Length, GetPtr(TempVal))
        MeanValue = TempVal(0) : Return RetVal
    End Function

    '================================================================================
    'Copy
    <Runtime.InteropServices.DllImportAttribute(IPP_S_DLL)> Private Shared Function ippsCopy_64f(ByVal pSrc As IntPtr, ByVal pDst As IntPtr, ByVal len As Integer) As IppStatus
    End Function
    Public Shared Function Copy(ByRef Vector As Double(,)) As Double(,)
        Dim RetVal(Vector.GetUpperBound(0), Vector.GetUpperBound(1)) As Double
        ippsCopy_64f(GetPtr(Vector), GetPtr(RetVal), RetVal.Length)
        Return RetVal
    End Function

End Class
