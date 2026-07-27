Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports Negar.Data

Namespace Negar.Business
    Public Class CatalogService
        Public Function GetProducts() As DataTable
            Dim compId = SessionContext.CurrentCompanyID
            If compId.HasValue Then
                Return Sql.ExecuteTable("SELECT p.ProductID, p.ProductCode, p.ProductName, p.Unit, p.DefaultPrice, p.Category, p.IsActive, p.DefaultWarehouseID, w.WarehouseName AS DefaultWarehouseName, p.TaxPercent FROM Products p LEFT JOIN Warehouses w ON p.DefaultWarehouseID = w.WarehouseID WHERE p.CompanyID = ? ORDER BY p.ProductName", compId.Value)
            Else
                Return Sql.ExecuteTable("SELECT p.ProductID, p.ProductCode, p.ProductName, p.Unit, p.DefaultPrice, p.Category, p.IsActive, p.DefaultWarehouseID, w.WarehouseName AS DefaultWarehouseName, p.TaxPercent FROM Products p LEFT JOIN Warehouses w ON p.DefaultWarehouseID = w.WarehouseID ORDER BY p.ProductName")
            End If
        End Function

        Public Function SaveProduct(productId As Integer?, code As String, name As String, unit As String, defaultPrice As Decimal,
                                    category As String, isActive As Boolean, baseUoMId As Object, secondaryUoMId As Object,
                                    nominalFactor As Object, productGroupId As Object, barcode As String, taxId As String,
                                    productType As String, purchasePrice As Decimal, minStock As Decimal, reorderPoint As Decimal,
                                    maxStock As Decimal, trackingType As String, locationId As Integer?, technicalName As String,
                                    Optional consumerMarkup As Decimal = 0, Optional consumerDiscount As Decimal = 0,
                                    Optional colleagueMarkup As Decimal = 0, Optional colleagueDiscount As Decimal = 0,
                                    Optional wholesaleMarkup As Decimal = 0, Optional wholesaleDiscount As Decimal = 0,
                                    Optional taxPercent As Decimal = 0, Optional tollPercent As Decimal = 0,
                                    Optional netWeight As Decimal = 0, Optional grossWeight As Decimal = 0,
                                    Optional length As Decimal = 0, Optional width As Decimal = 0, Optional height As Decimal = 0,
                                    Optional volume As Decimal = 0, Optional color As String = "", Optional material As String = "",
                                    Optional size As String = "", Optional brand As String = "", Optional countryOfOrigin As String = "",
                                    Optional physicalDescription As String = "", Optional image1 As String = "",
                                    Optional image2 As String = "", Optional image3 As String = "", Optional image4 As String = "",
                                    Optional image5 As String = "", Optional image6 As String = "",
                                    Optional defaultWarehouseId As Object = Nothing) As Integer

            Dim activeVal = If(isActive, 1, 0)
            Dim defWhVal = If(defaultWarehouseId IsNot Nothing AndAlso Not Convert.IsDBNull(defaultWarehouseId), defaultWarehouseId, DBNull.Value)
            Dim compIdVal As Object = If(SessionContext.CurrentCompanyID.HasValue, SessionContext.CurrentCompanyID.Value, DBNull.Value)

            If productId.HasValue AndAlso productId.Value > 0 Then
                Sql.ExecuteNonQuery("UPDATE Products SET ProductCode = ?, ProductName = ?, Unit = ?, DefaultPrice = ?, Category = ?, IsActive = ?, " &
                                    "BaseUoMID = ?, SecondaryUoMID = ?, NominalFactor = ?, ProductGroupID = ?, Barcode = ?, TaxID = ?, " &
                                    "ProductType = ?, PurchasePrice = ?, MinStock = ?, ReorderPoint = ?, MaxStock = ?, TrackingType = ?, LocationID = ?, TechnicalName = ?, " &
                                    "ConsumerMarkup = ?, ConsumerDiscount = ?, ColleagueMarkup = ?, ColleagueDiscount = ?, " &
                                    "WholesaleMarkup = ?, WholesaleDiscount = ?, TaxPercent = ?, TollPercent = ?, " &
                                    "NetWeight = ?, GrossWeight = ?, Length = ?, Width = ?, Height = ?, Volume = ?, " &
                                    "Color = ?, Material = ?, Size = ?, Brand = ?, CountryOfOrigin = ?, PhysicalDescription = ?, " &
                                    "Image1 = ?, Image2 = ?, Image3 = ?, Image4 = ?, Image5 = ?, Image6 = ?, DefaultWarehouseID = ?, CompanyID = COALESCE(CompanyID, ?) " &
                                    "WHERE ProductID = ?",
                                    code, name, unit, defaultPrice, category, activeVal,
                                    baseUoMId, secondaryUoMId, nominalFactor, productGroupId, barcode, taxId,
                                    productType, purchasePrice, minStock, reorderPoint, maxStock, trackingType, If(locationId, DBNull.Value), technicalName,
                                    consumerMarkup, consumerDiscount, colleagueMarkup, colleagueDiscount,
                                    wholesaleMarkup, wholesaleDiscount, taxPercent, tollPercent,
                                    netWeight, grossWeight, length, width, height, volume,
                                    color, material, size, brand, countryOfOrigin, physicalDescription,
                                    image1, image2, image3, image4, image5, image6, defWhVal, compIdVal, productId.Value)
                Return productId.Value
            Else
                Return Sql.ExecuteIdentity("INSERT INTO Products (ProductCode, ProductName, Unit, DefaultPrice, Category, IsActive, " &
                                       "BaseUoMID, SecondaryUoMID, NominalFactor, ProductGroupID, Barcode, TaxID, " &
                                       "ProductType, PurchasePrice, MinStock, ReorderPoint, MaxStock, TrackingType, LocationID, TechnicalName, " &
                                       "ConsumerMarkup, ConsumerDiscount, ColleagueMarkup, ColleagueDiscount, " &
                                       "WholesaleMarkup, WholesaleDiscount, TaxPercent, TollPercent, " &
                                       "NetWeight, GrossWeight, Length, Width, Height, Volume, " &
                                       "Color, Material, Size, Brand, CountryOfOrigin, PhysicalDescription, " &
                                       "Image1, Image2, Image3, Image4, Image5, Image6, DefaultWarehouseID, CompanyID) " &
                                       "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                                       code, name, unit, defaultPrice, category, activeVal,
                                       baseUoMId, secondaryUoMId, nominalFactor, productGroupId, barcode, taxId,
                                       productType, purchasePrice, minStock, reorderPoint, maxStock, trackingType, If(locationId, DBNull.Value), technicalName,
                                       consumerMarkup, consumerDiscount, colleagueMarkup, colleagueDiscount,
                                       wholesaleMarkup, wholesaleDiscount, taxPercent, tollPercent,
                                       netWeight, grossWeight, length, width, height, volume,
                                       color, material, size, brand, countryOfOrigin, physicalDescription,
                                       image1, image2, image3, image4, image5, image6, defWhVal, compIdVal)
            End If
        End Function

        Public Function GetProductById(productId As Integer) As DataRow
            Dim sqlText = "SELECT p.ProductID, p.ProductCode, p.ProductName, p.Unit, p.DefaultPrice, p.Category, p.IsActive, " &
                          "p.BaseUoMID, p.SecondaryUoMID, p.NominalFactor, p.ProductGroupID, p.Barcode, p.TaxID, " &
                          "p.ProductType, p.PurchasePrice, p.MinStock, p.ReorderPoint, p.MaxStock, p.TrackingType, p.LocationID, p.TechnicalName, " &
                          "p.ConsumerMarkup, p.ConsumerDiscount, p.ColleagueMarkup, p.ColleagueDiscount, " &
                          "p.WholesaleMarkup, p.WholesaleDiscount, p.TaxPercent, p.TollPercent, " &
                          "p.NetWeight, p.GrossWeight, p.Length, p.Width, p.Height, p.Volume, " &
                          "p.Color, p.Material, p.Size, p.Brand, p.CountryOfOrigin, p.PhysicalDescription, " &
                          "p.Image1, p.Image2, p.Image3, p.Image4, p.Image5, p.Image6, p.DefaultWarehouseID, p.CompanyID " &
                          "FROM Products p WHERE p.ProductID = ?"
            Dim compId = SessionContext.CurrentCompanyID
            If compId.HasValue Then
                sqlText &= " AND p.CompanyID = ?"
                Dim dtComp = Sql.ExecuteTable(sqlText, productId, compId.Value)
                If dtComp.Rows.Count > 0 Then Return dtComp.Rows(0)
                Return Nothing
            End If

            Dim dt = Sql.ExecuteTable(sqlText, productId)
            If dt.Rows.Count > 0 Then Return dt.Rows(0)
            Return Nothing
        End Function

        Public Sub DeleteProduct(productId As Integer)
            Dim compId = SessionContext.CurrentCompanyID
            If compId.HasValue Then
                Sql.ExecuteNonQuery("DELETE FROM Products WHERE ProductID = ? AND CompanyID = ?", productId, compId.Value)
            Else
                Sql.ExecuteNonQuery("DELETE FROM Products WHERE ProductID = ?", productId)
            End If
        End Sub

        Public Function GetWarehouses() As DataTable
            Dim compId = SessionContext.CurrentCompanyID
            If compId.HasValue Then
                Return Sql.ExecuteTable("SELECT *, WarehouseID || ' - ' || WarehouseName AS DisplayTitle FROM Warehouses WHERE CompanyID = ? ORDER BY WarehouseName", compId.Value)
            Else
                Return Sql.ExecuteTable("SELECT *, WarehouseID || ' - ' || WarehouseName AS DisplayTitle FROM Warehouses ORDER BY WarehouseName")
            End If
        End Function

        Public Function GetWarehouseById(warehouseId As Integer) As DataRow
            Dim dt = Sql.ExecuteTable("SELECT * FROM Warehouses WHERE WarehouseID = ?", warehouseId)
            If dt.Rows.Count > 0 Then Return dt.Rows(0)
            Return Nothing
        End Function

        Public Function GetNextProductCode() As String
            Dim compId = SessionContext.CurrentCompanyID
            Dim dt As DataTable
            If compId.HasValue Then
                dt = Sql.ExecuteTable("SELECT MAX(CAST(ProductCode AS INTEGER)) as MaxCode FROM Products WHERE CompanyID = ? AND ProductCode GLOB '[0-9]*'", compId.Value)
            Else
                dt = Sql.ExecuteTable("SELECT MAX(CAST(ProductCode AS INTEGER)) as MaxCode FROM Products WHERE ProductCode GLOB '[0-9]*'")
            End If

            If dt.Rows.Count > 0 AndAlso Not dt.Rows(0).IsNull("MaxCode") Then
                Dim maxCode = Convert.ToInt32(dt.Rows(0)("MaxCode"))
                Return (maxCode + 1).ToString()
            End If
            Return "1001"
        End Function

        Public Function GetWarehouseTypes() As DataTable
            Return Sql.ExecuteTable("SELECT TypeID, TypeName FROM WarehouseTypes ORDER BY TypeName")
        End Function

        Public Function SaveWarehouseType(typeId As Integer?, typeName As String) As Integer
            If typeId.HasValue AndAlso typeId.Value > 0 Then
                Sql.ExecuteNonQuery("UPDATE WarehouseTypes SET TypeName = ? WHERE TypeID = ?", typeName, typeId.Value)
                Return typeId.Value
            End If
            Return Sql.ExecuteIdentity("INSERT INTO WarehouseTypes (TypeName) VALUES (?)", typeName)
        End Function

        Public Function SaveWarehouse(warehouseId As Integer?, warehouseName As String, location As String, isActive As Boolean,
                                      warehouseType As String, phone As String, phone2 As String, phone3 As String,
                                      postalCode As String, capacity As Double, warehouseKeeper As String,
                                      costCenter As String, allowNegative As Boolean, description As String) As Integer

            Dim activeVal = If(isActive, 1, 0)
            Dim allowNegVal = If(allowNegative, 1, 0)
            Dim compIdVal As Object = If(SessionContext.CurrentCompanyID.HasValue, SessionContext.CurrentCompanyID.Value, DBNull.Value)

            If warehouseId.HasValue AndAlso warehouseId.Value > 0 Then
                Sql.ExecuteNonQuery("UPDATE Warehouses SET WarehouseName = ?, Location = ?, IsActive = ?, " &
                                    "WarehouseType = ?, Phone = ?, Phone2 = ?, Phone3 = ?, PostalCode = ?, " &
                                    "Capacity = ?, WarehouseKeeper = ?, CostCenter = ?, AllowNegativeStock = ?, " &
                                    "Description = ?, CompanyID = COALESCE(CompanyID, ?) WHERE WarehouseID = ?",
                                    warehouseName, location, activeVal, warehouseType, phone, phone2, phone3,
                                    postalCode, capacity, warehouseKeeper, costCenter, allowNegVal, description,
                                    compIdVal, warehouseId.Value)
                Return warehouseId.Value
            End If

            Return Sql.ExecuteIdentity("INSERT INTO Warehouses (WarehouseName, Location, IsActive, WarehouseType, Phone, Phone2, Phone3, PostalCode, Capacity, WarehouseKeeper, CostCenter, AllowNegativeStock, Description, CompanyID) " &
                                       "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                                       warehouseName, location, activeVal, warehouseType, phone, phone2, phone3,
                                       postalCode, capacity, warehouseKeeper, costCenter, allowNegVal, description, compIdVal)
        End Function

        Public Sub DeleteWarehouse(warehouseId As Integer)
            Sql.ExecuteNonQuery("DELETE FROM Warehouses WHERE WarehouseID = ?", warehouseId)
        End Sub

        Public Sub DeleteWarehouseType(typeId As Integer)
            Sql.ExecuteNonQuery("DELETE FROM WarehouseTypes WHERE TypeID = ?", typeId)
        End Sub

        Public Function GetWarehouseLocations(warehouseId As Integer) As DataTable
            Return Sql.ExecuteTable("SELECT LocationID, WarehouseID, ParentID, LocationType, Title, Code FROM WarehouseLocations WHERE WarehouseID = ? ORDER BY LocationType, LocationID", warehouseId)
        End Function

        Public Function SaveWarehouseLocation(locationId As Integer?, warehouseId As Integer, parentId As Integer?, locationType As Integer, title As String, code As String) As Integer
            If locationId.HasValue AndAlso locationId.Value > 0 Then
                Sql.ExecuteNonQuery("UPDATE WarehouseLocations SET Title = ?, Code = ? WHERE LocationID = ?", title, code, locationId.Value)
                Return locationId.Value
            End If
            Return Sql.ExecuteIdentity("INSERT INTO WarehouseLocations (WarehouseID, ParentID, LocationType, Title, Code) VALUES (?, ?, ?, ?, ?)", warehouseId, If(parentId, DBNull.Value), locationType, title, code)
        End Function

        Public Sub DeleteWarehouseLocation(locationId As Integer)
            Sql.ExecuteNonQuery("DELETE FROM WarehouseLocations WHERE LocationID = ?", locationId)
        End Sub

        Public Function GenerateNextLocationCode(warehouseId As Integer, locationType As Integer) As String
            Dim prefix As String = ""
            Select Case locationType
                Case 1 : prefix = "SAL"
                Case 2 : prefix = "BAK"
                Case 3 : prefix = "RAH"
                Case 4 : prefix = "GAF"
                Case 5 : prefix = "RAD"
                Case 6 : prefix = "BOX"
            End Select

            Dim dt = Sql.ExecuteTable("SELECT Code FROM WarehouseLocations WHERE WarehouseID = ? AND LocationType = ?", warehouseId, locationType)
            Dim maxIdx As Integer = 0
            For Each row As DataRow In dt.Rows
                Dim c = Convert.ToString(row("Code"))
                If c.StartsWith(prefix & "-") Then
                    Dim numPart = c.Substring(prefix.Length + 1)
                    Dim num As Integer
                    If Integer.TryParse(numPart, num) Then
                        If num > maxIdx Then maxIdx = num
                    End If
                End If
            Next
            Return prefix & "-" & (maxIdx + 1).ToString()
        End Function

        Public Function GetLocationPath(locationId As Integer) As Tuple(Of String, String)
            Dim titlePath As New System.Collections.Generic.List(Of String)()
            Dim codePath As New System.Collections.Generic.List(Of String)()

            Dim curId As Integer? = locationId
            While curId.HasValue
                Dim dt = Sql.ExecuteTable("SELECT ParentID, Title, Code FROM WarehouseLocations WHERE LocationID = ?", curId.Value)
                If dt.Rows.Count > 0 Then
                    Dim row = dt.Rows(0)
                    titlePath.Insert(0, Convert.ToString(row("Title")))
                    codePath.Insert(0, Convert.ToString(row("Code")))
                    curId = If(row.IsNull("ParentID"), CType(Nothing, Integer?), Convert.ToInt32(row("ParentID")))
                Else
                    curId = Nothing
                End If
            End While

            Return New Tuple(Of String, String)(String.Join(" > ", titlePath), String.Join(" > ", codePath))
        End Function
    End Class
End Namespace
