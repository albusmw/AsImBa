Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Globalization
Imports System.Reflection

Namespace ComponentModel

    'This classes can be used as converters for numerical entered values in property grid objects.
    'Formating is done in full precision (indicated by AutoFormat = True) as the value entered by the user is of the user defined precision

    '''<summary>Property converter for unit Hz</summary>
    Public Class DoublePropertyConverter : Inherits DoubleConverter

        Public Sub New()
            MyBase.New()
        End Sub

        Public Overrides Function ConvertFrom(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object) As Object

            'String to double conversion
            If TypeOf value Is String Then Return CDbl(value)

            'Double to string conversion
            If TypeOf value Is Double Then Return Format(CDbl(value), "0.000")

            'Default converter
            Return MyBase.ConvertFrom(context, culture, value)

        End Function

        Public Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object, ByVal destinationType As Type) As Object

            'Double to string conversion
            If (TypeOf value Is String AndAlso (destinationType Is GetType(String))) Then Return Format(CDbl(value), "0.000")

            'Double to string conversion
            If (TypeOf value Is System.Double AndAlso (destinationType Is GetType(String))) Then Return Format(DirectCast(value, System.Double), "0.000")

            'Default converter
            Return MyBase.ConvertTo(context, culture, value, destinationType)

        End Function

    End Class

    '''<summary>Default values for enums.</summary>
    Public Class EnumDefaultValueAttribute
        Inherits DefaultValueAttribute
        Public Sub New(ByVal value As Object)
            MyBase.New(value)
        End Sub
    End Class

    '''<summary>This converter class can be attached to an enum property and will reflect the Description attribute as drop-down field.</summary>
    Public Class EnumDesciptionConverter : Inherits EnumConverter

        Protected myVal As Type

        Public Sub New(ByVal type As Type)
            MyBase.New(type.GetType)
            myVal = type
        End Sub

        '''<summary>Specify if the property can be expanded by the "+" sign.</summary>
        '''<param name="context"></param>
        '''<returns>Always FALSE as there is nothing to expand here for enums...</returns>
        '''<remarks>Refer to http://www.codeproject.com/KB/cs/propertyeditor.aspx for details.</remarks>
        Public Overrides Function GetPropertiesSupported(ByVal context As ITypeDescriptorContext) As Boolean
            Return False
        End Function

        '''<summary>All entries which are displayed in the drop-down list.</summary>
        '''<param name="context">Not of relevance.</param>
        Public Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
            Dim ComposedValueList As New ArrayList
            Dim fis As FieldInfo() = myVal.GetFields()
            For Each fi As FieldInfo In fis
                Dim attributes As DescriptionAttribute() = CType(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
                If attributes.Length > 0 Then ComposedValueList.Add(fi.GetValue(fi.Name))
            Next fi
            Return New StandardValuesCollection(ComposedValueList)
        End Function

        Public Overrides Function ConvertFrom(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object) As Object

            'String to enum conversion
            If TypeOf value Is String Then
                Return GetEnumValue(myVal, CStr(value))
            End If

            'Enum to string conversion
            If TypeOf value Is System.Enum Then
                Return GetEnumDescription(CType(value, System.Enum))
            End If

            Return MyBase.ConvertFrom(context, culture, value)

        End Function

        Public Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object, ByVal destinationType As Type) As Object

            'Enum to string conversion
            If (TypeOf value Is System.Enum AndAlso (destinationType Is GetType(String))) Then
                Return GetEnumDescription(DirectCast(value, System.Enum))
            End If

            'Enum to string conversion
            If (TypeOf value Is String AndAlso (destinationType Is GetType(String))) Then
                Return GetEnumDescription(myVal, CStr(value))
            End If

            Return MyBase.ConvertTo(context, culture, value, destinationType)

        End Function

        '================================================================================
        'Helper and service functions (can also be used from outside)
        '================================================================================

        '''<summary>Get the description (derived from the Description attribute) of the passed enum value.</summary>
        '''<param name="value">Value of the enum to read out description.</param>
        '''<returns>Description attribute of this enum or the value.ToString value if the attribute is not present.</returns>
        Public Shared Function GetEnumDescription(ByVal value As System.Enum) As String
            Dim InfoField As FieldInfo = value.GetType.GetField(value.ToString)
            If IsNothing(InfoField) = False Then
                Dim attributes As DescriptionAttribute() = CType(InfoField.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
                If attributes.Length > 0 Then Return attributes(0).Description
            End If
            Return value.ToString
        End Function

        '''<summary>Get the description (derived from the Description attribute) of the passed enum value.</summary>
        '''<param name="value">Type of the enum to read out description.</param>
        '''<param name="name">Name of the enum (as defined by the enum).</param>
        '''<returns>Description attribute of this enum or the value.ToString value if the attribute is not present.</returns>
        Public Shared Function GetEnumDescription(ByVal value As Type, ByVal name As String) As String
            Dim InfoField As FieldInfo = value.GetField(name)
            If IsNothing(InfoField) = False Then
                Dim attributes As DescriptionAttribute() = CType(InfoField.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
                If attributes.Length > 0 Then Return attributes(0).Description
            End If
            Return name
        End Function

        '''<summary>String to enum.</summary>
        Public Shared Function GetEnumValue(ByVal value As Type, ByVal description As String) As Object

            Dim fis As FieldInfo() = value.GetFields
            Dim fi As FieldInfo

            For Each fi In fis

                Dim attributes As DescriptionAttribute() = CType(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())

                If ((attributes.Length > 0) AndAlso (attributes(0).Description = description)) Then
                    Return fi.GetValue(fi.Name)
                End If

                If (fi.Name = description) Then
                    Return fi.GetValue(fi.Name)
                End If

            Next

            Return description

        End Function

        '================================================================================
        'Services for the combo box
        '================================================================================

        '''<summary>Load all enum values to the given combo box, using the description.</summary>
        Public Shared Sub AllEnumsToComboBox(ByRef Box As Windows.Forms.ComboBox, ByVal EnumType As Type)
            Box.Items.Clear()
            If IsNothing(EnumType) = False Then
                For Each Value As Object In [Enum].GetValues(EnumType)
                    Box.Items.Add(GetEnumDescription(CType(Value, [Enum])))
                Next Value
            End If
        End Sub

        Public Shared Sub EnumToComboBox(ByRef Box As Windows.Forms.ComboBox, ByVal EnumValue As [Enum])
            If IsNothing(EnumValue) = False Then
                For Each Entry As String In Box.Items
                    If Entry = GetEnumDescription(EnumValue) Then
                        Box.SelectedItem = Entry
                        Exit For
                    End If
                Next Entry
            End If
        End Sub

        '''<summary>Get the enum value from the given combo box, as the description is displayed in the box.</summary>
        Public Shared Function EnumFromComboBox(ByRef Box As Windows.Forms.ComboBox, ByVal EnumType As Type) As Object
            For Each Value As Object In [Enum].GetValues(EnumType)
                If Box.Text = GetEnumDescription(CType(Value, [Enum])) Then
                    Return Value
                End If
            Next Value
            Return Nothing
        End Function

    End Class

End Namespace