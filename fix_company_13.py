import sqlite3
import os

db_path = r'C:\Negar\Database\Negar.db'
conn = sqlite3.connect(db_path)
cur = conn.cursor()

# 1. Clear SarfaslHesab for Company 13 where ParentAccountID is 0 or invalid
cur.execute("DELETE FROM SarfaslHesab WHERE CompanyID = 13;")
conn.commit()

# 2. Get Cod_Standard
cur.execute("SELECT AccountID, AccountCode, AccountName, AccountType, ParentAccountID, IsActive, AccountNature FROM Cod_Standard ORDER BY AccountID;")
std_rows = cur.fetchall()

std_to_new_map = {}

for r in std_rows:
    std_id, code, name, a_type, std_parent_id, is_active, nature = r
    
    new_parent_id = None
    if std_parent_id is not None and std_parent_id in std_to_new_map:
        new_parent_id = std_to_new_map[std_parent_id]
        
    cur.execute("""
        INSERT INTO SarfaslHesab (CompanyID, AccountCode, AccountName, AccountType, ParentAccountID, IsActive, AccountNature)
        VALUES (13, ?, ?, ?, ?, ?, ?);
    """, (code, name, a_type, new_parent_id, is_active, nature))
    
    new_id = cur.lastrowid
    std_to_new_map[std_id] = new_id

conn.commit()
print("Successfully populated Company 13 SarfaslHesab with correct ParentAccountIDs!")
conn.close()
