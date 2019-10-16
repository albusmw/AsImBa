Option Explicit On
Option Strict On

Imports System.Threading
Imports System.Runtime.InteropServices

Public Class cMIDIMonitor

    'http://www.codeproject.com/Articles/814885/MIDI-monitor-written-in-Visual-Basic

    Public Declare Function midiInGetNumDevs Lib "winmm.dll" () As Integer
    Public Declare Function midiInGetDevCaps Lib "winmm.dll" Alias "midiInGetDevCapsA" (ByVal uDeviceID As Integer, ByRef lpCaps As MIDIINCAPS, ByVal uSize As Integer) As Integer
    Public Declare Function midiInOpen Lib "winmm.dll" (ByRef hMidiIn As Integer, ByVal uDeviceID As Integer, ByVal dwCallback As MidiInCallback, ByVal dwInstance As Integer, ByVal dwFlags As Integer) As Integer
    Public Declare Function midiInStart Lib "winmm.dll" (ByVal hMidiIn As Integer) As Integer
    Public Declare Function midiInStop Lib "winmm.dll" (ByVal hMidiIn As Integer) As Integer
    Public Declare Function midiInReset Lib "winmm.dll" (ByVal hMidiIn As Integer) As Integer
    Public Declare Function midiInClose Lib "winmm.dll" (ByVal hMidiIn As Integer) As Integer

  Public Delegate Function MidiInCallback(ByVal hMidiIn As Integer, ByVal wMsg As UInteger, ByVal dwInstance As Integer, ByVal dwParam1 As UInt32, ByVal dwParam2 As Integer) As Integer
    Public ptrCallback As New MidiInCallback(AddressOf MidiInProc)
    Public Const CALLBACK_FUNCTION As Integer = &H30000
    Public Const MIDI_IO_STATUS = &H20

    Public Delegate Sub DisplayDataDelegate(ByVal dwParam1 As UInt32)

    Public AvailableMIDIDevice As String() = {}

    Private CurrentChannelValues As New Dictionary(Of Integer, Integer)
    Private LastChannelValues As New Dictionary(Of Integer, Integer)

    Private LastOutputData As New Dictionary(Of Integer, Integer)

    Private MinChannelValue As Integer = 0
    Private MaxChannelValue As Integer = 127

    Public Sub New()

        If midiInGetNumDevs() = 0 Then
            MsgBox("No MIDI devices connected")
            Exit Sub
        End If

        Dim InCaps As New MIDIINCAPS
        Dim DevCnt As Integer

        ReDim AvailableMIDIDevice(midiInGetNumDevs - 1)
        For DevCnt = 0 To midiInGetNumDevs - 1
            midiInGetDevCaps(DevCnt, InCaps, Len(InCaps))
            AvailableMIDIDevice(DevCnt) = InCaps.szPname
    Next DevCnt

    SelectMidiDevice(0)

    End Sub

    Public Structure MIDIINCAPS
        Dim wMid As Int16                                                               'Manufacturer ID
        Dim wPid As Int16                                                               'Product ID
        Dim vDriverVersion As Integer                                                   'Driver version
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=32)> Dim szPname As String       'Product Name
        Dim dwSupport As Integer                                                        'Reserved
    End Structure

    Dim hMidiIn As Integer
    Dim StatusByte As Byte
    Dim DataByte1 As Byte
    Dim DataByte2 As Byte
    Dim MonitorActive As Boolean = False

    Public Event NewMessage(ByVal Message As String)
    Public Event NewData(ByVal Channel As Integer, ByVal Value As Integer)

    Public Property HideMidiSysMessages() As Boolean
        Get
            Return MyHideMidiSysMessages
        End Get
        Set(value As Boolean)
            MyHideMidiSysMessages = value
        End Set
    End Property
    Dim MyHideMidiSysMessages As Boolean = False

  Function MidiInProc(ByVal hMidiIn As Integer, ByVal wMsg As UInteger, ByVal dwInstance As Integer, ByVal dwParam1 As UInt32, ByVal dwParam2 As Integer) As Integer

    If MonitorActive = True Then

            Dim Message As String = Hex(dwParam1).PadLeft(6, CChar("0"))

            Dim DecodedData As Integer = 0
            Dim Channel As Integer = -1
            Dim MessageHeader As String = Message.Substring(2, 2)
            Dim IgnoreMessage As Boolean = False

            Select Case MessageHeader
                Case "0A"
                    Dim Decoded As String = "Fader  =" & Val("&H" & Message.Substring(0, 2))

                Case "0B"
                    DecodedData = CInt("&H" & Message.Substring(0, 2)) : Channel = 1
                Case "0C"
                    DecodedData = CInt("&H" & Message.Substring(0, 2)) : Channel = 2
                Case "0D"
                    DecodedData = CInt("&H" & Message.Substring(0, 2)) : Channel = 3
                Case "0E"
                    DecodedData = CInt("&H" & Message.Substring(0, 2)) : Channel = 4
                Case "0F"
                    DecodedData = CInt("&H" & Message.Substring(0, 2)) : Channel = 5
                Case "10"
                    DecodedData = CInt("&H" & Message.Substring(0, 2)) : Channel = 6
                Case "11"
                    DecodedData = CInt("&H" & Message.Substring(0, 2)) : Channel = 7
                Case "12"
                    DecodedData = CInt("&H" & Message.Substring(0, 2)) : Channel = 8

                Case "18"
                    DecodedData = CInt("&H" & Message.Substring(0, 2)) : Channel = 1
                    If DecodedData = MaxChannelValue Then DecodedData = -1      'Press
                    If DecodedData = MinChannelValue Then IgnoreMessage = True
                Case "19"
                    DecodedData = CInt("&H" & Message.Substring(0, 2)) : Channel = 2
                    If DecodedData = MaxChannelValue Then DecodedData = -1      'Press
                    If DecodedData = MinChannelValue Then IgnoreMessage = True

            End Select

            If Not IgnoreMessage Then

                'Add new channel / store
                If CurrentChannelValues.ContainsKey(Channel) = False Then
                    CurrentChannelValues.Add(Channel, DecodedData)
                    LastChannelValues.Add(Channel, DecodedData)
                    LastOutputData.Add(Channel, 0)
                Else
                    If DecodedData <> -1 Then
                        LastChannelValues(Channel) = CurrentChannelValues(Channel)
                        CurrentChannelValues(Channel) = DecodedData
                    End If
                End If

                'React on "press"
                Dim CurrentData As Integer = -1
                If DecodedData = -1 Then
                    CurrentData = 64
                Else
                    'Realize an "endless rotaty"
                    Dim Increment As Integer = CurrentChannelValues(Channel) - LastChannelValues(Channel)

                    If Increment <> 0 Then
                        CurrentData = LastOutputData(Channel) + Increment
                    Else
                        If DecodedData = MinChannelValue Then CurrentData = LastOutputData(Channel) - 1
                        If DecodedData = MaxChannelValue Then CurrentData = LastOutputData(Channel) + 1
                    End If

                End If

                'Store last output data
                If LastOutputData.ContainsKey(Channel) = False Then LastOutputData.Add(Channel, CurrentData) Else LastOutputData(Channel) = CurrentData

                Dim Time As String = Format(dwParam2 / 1000, "000.000")

                RaiseEvent NewMessage(Format(hMidiIn, "000000") & ":" & Format(wMsg, "000000") & ":" & Message & ":" & " -> " & "...")
                RaiseEvent NewData(Channel, CurrentData)

            End If



        End If

    Return 0

  End Function

    Private Sub DisplayData(ByVal dwParam1 As UInt32)
        If ((HideMidiSysMessages = True) And ((dwParam1 And &HF0) = &HF0)) Then
            Exit Sub
        Else
            StatusByte = CByte((dwParam1 And &HFF))
            DataByte1 = CByte((dwParam1 And &HFF00) >> 8)
            DataByte2 = CByte((dwParam1 And &HFF0000) >> 16)
            RaiseEvent NewMessage(String.Format("{0:X2} {1:X2} {2:X2}{3}", StatusByte, DataByte1, DataByte2, vbCrLf))
        End If
    End Sub

    Public Sub SelectMidiDevice(ByVal DeviceID As Integer)
    Dim Ret_Open As Integer = midiInOpen(hMidiIn, DeviceID, ptrCallback, 0, CALLBACK_FUNCTION Or MIDI_IO_STATUS)
    Dim Ret_Start As Integer = midiInStart(hMidiIn)
        MonitorActive = True
    End Sub

    Public Sub StartMonitor()
        midiInStart(hMidiIn)
        MonitorActive = True
    End Sub

    Public Sub StopMonitor()
        midiInStop(hMidiIn)
        MonitorActive = False
    End Sub

    Private Sub Disconnect()
        MonitorActive = False
        midiInStop(hMidiIn)
        midiInReset(hMidiIn)
    End Sub

End Class