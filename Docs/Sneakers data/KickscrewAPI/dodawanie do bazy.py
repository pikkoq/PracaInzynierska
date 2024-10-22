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
    model_no = data.get('model_no', '')
    title = data.get('title', '')
    nickname = data.get('nickname', '') or ''
    brand = data.get('brand', '')
    series = data.get('series', '') or ''
    handle = data.get('handle', '')
    gender = data.get('gender', '')
    image = data.get('image', '')
    
    # Formatowanie daty wydania do formatu 'YYYY-MM-DD'
    release_date = format_release_date(data.get('release_date', ''))
    
    main_color = data.get('main_color', '') or ''
    colorway = ', '.join(data.get('colorway', []))  # Kolorystyka jako lista, łączymy na string
    price = data.get('lowest_price_original', 0.00)

    # Wstawianie danych do bazy
    cursor.execute(query, (model_no, title, nickname, brand, series, handle, gender, image, release_date, main_color, colorway, price))
    conn.commit()

def format_release_date(date_value):
    date_str = str(date_value)
    # Sprawdzamy, czy data ma 8 znaków (format: YYYYMMDD)
    if len(date_str) == 8 and date_str.isdigit():
        try:
            # Tworzymy poprawny format daty YYYY-MM-DD
            return date_str[:4] + '-' + date_str[4:6] + '-' + date_str[6:]
        except Exception as e:
            # Jeśli wystąpił błąd (choćby nie powinien), zwracamy None
            return '0001-01-01'
    else:
        # Jeśli nie ma daty lub jest w złym formacie, zwracamy None
        return '0001-01-01'

# Odczyt danych z pliku JSON i wstawienie ich do bazy
def process_json_file(file_path):
    with open(file_path, 'r', encoding='utf-8') as file:
        data = json.load(file)
        products = data.get('products', [])
        for product in products:
            insert_product(product)

# Przykładowe wywołanie funkcji dla pliku JSON
json_files = ['AdidasPage1.json',
'AdidasPage2.json',
'AdidasPage3.json',
'AdidasPage4.json',
'AdidasPage5.json',
'AdidasPage6.json',
'AdidasPage7.json',
'AdidasPage8.json',
'AdidasPage9.json',
'AdidasPage10.json',
'AdidasPage11.json',
'AdidasPage13.json',
'AdidasPage14.json',
'AdidasPage15.json',
'AdidasPage16.json',
'AdidasPage17.json',
'AdidasPage18.json',
'AdidasPage19.json',
'AdidasPage20.json',
'AdidasPage21.json',
'ConversePage1.json',
'ConversePage2.json',
'ConversePage3.json',
'ConversePage4.json',
'JordanPage1.json',
'JordanPage2.json',
'JordanPage3.json',
'JordanPage4.json',
'JordanPage5.json',
'JordanPage6.json',
'JordanPage7.json',
'JordanPage8.json',
'JordanPage9.json',
'JordanPage10.json',
'NewBalancePage1.json',
'NewBalancePage2.json',
'NewBalancePage3.json',
'NewBalancePage4.json',
'NewBalancePage5.json',
'NikePage1.json',
'NikePage2.json',
'NikePage3.json',
'NikePage4.json',
'NikePage5.json',
'NikePage6.json',
'NikePage7.json',
'NikePage8.json',
'NikePage9.json',
'NikePage10.json',
'NikePage11.json',
'NikePage12.json',
'NikePage13.json',
'NikePage14.json'
]  # Tutaj wstaw swoje pliki
for json_file in json_files:
    process_json_file(json_file)
    print(f"Dodano {json_file}")

# Zamknięcie połączenia
cursor.close()
conn.close()
