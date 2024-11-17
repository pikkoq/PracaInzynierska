import pyodbc
import json
import os

os.chdir(os.path.dirname(os.path.abspath(__file__)))

conn = pyodbc.connect(
    r'DRIVER={SQL Server};'
    r'SERVER=PIKKOLAPTOP\SQLEXPRESS;'
    r'DATABASE=ShoeBoardDataBase;'
    r'Trusted_Connection=yes;'
)

def import_json_to_table(json_data, table_name):
    cursor = conn.cursor()

    for item in json_data:
        cursor.execute(f"""
            INSERT INTO {table_name} (
                UserId, ShoeCatalogId, UserShoeCatalogId, Size, 
                ComfortRating, StyleRating, Season, Review, DateAdded, ShoeAddType
            ) 
            VALUES ( ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        """, 
        item["UserId"], 
        item["ShoeCatalogId"], 
        item["UserShoeCatalogId"], 
        item["Size"], 
        item["ComfortRating"], 
        item["StyleRating"], 
        item["Season"], 
        item["Review"], 
        item["DateAdded"], 
        item["ShoeAddType"])
    
    conn.commit()
    print(f"Data successfully imported into {table_name}")

with open('Shoes.json', 'r', encoding='utf-8') as file:
    data = json.load(file)

import_json_to_table(data, "Shoes")

conn.close()
