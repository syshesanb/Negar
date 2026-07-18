Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports Sys_Hes_Anb.Data

Namespace Sys_Hes_Anb.Business
    Public Class UnitOfMeasureService

        ' ══════════════════════════════════════════════════════════════════════
        '  مدیریت دسته‌بندی‌ها (UoM Categories)
        ' ══════════════════════════════════════════════════════════════════════

        ''' <summary>دریافت لیست تمام دسته‌بندی‌ها</summary>
        Public Function GetCategories() As DataTable
            Return Sql.ExecuteTable("SELECT CategoryID, CategoryName, CreatedDate FROM uom_categories ORDER BY CategoryName")
        End Function

        ''' <summary>ذخیره یا ویرایش دسته‌بندی</summary>
        Public Function SaveCategory(categoryId As Integer?, name As String) As Integer
            If String.IsNullOrWhiteSpace(name) Then
                Throw New ArgumentException("نام دسته‌بندی الزامی است.")
            End If

            If categoryId.HasValue AndAlso categoryId.Value > 0 Then
                Sql.ExecuteNonQuery("UPDATE uom_categories SET CategoryName = ? WHERE CategoryID = ?", name.Trim(), categoryId.Value)
                Return categoryId.Value
            End If

            Return Sql.ExecuteIdentity("INSERT INTO uom_categories (CategoryName) VALUES (?)", name.Trim())
        End Function

        ' ══════════════════════════════════════════════════════════════════════
        '  مدیریت واحدهای اندازه‌گیری (UoMs)
        ' ══════════════════════════════════════════════════════════════════════

        ''' <summary>دریافت تمام واحدهای اندازه‌گیری همراه با نام گروه</summary>
        Public Function GetAll() As DataTable
            Return Sql.ExecuteTable(
                "SELECT u.UoMID, u.CategoryID, c.CategoryName, u.UoMName, u.Abbreviation, " &
                "u.IsReferenceUoM, u.ConversionNumerator, u.ConversionDenominator, u.IsActive " &
                "FROM uoms u INNER JOIN uom_categories c ON u.CategoryID = c.CategoryID " &
                "ORDER BY c.CategoryName, u.UoMName")
        End Function

        ''' <summary>دریافت اطلاعات یک واحد اندازه‌گیری بر اساس شناسه</summary>
        Public Function GetById(uomId As Integer) As DataRow
            Dim dt = Sql.ExecuteTable(
                "SELECT UoMID, CategoryID, UoMName, Abbreviation, IsReferenceUoM, " &
                "ConversionNumerator, ConversionDenominator, IsActive " &
                "FROM uoms WHERE UoMID = ?", uomId)
            If dt.Rows.Count > 0 Then Return dt.Rows(0)
            Return Nothing
        End Function

        ''' <summary>دریافت لیست واحدهای فعال</summary>
        Public Function GetActive() As DataTable
            Return Sql.ExecuteTable(
                "SELECT u.UoMID, u.CategoryID, c.CategoryName, u.UoMName, u.Abbreviation " &
                "FROM uoms u INNER JOIN uom_categories c ON u.CategoryID = c.CategoryID " &
                "WHERE u.IsActive = 1 ORDER BY c.CategoryName, u.UoMName")
        End Function

        ''' <summary>دریافت لیست واحدهای فعال یک دسته‌بندی خاص</summary>
        Public Function GetActiveByCategory(categoryId As Integer) As DataTable
            Return Sql.ExecuteTable(
                "SELECT UoMID, UoMName, Abbreviation, IsReferenceUoM, ConversionNumerator, ConversionDenominator " &
                "FROM uoms WHERE CategoryID = ? AND IsActive = 1 ORDER BY UoMName", categoryId)
        End Function

        ''' <summary>دریافت واحد مرجع یک دسته‌بندی</summary>
        Public Function GetReferenceUoM(categoryId As Integer) As DataRow
            Dim dt = Sql.ExecuteTable(
                "SELECT UoMID, UoMName, Abbreviation FROM uoms " &
                "WHERE CategoryID = ? AND IsReferenceUoM = 1 AND IsActive = 1", categoryId)
            If dt.Rows.Count > 0 Then Return dt.Rows(0)
            Return Nothing
        End Function

        ''' <summary>ذخیره یا ویرایش واحد اندازه‌گیری</summary>
        Public Function Save(uomId As Integer?, categoryId As Integer, name As String, abbreviation As String,
                             isReferenceUoM As Boolean, numerator As Integer, denominator As Integer, isActive As Boolean) As Integer
            If String.IsNullOrWhiteSpace(name) Then
                Throw New ArgumentException("نام واحد اندازه‌گیری الزامی است.")
            End If

            If categoryId <= 0 Then
                Throw New ArgumentException("انتخاب دسته‌بندی معتبر الزامی است.")
            End If

            If numerator <= 0 OrElse denominator <= 0 Then
                Throw New ArgumentException("صورت و مخرج کسر ضریب تبدیل باید بزرگتر از صفر باشند.")
            End If

            ' اگر این واحد به عنوان واحد مرجع ثبت می‌شود، بقیه واحدهای این گروه را غیر مرجع می‌کنیم
            If isReferenceUoM Then
                numerator = 1
                denominator = 1
                Dim excludeUomId = If(uomId.HasValue, uomId.Value, -1)
                Sql.ExecuteNonQuery("UPDATE uoms SET IsReferenceUoM = 0 WHERE CategoryID = ? AND UoMID <> ?", categoryId, excludeUomId)
            Else
                ' بررسی اینکه آیا این گروه واحد مرجع دارد؟ اگر ندارد و این اولین واحد است، آن را مرجع می‌کنیم
                Dim refCount = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM uoms WHERE CategoryID = ? AND IsReferenceUoM = 1", categoryId), 0))
                If refCount = 0 Then
                    isReferenceUoM = True
                    numerator = 1
                    denominator = 1
                End If
            End If

            If uomId.HasValue AndAlso uomId.Value > 0 Then
                Sql.ExecuteNonQuery(
                    "UPDATE uoms SET CategoryID = ?, UoMName = ?, Abbreviation = ?, IsReferenceUoM = ?, " &
                    "ConversionNumerator = ?, ConversionDenominator = ?, IsActive = ? WHERE UoMID = ?",
                    categoryId, name.Trim(), If(abbreviation, ""), If(isReferenceUoM, 1, 0), numerator, denominator, If(isActive, 1, 0), uomId.Value)
                Return uomId.Value
            End If

            Return Sql.ExecuteIdentity(
                "INSERT INTO uoms (CategoryID, UoMName, Abbreviation, IsReferenceUoM, ConversionNumerator, ConversionDenominator, IsActive) " &
                "VALUES (?, ?, ?, ?, ?, ?, ?)",
                categoryId, name.Trim(), If(abbreviation, ""), If(isReferenceUoM, 1, 0), numerator, denominator, If(isActive, 1, 0))
        End Function

        ''' <summary>حذف واحد اندازه‌گیری در صورت عدم استفاده در محصولات</summary>
        Public Sub Delete(uomId As Integer)
            ' بررسی استفاده در محصولات به عنوان واحد پایه یا ثانویه
            Dim usageCount = Convert.ToInt32(If(Sql.ExecuteScalar(
                "SELECT COUNT(*) FROM Products WHERE BaseUoMID = ? OR SecondaryUoMID = ?", uomId, uomId), 0))
            If usageCount > 0 Then
                Throw New InvalidOperationException("این واحد اندازه‌گیری در شناسنامه " & usageCount & " کالا استفاده شده است و قابل حذف نیست.")
            End If

            ' بررسی استفاده در ضرایب تبدیل کالا
            Dim convUsage = Convert.ToInt32(If(Sql.ExecuteScalar(
                "SELECT COUNT(*) FROM product_uom_conversions WHERE FromUoMID = ? OR ToUoMID = ?", uomId, uomId), 0))
            If convUsage > 0 Then
                Throw New InvalidOperationException("این واحد اندازه‌گیری در جدول ضرایب تبدیل کالاها استفاده شده است و قابل حذف نیست.")
            End If

            ' بررسی اینکه آیا این واحد تنها واحد مرجع گروه است و واحدهای دیگری در این گروه وجود دارند؟
            Dim row = GetById(uomId)
            If row IsNot Nothing AndAlso Convert.ToBoolean(row("IsReferenceUoM")) Then
                Dim catId = Convert.ToInt32(row("CategoryID"))
                Dim siblings = Convert.ToInt32(If(Sql.ExecuteScalar("SELECT COUNT(*) FROM uoms WHERE CategoryID = ? AND UoMID <> ?", catId, uomId), 0))
                If siblings > 0 Then
                    Throw New InvalidOperationException("این واحد، واحد مرجع دسته‌بندی است و به دلیل وجود واحدهای دیگر در این گروه، ابتدا باید واحد مرجع دیگری تعیین نمایید.")
                End If
            End If

            Sql.ExecuteNonQuery("DELETE FROM uoms WHERE UoMID = ?", uomId)
        End Sub

        ''' <summary>جستجو در واحدها</summary>
        Public Function Search(keyword As String) As DataTable
            Dim kw = "%" & keyword & "%"
            Return Sql.ExecuteTable(
                "SELECT u.UoMID, u.CategoryID, c.CategoryName, u.UoMName, u.Abbreviation, " &
                "u.IsReferenceUoM, u.ConversionNumerator, u.ConversionDenominator, u.IsActive " &
                "FROM uoms u INNER JOIN uom_categories c ON u.CategoryID = c.CategoryID " &
                "WHERE u.UoMName LIKE ? OR u.Abbreviation LIKE ? OR c.CategoryName LIKE ? " &
                "ORDER BY c.CategoryName, u.UoMName", kw, kw, kw)
        End Function

    End Class
End Namespace
