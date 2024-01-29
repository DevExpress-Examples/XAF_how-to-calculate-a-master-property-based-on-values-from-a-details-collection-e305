Imports DevExpress.Data.Filtering
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Xpo
Imports System

Namespace CalculatedPropertiesSolution.Module.BusinessObjects

    <DefaultClassOptions>
    Public Class Product
        Inherits BaseObject

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub

        Private fName As String

        Public Property Name As String
            Get
                Return fName
            End Get

            Set(ByVal value As String)
                SetPropertyValue("Name", fName, value)
            End Set
        End Property

        <Association("Product-Orders"), Aggregated>
        Public ReadOnly Property Orders As XPCollection(Of Order)
            Get
                Return GetCollection(Of Order)("Orders")
            End Get
        End Property

        Private fOrdersCount As Integer? = Nothing

        Public ReadOnly Property OrdersCount As Integer?
            Get
                If Not IsLoading AndAlso Not IsSaving AndAlso fOrdersCount Is Nothing Then UpdateOrdersCount(False)
                Return fOrdersCount
            End Get
        End Property

        Private fOrdersTotal As Decimal? = Nothing

        Public ReadOnly Property OrdersTotal As Decimal?
            Get
                If Not IsLoading AndAlso Not IsSaving AndAlso fOrdersTotal Is Nothing Then UpdateOrdersTotal(False)
                Return fOrdersTotal
            End Get
        End Property

        Private fMaximumOrder As Decimal? = Nothing

        Public ReadOnly Property MaximumOrder As Decimal?
            Get
                If Not IsLoading AndAlso Not IsSaving AndAlso fMaximumOrder Is Nothing Then UpdateMaximumOrder(False)
                Return fMaximumOrder
            End Get
        End Property

        Public Sub UpdateOrdersCount(ByVal forceChangeEvents As Boolean)
            Dim oldOrdersCount As Integer? = fOrdersCount
            fOrdersCount = Convert.ToInt32(Evaluate(CriteriaOperator.Parse("Orders.Count")))
            If forceChangeEvents Then OnChanged("OrdersCount", oldOrdersCount, fOrdersCount)
        End Sub

        Public Sub UpdateOrdersTotal(ByVal forceChangeEvents As Boolean)
            Dim oldOrdersTotal As Decimal? = fOrdersTotal
            Dim tempTotal As Decimal = 0D
            For Each detail As Order In Orders
                tempTotal += detail.Total
            Next

            fOrdersTotal = tempTotal
            If forceChangeEvents Then OnChanged("OrdersTotal", oldOrdersTotal, fOrdersTotal)
        End Sub

        Public Sub UpdateMaximumOrder(ByVal forceChangeEvents As Boolean)
            Dim oldMaximumOrder As Decimal? = fMaximumOrder
            Dim tempMaximum As Decimal = 0D
            For Each detail As Order In Orders
                If detail.Total > tempMaximum Then tempMaximum = detail.Total
            Next

            fMaximumOrder = tempMaximum
            If forceChangeEvents Then OnChanged("MaximumOrder", oldMaximumOrder, fMaximumOrder)
        End Sub

        Protected Overrides Sub OnLoaded()
            Reset()
            MyBase.OnLoaded()
        End Sub

        Private Sub Reset()
            fOrdersCount = Nothing
            fOrdersTotal = Nothing
            fMaximumOrder = Nothing
        End Sub
    End Class
End Namespace
