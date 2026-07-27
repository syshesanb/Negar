-- Migration script v001_ensure_schema_updates.sql
-- این اسکریپت ساختار جداول را در دیتابیس‌های قبلی بروزرسانی می‌کند.

CREATE TABLE IF NOT EXISTS AppSettings (
    SettingID INTEGER PRIMARY KEY AUTOINCREMENT,
    SettingKey TEXT UNIQUE NOT NULL,
    SettingValue TEXT,
    SettingCategory TEXT
);

CREATE TABLE IF NOT EXISTS BackgroundImages (
    ImageID INTEGER PRIMARY KEY AUTOINCREMENT,
    ImageName TEXT,
    ImageData BLOB,
    CreatedDate DATETIME
);
