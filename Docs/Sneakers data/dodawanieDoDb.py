import json
import pyodbc
import os

os.chdir(os.path.dirname(os.path.abspath(__file__)))
# Połączenie z bazą danych SQL Server
conn = pyodbc.connect(
    r'DRIVER={SQL Server};'
    r'SERVER=pikkoLaptop\SQLEXPRESS;'
    r'DATABASE=ShoeBoardDataBase;'
    r'Trusted_Connection=yes;'
)
cursor = conn.cursor()

# Funkcja do wstawiania danych do bazy
def insert_product(data):
    query = """
    INSERT INTO ShoeCatalogs (Model_No, Title, Nickname, Brand, Series, Url_Link_Handler, Gender, Image_Url, Release_Date, Main_Color, Colorway, Price)
    VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
    """
    
    # Przygotowanie danych do wstawienia
    model_no = 'Stockx'  # Używamy 'urlKey' jako 'Model_No'
    title = data.get('title', '')
    nickname = data.get('name', '')  # Używamy 'name' jako 'Nickname'
    brand = data.get('brand', '')
    series = data.get('model', '')  # Używamy 'model' jako 'Series'
    handle = data.get('urlKey', '')  # Możesz zaktualizować według potrzeb
    gender = data.get('gender', '').upper()
    
    # Sprawdzanie, czy URL zdjęcia istnieje
    image = data.get('media', {}).get('thumbUrl', '')  # Możesz użyć 'smallImageUrl' lub innego
    # Formatowanie daty wydania do formatu 'YYYY-MM-DD'
    release_date = format_release_date(next((trait['value'] for trait in data.get('productTraits', []) if trait['name'] == 'Release Date'), ''))
    
    main_color = data.get('name', '')   # Wprowadź logikę pozyskiwania koloru, jeśli dostępna
    colorway = data.get('name', '')   # Wprowadź logikę pozyskiwania kolorystyki, jeśli dostępna
    # Domyślna cena, z salesInformation
    price = next((float(trait['value']) for trait in data.get('productTraits', []) if trait['name'] == 'Retail Price'), 0.0)

    # Wstawianie danych do bazy
    cursor.execute(query, (model_no, title, nickname, brand, series, handle, gender, image, release_date, main_color, colorway, price))
    conn.commit()

def format_release_date(date_value):
    date_str = str(date_value)
    # Sprawdzamy, czy data ma poprawny format (YYYY-MM-DD)
    if len(date_str) >= 10:
        try:
            return date_str[:10]  # Przycinamy do 'YYYY-MM-DD'
        except Exception as e:
            return '0001-01-01'
    else:
        return '0001-01-01'

# Odczyt danych z pliku JSON i wstawienie ich do bazy
def process_json_file(file_path):
    with open(file_path, 'r', encoding='utf-8') as file:
        data = json.load(file)
        products = data.get('data', {}).get('results', [])  # Używamy nowej ścieżki do produktów
        for product in products:
            insert_product(product)

# Przykładowe wywołanie funkcji dla pliku JSON
json_files = [
    'call3NikePage1.json',
    'call4AdidasPage1.json',
    'call5Converse.json',
    'call6NewBalance.json',
    'call7Crocs.json',
    'call8Puma.json',
    'call9Jordan.json',
    'call10Supreme.json',
    'call11ASiCS.json',
    'call12Timberland.json',
    'call13Reebok.json',
    'call14Vans.json',
    'call15TravisScott.json',
    'call16Balenciaga.json',
    'call17NikeForce.json',
    'call18NikeAirMax.json',
    # Dodaj inne pliki JSON według potrzeby
]

for json_file in json_files:
    process_json_file(json_file)
    print(f"Dodano {json_file}")

# Zamknięcie połączenia
cursor.close()
conn.close()
