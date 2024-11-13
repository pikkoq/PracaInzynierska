import pyodbc
import pandas as pd
import os

os.chdir(os.path.dirname(os.path.abspath(__file__)))
conn = pyodbc.connect(
    r'DRIVER={SQL Server};'
    r'SERVER=PIKKO\SQLEXPRESS;'
    r'DATABASE=ShoeBoardDataBase;'
    r'Trusted_Connection=yes;'
)

def import_json_to_table(file_name, table_name):
    df = pd.read_json(file_name)
    cursor = conn.cursor()
    
    for index, row in df.iterrows():
        access_failed_count = int(row["AccessFailedCount"]) if pd.notna(row["AccessFailedCount"]) else None
        lockout_end = row["LockoutEnd"] if pd.notna(row["LockoutEnd"]) else None
        phone_number = row["PhoneNumber"] if pd.notna(row["PhoneNumber"]) else None

        cursor.execute(f"""
            INSERT INTO {table_name} (
                Id, DateCreated, ProfilePicturePath, Bio, UserName, 
                NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, 
                PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, 
                PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount
            ) 
            VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        """, 
        row["Id"], row["DateCreated"], row["ProfilePicturePath"], row["Bio"], row["UserName"],
        row["NormalizedUserName"], row["Email"], row["NormalizedEmail"], row["EmailConfirmed"],
        row["PasswordHash"], row["SecurityStamp"], row["ConcurrencyStamp"], 
        phone_number,
        row["PhoneNumberConfirmed"], row["TwoFactorEnabled"], 
        lockout_end,
        row["LockoutEnabled"], access_failed_count)
        
    conn.commit()
    print(f"Data from {file_name} imported to {table_name}")

import_json_to_table("AspNetUsers.json", "AspNetUsers")
conn.close()