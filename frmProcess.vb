Option Explicit On
Option Strict On

Public Class frmProcess

    Private Sub frmProcess_Load(sender As Object, e As EventArgs) Handles Me.Load

        'List all channels
        With cbChannels
            'Add all entries
            .Items.Clear()
            For Each Entry As String In DB.Channels.Keys
                .Items.Add(Entry & " (" & DB.Channels(Entry).SourceFileName & ")")
            Next Entry
            'Select last entry
            If .Items.Count > 0 Then .SelectedIndex = .Items.Count - 1
        End With

        'Tooltop on file
        ttMain.SetToolTip(cbChannels, CStr(cbChannels.SelectedItem))

        'Reset all action dropdown's
        cbRescale.SelectedIndex = 0
        cbPowers.SelectedIndex = 0
        cbFlipRotate.SelectedIndex = 0
        cbComplexOperations.SelectedIndex = 0

    End Sub

    '''<summary>BEFORE processing, get the channel that is processed.</summary>
    Private Function DisplayedChannel() As String
        If cbChannels.SelectedIndex <> -1 Then
            tsslStatus.Text = "Processing channel " & cbChannels.SelectedItem.ToString.Trim & " ..."
            System.Windows.Forms.Application.DoEvents()
            Dim SelectedChannel As String = CStr(cbChannels.SelectedItem)
            If String.IsNullOrEmpty(SelectedChannel) = True Then
                Return String.Empty
            Else
                Return SelectedChannel.Substring(0, SelectedChannel.IndexOf("(") - 1).Trim
            End If
        Else
            Return String.Empty
        End If
    End Function

    '''<summary>AFTER processing, update all forms that display the channel.</summary>
    Private Sub UpdateFormsDisplayingChannel(ByVal UpdateStatistics As Boolean)
        Dim openForms As Windows.Forms.FormCollection = Application.OpenForms
        For Each frm As Windows.Forms.Form In openForms
            If frm.GetType.Name = "frmImage" Then
                With CType(frm, frmImage)
                    If .DisplayedChannel = DisplayedChannel() Then
                        If UpdateStatistics Then DB.Channels(.DisplayedChannel).CalculateStatistics()
                        .UpdateImage()
                    End If
                End With
            End If
        Next frm
        tsslStatus.Text = "IDLE"
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub cbRescale_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbRescale.SelectedIndexChanged
        Dim ChannelToEdit As String = DisplayedChannel() : If String.IsNullOrEmpty(ChannelToEdit) = True Then Exit Sub
        Select Case cbRescale.SelectedIndex
            Case 0
                'Do nothing
                Exit Sub
            Case 1
                'Modify the channel
                DB.Channels(ChannelToEdit).Expand(0, 255)
            Case 2
                'Modify the channel
                DB.Channels(ChannelToEdit).Expand(0, 65535)
            Case 3
                'Modify the channel
                DB.Channels(ChannelToEdit).Expand(0, 1)
            Case 4
                'User
                Dim Min As Double = Val(InputBox("Minimum:", "Minimum", "0").Replace(",", "."))
                Dim Max As Double = Val(InputBox("Maximum:", "Maximum", "255").Replace(",", "."))
                DB.Channels(ChannelToEdit).Expand(Min, Max)
        End Select

        'Update all form that display the channel
        UpdateFormsDisplayingChannel(True)
        cbRescale.SelectedIndex = 0

    End Sub

    Private Sub cbPowers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbPowers.SelectedIndexChanged

        Dim ChannelToEdit As String = DisplayedChannel() : If String.IsNullOrEmpty(ChannelToEdit) = True Then Exit Sub
        Select Case cbPowers.SelectedIndex

            Case 0
                'Do nothing
                Exit Sub

            Case 1
                '^2
                'Modify the channel
                DB.IPP.Sqr(DB.Channels(ChannelToEdit).ImageData)

            Case 2
                '^3
                'Modify the channel
                Dim Copy As Double(,) = DB.IPP.Copy(DB.Channels(ChannelToEdit).ImageData)
                DB.IPP.Mul(DB.Channels(ChannelToEdit).ImageData, Copy)
                DB.IPP.Mul(Copy, DB.Channels(ChannelToEdit).ImageData)

            Case 3
                'sqrt(...)
                DB.IPP.Sqrt(DB.Channels(ChannelToEdit).ImageData)

        End Select

        'Update all form that display the channel
        UpdateFormsDisplayingChannel(True)
        cbPowers.SelectedIndex = 0

    End Sub

    Private Sub cbFlipRotate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbFlipRotate.SelectedIndexChanged
        Dim ChannelToEdit As String = DisplayedChannel() : If String.IsNullOrEmpty(ChannelToEdit) = True Then Exit Sub
        Select Case cbFlipRotate.SelectedIndex
            Case 0
                'Do nothing
                Exit Sub
            Case 1
                'Flip left-right
                DB.Channels(ChannelToEdit).FlipLR()
            Case 2
                'Flip top-buttom
                DB.Channels(ChannelToEdit).FlipTB()
            Case 3
                'Rotate clockwise
                DB.Channels(ChannelToEdit).RotateCW()
            Case 4
                'Rotate counter-clock wise
                DB.Channels(ChannelToEdit).RotateCCW()
        End Select

        'Update all form that display the channel
        UpdateFormsDisplayingChannel(False)
        cbFlipRotate.SelectedIndex = 0

    End Sub

    Private Sub cbChannels_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbChannels.SelectedIndexChanged
        'Tooltop on file
        ttMain.SetToolTip(cbChannels, CStr(cbChannels.SelectedItem))
    End Sub

    Private Sub cbComplexOperations_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbComplexOperations.SelectedIndexChanged

        Dim ChannelToEdit As String = DisplayedChannel() : If String.IsNullOrEmpty(ChannelToEdit) = True Then Exit Sub

        Select Case cbComplexOperations.SelectedIndex

            Case 0
                'Do nothing
                Exit Sub

            Case 1
                'Invert within given range
                DB.Channels(ChannelToEdit).Invert()

            Case 2
                'Sin
                DB.Channels(ChannelToEdit).Expand(0, 1)
                DB.IPP.Sin(DB.Channels(ChannelToEdit).ImageData)

            Case 3
                'Experimental - smear the complete intensity range over the complete range (0...255) currently
                '  After that, each pixel has a different intensity and the historgram is a straight horizontal line
                'Moved to ImageProcessing.vb

            Case 4

                'Push low and high intensity
                Dim SplitPercentage As Double = 5
                Dim Multiplier As Double = 5
                Dim Min As Double
                Dim Max As Double
                DB.IPP.MinMax(DB.Channels(ChannelToEdit).ImageData, Min, Max)
                MathEx.MaxMinIgnoreNAN(DB.Channels(ChannelToEdit).ImageData, Min, Max)
                Dim Below As Double = Min + ((SplitPercentage / 100) * (Max - Min))
                Dim Above As Double = Min + (((100 - SplitPercentage) / 100) * (Max - Min))
                For Idx1 As Integer = 0 To DB.Channels(ChannelToEdit).ImageData.GetUpperBound(0)
                    For Idx2 As Integer = 0 To DB.Channels(ChannelToEdit).ImageData.GetUpperBound(1)
                        Dim CurrentIntensity As Double = DB.Channels(ChannelToEdit).ImageData(Idx1, Idx2)
                        If CurrentIntensity <= Below Then
                            CurrentIntensity *= Multiplier
                        Else
                            If CurrentIntensity >= Above Then
                                CurrentIntensity = Max - CurrentIntensity
                                CurrentIntensity *= Multiplier
                                CurrentIntensity = Max - CurrentIntensity
                            End If
                        End If
                        DB.Channels(ChannelToEdit).ImageData(Idx1, Idx2) = CurrentIntensity
                    Next Idx2
                Next Idx1

            Case 5

                'Remove stars
                Dim StarsToRemove As Integer = 10
                StarRemover.RemoveStars(DB.Channels(ChannelToEdit).ImageData, StarsToRemove)

            Case 6

                'Remove highest amplitude(s)
                Dim Min As Double
                Dim Max As Double
                Dim Range As Double = Val(InputBox("Range:", "Range", "10").Replace(",", "."))
                DB.IPP.MinMax(DB.Channels(ChannelToEdit).ImageData, Min, Max)
                For Idx1 As Integer = 0 To DB.Channels(ChannelToEdit).ImageData.GetUpperBound(0)
                    For Idx2 As Integer = 0 To DB.Channels(ChannelToEdit).ImageData.GetUpperBound(1)
                        Dim CurrentIntensity As Double = DB.Channels(ChannelToEdit).ImageData(Idx1, Idx2)
                        If CurrentIntensity >= Max - Range Then
                            CurrentIntensity = Min
                        End If
                        DB.Channels(ChannelToEdit).ImageData(Idx1, Idx2) = CurrentIntensity
                    Next Idx2
                Next Idx1

            Case 7

                Dim Min As Double
                Dim Max As Double
                DB.IPP.MinMax(DB.Channels(ChannelToEdit).ImageData, Min, Max)

                'Calculate center-of-object
                Dim CenterOfMass As PointF = ImageAnalysis.CenterOfMass(DB.Channels(ChannelToEdit))

                For Radius As Double = 50 To 1000 Step 50
                    For Angle As Double = 0 To 2 * Math.PI Step Math.PI / 3600
                        Dim X As Double = CenterOfMass.X + (Radius * Math.Sin(Angle))
                        Dim Y As Double = CenterOfMass.Y + (Radius * Math.Cos(Angle))
                        DB.Channels(ChannelToEdit).ImageData(CInt(X), CInt(Y)) = Min
                    Next Angle
                Next Radius

            Case 8

                'Grid bright start
                InImplementation.GridBrightStars(ChannelToEdit, 16, 1, "C:\GridOut.bmp")

        End Select

        'Update all form that display the channel
        UpdateFormsDisplayingChannel(True)
        cbComplexOperations.SelectedIndex = 0

    End Sub

    Private Sub btnHisto_Click(sender As Object, e As EventArgs) Handles btnHisto.Click

        Dim Hist As New frmHistogram
        Hist.Show()
        Hist.LoadData(DB.Channels(DisplayedChannel).HistCalc.Histogram, DB.Channels(DisplayedChannel).HistCalc.Minimum, DB.Channels(DisplayedChannel).HistCalc.Maximum)

    End Sub

End Class