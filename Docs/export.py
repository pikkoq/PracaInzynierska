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

def export_table_to_json(table_name, file_name):
    query = f"SELECT * FROM {table_name}"
    df = pd.read_sql(query, conn)
    df.to_json(file_name, orient="records")
    print(f"Table {table_name} exported to {file_name}")

export_table_to_json("AspNetUsers", "AspNetUsers.json")
conn.close()