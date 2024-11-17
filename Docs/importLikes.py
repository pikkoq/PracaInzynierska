import pyodbc
import pandas as pd
import os

os.chdir(os.path.dirname(os.path.abspath(__file__)))
conn = pyodbc.connect(
    r'DRIVER={SQL Server};'
    r'SERVER=PIKKOLAPTOP\SQLEXPRESS;'
    r'DATABASE=ShoeBoardDataBase;'
    r'Trusted_Connection=yes;'
)

def import_json_to_table(file_name, table_name):
    df = pd.read_json(file_name)
    cursor = conn.cursor()
    
    for index, row in df.iterrows():
        

        cursor.execute(f"""
            INSERT INTO {table_name} (
                 PostId, UserId
            ) 
            VALUES ( ?, ?)
        """, 
         row["PostId"], row["UserId"]
        )        
    conn.commit()
    print(f"Data from {file_name} imported to {table_name}")

import_json_to_table("Likes.json", "Likes")
conn.close()