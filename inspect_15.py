import sqlite3
import os

db_path = r'C:\Negar\Database\Negar.db'
conn = sqlite3.connect(db_path)
cur = conn.cursor()

print("=== SarfaslHesab for CompanyID 15 ===")
cur.execute("SELECT AccountID, CompanyID, AccountCode, ParentAccountID FROM SarfaslHesab WHERE CompanyID = 15 LIMIT 15;")
for row in cur.fetchall():
    print(row)

conn.close()
