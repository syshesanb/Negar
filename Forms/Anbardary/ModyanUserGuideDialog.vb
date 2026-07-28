Option Strict Off
Option Explicit On

Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports Negar.Business

Namespace Negar.Forms.Anbardary
    ''' <summary>
    ''' دیالوگ راهنمای جامـع کار با سامانه مودیان و پایانه‌های فروشگاهی
    ''' </summary>
    Public Class ModyanUserGuideDialog
        Inherits Form

        Private tabsGuide As TabControl
        Private tabStep1 As TabPage
        Private tabStep2 As TabPage
        Private tabStep3 As TabPage
        Private tabStep4 As TabPage
        Private tabStep5 As TabPage
        Private btnClose As Button

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub InitializeComponent()
            Me.Text = "📖 راهنمای جامع کار با سامانه مودیان و پایانه‌های فروشگاهی"
            Me.Size = New Size(920, 620)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.MinimizeBox = False
            Me.MaximizeBox = False
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.Font = New Font("Tahoma", 9.5!)
            Me.BackColor = Color.FromArgb(245, 247, 250)

            ' Top Header Banner
            Dim pnlHeader As New Panel()
            pnlHeader.Dock = DockStyle.Top
            pnlHeader.Height = 55
            pnlHeader.BackColor = Color.FromArgb(31, 78, 120)

            Dim lblTitle As New Label()
            lblTitle.Dock = DockStyle.Fill
            lblTitle.Text = "🏛️ راهنمای گام‌به‌گام و نحوه کار با سامانه مودیان در نرم‌افزار نگار"
            lblTitle.ForeColor = Color.White
            lblTitle.Font = New Font("B Yekan", 12.0!, FontStyle.Bold)
            lblTitle.TextAlign = ContentAlignment.MiddleCenter
            pnlHeader.Controls.Add(lblTitle)
            Me.Controls.Add(pnlHeader)

            ' Bottom Footer Panel
            Dim pnlFooter As New Panel()
            pnlFooter.Dock = DockStyle.Bottom
            pnlFooter.Height = 50
            pnlFooter.BackColor = Color.FromArgb(235, 240, 245)

            Me.btnClose = New Button()
            Me.btnClose.Text = "بستن راهنما"
            Me.btnClose.Size = New Size(120, 34)
            Me.btnClose.Location = New Point(15, 8)
            Me.btnClose.BackColor = Color.FromArgb(41, 128, 185)
            Me.btnClose.ForeColor = Color.White
            Me.btnClose.FlatStyle = FlatStyle.Flat
            Me.btnClose.FlatAppearance.BorderSize = 0
            AddHandler Me.btnClose.Click, Sub(s, e) Me.Close()
            pnlFooter.Controls.Add(Me.btnClose)
            Me.Controls.Add(pnlFooter)

            ' TabControl setup
            Me.tabsGuide = New TabControl()
            Me.tabsGuide.Dock = DockStyle.Fill
            Me.tabsGuide.Font = New Font("Tahoma", 9.5!)

            Me.tabStep1 = New TabPage("۱. قوانین و مشمولین")
            Me.tabStep2 = New TabPage("۲. کلیدها و حافظه مالیاتی")
            Me.tabStep3 = New TabPage("۳. شناسه ۱۳ رقمی کالا")
            Me.tabStep4 = New TabPage("۴. فاکتور نوع ۱ و ۲")
            Me.tabStep5 = New TabPage("۵. مراحل ارسال و استعلام")

            BuildStep1(Me.tabStep1)
            BuildStep2(Me.tabStep2)
            BuildStep3(Me.tabStep3)
            BuildStep4(Me.tabStep4)
            BuildStep5(Me.tabStep5)

            Me.tabsGuide.TabPages.Add(Me.tabStep1)
            Me.tabsGuide.TabPages.Add(Me.tabStep2)
            Me.tabsGuide.TabPages.Add(Me.tabStep3)
            Me.tabsGuide.TabPages.Add(Me.tabStep4)
            Me.tabsGuide.TabPages.Add(Me.tabStep5)

            Me.Controls.Add(Me.tabsGuide)
            pnlHeader.BringToFront()
        End Sub

        Private Sub BuildStep1(page As TabPage)
            Dim txt As New RichTextBox()
            txt.Dock = DockStyle.Fill
            txt.ReadOnly = True
            txt.BackColor = Color.White
            txt.Font = New Font("Tahoma", 10.0!)
            txt.RightToLeft = RightToLeft.Yes
            txt.BorderStyle = BorderStyle.None

            txt.AppendText("📌 گام ۱: آشنایی با سامانه مودیان و قوانین مشمولین" & vbCrLf & vbCrLf)
            txt.AppendText("• سامانه مودیان چیست؟" & vbCrLf)
            txt.AppendText("  سامانه مودیان یک سامانه آنلاین تحت مدیریت سازمان امور مالیاتی است که تمام کسب‌وکارها ملزم به ارسال صورتحساب‌های الکترونیکی خود به آن هستند." & vbCrLf & vbCrLf)
            txt.AppendText("• چه کسانی مشمول هستند؟" & vbCrLf)
            txt.AppendText("  ۱. تمامی اشخاص حقوقی (شرکت‌ها و موسسات ثبت‌شده) - الزام ۱۰۰٪ قانون." & vbCrLf)
            txt.AppendText("  ۲. تمامی مشاغل و اصناف (پزشکان، وکلا، فروشگاه‌ها، طلا‌فروشان، بنکداران و غیره)." & vbCrLf & vbCrLf)
            txt.AppendText("• تکالیف اصلی کاربر:" & vbCrLf)
            txt.AppendText("  - صدور و ارسال فاکتور الکترونیکی ظرف مهلت قانونی (حداکثر ۲۱ روز)." & vbCrLf)
            txt.AppendText("  - ثبت دقیق شناسه کالا/خدمت و نحوه تسویه (نقد/نسیه)." & vbCrLf)

            page.Controls.Add(txt)
        End Sub

        Private Sub BuildStep2(page As TabPage)
            Dim txt As New RichTextBox()
            txt.Dock = DockStyle.Fill
            txt.ReadOnly = True
            txt.BackColor = Color.White
            txt.Font = New Font("Tahoma", 10.0!)
            txt.RightToLeft = RightToLeft.Yes
            txt.BorderStyle = BorderStyle.None

            txt.AppendText("🔑 گام ۲: دریافت کلیدهای دیجیتال و شناسه یکتای حافظه مالیاتی" & vbCrLf & vbCrLf)
            txt.AppendText("برای فعال‌سازی سامانه مودیان در نرم‌افزار نگار، مراحل زیر را طی کنید:" & vbCrLf & vbCrLf)
            txt.AppendText("۱. ورود به کارپوشه مالیاتی:" & vbCrLf)
            txt.AppendText("   وارد سامانه my.tax.gov.ir شوید و پرونده مالیاتی خود را انتخاب کنید." & vbCrLf & vbCrLf)
            txt.AppendText("۲. ساخت فایل امضای دیجیتال (CSR):" & vbCrLf)
            txt.AppendText("   با مراجعه به دفاتر اسناد رسمی یا نرم‌افزار ساخت CSR، کلید عمومی (Public Key) و خصوصی (Private Key) دریافت کنید." & vbCrLf & vbCrLf)
            txt.AppendText("۳. دریافت شناسه یکتای حافظه مالیاتی (کد ۶ رقمی):" & vbCrLf)
            txt.AppendText("   در کارپوشه مالیاتی، بخش «شناسه‌های یکتای حافظه مالیاتی»، کلید عمومی را بارگذاری نموده و کد ۶ رقمی دریافت کنید." & vbCrLf & vbCrLf)
            txt.AppendText("۴. ثبت کد ۶ رقمی در نرم‌افزار نگار در دکمه «🔑 تنظیم کلیدها و حافظه مالیاتی»." & vbCrLf)

            page.Controls.Add(txt)
        End Sub

        Private Sub BuildStep3(page As TabPage)
            Dim txt As New RichTextBox()
            txt.Dock = DockStyle.Fill
            txt.ReadOnly = True
            txt.BackColor = Color.White
            txt.Font = New Font("Tahoma", 10.0!)
            txt.RightToLeft = RightToLeft.Yes
            txt.BorderStyle = BorderStyle.None

            txt.AppendText("🏷️ گام ۳: استخراج و نگاشت شناسه کالا و خدمت (Tax Code)" & vbCrLf & vbCrLf)
            txt.AppendText("هر کالا یا خدمتی که در فاکتور فروخته می‌شود باید دارای «شناسه مالیاتی ۱۳ رقمی» باشد:" & vbCrLf & vbCrLf)
            txt.AppendText("• شناسه عمومی کالا (General Tax Code):" & vbCrLf)
            txt.AppendText("  کدهای استاندارد عمومی که سازمان امور مالیاتی برای هر دسته‌بندی کالا ارائه داده است." & vbCrLf & vbCrLf)
            txt.AppendText("• شناسه اختصاصی کالا (Specific Tax Code):" & vbCrLf)
            txt.AppendText("  کد ۱۳ رقمی اختصاصی که واردکنندگان و تولیدکنندگان از سامانه جامع تجارت (ntsw.ir) دریافت می‌کنند." & vbCrLf & vbCrLf)
            txt.AppendText("• نحوه نگاشت در نگار:" & vbCrLf)
            txt.AppendText("  در فرم «تعریف کالاها»، در فیلد «کد عمومی/اختصاصی مودیان»، شناسه ۱۳ رقمی مربوطه را وارد نمایید." & vbCrLf)

            page.Controls.Add(txt)
        End Sub

        Private Sub BuildStep4(page As TabPage)
            Dim txt As New RichTextBox()
            txt.Dock = DockStyle.Fill
            txt.ReadOnly = True
            txt.BackColor = Color.White
            txt.Font = New Font("Tahoma", 10.0!)
            txt.RightToLeft = RightToLeft.Yes
            txt.BorderStyle = BorderStyle.None

            txt.AppendText("📄 گام ۴: تفاوت صورتحساب نوع ۱ و نوع ۲" & vbCrLf & vbCrLf)
            txt.AppendText("۱. صورتحساب نوع ۱ (رسمی B2B - شرکتی/عمده):" & vbCrLf)
            txt.AppendText("   - مخصوص فروش به شرکت‌ها، اشخاص حقوقی و خریداران عمده." & vbCrLf)
            txt.AppendText("   - نیازمند درج کامل: شناسه ملی/کد ملی خریدار، کد اقتصادی، آدرس و نحوه تسویه (نقد/نسیه)." & vbCrLf)
            txt.AppendText("   - خریدار می‌تواند از اعتبار مالیات بر ارزش افزوده آن استفاده کند." & vbCrLf & vbCrLf)
            txt.AppendText("۲. صورتحساب نوع ۲ (فروشگاهی B2C - سر صندوق):" & vbCrLf)
            txt.AppendText("   - مخصوص فروش به مصرف‌کننده نهایی در صندوق فروشگاه (POS)." & vbCrLf)
            txt.AppendText("   - نیازی به دریافت کد ملی یا مشخصات خریدار ندارد." & vbCrLf)
            txt.AppendText("   - تسویه حساب معمولاً به صورت نقد یا دستگاه کارتخوان انجام می‌شود." & vbCrLf)

            page.Controls.Add(txt)
        End Sub

        Private Sub BuildStep5(page As TabPage)
            Dim txt As New RichTextBox()
            txt.Dock = DockStyle.Fill
            txt.ReadOnly = True
            txt.BackColor = Color.White
            txt.Font = New Font("Tahoma", 10.0!)
            txt.RightToLeft = RightToLeft.Yes
            txt.BorderStyle = BorderStyle.None

            txt.AppendText("🚀 گام ۵: نحوه ارسال و پیگیری صورتحساب‌ها در کارپوشه" & vbCrLf & vbCrLf)
            txt.AppendText("۱. صدور فاکتور در نرم‌افزار نگار (از فرم فاکتور فروش یا فروش سریع POS)." & vbCrLf)
            txt.AppendText("۲. ورود به تب «🏛️ سامانه مودیان» و انتخاب فاکتورهای آماده ارسال." & vbCrLf)
            txt.AppendText("۳. کلیک روی دکمه «🚀 ارسال به سامانه مودیان»:" & vbCrLf)
            txt.AppendText("   نرم‌افزار نگار به صورت خودکار فاکتور را امضای دیجیتال نموده، شماره مالیاتی ۲۲ رقمی اختصاص داده و به سازمان ارسال می‌کند." & vbCrLf & vbCrLf)
            txt.AppendText("۴. استعلام وضعیت:" & vbCrLf)
            txt.AppendText("   با کلیک روی دکمه «🔄 استعلام وضعیت»، آخرین وضعیت فاکتور (تاییدشده / دارای خطا) در کارپوشه استعلام می‌شود." & vbCrLf)

            page.Controls.Add(txt)
        End Sub
    End Class
End Namespace
