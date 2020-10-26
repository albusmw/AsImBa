Option Explicit On
Option Strict On

Imports System.ComponentModel

Public Class cImageStat

    Const CrLf As String = Chr(10) & Chr(13)

    Public Event DetailSettingChanged()
    Public Event ImageDataChanged()

    Public Enum eExpandDynamicsMode
        DoNotExpand
        AutoExpand
        FreezeExpand
    End Enum

    <Category("0.) Detailed view")>
    <DisplayName("a) Expand dynamics")> _
    <Description("Expand the dynamics within the zoom region to fill the complete available color map range." & CrLf & "Keyboard shortcut: <E>")> _
    Public Property ExpandDynamicsMode() As eExpandDynamicsMode
        Get
            Return MyExpandDynamicsMode
        End Get
        Set(value As eExpandDynamicsMode)
            MyExpandDynamicsMode = value
            RaiseEvent DetailSettingChanged()
        End Set
    End Property
    Private MyExpandDynamicsMode As eExpandDynamicsMode = eExpandDynamicsMode.AutoExpand

    <Category("0.) Detailed view")>
    <DisplayName("b) Color map")> _
    <Description("Color map to apply on the detailed view." & CrLf & "Keyboard shortcut: <M>")>
    <TypeConverter(GetType(ComponentModelEx.EnumDesciptionConverter))>
    Public Property ColorMap() As cColorMaps.eMaps
        Get
            Return MyColorMap
        End Get
        Set(value As cColorMaps.eMaps)
            MyColorMap = value
            RaiseEvent DetailSettingChanged()
        End Set
    End Property
    Private MyColorMap As cColorMaps.eMaps = cColorMaps.eMaps.Jet

    <Category("0.) Detailed view")>
    <DisplayName("c) Lock location")> _
    <Description("Lock the detailed view location (do not follow the mouse move)." & CrLf & "Keyboard shortcut: <L>")> _
    Public Property LockLocation() As Boolean
        Get
            Return MyLockLocation
        End Get
        Set(value As Boolean)
            MyLockLocation = value
        End Set
    End Property
    Private MyLockLocation As Boolean = False

End Class
