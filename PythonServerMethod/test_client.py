import requests

files = {"image": open("Test.jpg", "rb")}
res = requests.post("http://127.0.0.1:5000/detect", files=files)

print(res.json())