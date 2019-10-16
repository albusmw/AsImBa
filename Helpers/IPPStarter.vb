Option Explicit On
Option Strict On

Public Class IPPStarter

    '''<summary>Function to locate all IPP DLL's valid for the usage conditions.</summary>
    '''<param name="RequiredPlatform">Required platform (x86 or x64/IA64)</param>
    '''<returns></returns>
    '''<remarks></remarks>
    Public Shared Function SearchIPP(ByVal RequiredPlatform As System.Reflection.ProcessorArchitecture, ByVal DLLRoots As String()) As Boolean

        Dim Dir As String = "C:\"

        Dim Results As New List(Of String)

        'Search for all patterns and get only ipps "roots"
        Dim AlsoFoundHere As New List(Of String)
        'Create the query and run
        Dim SearchQuery As String = """" & Dir & """ " & DLLRoots(0) & "*.dll"
        Everything.Everything_SetSearchW(SearchQuery)
        Everything.Everything_QueryW(True)
        'Get all found files
        Dim bufsize As Integer = 260
        Dim buf As New System.Text.StringBuilder(bufsize)
        For Idx As Integer = 0 To Everything.Everything_GetNumResults() - 1
            Everything.Everything_GetResultFullPathNameW(Idx, buf, bufsize)
            Dim FoundFile As String = buf.ToString                                          'Get search result from DLL
            Dim FileName As String = System.IO.Path.GetFileName(FoundFile)                  'Get file name only
            FileName = FileName.Substring(DLLRoots(0).Length).Replace(".dll", String.Empty)     'Remove "ipps" and ".dll"
            Dim Found As Boolean = False
            If FileName.Length = 0 Then                                                     'This one finds "ipps.dll"
                Found = True
            Else
                Select Case FileName.Substring(0, 1)                                        'Also valid: "ipps2.dll","ipps-8.0.dll"
                    Case "-", "0", "1" To "9"
                        Found = True
                End Select
            End If
            'Add if also the other DLL's are found
            If Found Then
                If DLLRoots.Length = 1 Then
                    Results.Add(FoundFile)
                Else
                    Dim AllRootsFound As Boolean = True
                    For SearchIdx As Integer = 1 To DLLRoots.GetUpperBound(0)
                        If System.IO.File.Exists(FoundFile.Replace(DLLRoots(0), DLLRoots(SearchIdx))) = False Then
                            AllRootsFound = False
                        End If
                    Next SearchIdx
                    If AllRootsFound = True Then
                        Results.Add(FoundFile)
                    End If
                End If
            End If
        Next Idx

        'Check for image version
        Dim AllValid As New List(Of String)
        For Each Entry As String In Results
            Dim Buffer() As Byte = System.IO.File.ReadAllBytes(Entry)
            Dim Type As String = String.Empty
            'TODO: The PEOffset values can also be dynamic ...
            For Each PEOffset As Integer In New Integer() {248, 256, 264}
                If Chr(Buffer(PEOffset)) = "P" And Chr(Buffer(PEOffset + 1)) = "E" And Buffer(PEOffset + 2) = 0 And Buffer(PEOffset + 3) = 0 Then
                    If Buffer(PEOffset + 4) = &H4C And Buffer(PEOffset + 5) = &H1 Then
                        'x86
                        Type = "x86"
                        If RequiredPlatform = Reflection.ProcessorArchitecture.X86 Then AllValid.Add(Entry)
                    Else
                        If Buffer(PEOffset + 4) = &H64 And Buffer(PEOffset + 5) = &H86 Then
                            'x64
                            Type = "x64"
                            If RequiredPlatform = Reflection.ProcessorArchitecture.IA64 Then AllValid.Add(Entry)
                        Else
                            '???
                            Type = "???"
                        End If
                    End If
                End If
                If String.IsNullOrEmpty(Type) = False Then Exit For
            Next PEOffset
            If String.IsNullOrEmpty(Type) = False Then
                If Type = "x64" Then Debug.Print(Type & " -> " & Entry)
            Else
                Debug.Print("No DLL" & " -> " & Entry)
            End If
        Next Entry

        'Use newest version
        Dim NewestFile As String = String.Empty
        Dim NewestVersion As Double = Double.MinValue
        For Each Entry As String In AllValid
            Dim FileNameOnly As String = System.IO.Path.GetFileNameWithoutExtension(Entry).Replace(DLLRoots(0), String.Empty).Replace("-", String.Empty)
            Dim Version As Double = 0
            Double.TryParse(FileNameOnly, Version)
            If Version > NewestVersion Then
                NewestVersion = Version
                NewestFile = Entry
            End If
        Next Entry

        'Try to run ...
        DB.IPP = New cIntelIPP(NewestFile, NewestFile.Replace("ipps", "ippvm"))
        Dim SingleArray(,) As Single = {{1, 2, 3, 4}, {5, 6, 7, 8}}
        DB.IPP.AddC(SingleArray, 5)

        Return True

    End Function

End Class
