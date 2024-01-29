Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Updating
Imports CalculatedPropertiesSolution.Module.BusinessObjects

Namespace CalculatedPropertiesSolution.Module.DatabaseUpdate

    Public Class Updater
        Inherits ModuleUpdater

        Public Sub New(ByVal objectSpace As IObjectSpace, ByVal currentDBVersion As Version)
            MyBase.New(objectSpace, currentDBVersion)
        End Sub

        Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
            MyBase.UpdateDatabaseAfterUpdateSchema()
            Dim product As Product = ObjectSpace.CreateObject(Of Product)()
            product.Name = "Chai"
            For i As Integer = 0 To 10 - 1
                Dim order As Order = ObjectSpace.CreateObject(Of Order)()
                order.Product = product
                order.Description = "Order " & i.ToString()
                order.Total = i
            Next

            ObjectSpace.CommitChanges()
        End Sub

        Public Overrides Sub UpdateDatabaseBeforeUpdateSchema()
            MyBase.UpdateDatabaseBeforeUpdateSchema()
        End Sub
    End Class
End Namespace
