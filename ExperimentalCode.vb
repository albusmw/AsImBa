Option Explicit On
Option Strict On

'''<summary>Multi-threaded LUT code.</summary>
Public Class cLUT_MultiThread

    Public DataIn(,) As UInt16
    Public DataOut(,) As Byte

    Dim WithEvents BGW_1 As New System.ComponentModel.BackgroundWorker
    Dim WithEvents BGW_2 As New System.ComponentModel.BackgroundWorker
    Dim WithEvents BGW_3 As New System.ComponentModel.BackgroundWorker
    Dim WithEvents BGW_4 As New System.ComponentModel.BackgroundWorker

    Public Sub Process(ByRef LUT As SortedDictionary(Of UInt32, Byte))

        Dim Chunk1 As Integer = CInt(DataIn.GetUpperBound(0) * 0.25)
        Dim Chunk2 As Integer = CInt(DataIn.GetUpperBound(0) * 0.5)
        Dim Chunk3 As Integer = CInt(DataIn.GetUpperBound(0) * 0.75)
        Dim ChunkEnd As Integer = DataIn.GetUpperBound(0)

        BGW_1.RunWorkerAsync(New Object() {New Integer() {0, Chunk1}, LUT, DataIn, DataOut})
        BGW_2.RunWorkerAsync(New Object() {New Integer() {Chunk1 + 1, Chunk2}, LUT, DataIn, DataOut})
        BGW_3.RunWorkerAsync(New Object() {New Integer() {Chunk2 + 1, Chunk3}, LUT, DataIn, DataOut})
        BGW_4.RunWorkerAsync(New Object() {New Integer() {Chunk3 + 1, ChunkEnd}, LUT, DataIn, DataOut})

        Do
            System.Windows.Forms.Application.DoEvents()
        Loop Until BGW_1.IsBusy = False And BGW_2.IsBusy = False And BGW_3.IsBusy = False And BGW_4.IsBusy = False

    End Sub

    Private Sub BGW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BGW_1.DoWork, BGW_2.DoWork, BGW_3.DoWork, BGW_4.DoWork
        Dim Arguments As Object() = CType(e.Argument, Object())
        Dim StartStop As Integer() = CType(Arguments(0), Integer())
        Dim LUT As SortedDictionary(Of UInt32, Byte) = CType(Arguments(1), Global.System.Collections.Generic.SortedDictionary(Of UInteger, Byte))
        For Idx1 As Integer = StartStop(0) To StartStop(1)
            For Idx2 As Integer = 0 To DataIn.GetUpperBound(1)
                DataOut(Idx1, Idx2) = LUT(DataIn(Idx1, Idx2))
            Next Idx2
        Next Idx1
        e.Result = "Done"
    End Sub

End Class
