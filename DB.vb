Option Explicit On
Option Strict On

Public Class DB

    '''<summary>All channels loaded.</summary>
    Public Shared Channels As New Dictionary(Of String, cImageMonochrome)
    '''<summary>Core logging component.</summary>
    Public Shared Log As New cLogging
    '''<summary>Link to the Intel IPP.</summary>
    Public Shared IPP As cIntelIPP

    '''<summary>Create a new channel and return the GUID of this channel.</summary>
    '''<param name="Image">Image to add.</param>
    '''<returns>GUID created.</returns>
    Public Shared Function AddChannel(ByRef Image As cImageMonochrome) As String
        Dim ImageGUID As String = Guid.NewGuid.ToString
        Channels.Add(ImageGUID, Image)
        Image.ChannelListIdx = Channels.Count - 1
        FireEvent(Image.ChannelListIdx, 0, "New channel added, source data <" & Image.SourceFileName & ">")
        Return ImageGUID
    End Function

    Public Shared Sub FireEvent(ByVal ChannelIdx As Integer, ByVal Level As Integer, ByVal Message As String)
        Log.Add(Format(ChannelIdx, "0000") & "|" & Space(2 * Level) & Message)
    End Sub

End Class
