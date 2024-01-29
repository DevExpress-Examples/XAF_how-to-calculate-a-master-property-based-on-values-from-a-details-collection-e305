Imports System
Imports DevExpress.ExpressApp
Imports System.ComponentModel
Imports DevExpress.ExpressApp.Web
Imports DevExpress.ExpressApp.Xpo

Namespace CalculatedPropertiesSolution.Web

    ' For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/DevExpressExpressAppWebWebApplicationMembersTopicAll.aspx
    Public Partial Class CalculatedPropertiesSolutionAspNetApplication
        Inherits WebApplication

        Private module1 As DevExpress.ExpressApp.SystemModule.SystemModule

        Private module2 As DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule

        Private module3 As [Module].CalculatedPropertiesSolutionModule

        Public Sub New()
            InitializeComponent()
            LinkNewObjectToParentImmediately = False
            Editors.ASPx.ASPxGridListEditor.AllowFilterControlHierarchy = True
            Editors.ASPx.ASPxGridListEditor.MaxFilterControlHierarchyDepth = 3
            Editors.ASPx.ASPxCriteriaPropertyEditor.AllowFilterControlHierarchyDefault = True
            Editors.ASPx.ASPxCriteriaPropertyEditor.MaxHierarchyDepthDefault = 3
        End Sub

        Protected Overrides Sub CreateDefaultObjectSpaceProvider(ByVal args As CreateCustomObjectSpaceProviderEventArgs)
            args.ObjectSpaceProvider = New XPObjectSpaceProvider(GetDataStoreProvider(args.ConnectionString, args.Connection), True)
            args.ObjectSpaceProviders.Add(New NonPersistentObjectSpaceProvider(TypesInfo, Nothing))
        End Sub

        Private Function GetDataStoreProvider(ByVal connectionString As String, ByVal connection As System.Data.IDbConnection) As IXpoDataStoreProvider
            Dim application As System.Web.HttpApplicationState = If(System.Web.HttpContext.Current IsNot Nothing, System.Web.HttpContext.Current.Application, Nothing)
            Dim dataStoreProvider As IXpoDataStoreProvider = Nothing
            If application IsNot Nothing AndAlso application("DataStoreProvider") IsNot Nothing Then
                dataStoreProvider = TryCast(application("DataStoreProvider"), IXpoDataStoreProvider)
            Else
                dataStoreProvider = XPObjectSpaceProvider.GetDataStoreProvider(connectionString, connection, True)
                If application IsNot Nothing Then
                    application("DataStoreProvider") = dataStoreProvider
                End If
            End If

            Return dataStoreProvider
        End Function

        Private Sub CalculatedPropertiesSolutionAspNetApplication_DatabaseVersionMismatch(ByVal sender As Object, ByVal e As DatabaseVersionMismatchEventArgs)
#If EASYTEST
            e.Updater.Update();
            e.Handled = true;
#Else
            If System.Diagnostics.Debugger.IsAttached Then
                e.Updater.Update()
                e.Handled = True
            Else
                Dim message As String = "The application cannot connect to the specified database, " & "because the database doesn't exist, its version is older " & "than that of the application or its schema does not match " & "the ORM data model structure. To avoid this error, use one " & "of the solutions from the https://www.devexpress.com/kb=T367835 KB Article."
                If e.CompatibilityError IsNot Nothing AndAlso e.CompatibilityError.Exception IsNot Nothing Then
                    message += Microsoft.VisualBasic.Constants.vbCrLf & Microsoft.VisualBasic.Constants.vbCrLf & "Inner exception: " & e.CompatibilityError.Exception.Message
                End If

                Throw New InvalidOperationException(message)
            End If
#End If
        End Sub

        Private Sub InitializeComponent()
            module1 = New SystemModule.SystemModule()
            module2 = New SystemModule.SystemAspNetModule()
            module3 = New [Module].CalculatedPropertiesSolutionModule()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            ' 
            ' CalculatedPropertiesSolutionAspNetApplication
            ' 
            ApplicationName = "CalculatedPropertiesSolution"
            CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema
            Modules.Add(module1)
            Modules.Add(module2)
            Modules.Add(module3)
            AddHandler DatabaseVersionMismatch, New EventHandler(Of DatabaseVersionMismatchEventArgs)(AddressOf CalculatedPropertiesSolutionAspNetApplication_DatabaseVersionMismatch)
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        End Sub
    End Class
End Namespace
