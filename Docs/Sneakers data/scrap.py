import http.client
import json

conn = http.client.HTTPSConnection("kickscrew-sneakers-data.p.rapidapi.com")

headers = {
    'x-rapidapi-key': "68f69871a9msh9eaa5fbb68662a9p12dd75jsn70587b78d654",
    'x-rapidapi-host': "kickscrew-sneakers-data.p.rapidapi.com"
}

# Pętla od 1 do 10, która zmienia numer strony w zapytaniu
for page in range(11, 15):
    # Ustawianie endpointa z odpowiednią stroną
    endpoint = f"/brand-collection?brand=nike&hitsPerPage=100&page={page}"
    
    # Wysłanie zapytania
    conn.request("GET", endpoint, headers=headers)
    
    # Odczytanie odpowiedzi
    res = conn.getresponse()
    data = res.read()
    
    # Dekodowanie odpowiedzi JSON i zapisanie do pliku
    json_data = json.loads(data.decode("utf-8"))
    
    # Zapisanie odpowiedzi do pliku AdidasPage1.json, AdidasPage2.json, itd.
    with open(f"NikePage{page}.json", "w") as file:
        json.dump(json_data, file, indent=4)

    print(f"Zapisano dane z Page {page}")
