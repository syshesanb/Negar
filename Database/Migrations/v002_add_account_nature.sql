-- Migration script v002_add_account_nature.sql
-- اضافه کردن فیلد ماهیت مانده حساب به جدول سرفصل حساب‌ها

ALTER TABLE SarfaslHesab ADD COLUMN AccountNature TEXT;
