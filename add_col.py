import sqlite3
import os

for root, dirs, files in os.walk(r'C:\Negar'):
    for f in files:
        if f.endswith('.db'):
            path = os.path.join(root, f)
            try:
                conn = sqlite3.connect(path)
                cur = conn.cursor()
                cur.execute("PRAGMA table_info(Companies);")
                cols = [c[1] for c in cur.fetchall()]
                if 'CodingType' not in cols:
                    cur.execute("ALTER TABLE Companies ADD COLUMN CodingType TEXT;")
                    conn.commit()
                    print('Added CodingType column to:', path)
                else:
                    print('CodingType already exists in:', path)
                conn.close()
            except Exception as e:
                print('Error in', path, ':', e)
