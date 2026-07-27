import sqlite3
import os

db_path = r'C:\Negar\Database\Negar.db'
conn = sqlite3.connect(db_path)
cur = conn.cursor()

print("=== Cod_Standard ===")
cur.execute("SELECT AccountID, AccountCode, ParentAccountID FROM Cod_Standard LIMIT 15;")
for row in cur.fetchall():
    print(row)

print("=== SarfaslHesab for CompanyID 13 ===")
cur.execute("SELECT AccountID, CompanyID, AccountCode, ParentAccountID FROM SarfaslHesab WHERE CompanyID = 13 LIMIT 15;")
for row in cur.fetchall():
    print(row)

conn.close()
